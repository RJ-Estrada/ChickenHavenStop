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
    /// Interaction logic for Edit_Price.xaml
    /// </summary>
    public partial class Edit_Price : Window
    {
        private string username;
        private string role;

        public Edit_Price(string? MenuForChickenText, string? ChickenPriceText, string? MenuForPizzaText, string? PizzaPriceText, string? MenuForDrinksText,
            string? DrinksPriceText, string? MenuForMealsText, string? MealPriceText, string username, string role)
        {
            InitializeComponent();

            // Initialize UI elements with the provided data
            InitializeUI(MenuForChickenText, ChickenPriceText, MenuForPizzaText, PizzaPriceText, MenuForDrinksText, DrinksPriceText, MenuForMealsText, MealPriceText);

            // Attach the Closed event handler
            Closed += Edit_Price_Closed;
            this.username = username;
            this.role = role;
        }

        private void Edit_Price_Closed(object sender, EventArgs e)
        {
            // Clear the TextBox controls
            item_name.Text = string.Empty;
            item_price.Text = string.Empty;
        }

        // Method to initialize and populate UI elements
        private void InitializeUI(string? MenuForChickenText, string? ChickenPriceText, string? MenuForPizzaText, string? PizzaPriceText,
                          string? MenuForDrinksText, string? DrinksPriceText, string? MenuForMealsText, string? MealPriceText)
        {
            string menuItems = "";
            string itemPrices = "";

            if (!string.IsNullOrEmpty(MenuForChickenText))
            {
                menuItems += MenuForChickenText + Environment.NewLine;
                itemPrices += ChickenPriceText + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(MenuForPizzaText))
            {
                menuItems += MenuForPizzaText + Environment.NewLine;
                itemPrices += PizzaPriceText + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(MenuForDrinksText))
            {
                menuItems += MenuForDrinksText + Environment.NewLine;
                itemPrices += DrinksPriceText + Environment.NewLine;
            }

            if (!string.IsNullOrEmpty(MenuForMealsText))
            {
                menuItems += MenuForMealsText + Environment.NewLine;
                itemPrices += MealPriceText + Environment.NewLine;
            }

            // Remove the trailing newline characters
            if (menuItems.EndsWith(Environment.NewLine))
            {
                menuItems = menuItems.Remove(menuItems.Length - Environment.NewLine.Length);
            }

            if (itemPrices.EndsWith(Environment.NewLine))
            {
                itemPrices = itemPrices.Remove(itemPrices.Length - Environment.NewLine.Length);
            }

            // Assign values to the "item_name" and "item_price" TextBoxes
            item_name.Text = menuItems;
            item_price.Text = itemPrices;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string Item_name = item_name.Text; // Assuming you have a TextBox named item_name
                string Item_price = item_price.Text; // Assuming you have a TextBox named item_price

                decimal itemPriceDecimal;

                if (decimal.TryParse(Item_price, out itemPriceDecimal))
                {
                    string updateQuery = "";
                    decimal currentPrice = GetCurrentPrice(Item_name, connection);

                    if (itemPriceDecimal == currentPrice)
                    {
                        MessageBox.Show("No changes made to the item price.");
                        return;
                    }

                    if (Item_name.StartsWith("CK"))
                    {
                        updateQuery = "UPDATE price SET ChickenPrice = @item_price WHERE MenuForChicken = @item_name";
                    }
                    else if (Item_name.StartsWith("PZ"))
                    {
                        updateQuery = "UPDATE price SET PizzaPrice = @item_price WHERE MenuForPizza = @item_name";
                    }
                    else if (Item_name.StartsWith("DK"))
                    {
                        updateQuery = "UPDATE price SET DrinksPrice = @item_price WHERE MenuForDrinks = @item_name";
                    }
                    else if (Item_name.StartsWith("ML"))
                    {
                        updateQuery = "UPDATE price SET MealPrice = @item_price WHERE MenuForMeals = @item_name";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(updateQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@item_name", Item_name);
                        cmd.Parameters.AddWithValue("@item_price", itemPriceDecimal);

                        int rowsAffected = cmd.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("The price item has been successfully updated.", "Item Price Update", MessageBoxButton.OK);
                            Approve approve = new Approve(username, role);
                            approve.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("No matching item found in the database.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid price value. Please enter a valid price for the item.");
                }
            }
        }

        private decimal GetCurrentPrice(string itemName, MySqlConnection connection)
        {
            string selectQuery = "";
            decimal currentPrice = 0;

            if (itemName.StartsWith("CK"))
            {
                selectQuery = "SELECT ChickenPrice FROM price WHERE MenuForChicken = @item_name";
            }
            else if (itemName.StartsWith("PZ"))
            {
                selectQuery = "SELECT PizzaPrice FROM price WHERE MenuForPizza = @item_name";
            }
            else if (itemName.StartsWith("DK"))
            {
                selectQuery = "SELECT DrinksPrice FROM price WHERE MenuForDrinks = @item_name";
            }
            else if (itemName.StartsWith("ML"))
            {
                selectQuery = "SELECT MealPrice FROM price WHERE MenuForMeals = @item_name";
            }

            using (MySqlCommand selectCmd = new MySqlCommand(selectQuery, connection))
            {
                selectCmd.Parameters.AddWithValue("@item_name", itemName);
                object result = selectCmd.ExecuteScalar();

                if (result != null && result != DBNull.Value)
                {
                    currentPrice = Convert.ToDecimal(result);
                }
            }

            return currentPrice;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Approve approve = new Approve(username, role);
            approve.Show();
            this.Close();
        }
    }
}