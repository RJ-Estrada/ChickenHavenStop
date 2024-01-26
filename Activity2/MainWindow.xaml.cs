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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Security.Cryptography;

namespace Activity2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int consecutiveWrongAttempts = 0;
        private DateTime buttonDisabledTime = DateTime.MinValue;
        private TimeSpan disableDuration = TimeSpan.FromSeconds(30);

        private Window? currentChildWindow = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenChildWindow(Window newWindow)
        {
            // Close the currently open child window (if any)
            if (currentChildWindow != null)
            {
                currentChildWindow.Close();
            }

            // Show the new child window
            currentChildWindow = newWindow;
            newWindow.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Signup signup = new Signup();
            OpenChildWindow(signup);
            this.Close(); // Close the MainWindow
        }

        string role = string.Empty;
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            // Check if the button should be disabled
            if (DateTime.Now - buttonDisabledTime < disableDuration)
            {
                MessageBox.Show("Login button is currently disabled.", "Error Message");
                return;
            }

            string username = user.Text;
            string password = pass.Password;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error Message!");
                return;
            }

            if ((username == "admin" && password == "admin123") ||
                (username == "manager" && password == "manager123") ||
                (username == "staff" && password == "staff123"))
            {
                MessageBox.Show($"Welcome, {username}!", "Login Successful");

                if (username == "admin")
                {
                    OrderHistory orderHistory = new OrderHistory(username, role);
                    OpenChildWindow(orderHistory);
                }
                else
                {
                    Dashboard dashboard = new Dashboard(username, role);
                    OpenChildWindow(dashboard);
                }
                this.Close(); // Close the MainWindow
                return; // Exit the method without showing any further messages
            }

            // Check the database for the user and status
            string connectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    // Retrieve the hashed password, status, and role from the "updated_status" table based on the username
                    string getUserQuery = "SELECT HashedPassword, Status, Role FROM updated_status WHERE BINARY UsernameUp = @Username";

                    using (MySqlCommand command = new MySqlCommand(getUserQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string storedHashedPassword = reader["HashedPassword"]?.ToString() ?? string.Empty;
                                string status = reader["Status"]?.ToString() ?? string.Empty;
                                string role = reader["Role"]?.ToString() ?? string.Empty;

                                if (!string.IsNullOrEmpty(storedHashedPassword))
                                {
                                    // Hash the provided plaintext password
                                    string hashedPassword = HashPassword(password);

                                    // Compare the hashed password with the stored hashed password
                                    if (hashedPassword == storedHashedPassword)
                                    {
                                        if (status == "Approved")
                                        {
                                            // Passwords match, user is authenticated and status is approved
                                            MessageBox.Show($"Welcome, {username}!", "Login Successful");
                                            // Reset the consecutive wrong attempts on successful login

                                            if (role == "Admin")
                                            {
                                                OrderHistory orderHistory = new OrderHistory(username, role);
                                                OpenChildWindow(orderHistory);
                                            }
                                            else
                                            {
                                                Dashboard dashboard = new Dashboard(username, role);
                                                OpenChildWindow(dashboard);
                                            }

                                            this.Close(); // Close the MainWindow
                                        }
                                        else if (status == "Rejected")
                                        {
                                            // Status is rejected, user cannot log in
                                            MessageBox.Show("Your account has been rejected. Please contact an administrator for assistance.", "Error Message");
                                            consecutiveWrongAttempts++;
                                        }
                                        else if (status == "Pending")
                                        {
                                            // Account status is pending
                                            MessageBox.Show("Account has not been approved yet. Please wait for the admin to approve your account.", "Pending Approval");
                                            consecutiveWrongAttempts++;
                                        }
                                    }
                                    else
                                    {
                                        // Passwords don't match
                                        MessageBox.Show("Incorrect password.", "Error Message");
                                        // Increment the consecutive wrong attempts
                                        consecutiveWrongAttempts++;
                                    }
                                }
                                else
                                {
                                    // Passwords match but username not found in the database
                                    MessageBox.Show("Incorrect username.", "Error Message");
                                    // Increment the consecutive wrong attempts
                                    consecutiveWrongAttempts++;
                                }
                            }
                            else
                            {
                                // Username not found in the database
                                MessageBox.Show("Account doesn't exist", "Error Message");
                                // Increment the consecutive wrong attempts
                                consecutiveWrongAttempts++;
                            }
                        }
                    }

                    // Check if consecutive wrong attempts reached the limit
                    if (consecutiveWrongAttempts >= 3)
                    {
                        // Disable the login button and record the time
                        Login.IsEnabled = false;
                        buttonDisabledTime = DateTime.Now;
                        MessageBox.Show("You have reached 3 consecutive incorrect login attempts. The login button will be enabled again after 30 seconds." +
                        " Please use the forgot password if you are still having trouble logging in.", "Error Message!");

                        // Start a timer to re-enable the button after 30 seconds
                        var timer = new System.Windows.Threading.DispatcherTimer();
                        timer.Interval = disableDuration;
                        timer.Tick += (s, ev) =>
                        {
                            Login.IsEnabled = true;
                            MessageBox.Show("Login button is now enabled.", "Info");
                            timer.Stop();

                            // Reset the consecutive wrong attempts
                            consecutiveWrongAttempts = 0;
                        };
                        timer.Start();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error Message");
                }
            }
        }

        // Helper function to hash a password using SHA-256
        private string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();

                foreach (byte b in hashedBytes)
                {
                    builder.Append(b.ToString("x2"));
                }

                return builder.ToString();
            }
        }

        private void forgotpass_btn_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword forgotPassword = new ForgotPassword();
            forgotPassword.Show();
            this.Close();
        }
    }
}