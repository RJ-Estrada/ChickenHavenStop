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
using Microsoft.Win32;
using MySql.Data.MySqlClient;

namespace Activity2
{
    /// <summary>
    /// Interaction logic for AddMenu.xaml
    /// </summary>
    public partial class AddMenu : Window
    {
        private string username;
        private string role;

        public AddMenu(string username, string role)
        {
            InitializeComponent();
            this.username = username;
            InitializeComboBoxes();
            this.role = role;
        }

        private void InitializeComboBoxes()
        {
            menu_option.Items.Add("MenuForChicken");
            menu_option.Items.Add("MenuForPizza");
            menu_option.Items.Add("MenuForDrinks");
            menu_option.Items.Add("MenuForMeals");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Approve approve = new Approve(username, role);
            approve.Show();
            this.Close();
        }

        private void add_menu_Click(object sender, RoutedEventArgs e)
        {
            // Check if the menu_img is not null and has a valid image
            if (menu_img.Source == null)
            {
                MessageBox.Show("Please select an image for the menu before adding it.");
                return;
            }

            string? selectedOption = menu_option.SelectedItem as string;

            if (selectedOption != null)
            {
                string tableName = "price";
                string columnName = "";
                string imageColumnName = "";

                switch (selectedOption)
                {
                    case "MenuForChicken":
                        columnName = "ChickenPrice";
                        imageColumnName = "Chicken_img";
                        break;
                    case "MenuForPizza":
                        columnName = "PizzaPrice";
                        imageColumnName = "Pizza_img";
                        break;
                    case "MenuForDrinks":
                        columnName = "DrinksPrice";
                        imageColumnName = "Drinks_img";
                        break;
                    case "MenuForMeals":
                        columnName = "MealPrice";
                        imageColumnName = "Meal_img";
                        break;
                    default:
                        MessageBox.Show("Invalid menu option selected.");
                        return;
                }

                string menuName = menu_name.Text;
                string menuPrice = menu_price.Text;

                if (string.IsNullOrWhiteSpace(menuName) || string.IsNullOrWhiteSpace(menuPrice))
                {
                    MessageBox.Show("Please fill in all fields before adding the menu.");
                    return;
                }

                if (!int.TryParse(menuPrice, out int parsedMenuPriceInt))
                {
                    MessageBox.Show("Menu price should be a numerical value.");
                    return;
                }

                if (parsedMenuPriceInt == 0)
                {
                    MessageBox.Show("Menu price cannot be zero.");
                    return;
                }

                if (!double.TryParse(menuPrice, out double parsedMenuPriceDouble) || parsedMenuPriceDouble <= 0 || parsedMenuPriceDouble % 1 != 0)
                {
                    MessageBox.Show("Menu price should be a positive whole number.");
                    return;
                }

                string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Find the lowest empty row (where the specified column is 0)
                    string selectLowestEmptyRowQuery = $"SELECT RowNumber FROM {tableName} WHERE {columnName} = 0 LIMIT 1";
                    using (MySqlCommand findCommand = new MySqlCommand(selectLowestEmptyRowQuery, connection))
                    {
                        object lowestEmptyRow = findCommand.ExecuteScalar();
                        int rowNumber = (lowestEmptyRow != null && lowestEmptyRow != DBNull.Value) ? Convert.ToInt32(lowestEmptyRow) : -1;

                        // Check if rowNumber is -1
                        if (rowNumber == -1)
                        {
                            MessageBox.Show("You've reached the maximum limit of menus allowed for this category.", "Error");
                        }
                        else
                        {
                            // Insert data into the found empty row
                            string insertQuery = $"UPDATE {tableName} SET {columnName} = @MenuPrice, {imageColumnName} = @MenuImage, {selectedOption} = @MenuName WHERE RowNumber = @RowNumber";
                            using (MySqlCommand insertCommand = new MySqlCommand(insertQuery, connection))
                            {
                                insertCommand.Parameters.AddWithValue("@MenuName", menuName);
                                insertCommand.Parameters.AddWithValue("@MenuPrice", menuPrice);
                                insertCommand.Parameters.AddWithValue("@MenuImage", GetImageBytes(menu_img.Source as BitmapImage));
                                insertCommand.Parameters.AddWithValue("@RowNumber", rowNumber);

                                int rowsAffected = insertCommand.ExecuteNonQuery();

                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Menu added successfully.");
                                    Approve approve = new Approve(username, role);
                                    approve.Show();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Failed to add the menu.");
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a menu option.");
            }
        }

        private byte[] GetImageBytes(BitmapImage? bitmapImage)
        {
            if (bitmapImage == null)
            {
                // Handle the case where bitmapImage is null (you can throw an exception or return an empty byte array, depending on your requirements)
                throw new ArgumentNullException(nameof(bitmapImage), "BitmapImage cannot be null.");
            }

            byte[] data;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                data = ms.ToArray();
            }

            return data;
        }

        private void menu_option_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string? selectedStatus = menu_option.SelectedItem as string;
            if (selectedStatus != null)
            {
            }
        }

        private void select_img_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.png)|*.jpg;*.jpeg;*.png|All files (*.*)|*.*",
                Title = "Select an Image File"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Get the selected file name and display it
                string selectedFileName = openFileDialog.FileName;

                // Load the selected image into your image toolbox
                try
                {
                    BitmapImage bitmap = new BitmapImage(new Uri(selectedFileName));
                    menu_img.Source = bitmap;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading the image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}