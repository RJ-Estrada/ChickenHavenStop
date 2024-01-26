using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows;

namespace Activity2
{
    public class chicken_class
    {
        private ListBox item;
        private ListBox prices;
        private ListBox item_price;
        private ListBox qty;
        private Grid chickenGrid;
        private TextBox subTotal;
        private TextBox actual_subtotal;

        public chicken_class(ListBox item, ListBox prices, ListBox item_price, ListBox qty, Grid chickenGrid, TextBox subTotal, TextBox actual_subtotal)
        {
            this.item = item;
            this.prices = prices;
            this.item_price = item_price;
            this.qty = qty;
            this.chickenGrid = chickenGrid;
            this.subTotal = subTotal;
            this.actual_subtotal = actual_subtotal;
        }

        public void InitializeGrid(Grid grid)
        {
            chickenGrid = grid;
        }

        private BitmapImage LoadImageFromBytes(byte[] imageData)
        {
            BitmapImage imageSource = new BitmapImage();
            using (MemoryStream stream = new MemoryStream(imageData))
            {
                stream.Position = 0;
                imageSource.BeginInit();
                imageSource.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.UriSource = null;
                imageSource.StreamSource = stream;
                imageSource.EndInit();
            }
            return imageSource;
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

        public void ChickenAdd()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve button details from the 'MenuForChicken' and 'ChickenPrice' columns
                string query = "SELECT MenuForChicken, ChickenPrice, RowNumber, Chicken_img FROM price WHERE RowNumber = 7 AND MenuForChicken IS NOT NULL AND ChickenPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new button with the item name from the database as the content
                            string? itemName = reader["MenuForChicken"].ToString();
                            byte[] imageBytes = (byte[])reader["Chicken_img"]; // Retrieve the image data as a byte array

                            // Create a Grid to overlay the Image and TextBlock
                            Grid grid = new Grid();

                            // Load the database image as the background
                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            // Create an Image control for the image
                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform, // Ensures the image occupies the entire space without distortion
                            };

                            // Create a TextBlock for the item name
                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25, // Set the desired font size
                                Effect = new DropShadowEffect // Add a DropShadowEffect to the text
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center, // Center the text vertically
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            // Add the image and text to the Grid
                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid, // Set the content of the button to the Grid
                                Width = 264,  // Set the width to your desired value
                                Height = 238,  // Set the height to your desired value
                                BorderThickness = new Thickness(1), // Set the border thickness
                                BorderBrush = null, // Set BorderBrush to null initially
                                Effect = new DropShadowEffect() // Add a DropShadowEffect
                                {
                                    Color = Colors.Black, // Set the shadow color
                                    Direction = 320, // Set the shadow direction (degrees)
                                    ShadowDepth = 5, // Set the shadow depth
                                    Opacity = 0.5, // Set the shadow opacity
                                }
                            };

                            // Mouse enter event handler
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White; // Set the border color on hover
                            };

                            // Mouse leave event handler
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null; // Remove the border on mouse leave
                            };

                            // Click event handler
                            new_Button.Name = "ck7";
                            new_Button.Click += ck7_Click;

                            // Inside your while loop
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 0; // You can specify the column as needed

                            // Specify the margin values to match the specified button
                            Thickness newButtonMargin = new Thickness(0, 504, 0, 0);

                            // Set the Grid.Row property to 1 to make the button appear in the second row (Grid.Row="1")
                            Grid.SetRow(new_Button, 0);

                            // Set the HorizontalAlignment property to Left
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;

                            // Set the Margin property to match the specified button
                            new_Button.Margin = newButtonMargin;


                            // Add the new button to the Grid within the ScrollViewer
                            chickenGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }

        private void ck7_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 7";
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

        public void ChickenAdd2()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve button details from the 'MenuForChicken' and 'ChickenPrice' columns
                string query = "SELECT MenuForChicken, ChickenPrice, RowNumber, Chicken_img FROM price WHERE RowNumber = 8 AND MenuForChicken IS NOT NULL AND ChickenPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new button with the item name from the database as the content
                            string? itemName = reader["MenuForChicken"].ToString();
                            byte[] imageBytes = (byte[])reader["Chicken_img"]; // Retrieve the image data as a byte array

                            // Create a Grid to overlay the Image and TextBlock
                            Grid grid = new Grid();

                            // Load the database image as the background
                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            // Create an Image control for the image
                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform, // Ensures the image occupies the entire space without distortion
                            };

                            // Create a TextBlock for the item name
                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25, // Set the desired font size
                                Effect = new DropShadowEffect // Add a DropShadowEffect to the text
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center, // Center the text vertically
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            // Add the image and text to the Grid
                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid, // Set the content of the button to the Grid
                                Width = 264,  // Set the width to your desired value
                                Height = 238,  // Set the height to your desired value
                                BorderThickness = new Thickness(1), // Set the border thickness
                                BorderBrush = null, // Set BorderBrush to null initially
                                Effect = new DropShadowEffect() // Add a DropShadowEffect
                                {
                                    Color = Colors.Black, // Set the shadow color
                                    Direction = 320, // Set the shadow direction (degrees)
                                    ShadowDepth = 5, // Set the shadow depth
                                    Opacity = 0.5, // Set the shadow opacity
                                }
                            };

                            // Mouse enter event handler
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White; // Set the border color on hover
                            };

                            // Mouse leave event handler
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null; // Remove the border on mouse leave
                            };

                            // Click event handler
                            new_Button.Name = "ck8";
                            new_Button.Click += ck8_Click;

                            // Inside your while loop
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 1; // You can specify the column as needed

                            // Specify the margin values to match the specified button
                            Thickness newButtonMargin = new Thickness(269, 504, 0, 0);

                            // Set the Grid.Row property to 1 to make the button appear in the second row (Grid.Row="1")
                            Grid.SetRow(new_Button, 0);

                            // Set the HorizontalAlignment property to Left
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;

                            // Set the Margin property to match the specified button
                            new_Button.Margin = newButtonMargin;



                            // Add the new button to the Grid within the ScrollViewer
                            chickenGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }
        private void ck8_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 8";
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

        public void ChickenAdd3()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve button details from the 'MenuForChicken' and 'ChickenPrice' columns
                string query = "SELECT MenuForChicken, ChickenPrice, RowNumber, Chicken_img FROM price WHERE RowNumber = 9 AND MenuForChicken IS NOT NULL AND ChickenPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new button with the item name from the database as the content
                            string? itemName = reader["MenuForChicken"].ToString();
                            byte[] imageBytes = (byte[])reader["Chicken_img"]; // Retrieve the image data as a byte array

                            // Create a Grid to overlay the Image and TextBlock
                            Grid grid = new Grid();

                            // Load the database image as the background
                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            // Create an Image control for the image
                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform, // Ensures the image occupies the entire space without distortion
                            };

                            // Create a TextBlock for the item name
                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25, // Set the desired font size
                                Effect = new DropShadowEffect // Add a DropShadowEffect to the text
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center, // Center the text vertically
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            // Add the image and text to the Grid
                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid, // Set the content of the button to the Grid
                                Width = 264,  // Set the width to your desired value
                                Height = 238,  // Set the height to your desired value
                                BorderThickness = new Thickness(1), // Set the border thickness
                                BorderBrush = null, // Set BorderBrush to null initially
                                Effect = new DropShadowEffect() // Add a DropShadowEffect
                                {
                                    Color = Colors.Black, // Set the shadow color
                                    Direction = 320, // Set the shadow direction (degrees)
                                    ShadowDepth = 5, // Set the shadow depth
                                    Opacity = 0.5, // Set the shadow opacity
                                }
                            };

                            // Mouse enter event handler
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White; // Set the border color on hover
                            };

                            // Mouse leave event handler
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null; // Remove the border on mouse leave
                            };

                            // Click event handler
                            new_Button.Name = "ck9";
                            new_Button.Click += ck9_Click;

                            // Inside your while loop
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 1; // You can specify the column as needed

                            // Specify the margin values to match the specified button
                            Thickness newButtonMargin = new Thickness(538, 504, 0, 0);

                            // Set the Grid.Row property to 1 to make the button appear in the second row (Grid.Row="1")
                            Grid.SetRow(new_Button, 0);

                            // Set the HorizontalAlignment property to Left
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;

                            // Set the Margin property to match the specified button
                            new_Button.Margin = newButtonMargin;



                            // Add the new button to the Grid within the ScrollViewer
                            chickenGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }
        private void ck9_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 9";
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
        public void ChickenAdd4()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve button details from the 'MenuForChicken' and 'ChickenPrice' columns
                string query = "SELECT MenuForChicken, ChickenPrice, RowNumber, Chicken_img FROM price WHERE RowNumber = 10 AND MenuForChicken IS NOT NULL AND ChickenPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new button with the item name from the database as the content
                            string? itemName = reader["MenuForChicken"].ToString();
                            byte[] imageBytes = (byte[])reader["Chicken_img"]; // Retrieve the image data as a byte array

                            // Create a Grid to overlay the Image and TextBlock
                            Grid grid = new Grid();

                            // Load the database image as the background
                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            // Create an Image control for the image
                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform, // Ensures the image occupies the entire space without distortion
                            };

                            // Create a TextBlock for the item name
                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25, // Set the desired font size
                                Effect = new DropShadowEffect // Add a DropShadowEffect to the text
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center, // Center the text vertically
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            // Add the image and text to the Grid
                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid, // Set the content of the button to the Grid
                                Width = 264,  // Set the width to your desired value
                                Height = 238,  // Set the height to your desired value
                                BorderThickness = new Thickness(1), // Set the border thickness
                                BorderBrush = null, // Set BorderBrush to null initially
                                Effect = new DropShadowEffect() // Add a DropShadowEffect
                                {
                                    Color = Colors.Black, // Set the shadow color
                                    Direction = 320, // Set the shadow direction (degrees)
                                    ShadowDepth = 5, // Set the shadow depth
                                    Opacity = 0.5, // Set the shadow opacity
                                }
                            };

                            // Mouse enter event handler
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White; // Set the border color on hover
                            };

                            // Mouse leave event handler
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null; // Remove the border on mouse leave
                            };

                            // Click event handler
                            new_Button.Name = "ck10";
                            new_Button.Click += ck10_Click;

                            // Inside your while loop
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 1; // You can specify the column as needed

                            // Specify the margin values to match the specified button
                            Thickness newButtonMargin = new Thickness(0, 10, 0, 0);

                            // Set the Grid.Row property to 1 to make the button appear in the second row (Grid.Row="1")
                            Grid.SetRow(new_Button, 1);

                            // Set the HorizontalAlignment property to Left
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;

                            // Set the Margin property to match the specified button
                            new_Button.Margin = newButtonMargin;

                            // Set the Grid.RowSpan property to 3 to make the button span three rows


                            // Add the new button to the Grid within the ScrollViewer
                            chickenGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }
        private void ck10_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 10";
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
        public void ChickenAdd5()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve button details from the 'MenuForChicken' and 'ChickenPrice' columns
                string query = "SELECT MenuForChicken, ChickenPrice, RowNumber, Chicken_img FROM price WHERE RowNumber = 11 AND MenuForChicken IS NOT NULL AND ChickenPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new button with the item name from the database as the content
                            string? itemName = reader["MenuForChicken"].ToString();
                            byte[] imageBytes = (byte[])reader["Chicken_img"]; // Retrieve the image data as a byte array

                            // Create a Grid to overlay the Image and TextBlock
                            Grid grid = new Grid();

                            // Load the database image as the background
                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            // Create an Image control for the image
                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform, // Ensures the image occupies the entire space without distortion
                            };

                            // Create a TextBlock for the item name
                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25, // Set the desired font size
                                Effect = new DropShadowEffect // Add a DropShadowEffect to the text
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center, // Center the text vertically
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            // Add the image and text to the Grid
                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid, // Set the content of the button to the Grid
                                Width = 264,  // Set the width to your desired value
                                Height = 238,  // Set the height to your desired value
                                BorderThickness = new Thickness(1), // Set the border thickness
                                BorderBrush = null, // Set BorderBrush to null initially
                                Effect = new DropShadowEffect() // Add a DropShadowEffect
                                {
                                    Color = Colors.Black, // Set the shadow color
                                    Direction = 320, // Set the shadow direction (degrees)
                                    ShadowDepth = 5, // Set the shadow depth
                                    Opacity = 0.5, // Set the shadow opacity
                                }
                            };

                            // Mouse enter event handler
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White; // Set the border color on hover
                            };

                            // Mouse leave event handler
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null; // Remove the border on mouse leave
                            };

                            // Click event handler
                            new_Button.Name = "ck11";
                            new_Button.Click += ck11_Click;

                            // Inside your while loop
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 1; // You can specify the column as needed

                            // Specify the margin values to match the specified button
                            Thickness newButtonMargin = new Thickness(269, 756, 0, 0);

                            // Set the Grid.Row property to 1 to make the button appear in the second row (Grid.Row="1")
                            Grid.SetRow(new_Button, 0);

                            // Set the HorizontalAlignment property to Left
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;

                            // Set the Margin property to match the specified button
                            new_Button.Margin = newButtonMargin;

                            Grid.SetRowSpan(new_Button, 3);

                            // Add the new button to the Grid within the ScrollViewer
                            chickenGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }
        private void ck11_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 11";
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
        public void ChickenAdd6()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Query to retrieve button details from the 'MenuForChicken' and 'ChickenPrice' columns
                string query = "SELECT MenuForChicken, ChickenPrice, RowNumber, Chicken_img FROM price WHERE RowNumber = 12 AND MenuForChicken IS NOT NULL AND ChickenPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Create a new button with the item name from the database as the content
                            string? itemName = reader["MenuForChicken"].ToString();
                            byte[] imageBytes = (byte[])reader["Chicken_img"]; // Retrieve the image data as a byte array

                            // Create a Grid to overlay the Image and TextBlock
                            Grid grid = new Grid();

                            // Load the database image as the background
                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            // Create an Image control for the image
                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform, // Ensures the image occupies the entire space without distortion
                            };

                            // Create a TextBlock for the item name
                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25, // Set the desired font size
                                Effect = new DropShadowEffect // Add a DropShadowEffect to the text
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center, // Center the text vertically
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            // Add the image and text to the Grid
                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid, // Set the content of the button to the Grid
                                Width = 264,  // Set the width to your desired value
                                Height = 238,  // Set the height to your desired value
                                BorderThickness = new Thickness(1), // Set the border thickness
                                BorderBrush = null, // Set BorderBrush to null initially
                                Effect = new DropShadowEffect() // Add a DropShadowEffect
                                {
                                    Color = Colors.Black, // Set the shadow color
                                    Direction = 320, // Set the shadow direction (degrees)
                                    ShadowDepth = 5, // Set the shadow depth
                                    Opacity = 0.5, // Set the shadow opacity
                                }
                            };

                            // Mouse enter event handler
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White; // Set the border color on hover
                            };

                            // Mouse leave event handler
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null; // Remove the border on mouse leave
                            };

                            // Click event handler
                            new_Button.Name = "ck12";
                            new_Button.Click += ck12_Click;

                            // Inside your while loop
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 1; // You can specify the column as needed

                            // Specify the margin values to match the specified button
                            Thickness newButtonMargin = new Thickness(538, 756, 0, 0);

                            // Set the Grid.Row property to 1 to make the button appear in the second row (Grid.Row="1")
                            Grid.SetRow(new_Button, 0);

                            // Set the HorizontalAlignment property to Left
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;

                            // Set the Margin property to match the specified button
                            new_Button.Margin = newButtonMargin;

                            Grid.SetRowSpan(new_Button, 3);

                            // Add the new button to the Grid within the ScrollViewer
                            chickenGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }
        private void ck12_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Fetch the data from the MySQL database
                string query = "SELECT MenuForChicken, ChickenPrice FROM price WHERE RowNumber = 12";
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
    }
}