using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for ManagerPermission.xaml
    /// </summary>
    public partial class ManagerPermission : Window
    {
        public ManagerPermission()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the user's input from the "confirmcode" TextBox
            string userInput = confirmcode.Password;

            // Connect to the "dashboard" database
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Check if the user input matches any of the passwords in the "adminpass" table
                string query = "SELECT ManagerPass, AdminPass FROM adminpass";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? managerPass = reader["ManagerPass"]?.ToString();
                            string? adminPass = reader["AdminPass"]?.ToString();

                            if (!string.IsNullOrEmpty(managerPass) && userInput == managerPass)
                            {
                                // User's input matches the manager's password
                                MessageBox.Show("Void Confirm.", "Success");
                                // Indicate success
                                this.DialogResult = true;
                                return; // Exit the method
                            }
                            else if (!string.IsNullOrEmpty(adminPass) && userInput == adminPass)
                            {
                                // User's input matches the admin's password
                                MessageBox.Show("Void Confirm.", "Success");
                                // Indicate success
                                this.DialogResult = true;
                                return; // Exit the method
                            }
                        }
                        // If no matches were found
                        MessageBox.Show("Password did not match.", "Invalid Password");
                    }
                }
            }
            // Indicate failure
            this.DialogResult = false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}