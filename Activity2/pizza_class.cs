using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Effects;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Activity2
{
    public class pizza_class
    {
        private ListBox item;
        private ListBox prices;
        private ListBox item_price;
        private ListBox qty;
        private Grid pizzaGrid;
        private TextBox subTotal;
        private TextBox actual_subtotal;

        public pizza_class(ListBox item, ListBox prices, ListBox item_price, ListBox qty, Grid pizzaGrid, TextBox subTotal, TextBox actual_subtotal)
        {
            this.item = item;
            this.prices = prices;
            this.item_price = item_price;
            this.qty = qty;
            this.pizzaGrid = pizzaGrid;
            this.subTotal = subTotal;
            this.actual_subtotal = actual_subtotal;
        }

        public void InitializeGrid(Grid grid)
        {
            pizzaGrid = grid;
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
            string formattedTotal = total >= 1000 ? string.Format("{0:N2}", total) : total.ToString("N2");
            subTotal.Text = formattedTotal;
            actual_subtotal.Text = formattedTotal;
            subTotal.IsReadOnly = true;
            actual_subtotal.IsReadOnly = true;
        }

        public void PizzaAdd()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT MenuForPizza, PizzaPrice, RowNumber, Pizza_img FROM price WHERE RowNumber = 7 AND MenuForPizza IS NOT NULL AND PizzaPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? itemName = reader["MenuForPizza"].ToString();
                            byte[] imageBytes = (byte[])reader["Pizza_img"];

                            Grid grid = new Grid();

                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform,
                            };

                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid,
                                Width = 264,
                                Height = 238,
                                BorderThickness = new Thickness(1),
                                BorderBrush = null,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                }
                            };
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White;
                            };
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null;
                            };
                            new_Button.Name = "pz7";
                            new_Button.Click += pz7_Click;
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 0;
                            Thickness newButtonMargin = new Thickness(0, 504, 0, 0);
                            Grid.SetRow(new_Button, 0);
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;
                            new_Button.Margin = newButtonMargin;
                            pizzaGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }

        private void pz7_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 7";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            if (!item.Items.Contains(menuForPizza))
                            {
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();
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

        public void PizzaAdd2()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT MenuForPizza, PizzaPrice, RowNumber, Pizza_img FROM price WHERE RowNumber = 8 AND MenuForPizza IS NOT NULL AND PizzaPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? itemName = reader["MenuForPizza"].ToString();
                            byte[] imageBytes = (byte[])reader["Pizza_img"];

                            Grid grid = new Grid();

                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform,
                            };

                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid,
                                Width = 264,
                                Height = 238,
                                BorderThickness = new Thickness(1),
                                BorderBrush = null,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                }
                            };
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White;
                            };
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null;
                            };
                            new_Button.Name = "pz8";
                            new_Button.Click += pz8_Click;
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 0;
                            Thickness newButtonMargin = new Thickness(269, 504, 0, 0);
                            Grid.SetRow(new_Button, 0);
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;
                            new_Button.Margin = newButtonMargin;
                            pizzaGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }

        private void pz8_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 8";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            if (!item.Items.Contains(menuForPizza))
                            {
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();
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

        public void PizzaAdd3()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT MenuForPizza, PizzaPrice, RowNumber, Pizza_img FROM price WHERE RowNumber = 9 AND MenuForPizza IS NOT NULL AND PizzaPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? itemName = reader["MenuForPizza"].ToString();
                            byte[] imageBytes = (byte[])reader["Pizza_img"];

                            Grid grid = new Grid();

                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform,
                            };

                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid,
                                Width = 264,
                                Height = 238,
                                BorderThickness = new Thickness(1),
                                BorderBrush = null,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                }
                            };
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White;
                            };
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null;
                            };
                            new_Button.Name = "pz9";
                            new_Button.Click += pz9_Click;
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 0;
                            Thickness newButtonMargin = new Thickness(538, 504, 0, 0);
                            Grid.SetRow(new_Button, 0);
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;
                            new_Button.Margin = newButtonMargin;
                            pizzaGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }

        private void pz9_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 9";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            if (!item.Items.Contains(menuForPizza))
                            {
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();
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

        public void PizzaAdd4()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT MenuForPizza, PizzaPrice, RowNumber, Pizza_img FROM price WHERE RowNumber = 10 AND MenuForPizza IS NOT NULL AND PizzaPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? itemName = reader["MenuForPizza"].ToString();
                            byte[] imageBytes = (byte[])reader["Pizza_img"];

                            Grid grid = new Grid();

                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform,
                            };

                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid,
                                Width = 264,
                                Height = 238,
                                BorderThickness = new Thickness(1),
                                BorderBrush = null,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                }
                            };
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White;
                            };
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null;
                            };
                            new_Button.Name = "pz10";
                            new_Button.Click += pz10_Click;
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 0;
                            Thickness newButtonMargin = new Thickness(0, 10, 0, 0);
                            Grid.SetRow(new_Button, 1);
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;
                            new_Button.Margin = newButtonMargin;
                            pizzaGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }

        private void pz10_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 10";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            if (!item.Items.Contains(menuForPizza))
                            {
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();
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

        public void PizzaAdd5()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT MenuForPizza, PizzaPrice, RowNumber, Pizza_img FROM price WHERE RowNumber = 11 AND MenuForPizza IS NOT NULL AND PizzaPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? itemName = reader["MenuForPizza"].ToString();
                            byte[] imageBytes = (byte[])reader["Pizza_img"];

                            Grid grid = new Grid();

                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform,
                            };

                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid,
                                Width = 264,
                                Height = 238,
                                BorderThickness = new Thickness(1),
                                BorderBrush = null,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                }
                            };
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White;
                            };
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null;
                            };
                            new_Button.Name = "pz11";
                            new_Button.Click += pz11_Click;
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 0;
                            Thickness newButtonMargin = new Thickness(269, 756, 0, 0);
                            Grid.SetRow(new_Button, 0);
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;
                            new_Button.Margin = newButtonMargin;
                            Grid.SetRowSpan(new_Button, 3);
                            pizzaGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }

        private void pz11_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 11";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            if (!item.Items.Contains(menuForPizza))
                            {
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();
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

        public void PizzaAdd6()
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT MenuForPizza, PizzaPrice, RowNumber, Pizza_img FROM price WHERE RowNumber = 12 AND MenuForPizza IS NOT NULL AND PizzaPrice > 0";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string? itemName = reader["MenuForPizza"].ToString();
                            byte[] imageBytes = (byte[])reader["Pizza_img"];

                            Grid grid = new Grid();

                            BitmapImage imageSource = LoadImageFromBytes(imageBytes);

                            Image imageControl = new Image
                            {
                                Source = imageSource,
                                Stretch = Stretch.Uniform,
                            };

                            TextBlock textBlock = new TextBlock
                            {
                                Text = itemName,
                                TextAlignment = TextAlignment.Center,
                                TextWrapping = TextWrapping.Wrap,
                                Foreground = Brushes.White,
                                FontWeight = FontWeights.Bold,
                                FontSize = 25,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                },
                                VerticalAlignment = VerticalAlignment.Center,
                                Margin = new Thickness(0, 0, 0, 20),
                            };

                            grid.Children.Add(imageControl);
                            grid.Children.Add(textBlock);

                            Button new_Button = new Button
                            {
                                Content = grid,
                                Width = 264,
                                Height = 238,
                                BorderThickness = new Thickness(1),
                                BorderBrush = null,
                                Effect = new DropShadowEffect
                                {
                                    Color = Colors.Black,
                                    Direction = 320,
                                    ShadowDepth = 5,
                                    Opacity = 0.5,
                                }
                            };
                            new_Button.MouseEnter += (sender, e) =>
                            {
                                new_Button.BorderBrush = Brushes.White;
                            };
                            new_Button.MouseLeave += (sender, e) =>
                            {
                                new_Button.BorderBrush = null;
                            };
                            new_Button.Name = "pz12";
                            new_Button.Click += pz12_Click;
                            int row = Convert.ToInt32(reader["RowNumber"]);
                            int column = 0;
                            Thickness newButtonMargin = new Thickness(538, 756, 0, 0);
                            Grid.SetRow(new_Button, 0);
                            new_Button.HorizontalAlignment = HorizontalAlignment.Left;
                            new_Button.Margin = newButtonMargin;
                            Grid.SetRowSpan(new_Button, 3);
                            pizzaGrid.Children.Add(new_Button);
                            Grid.SetColumn(new_Button, column);
                        }
                    }
                }
            }
        }

        private void pz12_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT MenuForPizza, PizzaPrice FROM price WHERE RowNumber = 12";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string menuForPizza = reader["MenuForPizza"]?.ToString() ?? "DefaultMenuForPizza";
                            string pizzaPrice = reader["PizzaPrice"]?.ToString() ?? "DefaultPizzaPrice";

                            if (!item.Items.Contains(menuForPizza))
                            {
                                item.Items.Add(menuForPizza);
                                prices.Items.Add(pizzaPrice);
                                item_price.Items.Add(pizzaPrice);
                                UpdateSubTotal();
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