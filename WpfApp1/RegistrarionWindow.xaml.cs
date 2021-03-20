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
using System.Net.NetworkInformation;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для RegistrarionWindow.xaml
    /// </summary>
    public partial class RegistrarionWindow : Window
    {
        public RegistrarionWindow()
        {
            InitializeComponent();
        }


        private void Регистрация_Click(object sender, RoutedEventArgs e)
        {
   
 

            MainWindow.executeSQL("SELECT [EmailName] FROM [Account]");

            for (int i = 0; i < MainWindow.DT.Rows.Count; i++)
            {
        
                string SLogin = MainWindow.getStringFromSql(MainWindow.DT.Rows[i].ItemArray[0].ToString());

                Console.WriteLine(SLogin+" "+ SLogin.Length+" "+ Login.Text.Length);

                if (Login.Text==SLogin)
                {
                    MessageBox.Show("Пользователь с таким уже существует!");
                    Login.Text      = "";
                    Password.Text   = "";
                    return;
                }

            }

            if (Name.Text == "" || Surname.Text == "")
            {
                MessageBox.Show("Введите полные данные!");
                Login.Text = "";
                Password.Text = "";
                return;
            }

            if (Password.Text == "")
            {
                MessageBox.Show("Придумайте пароль!");
                Login.Text = "";
                Password.Text = "";
                return;
            }


            MainWindow.executeSQL("INSERT INTO Account(EmailName, Name, Surname, IPAddress, Roots, Password) VALUES('" + Login.Text + "', '" + Name.Text + "', '" + Surname.Text + "', '000.000.000.000', 0, '" + Password.Text + "')");

            MessageBox.Show("Вы зарегистрированы!");

            Close();
        }
    }
}
