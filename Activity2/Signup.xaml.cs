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
using MySql.Data.MySqlClient;
using System.Security.Cryptography;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;

namespace Activity2
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : Window
    {
        public Signup()
        {
            InitializeComponent();
            RolePick.Items.Add("Staff");
            RolePick.Items.Add("Manager");
            RolePick.Items.Add("Admin");
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            // Define your MySQL connection strings for the two databases
            string originalConnectionString = "Server=localhost;Database=activity2_database;User=root;Password=;";
            string updatedConnectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";

            try
            {
                using (MySqlConnection originalConnection = new MySqlConnection(originalConnectionString))
                using (MySqlConnection updatedConnection = new MySqlConnection(updatedConnectionString))
                {
                    originalConnection.Open();
                    updatedConnection.Open();

                    // Retrieve user input from textboxes
                    string name = Name.Text;
                    string email = Email.Text;
                    string studentNum = StudNum.Text;
                    string username = Username.Text;
                    string password = Password.Password;
                    string? selectedRole = RolePick.SelectedItem?.ToString();
                    string Mother = mother.Text; // New textbox for mother
                    string Pet = pet.Text;       // New textbox for pet
                    string Street = street.Text; // New textbox for street

                    // Check if any of the fields are empty
                    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(studentNum) ||
                        string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                        string.IsNullOrWhiteSpace(Mother) || string.IsNullOrWhiteSpace(Pet) || string.IsNullOrWhiteSpace(Street) ||
                        (RolePick.SelectedItem == null || string.IsNullOrWhiteSpace(RolePick.SelectedItem.ToString())))
                    {
                        MessageBox.Show("Please fill in all the fields before registering.", "Error Message");
                        return; // Exit the method without proceeding further
                    }


                    // Check if the name contains a number using a regular expression
                    if (Regex.IsMatch(name, @"\d"))
                    {
                        MessageBox.Show("Name cannot contain numbers. Please enter a valid name.", "Error Message");
                        return; // Exit the method without proceeding further
                    }

                    // Check if the email matches the required format
                    if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$"))
                    {
                        MessageBox.Show("Invalid email format. Please enter a valid email address.", "Error Message");
                        return; // Exit the method without proceeding further
                    }

                    // Check if the email already exists in the original database
                    string checkEmailSql = "SELECT COUNT(*) FROM userinfo WHERE Email = @Email";
                    using (MySqlCommand checkEmailCommand = new MySqlCommand(checkEmailSql, originalConnection))
                    {
                        checkEmailCommand.Parameters.AddWithValue("@Email", email);

                        int existingEmailCount = Convert.ToInt32(checkEmailCommand.ExecuteScalar());

                        if (existingEmailCount > 0)
                        {
                            MessageBox.Show("Email address already exists. Please use a different email.", "Error Message");
                            return; // Exit the method without proceeding further
                        }
                    }

                    // Check if the username already exists in the original database
                    string checkUsernameSql = "SELECT COUNT(*) FROM userinfo WHERE Username = @Username";
                    using (MySqlCommand checkUsernameCommand = new MySqlCommand(checkUsernameSql, originalConnection))
                    {
                        checkUsernameCommand.Parameters.AddWithValue("@Username", username);

                        int existingUserCount = Convert.ToInt32(checkUsernameCommand.ExecuteScalar());

                        if (existingUserCount > 0)
                        {
                            MessageBox.Show("Username already exists. Please choose a different username.", "Error Message");
                            return; // Exit the method without proceeding further
                        }
                    }

                    // Hash the user's password before storing it
                    string userPassword = Password.Password; // Get the user's password(1)
                    string hashedPassword = HashPassword(userPassword); // Hash the user's password(1)
                    string saltedPassword = userPassword + "Act2"; // Concatenate the user's password with "Act2"(2)
                    string hashedFixedSalt = HashPassword(saltedPassword); // Hash the concatenated password and salt(2)
                    string salt = GenerateRandomSalt(); // Generate a random salt(3)
                    string randomSaltedPassword = userPassword + salt; // Concatenate the user's password with the salt(3)
                    string hashedPerUser = HashPassword(randomSaltedPassword); // Hash the concatenated password and salt(3)

                    Random rand = new Random();
                    int randomUserCode = rand.Next(100000, 999999);
                    string userCode = randomUserCode.ToString("D6");

                    // Insert the new user into the original database
                    string insertUserSql = "INSERT INTO userinfo (UserCode, Name, Email, Address, Username, Role, HashedPass, FixedSalt, PerUser, RandomSalt, Status) " +
                        "VALUES (@UserCode, @Name, @Email, @Address, @Username, @Role, @HashedPass, @FixedSalt, @PerUser, @RandomSalt, @Status)";
                    using (MySqlCommand insertUserCommand = new MySqlCommand(insertUserSql, originalConnection))
                    {
                        insertUserCommand.Parameters.AddWithValue("@UserCode", userCode); // Use the same UserCode for both tables
                        insertUserCommand.Parameters.AddWithValue("@Name", name);
                        insertUserCommand.Parameters.AddWithValue("@Email", email);
                        insertUserCommand.Parameters.AddWithValue("@Address", studentNum);
                        insertUserCommand.Parameters.AddWithValue("@Username", username);
                        insertUserCommand.Parameters.AddWithValue("@Role", selectedRole);
                        insertUserCommand.Parameters.AddWithValue("@HashedPass", hashedPassword);
                        insertUserCommand.Parameters.AddWithValue("@FixedSalt", hashedFixedSalt);
                        insertUserCommand.Parameters.AddWithValue("@PerUser", hashedPerUser); // Store the hashed password with salt in PerUser column
                        insertUserCommand.Parameters.AddWithValue("@RandomSalt", salt); // Store the same random salt in RandomSalt column
                        insertUserCommand.Parameters.AddWithValue("@Status", "Pending");

                        int rowsAffected = insertUserCommand.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            // Insert the user data into the updated_acc_status database
                            string insertUpdatedUserSql = "INSERT INTO updated_status (UserCodeUp, UsernameUp, HashedPassword, EmailUp, Role, Mother, Pet, Street, Status) " +
                                "VALUES (@UserCodeUp, @UsernameUp, @HashedPassword, @EmailUp, @Role, @Mother, @Pet, @Street, @Status)";
                            using (MySqlCommand insertUpdatedUserCommand = new MySqlCommand(insertUpdatedUserSql, updatedConnection))
                            {
                                insertUpdatedUserCommand.Parameters.AddWithValue("@UserCodeUp", userCode); // Use the same UserCode
                                insertUpdatedUserCommand.Parameters.AddWithValue("@UsernameUp", username);
                                insertUpdatedUserCommand.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                                insertUpdatedUserCommand.Parameters.AddWithValue("@EmailUp", email);
                                insertUpdatedUserCommand.Parameters.AddWithValue("@Role", selectedRole);
                                insertUpdatedUserCommand.Parameters.AddWithValue("@Mother", Mother);
                                insertUpdatedUserCommand.Parameters.AddWithValue("@Pet", Pet);
                                insertUpdatedUserCommand.Parameters.AddWithValue("@Street", Street);
                                insertUpdatedUserCommand.Parameters.AddWithValue("@Status", "Pending");

                                int updatedRowsAffected = insertUpdatedUserCommand.ExecuteNonQuery();

                                if (updatedRowsAffected > 0)
                                {
                                    MessageBox.Show("Thank you for registering! Please wait for admin to approve your account.", "Registration Successful!");

                                    Name.Text = string.Empty;
                                    Email.Text = string.Empty;
                                    StudNum.Text = string.Empty;
                                    Username.Text = string.Empty;
                                    Password.Clear();
                                    mother.Text = string.Empty;
                                    pet.Text = string.Empty;
                                    street.Text = string.Empty;
                                    RolePick.SelectedItem = null;
                                }
                                else
                                {
                                    MessageBox.Show("Registration failed in the updated database. Please try again.");
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Registration failed in the original database. Please try again.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error Message");
            }
        }

        // Hash the password using SHA-256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // Generate a random salt
        private string GenerateRandomSalt()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 12)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void back_button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();

        }
        private string selectedRole; // Declare a class-level variable to store the selected role

        private void RolePick_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RolePick.SelectedItem != null)
            {
                selectedRole = RolePick.SelectedItem.ToString();
            }
        }
    }
}