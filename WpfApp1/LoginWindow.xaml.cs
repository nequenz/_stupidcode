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
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            MainWindow.executeSQL("SELECT [EmailName],[Password],[ID] FROM [Account]");

            for(int i = 0; i < MainWindow.DT.Rows.Count; i++)
            {
                string BLogin       = MainWindow.getStringFromSql(MainWindow.DT.Rows[i].ItemArray[0].ToString());
                string BPassword    = MainWindow.getStringFromSql(MainWindow.DT.Rows[i].ItemArray[1].ToString());



                if (Login.Text == BLogin && Password.Text == BPassword)
                {
                    MainWindow W = new MainWindow();
                    MainWindow.AccountID = Int32.Parse(MainWindow.getStringFromSql(MainWindow.DT.Rows[i].ItemArray[2].ToString()));
                    W.Show();
                    Close();
                    return;
                }
                

            }

            
            MessageBox.Show("Неправильный логин или пароль");
            Login.Text = "";
            Password.Text = "";

  

        }

        private void RegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            new RegistrarionWindow().Show();
        }
    }
}
