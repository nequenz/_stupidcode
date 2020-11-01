using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        public Window2()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object Sender, RoutedEventArgs e)
        {
            MainWindow.openConnection();

            MainWindow.SQL              = "USE Users; SELECT [ID], [Name], [Surname], [Email], [Phone], [Age] FROM UserEntity;";
            MainWindow.cmd.CommandType  = CommandType.Text;
            MainWindow.cmd.CommandText  = MainWindow.SQL;

            MainWindow.Adapter.SelectCommand = MainWindow.cmd;
            MainWindow.Adapter.Fill(MainWindow.DT);

            


            SQLGrid.ItemsSource = MainWindow.DT.DefaultView;

            MainWindow.closeConnection();


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.openConnection();
            if (MainWindow.DT.Rows.Count <= 0)
            {
                MainWindow.SQL = "USE Users; SELECT [ID], [Name], [Surname], [Email], [Phone], [Age] FROM UserEntity;";
                MainWindow.cmd.CommandType = CommandType.Text;
                MainWindow.cmd.CommandText = MainWindow.SQL;

                MainWindow.Adapter.SelectCommand = MainWindow.cmd;
                MainWindow.Adapter.Fill(MainWindow.DT);
            }

            MainWindow.Adapter.Update(MainWindow.DT);
            MainWindow.closeConnection();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MainWindow.openConnection();

           
            MainWindow.SQL              = "USE Users; " + SqlTextBox.Text;
            MainWindow.cmd.CommandType  = CommandType.Text;
            MainWindow.cmd.CommandText  = MainWindow.SQL;
            try
            {
                MainWindow.SqlReader = MainWindow.cmd.ExecuteReader();
            }
            catch (SqlException)
            {
                MessageBox.Show("Неверная интерпретация команд");
                MainWindow.closeConnection();
                return;
            }

            MainWindow.DT.Clear();
            MainWindow.DT.Load(MainWindow.SqlReader);

            MainWindow.closeConnection();

        }
    }
}
