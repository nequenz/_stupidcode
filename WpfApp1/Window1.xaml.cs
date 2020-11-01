using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();

            ID.Text = HashID.ToString();
        }

        private void TBname_TextChanged(object sender, TextChangedEventArgs e)
        {
            HashID = (uint)TBname.Text.GetHashCode();
            if (ID != null)
            {
                ID.Text = HashID.ToString();
            }
            

        }

        private void Surname_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Очень грубая форма хеш-функции, но для теста пойдет...

            HashID += (uint)Surname.Text.GetHashCode();
            if (ID != null)
            {
                ID.Text = HashID.ToString();
            }
                
        }





        private  uint HashID = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.openConnection();

            MainWindow.SQL = "use Users; insert into UserEntity(ID,Name,Surname,Email,Phone,Age) values ('"+(HashID).ToString()+"','"+TBname.Text+"','"+Surname.Text+"','"+Email.Text+"','"+Phone.Text+"','"+Age.Text+"');";
            MainWindow.cmd.CommandType = CommandType.Text;
            MainWindow.cmd.CommandText = MainWindow.SQL;

            int LCount = MainWindow.cmd.ExecuteNonQuery();

            if (LCount > 0) MessageBox.Show("Регистрация прошла успешна!\nЗатронуто строк:"+LCount.ToString());

            MainWindow.closeConnection();
        }
    }
}
