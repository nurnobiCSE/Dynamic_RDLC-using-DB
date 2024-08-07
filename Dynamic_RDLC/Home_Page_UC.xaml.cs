using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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

namespace Dynamic_RDLC
{
    /// <summary>
    /// Interaction logic for Home_Page_UC.xaml
    /// </summary>
    public partial class Home_Page_UC : UserControl
    {
        string connectionString = "Data Source=DESKTOP-4SK1J3Q\\SQL2019ENT;Initial Catalog=Inventory_db;Integrated Security = True;User Id = sa ;Password=1234;";
        public Home_Page_UC()
        {
            InitializeComponent();
            LoadDatagrid();
        }

        public void clearForm() {
            date_tbx.Text = "";
            name_tbx.Text = "";
            category_tbx.Text = "";
            quantity_tbx.Text = "";
            price_tbx.Text = "";
            totalprice_tbx.Text = "";
            model_tbx.Text = "";
        }

        public bool IsValid()
        {
            if (date_tbx.Text == string.Empty)
            {
                MessageBox.Show(" date is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (name_tbx.Text == string.Empty)
            {
                MessageBox.Show(" name is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (model_tbx.Text == string.Empty)
            {
                MessageBox.Show("model code is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (category_tbx.Text == string.Empty)
            {
                MessageBox.Show("category is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (quantity_tbx.Text == string.Empty)
            {
                MessageBox.Show("quantity is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            if (price_tbx.Text == string.Empty)
            {
                MessageBox.Show("price is required", "Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }

        public void LoadDatagrid() {
            SqlConnection connection = new SqlConnection(connectionString);
            SqlCommand sqlCommand = new SqlCommand("select * from Products", connection);
            DataTable dt = new DataTable();
            connection.Open();
            SqlDataReader dr = sqlCommand.ExecuteReader();
            dt.Load(dr);
            connection.Close();
            my_datagrid.ItemsSource = dt.DefaultView;
        }

        private void clear_btn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            clearForm();
        }

        private void add_btn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string insertQuery = "insert into Products values(@add_date,@product_category,@product_name,@product_model,@product_quantity,@product_price,@total_price)";
            if (IsValid())
            {
                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(insertQuery);
                command.Connection = conn;
                command.CommandType = System.Data.CommandType.Text;
                command.Parameters.AddWithValue("@add_date", date_tbx.Text);
                command.Parameters.AddWithValue("@product_category", category_tbx.Text);
                command.Parameters.AddWithValue("@product_name", name_tbx.Text);
                command.Parameters.AddWithValue("@product_model", model_tbx.Text);
                command.Parameters.AddWithValue("@product_quantity", quantity_tbx.Text);
                command.Parameters.AddWithValue("@product_price", price_tbx.Text);
                command.Parameters.AddWithValue("@total_price", totalprice_tbx.Text);

                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("New Product is added!!", "Saved", MessageBoxButton.OK, MessageBoxImage.Information);
                clearForm();
                LoadDatagrid();
            }
            else { MessageBox.Show("Something went wrong!!", "Wrong", MessageBoxButton.OK, MessageBoxImage.Error); }
        }

        private void get_report_btn_Click(object sender, RoutedEventArgs e)
        {
            ReportRDLC_UC reportRDLC_UC = new ReportRDLC_UC();

            // Get a reference to the MainWindow
            var main = Application.Current.MainWindow as MainWindow;

            if (main != null)
            {
                // Clear the existing content and add the new UserControl
                main.container.Children.Clear();
                main.container.Children.Add(reportRDLC_UC);
            }
        }

        private void price_tbx_TextChanged(object sender, TextChangedEventArgs e)
        {
            double quantity;
            double price;
            if (double.TryParse(quantity_tbx.Text, out quantity) && double.TryParse(price_tbx.Text, out price))
            {   
                double totalPrice = quantity * price;
                totalprice_tbx.Text = totalPrice.ToString("F2");
            }
        }
    }
}
