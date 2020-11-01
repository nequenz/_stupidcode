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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.Data;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object Sender,RoutedEventArgs e)
        {


            /*
            
            openConnection();
            SQL = "USE Users; SELECT [ID], [Name], [Surname], [Email], [Phone], [Age] FROM UserEntity;";
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = SQL;

            Adapter = new SqlDataAdapter(cmd);
            DT      = new DataTable();
            Adapter.Fill(DT);
            DataTableGrid.ItemsSource = DT.DefaultView;

            closeConnection();

            */







        }


        public static String GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["conString"].ToString();
        }

        public static void openConnection()
        {
            try
            {
                if(SQLC.State == ConnectionState.Closed)
                {
                    SQLC.ConnectionString = GetConnectionString();
                    SQLC.Open();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't open SQL server");
            }
        }

        public static void closeConnection()
        {
            try
            {
                if (SQLC.State == ConnectionState.Open)
                {
                    SQLC.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't close SQL server");
            }
        }



        public static string SQL;
        public static SqlDataReader SqlReader;
        public static SqlConnection SQLC        = new SqlConnection();
        public static SqlCommand cmd            = new SqlCommand("", SQLC);
        public static DataTable DT              = new DataTable();
        public static SqlDataAdapter Adapter    = new SqlDataAdapter();

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //to-do
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 _Win = new Window1();
            _Win.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window2 _Win = new Window2();
            _Win.Show();
        }
    }
}
