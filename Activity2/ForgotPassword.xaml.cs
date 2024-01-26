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
using System.Text.RegularExpressions;

namespace Activity2
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Window
    {
        public ForgotPassword()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string emailToRecover = Pass_Recover.Text;
            string usernameToRecover = user_recover.Text; // Get the username input

            // Check if both the email and username inputs are not empty
            if (string.IsNullOrEmpty(emailToRecover) || string.IsNullOrEmpty(usernameToRecover))
            {
                MessageBox.Show("Please enter both email and username.", "Error Message");
                return;
            }

            // Validate the email format using a regular expression
            if (!IsValidEmail(emailToRecover))
            {
                MessageBox.Show("Please enter a valid email address.", "Error Message");
                return;
            }

            // Query the database to find the email and username
            string connectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Create a SQL query to check if the email and username exist and have an "Approved" status
                    string query = "SELECT COUNT(*) FROM updated_status WHERE BINARY EmailUp = @Email AND BINARY UsernameUp = @Username AND Status = 'Approved'";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Email", emailToRecover);
                        command.Parameters.AddWithValue("@Username", usernameToRecover);

                        int count = Convert.ToInt32(command.ExecuteScalar());

                        if (count > 0)
                        {
                            // Email and username found, open the ResetPass window and pass the username
                            ResetPass resetPassWindow = new ResetPass(usernameToRecover);
                            resetPassWindow.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Email or username not found!", "Error Message");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error Message");
                }
            }
        }

        // Function to validate email format using regular expression
        private bool IsValidEmail(string email)
        {
            string pattern = @"^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$";
            return Regex.IsMatch(email, pattern);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}