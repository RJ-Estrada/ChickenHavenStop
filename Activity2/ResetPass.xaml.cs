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
using System.Security.Cryptography;

namespace Activity2
{
    /// <summary>
    /// Interaction logic for ResetPass.xaml
    /// </summary>
    public partial class ResetPass : Window
    {
        private string usernameToRecover; // Store the username for database queries

        public ResetPass(string username)
        {
            InitializeComponent();
            usernameToRecover = username;
        }

        private void check_ans_Click(object sender, RoutedEventArgs e)
        {
            string motherAnswer = mother_ans.Text;
            string petAnswer = pet_ans.Text;
            string streetAnswer = street_ans.Text;

            // Check if any of the textboxes are empty
            if (string.IsNullOrWhiteSpace(motherAnswer) || string.IsNullOrWhiteSpace(petAnswer) || string.IsNullOrWhiteSpace(streetAnswer))
            {
                MessageBox.Show("Please answer all the security questions.", "Error");
                return;
            }

            // Check if answers match the values in the database for the given username
            if (CheckAnswersInDatabase(usernameToRecover, motherAnswer, petAnswer, streetAnswer))
            {
                MessageBox.Show("Answers are correct. You can now reset your password.", "Success");

                // Enable the PasswordBoxes and Reset button
                new_pass.IsEnabled = true;
                con_pass.IsEnabled = true;
                Reset.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Incorrect answer(s)", "Error");
            }
        }

        private bool CheckAnswersInDatabase(string username, string motherAnswer, string petAnswer, string streetAnswer)
        {
            // Query the database to check if answers match
            string connectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL query to retrieve the answers for the given username
                    string query = "SELECT Mother, Pet, Street FROM updated_status WHERE UsernameUp = @Username";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string correctMotherAnswer = reader["Mother"]?.ToString() ?? "";
                                string correctPetAnswer = reader["Pet"]?.ToString() ?? "";
                                string correctStreetAnswer = reader["Street"]?.ToString() ?? "";

                                return motherAnswer == correctMotherAnswer &&
                                       petAnswer == correctPetAnswer &&
                                       streetAnswer == correctStreetAnswer;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error Message");
                }
            }

            return false; // Default to false if there is an error or answers don't match
        }

        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            // Get the new password from the "new_pass" and "con_pass" PasswordBoxes
            string newPassword = new_pass.Password;
            string confirmPassword = con_pass.Password;

            // Check if either of the PasswordBoxes is empty
            if (string.IsNullOrWhiteSpace(newPassword) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("Please answer all fields!", "Error");
                return;
            }

            // Check if the new password and confirmation match
            if (newPassword != confirmPassword)
            {
                MessageBox.Show("Password and confirmation do not match!", "Error");
                return;
            }
            // Hash the new password using SHA256
            string hashedPassword = HashPassword(newPassword);
            string fixedSalt = "Act2";
            string saltedPassword = newPassword + fixedSalt;
            string fixedhashedPassword = HashPassword(saltedPassword);
            string randomsalt = GenerateRandomSalt();
            string saltpass = newPassword + randomsalt;
            string hashedPerUser = HashPassword(saltpass);

            // Update the password in the userinfo table (activity2_database)
            if (UpdatePasswordInActivity2Database(usernameToRecover, hashedPassword, fixedhashedPassword, randomsalt, hashedPerUser))
            {
                // Update the password in the updated_status table (updated_acc_status)
                if (UpdatePasswordInUpdatedStatus(usernameToRecover, hashedPassword))
                {
                    MessageBox.Show("Password reset successfully.", "Success");
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Failed to update password in updated_acc_status.", "Error");
                }
            }
            else
            {
                MessageBox.Show("Failed to update password in activity2_database.", "Error");
            }
        }

        // Hash the password using SHA256
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

        // Update the password in the activity2_database (userinfo table)
        private bool UpdatePasswordInActivity2Database(string username, string hashedPassword, string fixedhashedPassword,
            string randomsalt, string hashedPerUser)
        {
            try
            {
                string connectionString = "Server=localhost;Database=activity2_database;User=root;Password=;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE userinfo SET HashedPass = @HashedPassword, FixedSalt = @FixedSalt, PerUser = @PerUser, RandomSalt = @RandomSalt WHERE Username = @Username";


                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@FixedSalt", fixedhashedPassword);
                        command.Parameters.AddWithValue("@PerUser", hashedPerUser);
                        command.Parameters.AddWithValue("@RandomSalt", randomsalt);

                        int rowsUpdated = command.ExecuteNonQuery();

                        return rowsUpdated > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error Message");
                return false;
            }
        }

        // Update the password in the updated_acc_status (updated_status table)
        private bool UpdatePasswordInUpdatedStatus(string username, string hashedPassword)
        {
            try
            {
                string connectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    string query = "UPDATE updated_status SET HashedPassword = @HashedPassword WHERE UsernameUp = @Username";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@HashedPassword", hashedPassword);
                        command.Parameters.AddWithValue("@Username", username);
                        int rowsUpdated = command.ExecuteNonQuery();

                        return rowsUpdated > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error Message");
                return false;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Show();
            this.Close();
        }
    }
}