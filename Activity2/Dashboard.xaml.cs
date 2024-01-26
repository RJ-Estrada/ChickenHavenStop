using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;

namespace Activity2
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Window
    {
        private string username;
        private string role;
        private chicken_class ChickenClassInstance;
        private pizza_class PizzaClassInstance;
        private drinks_class DrinksClassInstance;
        private meal_class MealClassInstance;

        public Dashboard(string username, string role)
        {
            InitializeComponent();
            this.username = username;
            this.role = role;
            ChickenClassInstance = new chicken_class(item, prices, item_price, qty, chickenGrid, subTotal, actual_subtotal);
            ChickenClassInstance.InitializeGrid(chickenGrid);
            ChickenClassInstance.ChickenAdd();
            PizzaClassInstance = new pizza_class(item, prices, item_price, qty, pizzaGrid, subTotal, actual_subtotal);
            PizzaClassInstance.InitializeGrid(pizzaGrid);
            PizzaClassInstance.PizzaAdd();
            DrinksClassInstance = new drinks_class(item, prices, item_price, qty, drinksGrid, subTotal, actual_subtotal);
            DrinksClassInstance.InitializeGrid(drinksGrid);
            DrinksClassInstance.DrinksAdd();
            MealClassInstance = new meal_class(item, prices, item_price, qty, mealGrid, subTotal, actual_subtotal);
            MealClassInstance.InitializeGrid(mealGrid);
            MealClassInstance.MealAdd();
        }

        private void UpdateSubTotal()
        {
            int total = 0;

            foreach (var priceString in prices.Items)
            {
                if (int.TryParse(priceString.ToString(), out int price))
                {
                    total += price;
                }
            }

            // Format the total with a comma if it's over 1000 and display with ".00" at the end
            string formattedTotal = total >= 1000 ? string.Format("{0:N2}", total) : total.ToString("N2");

            subTotal.Text = formattedTotal; // Update the main subtotal TextBox
            actual_subtotal.Text = formattedTotal; // Update the additional subtotal TextBox

            subTotal.IsReadOnly = true;
            actual_subtotal.IsReadOnly = true;
        }

        private void subTotal_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Your event handler logic here
        }

        private void home_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityAndZIndex(Visibility.Visible, 1, home_gif);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, checkout_txt, column_name, subT, disc, tot, rectangle, rectangle_tot, search_icon);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, actual_subtotal, subTotal, search_bar, pwd_disc, senior_disc);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, qty, item, prices, item_price, disc20_1, disc20_2);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, add, add_PZ, add_DK, add_ML, trash, order, cash, cash_amount);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, minus, minus_PZ, minus_DK, minus_ML);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, ck1, ck2, ck3, ck4, ck5, ck6, chicken_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, pz1, pz2, pz3, pz4, pz5, pz6, pizza_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, dk1, dk2, dk3, dk4, dk5, dk6, drinks_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, ml1, ml2, ml3, ml4, ml5, ml6, meal_panel);
        }

        private void Chicken_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityAndZIndex(Visibility.Visible, 1, ck1, ck2, ck3, ck4, ck5, ck6, chicken_panel);
            SetVisibilityAndZIndex(Visibility.Visible, 1, checkout_txt, column_name, subT, disc, tot, rectangle, rectangle_tot, search_icon);
            SetVisibilityAndZIndex(Visibility.Visible, 1, actual_subtotal, subTotal, search_bar, pwd_disc, senior_disc);
            SetVisibilityAndZIndex(Visibility.Visible, 1, qty, item, prices, item_price);
            SetVisibilityAndZIndex(Visibility.Visible, 1, add, minus, trash, order, cash, cash_amount);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, home_gif);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, pz1, pz2, pz3, pz4, pz5, pz6, pizza_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, dk1, dk2, dk3, dk4, dk5, dk6, drinks_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, ml1, ml2, ml3, ml4, ml5, ml6, meal_panel);
            chicken_class myChickenClass = new chicken_class(item, prices, item_price, qty, chickenGrid, subTotal, actual_subtotal);
            myChickenClass.ChickenAdd();
            myChickenClass.ChickenAdd2();
            myChickenClass.ChickenAdd3();
            myChickenClass.ChickenAdd4();
            myChickenClass.ChickenAdd5();
            myChickenClass.ChickenAdd6();
        }

        private void Pizza_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityAndZIndex(Visibility.Visible, 1, pz1, pz2, pz3, pz4, pz5, pz6, pizza_panel);
            SetVisibilityAndZIndex(Visibility.Visible, 1, checkout_txt, column_name, subT, disc, tot, rectangle, rectangle_tot, search_icon);
            SetVisibilityAndZIndex(Visibility.Visible, 1, actual_subtotal, subTotal, search_bar, pwd_disc, senior_disc);
            SetVisibilityAndZIndex(Visibility.Visible, 1, qty, item, prices, item_price);
            SetVisibilityAndZIndex(Visibility.Visible, 1, add_PZ, minus_PZ, trash, order, cash, cash_amount);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, home_gif);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, ck1, ck2, ck3, ck4, ck5, ck6, chicken_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, dk1, dk2, dk3, dk4, dk5, dk6, drinks_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, ml1, ml2, ml3, ml4, ml5, ml6, meal_panel);
            pizza_class myPizzaClass = new pizza_class(item, prices, item_price, qty, pizzaGrid, subTotal, actual_subtotal);
            myPizzaClass.PizzaAdd();
            myPizzaClass.PizzaAdd2();
            myPizzaClass.PizzaAdd3();
            myPizzaClass.PizzaAdd4();
            myPizzaClass.PizzaAdd5();
            myPizzaClass.PizzaAdd6();
        }

        private void Drinks_Click_1(object sender, RoutedEventArgs e)
        {
            SetVisibilityAndZIndex(Visibility.Visible, 1, dk1, dk2, dk3, dk4, dk5, dk6, drinks_panel);
            SetVisibilityAndZIndex(Visibility.Visible, 1, checkout_txt, column_name, subT, disc, tot, rectangle, rectangle_tot, search_icon);
            SetVisibilityAndZIndex(Visibility.Visible, 1, actual_subtotal, subTotal, search_bar, pwd_disc, senior_disc);
            SetVisibilityAndZIndex(Visibility.Visible, 1, qty, item, prices, item_price);
            SetVisibilityAndZIndex(Visibility.Visible, 1, add_DK, minus_DK, trash, order, cash, cash_amount);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, home_gif);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, ck1, ck2, ck3, ck4, ck5, ck6, chicken_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, pz1, pz2, pz3, pz4, pz5, pz6, pizza_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, ml1, ml2, ml3, ml4, ml5, ml6, meal_panel);
            drinks_class myDrinksClass = new drinks_class(item, prices, item_price, qty, drinksGrid, subTotal, actual_subtotal);
            myDrinksClass.DrinksAdd();
            myDrinksClass.DrinksAdd2();
            myDrinksClass.DrinksAdd3();
            myDrinksClass.DrinksAdd4();
            myDrinksClass.DrinksAdd5();
            myDrinksClass.DrinksAdd6();
        }

        private void Meal_Click(object sender, RoutedEventArgs e)
        {
            SetVisibilityAndZIndex(Visibility.Visible, 1, ml1, ml2, ml3, ml4, ml5, ml6, meal_panel);
            SetVisibilityAndZIndex(Visibility.Visible, 1, checkout_txt, column_name, subT, disc, tot, rectangle, rectangle_tot, search_icon);
            SetVisibilityAndZIndex(Visibility.Visible, 1, actual_subtotal, subTotal, search_bar, pwd_disc, senior_disc);
            SetVisibilityAndZIndex(Visibility.Visible, 1, qty, item, prices, item_price);
            SetVisibilityAndZIndex(Visibility.Visible, 1, add_ML, minus_ML, trash, order, cash, cash_amount);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, home_gif);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, ck1, ck2, ck3, ck4, ck5, ck6, chicken_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, pz1, pz2, pz3, pz4, pz5, pz6, pizza_panel);
            SetVisibilityAndZIndex(Visibility.Collapsed, 0, dk1, dk2, dk3, dk4, dk5, dk6, drinks_panel);
            meal_class myMealClass = new meal_class(item, prices, item_price, qty, mealGrid, subTotal, actual_subtotal);
            myMealClass.MealAdd();
            myMealClass.MealAdd2();
            myMealClass.MealAdd3();
            myMealClass.MealAdd4();
            myMealClass.MealAdd5();
            myMealClass.MealAdd6();
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

        private void item_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Check if any item is selected in the ListBox
            if (item.SelectedItem != null)
            {
                string? selectedValue = item.SelectedItem.ToString();

                if (selectedValue != null && selectedValue.StartsWith("CK"))
                {
                    SetVisibilityAndZIndex(Visibility.Visible, 1, add, minus, trash);
                    SetVisibilityAndZIndex(Visibility.Collapsed, 0, add_PZ, minus_PZ, add_DK, minus_DK, add_ML, minus_ML);

                }
                else if (selectedValue != null && selectedValue.StartsWith("PZ"))
                {
                    SetVisibilityAndZIndex(Visibility.Visible, 1, add_PZ, minus_PZ, trash);
                    SetVisibilityAndZIndex(Visibility.Collapsed, 0, add, minus, add_DK, minus_DK, add_ML, minus_ML);
                }
                else if (selectedValue != null && selectedValue.StartsWith("DK"))
                {
                    SetVisibilityAndZIndex(Visibility.Visible, 1, add_DK, minus_DK, trash);
                    SetVisibilityAndZIndex(Visibility.Collapsed, 0, add_PZ, minus_PZ, add, minus, add_ML, minus_ML);
                }
                else if (selectedValue != null && selectedValue.StartsWith("ML"))
                {
                    SetVisibilityAndZIndex(Visibility.Visible, 1, add_ML, minus_ML, trash);
                    SetVisibilityAndZIndex(Visibility.Collapsed, 0, add_PZ, minus_PZ, add, minus, add_DK, minus_DK);
                }
            }
            else
            {
                trash.Visibility = Visibility.Collapsed;
                add.Visibility = Visibility.Collapsed;
                minus.Visibility = Visibility.Collapsed;
                add_PZ.Visibility = Visibility.Collapsed;
                minus_PZ.Visibility = Visibility.Collapsed;
                add_DK.Visibility = Visibility.Collapsed;
                minus_DK.Visibility = Visibility.Collapsed;
                add_ML.Visibility = Visibility.Collapsed;
                minus_ML.Visibility = Visibility.Collapsed;
            }
        }

        private void back_btn_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }

        #region
        private void add_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                int selectedIndex = item.SelectedIndex;
                if (selectedIndex == -1)
                {
                    MessageBox.Show("Please choose the item for which you'd like to increase the quantity.", "Error");
                    return;
                }

                if (qty.Items.Count > selectedIndex && qty.Items[selectedIndex] is string selectedQtyString)
                {
                    // Get the selected item from the "item" ListBox
                    string? selectedItem = item.SelectedItem.ToString();

                    // Fetch the ChickenPrice from the database based on the selected item
                    string query = "SELECT ChickenPrice FROM price WHERE MenuForChicken = @SelectedItem";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SelectedItem", selectedItem);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int chickenPrice = reader.GetInt32(0);

                            if (prices.Items.Count > selectedIndex && prices.Items[selectedIndex] is string originalPriceString)
                            {
                                if (int.TryParse(originalPriceString, out int originalPrice))
                                {
                                    // Increment the quantity by 1
                                    int newQuantity = int.Parse(selectedQtyString) + 1;
                                    qty.Items[selectedIndex] = newQuantity.ToString();

                                    // Add the fetched ChickenPrice to the originalPrice
                                    int newPrice = originalPrice + chickenPrice;
                                    prices.Items[selectedIndex] = newPrice.ToString();
                                    UpdateSubTotal();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void minus_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = item.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Please choose the item for which you'd like to decrease the quantity.", "Error");
                return;
            }

            if (qty.Items.Count > selectedIndex && qty.Items[selectedIndex] is string selectedQtyString)
            {
                if (int.TryParse(selectedQtyString, out int selectedQty) && selectedQty > 1)
                {
                    selectedQty--;
                    qty.Items[selectedIndex] = selectedQty.ToString();

                    using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=dashboard;User=root;Password=;"))
                    {
                        connection.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ChickenPrice FROM price WHERE MenuForChicken = @SelectedItem", connection))
                        {
                            string? selectedItem = item.SelectedItem.ToString();
                            cmd.Parameters.AddWithValue("@SelectedItem", selectedItem);

                            int chickenPrice = Convert.ToInt32(cmd.ExecuteScalar());

                            // Subtract the ChickenPrice from the selected price and update the "prices" ListBox
                            int selectedPrice = Convert.ToInt32(prices.Items[selectedIndex].ToString());
                            selectedPrice -= chickenPrice;
                            prices.Items[selectedIndex] = selectedPrice.ToString();
                            UpdateSubTotal();
                        }
                    }
                }
                else
                {
                    // Display an error message if the quantity is already at the minimum (1)
                    MessageBox.Show("You have reached the minimum quantity.", "Error");
                }
            }
        }

        private void trash_Click(object sender, RoutedEventArgs e)
        {
            bool showError = true;

            if (username == "admin" || username == "manager")
            {
                int selectedIndex = item.SelectedIndex;
                if (item.SelectedItem == null)
                {
                    MessageBox.Show("Please choose the item for which you'd like to void.", "Error");
                }
                if (item.SelectedItem != null)
                {
                    if (selectedIndex >= 0 && selectedIndex < item.Items.Count)
                    {
                        item.Items.RemoveAt(selectedIndex);
                        qty.Items.RemoveAt(selectedIndex);
                        prices.Items.RemoveAt(selectedIndex);
                        item_price.Items.RemoveAt(selectedIndex);
                        MessageBox.Show("Item has been deleted", "Item Delete");
                        UpdateSubTotal();
                        showError = false;
                    }
                }
            }
            else if (username == "staff")
            {
                int selectedIndex = item.SelectedIndex;
                if (item.SelectedItem == null)
                {
                    MessageBox.Show("Please choose the item for which you'd like to void.", "Error");
                }
                if (item.SelectedItem != null)
                {
                    MessageBox.Show("You need manager's permission to delete this item.", "Permission Required");

                    ManagerPermission managerPermission = new ManagerPermission();
                    if (managerPermission.ShowDialog() == true)
                    {
                        if (item.SelectedItem != null)
                        {


                            if (selectedIndex >= 0 && selectedIndex < item.Items.Count)
                            {
                                item.Items.RemoveAt(selectedIndex);
                                qty.Items.RemoveAt(selectedIndex);
                                prices.Items.RemoveAt(selectedIndex);
                                item_price.Items.RemoveAt(selectedIndex);
                                MessageBox.Show("Item has been deleted", "Item Delete");
                                UpdateSubTotal();
                                showError = false;
                            }
                        }
                    }
                }
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
                            int selectedIndex = item.SelectedIndex;
                            if (item.SelectedItem == null)
                            {
                                MessageBox.Show("Please choose the item for which you'd like to void.", "Error");
                            }
                            if (item.SelectedItem != null)
                            {

                                if (selectedIndex >= 0 && selectedIndex < item.Items.Count)
                                {
                                    item.Items.RemoveAt(selectedIndex);
                                    qty.Items.RemoveAt(selectedIndex);
                                    prices.Items.RemoveAt(selectedIndex);
                                    item_price.Items.RemoveAt(selectedIndex);
                                    MessageBox.Show("Item has been deleted", "Item Delete");
                                    UpdateSubTotal();
                                    showError = false;
                                }
                            }
                        }
                        else if (role == "Staff")
                        {
                            int selectedIndex = item.SelectedIndex;
                            if (item.SelectedItem == null)
                            {
                                MessageBox.Show("Please choose the item for which you'd like to void.", "Error");
                            }

                            if (item.SelectedItem != null)
                            {
                                MessageBox.Show("You need manager's permission to delete this item.", "Permission Required");

                                ManagerPermission managerPermission = new ManagerPermission();
                                if (managerPermission.ShowDialog() == true)
                                {
                                    if (item.SelectedItem != null)
                                    {
                                        if (selectedIndex >= 0 && selectedIndex < item.Items.Count)
                                        {
                                            item.Items.RemoveAt(selectedIndex);
                                            qty.Items.RemoveAt(selectedIndex);
                                            prices.Items.RemoveAt(selectedIndex);
                                            item_price.Items.RemoveAt(selectedIndex);
                                            MessageBox.Show("Item has been deleted", "Item Delete");
                                            UpdateSubTotal();
                                            showError = false;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("You are not authorized to delete items.", "Access Denied");
                            showError = false;
                        }
                    }
                }
            }
        }

        private void ck1_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 1";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForChicken = reader["MenuForChicken"]?.ToString() ?? "DefaultMenuForChicken";
                            string chickenPrice = reader["ChickenPrice"]?.ToString() ?? "DefaultChickenPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForChicken))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForChicken);
                                prices.Items.Add(chickenPrice);
                                item_price.Items.Add(chickenPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ck2_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 2";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForChicken = reader["MenuForChicken"]?.ToString() ?? "DefaultMenuForChicken";
                            string chickenPrice = reader["ChickenPrice"]?.ToString() ?? "DefaultChickenPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForChicken))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForChicken);
                                prices.Items.Add(chickenPrice);
                                item_price.Items.Add(chickenPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ck3_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 3";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForChicken = reader["MenuForChicken"]?.ToString() ?? "DefaultMenuForChicken";
                            string chickenPrice = reader["ChickenPrice"]?.ToString() ?? "DefaultChickenPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForChicken))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForChicken);
                                prices.Items.Add(chickenPrice);
                                item_price.Items.Add(chickenPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ck4_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 4";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForChicken = reader["MenuForChicken"]?.ToString() ?? "DefaultMenuForChicken";
                            string chickenPrice = reader["ChickenPrice"]?.ToString() ?? "DefaultChickenPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForChicken))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForChicken);
                                prices.Items.Add(chickenPrice);
                                item_price.Items.Add(chickenPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ck5_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 5";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForChicken = reader["MenuForChicken"]?.ToString() ?? "DefaultMenuForChicken";
                            string chickenPrice = reader["ChickenPrice"]?.ToString() ?? "DefaultChickenPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForChicken))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForChicken);
                                prices.Items.Add(chickenPrice);
                                item_price.Items.Add(chickenPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ck6_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 6";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForChicken = reader["MenuForChicken"]?.ToString() ?? "DefaultMenuForChicken";
                            string chickenPrice = reader["ChickenPrice"]?.ToString() ?? "DefaultChickenPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForChicken))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForChicken);
                                prices.Items.Add(chickenPrice);
                                item_price.Items.Add(chickenPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void add_PZ_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                int selectedIndex = item.SelectedIndex;
                if (selectedIndex == -1)
                {
                    MessageBox.Show("Please choose the item for which you'd like to increase the quantity.", "Error");
                    return;
                }

                if (qty.Items.Count > selectedIndex && qty.Items[selectedIndex] is string selectedQtyString)
                {
                    // Get the selected item from the "item" ListBox
                    string? selectedItem = item.SelectedItem.ToString();

                    // Fetch the PizzaPrice from the database based on the selected item
                    string query = "SELECT PizzaPrice FROM price WHERE MenuForPizza = @SelectedItem";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SelectedItem", selectedItem);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int pizzaPrice = reader.GetInt32(0);

                            if (prices.Items.Count > selectedIndex && prices.Items[selectedIndex] is string originalPriceString)
                            {
                                if (int.TryParse(originalPriceString, out int originalPrice))
                                {
                                    // Increment the quantity by 1
                                    int newQuantity = int.Parse(selectedQtyString) + 1;
                                    qty.Items[selectedIndex] = newQuantity.ToString();

                                    // Add the fetched ChickenPrice to the originalPrice
                                    int newPrice = originalPrice + pizzaPrice;
                                    prices.Items[selectedIndex] = newPrice.ToString();
                                    UpdateSubTotal();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void minus_PZ_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = item.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Please choose the item for which you'd like to decrease the quantity.", "Error");
                return;
            }

            if (qty.Items.Count > selectedIndex && qty.Items[selectedIndex] is string selectedQtyString)
            {
                if (int.TryParse(selectedQtyString, out int selectedQty) && selectedQty > 1)
                {
                    selectedQty--;
                    qty.Items[selectedIndex] = selectedQty.ToString();

                    using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=dashboard;User=root;Password=;"))
                    {
                        connection.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT PizzaPrice FROM price WHERE MenuForPizza = @SelectedItem", connection))
                        {
                            string? selectedItem = item.SelectedItem.ToString();
                            cmd.Parameters.AddWithValue("@SelectedItem", selectedItem);

                            int pizzaPrice = Convert.ToInt32(cmd.ExecuteScalar());

                            // Subtract the PizzaPrice from the selected price and update the "prices" ListBox
                            int selectedPrice = Convert.ToInt32(prices.Items[selectedIndex].ToString());
                            selectedPrice -= pizzaPrice;
                            prices.Items[selectedIndex] = selectedPrice.ToString();
                            UpdateSubTotal();
                        }
                    }
                }
                else
                {
                    // Display an error message if the quantity is already at the minimum (1)
                    MessageBox.Show("You have reached the minimum quantity.", "Error");
                }
            }
        }

        private void pz1_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 1";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForPizza))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void pz2_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 2";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForPizza))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void pz3_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 3";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForPizza))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void pz4_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 4";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForPizza))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void pz5_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 5";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForPizza))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void pz6_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 6";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForPizza))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void add_DK_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                int selectedIndex = item.SelectedIndex;
                if (selectedIndex == -1)
                {
                    MessageBox.Show("Please choose the item for which you'd like to increase the quantity.", "Error");
                    return;
                }

                if (qty.Items.Count > selectedIndex && qty.Items[selectedIndex] is string selectedQtyString)
                {
                    // Get the selected item from the "item" ListBox
                    string? selectedItem = item.SelectedItem.ToString();

                    // Fetch the DrinksPrice from the database based on the selected item
                    string query = "SELECT DrinksPrice FROM price WHERE MenuForDrinks = @SelectedItem";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SelectedItem", selectedItem);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int drinksPrice = reader.GetInt32(0);

                            if (prices.Items.Count > selectedIndex && prices.Items[selectedIndex] is string originalPriceString)
                            {
                                if (int.TryParse(originalPriceString, out int originalPrice))
                                {
                                    // Increment the quantity by 1
                                    int newQuantity = int.Parse(selectedQtyString) + 1;
                                    qty.Items[selectedIndex] = newQuantity.ToString();

                                    // Add the fetched ChickenPrice to the originalPrice
                                    int newPrice = originalPrice + drinksPrice;
                                    prices.Items[selectedIndex] = newPrice.ToString();
                                    UpdateSubTotal();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void minus_DK_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = item.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Please choose the item for which you'd like to decrease the quantity.", "Error");
                return;
            }

            if (qty.Items.Count > selectedIndex && qty.Items[selectedIndex] is string selectedQtyString)
            {
                if (int.TryParse(selectedQtyString, out int selectedQty) && selectedQty > 1)
                {
                    selectedQty--;
                    qty.Items[selectedIndex] = selectedQty.ToString();

                    using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=dashboard;User=root;Password=;"))
                    {
                        connection.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT DrinksPrice FROM price WHERE MenuForDrinks = @SelectedItem", connection))
                        {
                            string? selectedItem = item.SelectedItem.ToString();
                            cmd.Parameters.AddWithValue("@SelectedItem", selectedItem);

                            int drinksPrice = Convert.ToInt32(cmd.ExecuteScalar());

                            // Subtract the DrinksPrice from the selected price and update the "prices" ListBox
                            int selectedPrice = Convert.ToInt32(prices.Items[selectedIndex].ToString());
                            selectedPrice -= drinksPrice;
                            prices.Items[selectedIndex] = selectedPrice.ToString();
                            UpdateSubTotal();
                        }
                    }
                }
                else
                {
                    // Display an error message if the quantity is already at the minimum (1)
                    MessageBox.Show("You have reached the minimum quantity.", "Error");
                }
            }
        }

        private void dk1_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForDrinks, DrinksPrice FROM price WHERE RowNumber = 1";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForDrinks = reader["MenuForDrinks"]?.ToString() ?? "DefaultMenuForDrinks";
                            string drinksPrice = reader["DrinksPrice"]?.ToString() ?? "DefaultDrinksPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForDrinks))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForDrinks);
                                prices.Items.Add(drinksPrice);
                                item_price.Items.Add(drinksPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void dk2_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForDrinks, DrinksPrice FROM price WHERE RowNumber = 2";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForDrinks = reader["MenuForDrinks"]?.ToString() ?? "DefaultMenuForDrinks";
                            string drinksPrice = reader["DrinksPrice"]?.ToString() ?? "DefaultDrinksPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForDrinks))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForDrinks);
                                prices.Items.Add(drinksPrice);
                                item_price.Items.Add(drinksPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void dk3_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForDrinks, DrinksPrice FROM price WHERE RowNumber = 3";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForDrinks = reader["MenuForDrinks"]?.ToString() ?? "DefaultMenuForDrinks";
                            string drinksPrice = reader["DrinksPrice"]?.ToString() ?? "DefaultDrinksPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForDrinks))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForDrinks);
                                prices.Items.Add(drinksPrice);
                                item_price.Items.Add(drinksPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void dk4_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForDrinks, DrinksPrice FROM price WHERE RowNumber = 4";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForDrinks = reader["MenuForDrinks"]?.ToString() ?? "DefaultMenuForDrinks";
                            string drinksPrice = reader["DrinksPrice"]?.ToString() ?? "DefaultDrinksPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForDrinks))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForDrinks);
                                prices.Items.Add(drinksPrice);
                                item_price.Items.Add(drinksPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void dk5_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForDrinks, DrinksPrice FROM price WHERE RowNumber = 5";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForDrinks = reader["MenuForDrinks"]?.ToString() ?? "DefaultMenuForDrinks";
                            string drinksPrice = reader["DrinksPrice"]?.ToString() ?? "DefaultDrinksPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForDrinks))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForDrinks);
                                prices.Items.Add(drinksPrice);
                                item_price.Items.Add(drinksPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void dk6_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForDrinks, DrinksPrice FROM price WHERE RowNumber = 6";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForDrinks = reader["MenuForDrinks"]?.ToString() ?? "DefaultMenuForDrinks";
                            string drinksPrice = reader["DrinksPrice"]?.ToString() ?? "DefaultDrinksPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForDrinks))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForDrinks);
                                prices.Items.Add(drinksPrice);
                                item_price.Items.Add(drinksPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void add_ML_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                int selectedIndex = item.SelectedIndex;
                if (selectedIndex == -1)
                {
                    MessageBox.Show("Please choose the item for which you'd like to increase the quantity.", "Error");
                    return;
                }

                if (qty.Items.Count > selectedIndex && qty.Items[selectedIndex] is string selectedQtyString)
                {
                    // Get the selected item from the "item" ListBox
                    string? selectedItem = item.SelectedItem.ToString();

                    // Fetch the MealPrice from the database based on the selected item
                    string query = "SELECT MealPrice FROM price WHERE MenuForMeals = @SelectedItem";
                    MySqlCommand command = new MySqlCommand(query, connection);
                    command.Parameters.AddWithValue("@SelectedItem", selectedItem);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int mealPrice = reader.GetInt32(0);

                            if (prices.Items.Count > selectedIndex && prices.Items[selectedIndex] is string originalPriceString)
                            {
                                if (int.TryParse(originalPriceString, out int originalPrice))
                                {
                                    // Increment the quantity by 1
                                    int newQuantity = int.Parse(selectedQtyString) + 1;
                                    qty.Items[selectedIndex] = newQuantity.ToString();

                                    // Add the fetched ChickenPrice to the originalPrice
                                    int newPrice = originalPrice + mealPrice;
                                    prices.Items[selectedIndex] = newPrice.ToString();
                                    UpdateSubTotal();
                                }
                            }
                        }
                    }
                }
            }
        }

        private void minus_ML_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = item.SelectedIndex;

            if (selectedIndex == -1)
            {
                MessageBox.Show("Please choose the item for which you'd like to decrease the quantity.", "Error");
                return;
            }

            if (qty.Items.Count > selectedIndex && qty.Items[selectedIndex] is string selectedQtyString)
            {
                if (int.TryParse(selectedQtyString, out int selectedQty) && selectedQty > 1)
                {
                    selectedQty--;
                    qty.Items[selectedIndex] = selectedQty.ToString();

                    using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=dashboard;User=root;Password=;"))
                    {
                        connection.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT MealPrice FROM price WHERE MenuForMeals = @SelectedItem", connection))
                        {
                            string? selectedItem = item.SelectedItem.ToString();
                            cmd.Parameters.AddWithValue("@SelectedItem", selectedItem);

                            int mealPrice = Convert.ToInt32(cmd.ExecuteScalar());

                            // Subtract the MealPrice from the selected price and update the "prices" ListBox
                            int selectedPrice = Convert.ToInt32(prices.Items[selectedIndex].ToString());
                            selectedPrice -= mealPrice;
                            prices.Items[selectedIndex] = selectedPrice.ToString();
                            UpdateSubTotal();
                        }
                    }
                }
                else
                {
                    // Display an error message if the quantity is already at the minimum (1)
                    MessageBox.Show("You have reached the minimum quantity.", "Error");
                }
            }
        }

        private void ml1_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForMeals, MealPrice FROM price WHERE RowNumber = 1";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForMeals = reader["MenuForMeals"]?.ToString() ?? "DefaultMenuForMeals";
                            string mealPrice = reader["MealPrice"]?.ToString() ?? "DefaultMealPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForMeals))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForMeals);
                                prices.Items.Add(mealPrice);
                                item_price.Items.Add(mealPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ml2_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForMeals, MealPrice FROM price WHERE RowNumber = 2";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForMeals = reader["MenuForMeals"]?.ToString() ?? "DefaultMenuForMeals";
                            string mealPrice = reader["MealPrice"]?.ToString() ?? "DefaultMealPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForMeals))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForMeals);
                                prices.Items.Add(mealPrice);
                                item_price.Items.Add(mealPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ml3_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForMeals, MealPrice FROM price WHERE RowNumber = 3";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForMeals = reader["MenuForMeals"]?.ToString() ?? "DefaultMenuForMeals";
                            string mealPrice = reader["MealPrice"]?.ToString() ?? "DefaultMealPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForMeals))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForMeals);
                                prices.Items.Add(mealPrice);
                                item_price.Items.Add(mealPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ml4_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForMeals, MealPrice FROM price WHERE RowNumber = 4";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForMeals = reader["MenuForMeals"]?.ToString() ?? "DefaultMenuForMeals";
                            string mealPrice = reader["MealPrice"]?.ToString() ?? "DefaultMealPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForMeals))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForMeals);
                                prices.Items.Add(mealPrice);
                                item_price.Items.Add(mealPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ml5_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForMeals, MealPrice FROM price WHERE RowNumber = 5";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForMeals = reader["MenuForMeals"]?.ToString() ?? "DefaultMenuForMeals";
                            string mealPrice = reader["MealPrice"]?.ToString() ?? "DefaultMealPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForMeals))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForMeals);
                                prices.Items.Add(mealPrice);
                                item_price.Items.Add(mealPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }

        private void ml6_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForMeals, MealPrice FROM price WHERE RowNumber = 6";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Retrieve the values from the database
                            string menuForMeals = reader["MenuForMeals"]?.ToString() ?? "DefaultMenuForMeals";
                            string mealPrice = reader["MealPrice"]?.ToString() ?? "DefaultMealPrice";

                            // Check if the item already exists in the list boxes
                            if (!item.Items.Contains(menuForMeals))
                            {
                                // Display the values in the list boxes
                                item.Items.Add(menuForMeals);
                                prices.Items.Add(mealPrice);
                                item_price.Items.Add(mealPrice);
                                UpdateSubTotal();

                                // Set qty list box to "1"
                                qty.Items.Add("1");
                            }
                            else
                            {
                                MessageBox.Show("Item has already been added.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
        }
        #endregion

        private void order_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                MessageBox.Show("Username is not available. Please log in again.", "Error");
                return;
            }
            decimal subTotalValue;
            if (!decimal.TryParse(subTotal.Text, out subTotalValue) || subTotalValue <= 0)
            {
                MessageBox.Show("There are currently no items being selected for purchase.", "Error");
                return;
            }
            if (string.IsNullOrWhiteSpace(cash_amount.Text))
            {
                MessageBox.Show("Please enter the cash amount.", "Error");
                return;
            }
            decimal cashAmount;
            if (!decimal.TryParse(cash_amount.Text, out cashAmount))
            {
                MessageBox.Show("Invalid Cash Amount input.", "Error");
                return;
            }
            if (cashAmount < subTotalValue)
            {
                MessageBox.Show("The available cash amount is insufficient to cover the total cost of the selected items.", "Error");
                return;
            }
            // Generate an order code
            string orderCode = GenerateOrderCode();

            // Calculate the overall quantity from the "qty" ListBox
            int overallQuantity = CalculateOverallQuantity();

            if (overallQuantity < 0)
            {
                MessageBox.Show("Error calculating the overall quantity.", "Error");
                return;
            }

            // Get the value from the "subTotal" TextBox
            decimal totalPrice;
            if (!decimal.TryParse(subTotal.Text, out totalPrice))
            {
                MessageBox.Show("Invalid Total Price input.", "Error");
                return;
            }

            // Connect to the "dashboard" database
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Insert data into the "void" table
                string insertQuery = "INSERT INTO void (Username, Overall_QTY, Order_Code, Total_Price, Status) " +
                                     "VALUES (@Username, @Overall_QTY, @Order_Code, @Total_Price, @Status)";

                using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                {
                    insertCommand.Parameters.AddWithValue("@Username", username);
                    insertCommand.Parameters.AddWithValue("@Overall_QTY", overallQuantity);
                    insertCommand.Parameters.AddWithValue("@Order_Code", orderCode);
                    insertCommand.Parameters.AddWithValue("@Total_Price", totalPrice);
                    insertCommand.Parameters.AddWithValue("@Status", "Ordered");

                    int rowsAffected = insertCommand.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Your order has been placed. Thank you for ordering in our shop!", "Success");
                        // Call UpdateTotalSalesTable to update sales data and clear the fields
                        UpdateTotalSalesTable(connection);
                        AddReceiptTable(connection, orderCode);

                        // Clear all items in the "qty" ListBox
                        qty.Items.Clear();
                        item.Items.Clear();
                        prices.Items.Clear();
                        item_price.Items.Clear();
                        cash_amount.Text = string.Empty;
                        actual_subtotal.Text = string.Empty;
                        subTotal.Text = string.Empty;
                        pwd_disc.IsChecked = false;
                        senior_disc.IsChecked = false;
                        SetVisibilityAndZIndex(Visibility.Collapsed, 0, disc20_1, disc20_2);
                        PrintReceipt printReceipt = new PrintReceipt(username, orderCode, role);
                        printReceipt.Show();

                    }
                    else
                    {
                        MessageBox.Show("Failed to submit the order.", "Error");
                    }
                }
            }
        }

        private void UpdateTotalSalesTable(MySqlConnection connection)
        {
            // Iterate through items in the "item" ListBox
            for (int i = 0; i < item.Items.Count; i++)
            {
                string currentItem = item.Items[i]?.ToString(); // Use null-conditional operator

                if (!string.IsNullOrWhiteSpace(currentItem))
                {
                    if (int.TryParse(qty.Items[i]?.ToString(), out int quantity) &&
                        decimal.TryParse(prices.Items[i]?.ToString(), out decimal price))
                    {
                        // Attempt to update existing rows in the "total_sales" table in the "sales" database
                        if (TryUpdateTotalSalesRow(connection, currentItem, quantity, price))
                        {
                            // Row updated successfully
                        }
                        else
                        {
                            // If the item is not found in the "total_sales" table, add a new row
                            AddNewTotalSalesRow(connection, currentItem, quantity, price);
                        }
                    }
                    else
                    {
                        // Handle parsing errors if necessary.
                        Console.WriteLine("Failed to parse quantity or price for item: " + currentItem);
                    }
                }
                else
                {
                    // Handle cases where currentItem is null or empty.
                    Console.WriteLine("Current item is null or empty.");
                }
            }
        }

        private bool TryUpdateTotalSalesRow(MySqlConnection connection, string item, int quantity, decimal price)
        {
            // Attempt to update the "total_sales" table in the "sales" database for the given item
            using (MySqlCommand updateCommand = new MySqlCommand("UPDATE sales.total_sales SET Sales = Sales + @quantity, Revenue = Revenue + @revenue WHERE Items = @item", connection))
            {
                updateCommand.Parameters.AddWithValue("@quantity", quantity);
                updateCommand.Parameters.AddWithValue("@revenue", price); // Add the price to the existing revenue
                updateCommand.Parameters.AddWithValue("@item", item);

                int rowsAffected = updateCommand.ExecuteNonQuery();

                return rowsAffected > 0; // If rows were affected, the update was successful

            }
        }

        private void AddNewTotalSalesRow(MySqlConnection connection, string item, int quantity, decimal price)
        {
            // Add a new row to the "total_sales" table in the "sales" database
            using (MySqlCommand insertCommand = new MySqlCommand("INSERT INTO sales.total_sales (Items, Sales, Revenue) VALUES (@item, @quantity, @revenue)", connection))
            {
                insertCommand.Parameters.AddWithValue("@item", item);
                insertCommand.Parameters.AddWithValue("@quantity", quantity);
                insertCommand.Parameters.AddWithValue("@revenue", price); // Set the initial revenue as the price

                int rowsAffected = insertCommand.ExecuteNonQuery();

                // Handle potential errors or success accordingly
            }
        }

        private void AddReceiptTable(MySqlConnection connection, string orderCode)
        {
            if (!string.IsNullOrWhiteSpace(username))
            {
                decimal total = decimal.TryParse(subTotal.Text, out decimal totalValue) ? totalValue : 0;
                decimal subtotal = decimal.TryParse(actual_subtotal.Text, out decimal subtotalValue) ? subtotalValue : 0;
                decimal cashAmount = decimal.TryParse(cash_amount.Text, out decimal cashValue) ? cashValue : 0;
                string discount;
                if (pwd_disc.IsChecked == true && senior_disc.IsChecked == true)
                {
                    discount = "PWD ID (-20%) and Senior ID (-20%)";
                }
                else if (pwd_disc.IsChecked == true)
                {
                    discount = "PWD ID (-20%)";
                }
                else if (senior_disc.IsChecked == true)
                {
                    discount = "Senior ID (-20%)";
                }
                else
                {
                    discount = "No Discount";
                }

                using (MySqlCommand insertCommand = new MySqlCommand("INSERT INTO dashboard.receipt (OrderCode, QTY, Items, Price, Amount, Total, Subtotal, Cash, Discounted, Change_Money, Username) " +
                "VALUES (@OrderCode, @QTY, @Items, @Price, @Amount, @Total, @Subtotal, @Cash, @Discounted, @Change_Money, @Username)", connection))
                {
                    for (int i = 0; i < qty.Items.Count; i++)
                    {
                        decimal price = decimal.TryParse(item_price.Items[i]?.ToString(), out decimal priceValue) ? priceValue : 0;
                        decimal amount = decimal.TryParse(prices.Items[i]?.ToString(), out decimal amountValue) ? amountValue : 0;
                        int qtyValue = int.TryParse(qty.Items[i]?.ToString(), out int parsedQty) ? parsedQty : 0;

                        if (!string.IsNullOrWhiteSpace(item.Items[i]?.ToString()))
                        {
                            insertCommand.Parameters.Clear(); // Clear existing parameters
                            insertCommand.Parameters.AddWithValue("@OrderCode", orderCode); // Use the provided order code
                            insertCommand.Parameters.AddWithValue("@Items", item.Items[i]?.ToString());
                            insertCommand.Parameters.AddWithValue("@Price", price);
                            insertCommand.Parameters.AddWithValue("@Amount", amount);
                            insertCommand.Parameters.AddWithValue("@QTY", qtyValue);
                            insertCommand.Parameters.AddWithValue("@Total", total);
                            insertCommand.Parameters.AddWithValue("@Subtotal", subtotal);
                            insertCommand.Parameters.AddWithValue("@Cash", cashAmount); // Insert the cash amount
                            insertCommand.Parameters.AddWithValue("@Discounted", discount);

                            decimal changeMoney = cashAmount - total;
                            insertCommand.Parameters.AddWithValue("@Change_Money", changeMoney);
                            insertCommand.Parameters.AddWithValue("@Username", username);

                            int rowsAffected = insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        private int CalculateOverallQuantity()
        {
            int totalQuantity = 0;

            foreach (var qtyString in qty.Items)
            {
                if (int.TryParse(qtyString.ToString(), out int quantity))
                {
                    totalQuantity += quantity;
                }
            }

            return totalQuantity;
        }

        private string GenerateOrderCode()
        {
            // Generate a unique order code with a shorter alphanumeric identifier.
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string uniqueIdentifier = GenerateRandomAlphanumeric(8);
            string orderCode = "ORD-" + timestamp + "-" + uniqueIdentifier;

            return orderCode;
        }

        private string GenerateRandomAlphanumeric(int length)
        {
            const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = characters[random.Next(characters.Length)];
            }

            return new string(result);
        }

        private void Void_Click(object sender, RoutedEventArgs e)
        {
            OrderHistory orderHistory = new OrderHistory(username, role);
            orderHistory.Show();
            this.Close();
        }

        private void pwd_disc_Checked(object sender, RoutedEventArgs e)
        {
            if (pwd_disc.IsChecked == true)
            {
                ApplyDiscount(0.20);
                SetVisibilityAndZIndex(Visibility.Visible, 1, disc20_1);
                SetVisibilityAndZIndex(Visibility.Collapsed, 0, disc20_2);
                senior_disc.IsChecked = false; // Uncheck the other checkbox
            }
        }

        private void pwd_disc_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!senior_disc.IsChecked == true)
            {
                if (double.TryParse(actual_subtotal.Text, out double originalSubTotalValue))
                {
                    SetVisibilityAndZIndex(Visibility.Collapsed, 0, disc20_1);
                    subTotal.Text = originalSubTotalValue.ToString("N2");
                }
            }
        }

        private void senior_disc_Checked(object sender, RoutedEventArgs e)
        {
            if (senior_disc.IsChecked == true)
            {
                ApplyDiscount(0.20);
                SetVisibilityAndZIndex(Visibility.Visible, 1, disc20_2);
                SetVisibilityAndZIndex(Visibility.Collapsed, 0, disc20_1);
                pwd_disc.IsChecked = false; // Uncheck the other checkbox
            }
        }

        private void senior_disc_Unchecked(object sender, RoutedEventArgs e)
        {
            if (!pwd_disc.IsChecked == true)
            {
                if (double.TryParse(actual_subtotal.Text, out double originalSubTotalValue))
                {
                    SetVisibilityAndZIndex(Visibility.Collapsed, 0, disc20_2);
                    subTotal.Text = originalSubTotalValue.ToString("N2");
                }
            }
        }

        private void ApplyDiscount(double discountPercentage)
        {
            // Check if the "actual_subtotal" TextBox contains a valid numeric value
            if (double.TryParse(actual_subtotal.Text, out double actualSubTotalValue))
            {
                // Calculate the discounted value
                double discountedValue = actualSubTotalValue - (actualSubTotalValue * discountPercentage);

                // Update the "subTotal" TextBox with the discounted value
                subTotal.Text = discountedValue.ToString("N2");
            }
        }

        private void search_bar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Get the search text from the search bar
            string searchText = search_bar.Text.ToLower();

            // Loop through the buttons in each panel and filter based on search text
            FilterButtons(meal_panel, "ml", searchText);
            FilterButtons(drinks_panel, "dk", searchText);
            FilterButtons(pizza_panel, "pz", searchText);
            FilterButtons(chicken_panel, "ck", searchText);
        }

        private void FilterButtons(ScrollViewer panel, string buttonPrefix, string searchText)
        {
            // Loop through the children of the panel (assuming buttons are directly within the Grid)
            foreach (UIElement element in ((Grid)panel.Content).Children)
            {
                if (element is Button button)
                {
                    // Get the button name
                    string buttonName = button.Name.ToLower();

                    // Check if the button name contains the search text
                    if (buttonName.Contains(buttonPrefix) && buttonName.Contains(searchText))
                    {
                        // Show the button
                        button.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        // Hide the button
                        button.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }
    }
}