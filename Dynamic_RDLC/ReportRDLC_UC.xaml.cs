using Microsoft.Reporting.WinForms;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace Dynamic_RDLC
{
    /// <summary>
    /// Interaction logic for ReportRDLC_UC.xaml
    /// </summary>
    public partial class ReportRDLC_UC : UserControl
    {
        public ReportRDLC_UC()
        {
            InitializeComponent();
            LoadReport();
        }

        private void back_btn_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Home_Page_UC home_page_uc = new Home_Page_UC();

            // Get a reference to the MainWindow
            var main = Application.Current.MainWindow as MainWindow;

            if (main != null)
            {
                // Clear the existing content and add the new UserControl
                main.container.Children.Clear();
                main.container.Children.Add(home_page_uc);
            }
        }

        private void LoadReport()
        {
            // Create a connection to your database
            using (SqlConnection connection = new SqlConnection("Data Source=DESKTOP-4SK1J3Q\\SQL2019ENT;Initial Catalog=Inventory_db;Integrated Security = True;User Id = sa ;Password=1234;"))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("SELECT add_date, product_category, product_name, product_model, product_quantity, product_price, total_price FROM Products", connection);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                ReportDataSource rds = new ReportDataSource("DataSet1", dt); // "DataSet1" should match the name in your RDLC report

                reportView.LocalReport.DataSources.Clear();
                reportView.LocalReport.DataSources.Add(rds);
                reportView.LocalReport.ReportPath = "C:\\Users\\HP\\source\\repos\\Dynamic_RDLC\\Dynamic_RDLC\\product_rdlc.rdlc"; // Set the path of .rdlc file

                reportView.RefreshReport();
                reportView.SetDisplayMode(DisplayMode.PrintLayout);
                reportView.ZoomMode = ZoomMode.Percent;
                reportView.ZoomPercent = 100;
            }
        }
    }
}
