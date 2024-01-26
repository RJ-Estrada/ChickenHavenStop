using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Activity2
{
    /// <summary>
    /// Interaction logic for EditUserInfo.xaml
    /// </summary>
    public partial class EditUserInfo : Window
    {
        private string username;
        private string role;

        public EditUserInfo(string? code, string? name, string? email, string? address, string? user, string? userRole, string? userStatus, string username, string role)
        {
            InitializeComponent();
            this.username = username;
            this.role = role;

            InitializeComboBoxes();
            name_info.Text = name;
            email_info.Text = email;
            address_info.Text = address;
            username_info.Text = user;
            role_info.SelectedItem = userRole;
            status_info.SelectedItem = userStatus;
        }

        private void InitializeComboBoxes()
        {
            // Add items to the ComboBoxes
            role_info.Items.Add("Staff");
            role_info.Items.Add("Manager");
            role_info.Items.Add("Admin");

            status_info.Items.Add("Pending");
            status_info.Items.Add("Approved");
            status_info.Items.Add("Rejected");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the updated values from the UI elements
            string updatedName = name_info.Text;
            string updatedEmail = email_info.Text;
            string updatedAddress = address_info.Text;
            string updatedUsername = username_info.Text;
            string? updatedRole = role_info.SelectedItem as string;
            string? updatedStatus = status_info.SelectedItem as string;

            // Create a flag to check if any field has changed
            bool fieldsChanged = false;

            // Check for empty or null values
            if (string.IsNullOrWhiteSpace(updatedName) ||
                string.IsNullOrWhiteSpace(updatedEmail) ||
                string.IsNullOrWhiteSpace(updatedAddress) ||
                string.IsNullOrWhiteSpace(updatedUsername) ||
                string.IsNullOrWhiteSpace(updatedRole) ||
                string.IsNullOrWhiteSpace(updatedStatus))
            {
                // Show an error message if any of the fields are empty
                MessageBox.Show("Please fill in all fields before updating.", "Validation Error");
            }
            else if (ContainsNumber(updatedName))
            {
                // Show an error message if the name contains a number
                MessageBox.Show("Name cannot contain a number.", "Validation Error");
            }
            else if (!IsValidEmail(updatedEmail))
            {
                // Show an error message if the email format is invalid
                MessageBox.Show("Email is not in a valid format.", "Validation Error");
            }
            else
            {
                // Establish a MySQL database connection
                string connectionString = "Server=localhost;Database=activity2_database;User=root;Password=;";
                using (var connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Check if any field has changed
                    string selectQuery = "SELECT Name, Email, Address, Username, Role, Status FROM userinfo WHERE Username = @Username";
                    using (var selectCommand = new MySqlCommand(selectQuery, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@Username", updatedUsername);

                        using (var reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                if (reader["Name"].ToString() != updatedName ||
                                    reader["Email"].ToString() != updatedEmail ||
                                    reader["Address"].ToString() != updatedAddress ||
                                    reader["Role"].ToString() != updatedRole ||
                                    reader["Status"].ToString() != updatedStatus)
                                {
                                    fieldsChanged = true; // At least one field has changed
                                }
                            }
                        }
                    }

                    if (fieldsChanged)
                    {
                        // At least one field has changed, proceed with the update
                        string updateQuery = "UPDATE userinfo SET " +
                            "Name = @Name, " +
                            "Email = @Email, " +
                            "Address = @Address, " +
                            "Username = @Username, " +
                            "Role = @Role, " +
                            "Status = @Status " +
                            "WHERE Username = @Username";

                        using (var command = new MySqlCommand(updateQuery, connection))
                        {
                            command.Parameters.AddWithValue("@Name", updatedName);
                            command.Parameters.AddWithValue("@Email", updatedEmail);
                            command.Parameters.AddWithValue("@Address", updatedAddress);
                            command.Parameters.AddWithValue("@Role", updatedRole);
                            command.Parameters.AddWithValue("@Status", updatedStatus);
                            command.Parameters.AddWithValue("@Username", updatedUsername);

                            // Execute the MySQL command to update the record
                            int rowsAffected = command.ExecuteNonQuery();

                            if (rowsAffected > 0)
                            {
                                // The update was successful
                                MessageBox.Show("Record updated successfully.", "Update Success");
                            }
                            else
                            {
                                // The update failed, handle as needed
                                MessageBox.Show("Failed to update the record.", "Update Error");
                            }
                        }
                    }
                    else
                    {
                        // No fields have changed
                        MessageBox.Show("No changes have been made.");
                    }
                }

                string updatedStatusConnectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";
                using (var updatedStatusConnection = new MySqlConnection(updatedStatusConnectionString))
                {
                    updatedStatusConnection.Open();

                    // Create a MySQL UPDATE query to update the record in the second database
                    string updateQuery = "UPDATE updated_status SET " +
                        "EmailUp = @EmailUp, " +
                        "Role = @Role, " +
                        "Status = @Status " +
                        "WHERE UsernameUp = @Username";

                    using (var command = new MySqlCommand(updateQuery, updatedStatusConnection))
                    {
                        command.Parameters.AddWithValue("@EmailUp", updatedEmail);
                        command.Parameters.AddWithValue("@Role", updatedRole);
                        command.Parameters.AddWithValue("@Status", updatedStatus);
                        command.Parameters.AddWithValue("@Username", updatedUsername);

                        // Execute the MySQL command to update the record in the second database
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // The update in the second database was successful
                        }
                        else
                        {
                            // The update in the second database failed, handle as needed
                            MessageBox.Show("Failed to update the record in the second database.", "Update Error");
                        }
                    }
                }
                // Close the window or perform other actions as needed
                Approve approve = new Approve(username, role);
                approve.Show();
                this.Close();
            }
        }

        private bool ContainsNumber(string text)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(text, @"\d");
        }

        private bool IsValidEmail(string email)
        {
            // Use a regular expression to check if the email has "@" and ends with ".com"
            return System.Text.RegularExpressions.Regex.IsMatch(email, @"^.+@.+\.[cC][oO][mM]$");
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Approve approve = new Approve(username, role);
            approve.Show();
            this.Close();
        }

        private void role_info_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedRole = role_info.SelectedItem as string;
            if (selectedRole != null)
            {
                // Handle the selected role as needed.
            }
        }

        private void status_info_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedStatus = status_info.SelectedItem as string;
            if (selectedStatus != null)
            {
                // Handle the selected status as needed.
            }
        }
    }
}