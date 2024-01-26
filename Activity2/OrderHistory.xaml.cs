using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for OrderHistory.xaml
    /// </summary>
    public partial class OrderHistory : Window
    {
        private string username;
        private string role;

        public OrderHistory(string username, string role)
        {
            InitializeComponent();
            this.username = username;
            PopulateDataGrid();
            this.role = role;
        }

        private void PopulateDataGrid()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SQL query to retrieve data from the database
                    string query = "SELECT Username, Overall_QTY, Order_Code, Total_Price FROM void";

                    using (MySqlCommand selectCommand = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(selectCommand)) // Changed 'command' to 'selectCommand'
                        {
                            // Create a DataTable to hold the retrieved data
                            DataTable dataTable = new DataTable();

                            // Fill the DataTable with data from the database
                            adapter.Fill(dataTable);

                            // Bind the DataTable to your DataGrid
                            voidtable.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error");
            }
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            if (username == "admin" || role == "Admin")
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
            }
            else
            {
                Dashboard dashboard = new Dashboard(username, role);
                dashboard.Show();
            }
            this.Close();
        }

        private void UpdateOrderStatus(string orderCode, string newStatus)
        {
            // Connect to the "dashboard" database
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Update the "Status" column to the new status
                string updateQuery = "UPDATE void SET Status = @NewStatus WHERE Order_Code = @OrderCode";

                using (MySqlCommand updateCommand = new MySqlCommand(updateQuery, connection))
                {
                    updateCommand.Parameters.AddWithValue("@NewStatus", newStatus);
                    updateCommand.Parameters.AddWithValue("@OrderCode", orderCode);

                    int rowsAffected = updateCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        // Status updated successfully
                    }
                    else
                    {
                        MessageBox.Show("Failed to update status.", "Error");
                    }
                }
            }
        }

        private void Admin_Win_Click(object sender, RoutedEventArgs e)
        {
            if (username == "admin")
            {
                Approve approve = new Approve(username, role);
                approve.Show();
                this.Close();
            }
            else
            {
                // Connection string for your MySQL database
                string connectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("SELECT Role FROM updated_status WHERE UsernameUp = @username", connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        string role = (string)command.ExecuteScalar();

                        if (role == "Admin")
                        {
                            Approve approve = new Approve(username, role);
                            approve.Show();
                            this.Close();
                        }
                        else
                        {
                            // Display a message that the user is not authorized
                            MessageBox.Show("You are not authorized to enter this setting.", "Access Denied");
                        }
                    }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (username == "admin" || username == "manager")
            {
                Sales sales = new Sales(username, role);
                sales.Show();
                this.Close();
            }
            else
            {
                // Connection string for your MySQL database
                string connectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand("SELECT Role FROM updated_status WHERE UsernameUp = @username", connection))
                    {
                        command.Parameters.AddWithValue("@username", username);

                        string role = (string)command.ExecuteScalar();

                        if (role == "Admin" || role == "Manager")
                        {
                            Sales sales = new Sales(username, role);
                            sales.Show();
                            this.Close();
                        }
                        else
                        {
                            // Display a message that the user is not authorized
                            MessageBox.Show("You are not authorized to see the sales of the shop.", "Access Denied");
                        }
                    }
                }
            }
        }

        private void print_receipt_Click(object sender, RoutedEventArgs e)
        {
            // Check if the username is "admin" or the role is "Admin"
            if (username == "admin" || role == "Admin")
            {
                MessageBox.Show("You do not have permission to access this feature.", "Permission Denied");
                return;
            }
            // Get the selected row from the DataGrid
            DataRowView selectedRow = (DataRowView)voidtable.SelectedItem;

            if (selectedRow != null)
            {
                // Assuming your DataGrid is named "voidtable" and is bound to a DataView
                DataView dv = (DataView)voidtable.ItemsSource;

                // Access "OrderCode" dynamically from the data source
                if (dv.Table.Columns.Contains("Order_Code"))
                {
                    string orderCode = selectedRow["Order_Code"].ToString();

                    PrintReceipt printReceipt = new PrintReceipt(username, orderCode, role);
                    printReceipt.Show();
                }
                else
                {
                    // Handle the case where "OrderCode" column doesn't exist in the data source
                    MessageBox.Show("The 'OrderCode' column does not exist in the data source.", "Error");
                }
            }
            else
            {
                // Handle the case where no row is selected in the DataGrid
                MessageBox.Show("Please select an order to print.", "Error");
            }
        }
    }
}