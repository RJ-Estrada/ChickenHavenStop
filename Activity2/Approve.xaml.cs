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
using System.Data;
using MySql.Data.MySqlClient;

namespace Activity2
{
    /// <summary>
    /// Interaction logic for Approve.xaml
    /// </summary>
    public partial class Approve : Window
    {
        private string username;
        private string role;

        public Approve(string username, string role)
        {
            InitializeComponent();
            this.username = username;
            this.role = role;
            PopulateUserInfoDataGrid();
            PopulateChickenPriceDataGrid();
            PopulatePizzaPriceDataGrid();
            PopulateDrinksPriceDataGrid();
            PopulateMealPriceDataGrid();
        }

        private void PopulateUserInfoDataGrid()
        {
            string connectionString = "Server=localhost;Database=activity2_database;User=root;Password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SQL query to retrieve data from the database
                    string query = "SELECT UserCode, Name, Email, Address, Username, Role, Status FROM userinfo";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // Create a DataTable to hold the retrieved data
                            DataTable dataTable = new DataTable();

                            // Fill the DataTable with data from the database
                            adapter.Fill(dataTable);

                            // Bind the DataTable to your DataGrid
                            retrieved_info.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void PopulateChickenPriceDataGrid()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SQL query to retrieve data from the database
                    string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE ChickenPrice IS NOT NULL AND ChickenPrice <> 0";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // Create a DataTable to hold the retrieved data
                            DataTable dataTable = new DataTable();

                            // Fill the DataTable with data from the database
                            adapter.Fill(dataTable);

                            // Bind the DataTable to your DataGrid
                            MenuForChicken.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void PopulatePizzaPriceDataGrid()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SQL query to retrieve data from the database
                    string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE PizzaPrice IS NOT NULL AND PizzaPrice <> 0";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // Create a DataTable to hold the retrieved data
                            DataTable dataTable = new DataTable();

                            // Fill the DataTable with data from the database
                            adapter.Fill(dataTable);

                            // Bind the DataTable to your DataGrid
                            MenuForPizza1.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void PopulateDrinksPriceDataGrid()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SQL query to retrieve data from the database
                    string query = "SELECT MenuForDrinks, DrinksPrice FROM price WHERE DrinksPrice IS NOT NULL AND DrinksPrice <> 0";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // Create a DataTable to hold the retrieved data
                            DataTable dataTable = new DataTable();

                            // Fill the DataTable with data from the database
                            adapter.Fill(dataTable);

                            // Bind the DataTable to your DataGrid
                            MenuForDrinks.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void PopulateMealPriceDataGrid()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Create a SQL query to retrieve data from the database
                    string query = "SELECT MenuForMeals, MealPrice FROM price WHERE MealPrice IS NOT NULL AND MealPrice <> 0";

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            // Create a DataTable to hold the retrieved data
                            DataTable dataTable = new DataTable();

                            // Fill the DataTable with data from the database
                            adapter.Fill(dataTable);

                            // Bind the DataTable to your DataGrid
                            MenuForMeals.ItemsSource = dataTable.DefaultView;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle DataGrid selection changes if needed
        }

        private void acc_approve_Click(object sender, RoutedEventArgs e)
        {
            // Check if a row is selected
            if (retrieved_info.SelectedItem != null)
            {
                // Get the selected row as a DataRowView
                DataRowView selectedRow = (DataRowView)retrieved_info.SelectedItem;

                // Extract the values of Username and Email from the selected row
                string username = selectedRow["Username"]?.ToString() ?? "";

                // Define your connection string for the "activity2_database" database
                string activity2ConnectionString = "Server=localhost;Database=activity2_database;User=root;Password=;";

                try
                {
                    using (MySqlConnection activity2Connection = new MySqlConnection(activity2ConnectionString))
                    {
                        activity2Connection.Open();

                        // Check the status of the account in the "userinfo" table
                        string checkStatusQuery = "SELECT Status FROM userinfo WHERE Username = @Username";
                        using (MySqlCommand checkStatusCommand = new MySqlCommand(checkStatusQuery, activity2Connection))
                        {
                            checkStatusCommand.Parameters.AddWithValue("@Username", username);
                            string status = checkStatusCommand.ExecuteScalar()?.ToString() ?? "";

                            // Check if the account's status is not "Pending"
                            if (status != "Pending")
                            {
                                MessageBox.Show("This account's status has already been updated!", "Error Message");
                                return; // Exit the method to prevent duplication
                            }
                        }

                        // Define your connection string for the "updated_acc_status" database
                        string updatedAccConnectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";

                        using (MySqlConnection updatedAccConnection = new MySqlConnection(updatedAccConnectionString))
                        {
                            updatedAccConnection.Open();

                            // Update the status in the "userinfo" table
                            string updateStatusQuery = "UPDATE userinfo SET Status = 'Approved' WHERE Username = @Username";
                            using (MySqlCommand updateStatusCommand = new MySqlCommand(updateStatusQuery, activity2Connection))
                            {
                                updateStatusCommand.Parameters.AddWithValue("@Username", username);
                                int rowsUpdated = updateStatusCommand.ExecuteNonQuery();

                                if (rowsUpdated > 0)
                                {
                                    // Successfully updated status in userinfo table
                                }
                                else
                                {
                                    MessageBox.Show("Status update failed in userinfo table.");
                                }
                            }

                            // Update the status in the "updated_status" table
                            string updateStatusInUpdatedStatusQuery = "UPDATE updated_status SET Status = 'Approved' WHERE UsernameUp = @Username";
                            using (MySqlCommand updateStatusInUpdatedStatusCommand = new MySqlCommand(updateStatusInUpdatedStatusQuery, updatedAccConnection))
                            {
                                updateStatusInUpdatedStatusCommand.Parameters.AddWithValue("@Username", username);
                                int rowsUpdated = updateStatusInUpdatedStatusCommand.ExecuteNonQuery();

                                if (rowsUpdated > 0)
                                {
                                    MessageBox.Show("Account Approved!", "Account Status");
                                }
                                else
                                {
                                    MessageBox.Show("Status update failed in updated_status table.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to approve.", "Error Message");
            }
        }



        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (retrieved_info.SelectedItem != null)
            {
                DataRowView selectedRow = (DataRowView)retrieved_info.SelectedItem;

                string username = selectedRow["Username"]?.ToString() ?? "";

                string activity2ConnectionString = "Server=localhost;Database=activity2_database;User=root;Password=;";
                string updatedAccConnectionString = "Server=localhost;Database=updated_acc_status;User=root;Password=;";

                try
                {
                    using (MySqlConnection activity2Connection = new MySqlConnection(activity2ConnectionString))
                    {
                        activity2Connection.Open();

                        using (MySqlConnection updatedAccConnection = new MySqlConnection(updatedAccConnectionString))
                        {
                            updatedAccConnection.Open();

                            // Check the status in the updated_status table
                            string checkUpdatedStatusQuery = "SELECT Status FROM updated_status WHERE UsernameUp = @Username";
                            using (MySqlCommand checkUpdatedStatusCommand = new MySqlCommand(checkUpdatedStatusQuery, updatedAccConnection))
                            {
                                checkUpdatedStatusCommand.Parameters.AddWithValue("@Username", username);
                                string updatedStatus = checkUpdatedStatusCommand.ExecuteScalar()?.ToString() ?? "";

                                // Check the status in the userinfo table in activity2_database
                                string checkActivity2StatusQuery = "SELECT Status FROM userinfo WHERE Username = @Username";
                                using (MySqlCommand checkActivity2StatusCommand = new MySqlCommand(checkActivity2StatusQuery, activity2Connection))
                                {
                                    checkActivity2StatusCommand.Parameters.AddWithValue("@Username", username);
                                    string activity2Status = checkActivity2StatusCommand.ExecuteScalar()?.ToString() ?? "";

                                    if (updatedStatus == "Pending" && activity2Status == "Pending")
                                    {
                                        // Update the status in the updated_status table
                                        string updateUpdatedStatusQuery = "UPDATE updated_status SET Status = 'Rejected' WHERE UsernameUp = @Username";
                                        using (MySqlCommand updateUpdatedStatusCommand = new MySqlCommand(updateUpdatedStatusQuery, updatedAccConnection))
                                        {
                                            updateUpdatedStatusCommand.Parameters.AddWithValue("@Username", username);
                                            int updatedUpdatedStatusRows = updateUpdatedStatusCommand.ExecuteNonQuery();

                                            if (updatedUpdatedStatusRows > 0)
                                            {
                                                // Update the status in the userinfo table in activity2_database
                                                string updateActivity2StatusQuery = "UPDATE userinfo SET Status = 'Rejected' WHERE Username = @Username";
                                                using (MySqlCommand updateActivity2StatusCommand = new MySqlCommand(updateActivity2StatusQuery, activity2Connection))
                                                {
                                                    updateActivity2StatusCommand.Parameters.AddWithValue("@Username", username);
                                                    int updatedActivity2StatusRows = updateActivity2StatusCommand.ExecuteNonQuery();

                                                    if (updatedActivity2StatusRows > 0)
                                                    {
                                                        MessageBox.Show("Account Rejected!", "Account Status");
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("Status update failed in userinfo table.");
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("Status update failed in updated_status table.");
                                            }
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("This account's status has already been updated!", "Error Message");
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to reject.", "Error Message");
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            OrderHistory orderHistory = new OrderHistory(username, role);
            orderHistory.Show();
            this.Close();
        }

        private void update_info_Click(object sender, RoutedEventArgs e)
        {
            if (retrieved_info.SelectedItem != null)
            {
                DataRowView row = (DataRowView)retrieved_info.SelectedItem;

                // Extract data from the selected row
                string? code = row["UserCode"].ToString();
                string? name = row["Name"].ToString();
                string? email = row["Email"].ToString();
                string? address = row["Address"].ToString();
                string? Username = row["Username"].ToString();
                string? Role = row["Role"].ToString();
                string? status = row["Status"].ToString();

                // Pass the data to the EditUserInfo window
                EditUserInfo editUserInfo = new EditUserInfo(code, name, email, address, Username, Role, status, username, role);
                editUserInfo.Show();
                this.Close();
            }
            else
            {
                // No row selected, show an error message
                MessageBox.Show("Please select a row to update the user's information.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void price_edit_Click(object sender, RoutedEventArgs e)
        {
            if (MenuForChicken.SelectedItem != null || MenuForPizza1.SelectedItem != null ||
                MenuForDrinks.SelectedItem != null || MenuForMeals.SelectedItem != null)
            {
                string MenuForChickenText = "";
                string ChickenPriceText = "";
                string MenuForPizzaText = "";
                string PizzaPriceText = "";
                string MenuForDrinksText = "";
                string DrinksPriceText = "";
                string MenuForMealsText = "";
                string MealPriceText = "";

                if (MenuForChicken.SelectedItem != null)
                {
                    var chickenRow = (DataRowView)MenuForChicken.SelectedItem;
                    MenuForChickenText = chickenRow["MenuForChicken"].ToString();
                    ChickenPriceText = chickenRow["ChickenPrice"].ToString();
                }

                if (MenuForPizza1.SelectedItem != null)
                {
                    var pizzaRow = (DataRowView)MenuForPizza1.SelectedItem;
                    MenuForPizzaText = pizzaRow["MenuForPizza"].ToString();
                    PizzaPriceText = pizzaRow["PizzaPrice"].ToString();
                }

                if (MenuForDrinks.SelectedItem != null)
                {
                    var drinksRow = (DataRowView)MenuForDrinks.SelectedItem;
                    MenuForDrinksText = drinksRow["MenuForDrinks"].ToString();
                    DrinksPriceText = drinksRow["DrinksPrice"].ToString();
                }

                if (MenuForMeals.SelectedItem != null)
                {
                    var mealsRow = (DataRowView)MenuForMeals.SelectedItem;
                    MenuForMealsText = mealsRow["MenuForMeals"].ToString();
                    MealPriceText = mealsRow["MealPrice"].ToString();
                }

                Edit_Price edit_Price = new Edit_Price(MenuForChickenText, ChickenPriceText, MenuForPizzaText, PizzaPriceText, MenuForDrinksText,
                    DrinksPriceText, MenuForMealsText, MealPriceText, username, role);
                edit_Price.Show();
                this.Close();
            }
            else
            {
                // No row selected, show an error message
                MessageBox.Show("Please select a row to update the item's price.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void add_menu_Click(object sender, RoutedEventArgs e)
        {
            AddMenu addMenu = new AddMenu(username, role);
            addMenu.Show();
            this.Close();
        }

        private void SetVisibilityAndZIndex(Visibility visibility, int zIndex, params UIElement[] elements)
        {
            foreach (var element in elements)
            {
                element.Visibility = visibility;

                if (element is FrameworkElement frameworkElement)
                {
                    Panel.SetZIndex(frameworkElement, zIndex);
                }
            }
        }

        private void MenuSettings_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityAndZIndex(Visibility.Visible, 1, MenuForChicken, MenuForDrinks, MenuForMeals, MenuForPizza1);
            SetVisibilityAndZIndex(Visibility.Visible, 1, price_edit, add_menu, shop, shop2, bck_app);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, MenuSettings, UserSettings, main_back);
        }

        private void bck_app_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityAndZIndex(Visibility.Visible, 1, MenuSettings, UserSettings, main_back);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, MenuForChicken, MenuForDrinks, MenuForMeals, MenuForPizza1);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, price_edit, add_menu, shop, shop2, bck_app);
        }

        private void UserSettings_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityAndZIndex(Visibility.Visible, 1, retrieved_info, userreg, userreg2, update_info, bck_app2);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, MenuSettings, UserSettings, main_back);
        }

        private void bck_app2_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityAndZIndex(Visibility.Visible, 1, MenuSettings, UserSettings, main_back);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, retrieved_info, userreg, userreg2, update_info, bck_app2);
        }
    }
}