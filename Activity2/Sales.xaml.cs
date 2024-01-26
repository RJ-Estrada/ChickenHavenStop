using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
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
    /// Interaction logic for Sales.xaml
    /// </summary>
    public partial class Sales : Window
    {
        private string username;
        private string role;
        public Sales(string username, string role)
        {
            InitializeComponent();
            this.username = username;
            this.role = role;
        }

        private void CalculateAndDisplaySalesTotals()
        {
            // Get the DataTable from the DataGrid's ItemsSource
            if (sales.ItemsSource is DataView dataView)
            {
                DataTable dataTable = dataView.Table;

                // Calculate the sum of "Sales" for items that start with specific prefixes
                decimal ckSalesTotal = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("Items").StartsWith("CK"))
                    .Sum(row => row.Field<decimal>("TotalQTY"));

                decimal pzSalesTotal = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("Items").StartsWith("PZ"))
                    .Sum(row => row.Field<decimal>("TotalQTY"));

                decimal dkSalesTotal = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("Items").StartsWith("DK"))
                    .Sum(row => row.Field<decimal>("TotalQTY"));

                decimal mlSalesTotal = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("Items").StartsWith("ML"))
                    .Sum(row => row.Field<decimal>("TotalQTY"));

                decimal allSalesTotal = dataTable.AsEnumerable()
                    .Sum(row => row.Field<decimal>("TotalQTY"));

                // Display the totals in the respective TextBoxes
                ck_sales.Text = ckSalesTotal.ToString();
                pz_sales.Text = pzSalesTotal.ToString();
                dk_sales.Text = dkSalesTotal.ToString();
                ml_sales.Text = mlSalesTotal.ToString();
                all_sales.Text = allSalesTotal.ToString();
            }
        }

        private void CalculateAndDisplayRevenueTotals()
        {
            // Get the DataTable from the DataGrid's ItemsSource
            if (sales.ItemsSource is DataView dataView)
            {
                DataTable dataTable = dataView.Table;

                // Calculate the sum of "Revenue" for items that start with specific prefixes
                decimal ckRevTotal = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("Items").StartsWith("CK"))
                    .Sum(row => row.Field<decimal>("TotalAmount"));

                decimal pzRevTotal = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("Items").StartsWith("PZ"))
                    .Sum(row => row.Field<decimal>("TotalAmount"));

                decimal dkRevTotal = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("Items").StartsWith("DK"))
                    .Sum(row => row.Field<decimal>("TotalAmount"));

                decimal mlRevTotal = dataTable.AsEnumerable()
                    .Where(row => row.Field<string>("Items").StartsWith("ML"))
                    .Sum(row => row.Field<decimal>("TotalAmount"));

                decimal allRevTotal = dataTable.AsEnumerable()
                    .Sum(row => row.Field<decimal>("TotalAmount"));

                // Format the totals with "₱" symbol and commas for thousands
                ck_rev.Text = FormatRevenue(ckRevTotal);
                pz_rev.Text = FormatRevenue(pzRevTotal);
                dk_rev.Text = FormatRevenue(dkRevTotal);
                ml_rev.Text = FormatRevenue(mlRevTotal);
                all_rev.Text = FormatRevenue(allRevTotal);
            }
        }

        private string FormatRevenue(decimal revenue)
        {
            // Format the revenue value as "₱X,XXX.XX" with commas for thousands and two decimal places
            return string.Format("₱{0:N2}", revenue);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OrderHistory orderHistory = new OrderHistory(username, role);
            orderHistory.Show();
            this.Close();
        }

        private void ViewSales_Click(object sender, RoutedEventArgs e)
        {
            // Check if both datepickers have a selected date
            if (!from_date.SelectedDate.HasValue || !to_date.SelectedDate.HasValue)
            {
                MessageBox.Show("Please select both From Date and To Date");
                return;
            }
            // Get the selected dates from datepickers
            DateTime fromDate = from_date.SelectedDate ?? DateTime.MinValue;
            DateTime toDate = to_date.SelectedDate ?? DateTime.MaxValue;

            // Validate if toDate is greater than or equal to fromDate
            if (toDate < fromDate)
            {
                MessageBox.Show("To Date must be greater than or equal to From Date");
                return;
            }

            // Prepare the SQL query to select aggregated QTY and Amount for each item ordered on a given date
            string queryString = "SELECT SUBSTRING(OrderCode, 5, 8) AS OrderDate, Items, SUM(QTY) AS TotalQTY, SUM(Amount) AS TotalAmount " +
                                 "FROM receipt " +
                                 "WHERE SUBSTRING(OrderCode, 5, 8) BETWEEN @FromDate AND @ToDate " +
                                 "GROUP BY SUBSTRING(OrderCode, 5, 8), Items";

            using (MySqlConnection connection = new MySqlConnection("Server=localhost;Database=dashboard;User=root;Password=;"))
            {
                using (MySqlCommand command = new MySqlCommand(queryString, connection))
                {
                    // Add parameters to the query
                    command.Parameters.AddWithValue("@FromDate", fromDate.ToString("yyyyMMdd"));
                    command.Parameters.AddWithValue("@ToDate", toDate.ToString("yyyyMMdd"));

                    try
                    {
                        // Open the connection
                        connection.Open();

                        // Create a DataTable to store the results
                        DataTable salesData = new DataTable();

                        // Use a DataAdapter to fill the DataTable
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(salesData);
                        }
                        salesData.Columns["TotalQTY"].DataType = typeof(decimal);
                        sales.ItemsSource = salesData.DefaultView;
                        CalculateAndDisplaySalesTotals();
                        CalculateAndDisplayRevenueTotals();
                    }
                    catch (Exception ex)
                    {
                        // Handle any exceptions that may occur during the database query
                        MessageBox.Show("Error: " + ex.Message);
                    }

                }
            }
        }
    }
}