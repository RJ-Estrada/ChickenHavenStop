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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MySql.Data.MySqlClient;
using static Activity2.OrderHistory;

namespace Activity2
{
    /// <summary>
    /// Interaction logic for PrintReceipt.xaml
    /// </summary>
    public partial class PrintReceipt : Window
    {
        private string username;
        private string orderCode;
        private string role;

        public PrintReceipt(string username, string orderCode, string role)
        {
            InitializeComponent();
            this.username = username;
            this.orderCode = orderCode;

            // Set the Text property of the TextBox to display the orderCode
            show_try.Text = orderCode;
            this.role = role;
        }

        private RenderTargetBitmap CreateBitmapFromVisual(Visual target, double width, double height)
        {
            // Create a RenderTargetBitmap
            RenderTargetBitmap renderTarget = new RenderTargetBitmap((int)width, (int)height, 96, 96, PixelFormats.Pbgra32);

            // Create a VisualBrush and draw the target visual
            VisualBrush visualBrush = new VisualBrush(target);
            DrawingVisual drawingVisual = new DrawingVisual();
            DrawingContext drawingContext = drawingVisual.RenderOpen();
            drawingContext.DrawRectangle(visualBrush, null, new Rect(new Point(0, 0), new Point(width, height)));
            drawingContext.Close();

            // Render the content to the RenderTargetBitmap
            renderTarget.Render(drawingVisual);

            return renderTarget;
        }

        private void PrintVisualContent(Visual target, double width, double height)
        {
            // Create a PrintDialog
            PrintDialog printDialog = new PrintDialog();

            if (printDialog.ShowDialog() == true)
            {
                // Create a FixedDocument
                FixedDocument document = new FixedDocument();
                PageContent pageContent = new PageContent();
                FixedPage fixedPage = new FixedPage();
                fixedPage.Width = width;
                fixedPage.Height = height;

                // Create an Image to display the captured content
                Image image = new Image();
                image.Source = CreateBitmapFromVisual(target, width, height);

                // Add the image to the FixedPage
                fixedPage.Children.Add(image);

                // Add the FixedPage to the PageContent
                ((IAddChild)pageContent).AddChild(fixedPage);

                // Add the PageContent to the FixedDocument
                document.Pages.Add(pageContent);

                // Print the document
                printDialog.PrintDocument(document.DocumentPaginator, "Print Job Name");
            }
        }
        public void PrintCanvasContent()
        {
            // Create a visual of the entire window
            Visual windowContent = this;

            // Calculate the width and height of the entire window
            double width = this.ActualWidth;
            double height = this.ActualHeight;

            PrintVisualContent(windowContent, width, height);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void show_receipt_Click(object sender, RoutedEventArgs e)
        {
            // Your database connection string
            string connectionString = "Server=localhost;Database=dashboard;User=root;Password=;";

            // Get the OrderCode from the "show_try" TextBox
            string orderCode = show_try.Text;

            // SQL query to retrieve data from the "receipt" table
            string query = "SELECT * FROM receipt WHERE OrderCode = @orderCode";

            try
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@orderCode", orderCode);

                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Clear existing items in the list boxes
                                qty_show.Items.Clear();
                                item_show.Items.Clear();
                                price_show.Items.Clear();
                                total_show.Items.Clear();

                                do
                                {
                                    qty_show.Items.Add(reader["QTY"]);
                                    item_show.Items.Add(reader["Items"]);
                                    price_show.Items.Add(reader["Price"]);
                                    total_show.Items.Add(reader["Amount"]);
                                } while (reader.Read());

                                // Update the text blocks
                                subtotal_priceshow.Text = "₱" + string.Format("{0:N0}", reader["Subtotal"]);
                                discount_show.Text = reader["Discounted"].ToString();
                                cashier_name.Text = reader["Username"].ToString();
                                total_priceshow.Text = "₱" + string.Format("{0:N0}", reader["Total"]);
                                cash_show.Text = "₱" + string.Format("{0:N0}", reader["Cash"]);
                                change_show.Text = "₱" + string.Format("{0:N0}", reader["Change_Money"]);
                            }
                            else
                            {
                                // Handle the case where the order code is not found in the database
                                MessageBox.Show("Order not found in the database.", "Error");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error");
            }
        }

        private void final_print_Click(object sender, RoutedEventArgs e)
        {
            PrintCanvasContent();
        }

    }
}