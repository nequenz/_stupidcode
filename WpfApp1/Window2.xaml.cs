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


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void AddressBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            List<string> NamesList = Folder.getNamesFromFilter(AddressBox.Text);

            if (NamesBox != null)
            {
                NamesBox.Items.Clear();
            }
            
            if (NamesList.Count == 0)
            {
                return;
            }

            foreach(string name in NamesList)
            {
                NamesBox.Items.Add(name);
            }


        }

        private void NamesBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (NamesBox.SelectedItem != null)
            {
                AddressBox.Text = NamesBox.SelectedItem.ToString();
            }
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {

            if (AddressBox.Text == "")
            {
                System.Windows.MessageBox.Show("Введите имя адресата!");
            }
            else
            {
                string MessageText = new TextRange(MessageBox.Document.ContentStart, MessageBox.Document.ContentEnd).Text;
                MainWindow.sendMessage(MainWindow.YourMailAddress, AddressBox.Text, ThemeBox.Text, MessageText);
                System.Windows.MessageBox.Show("Сообщение отправлено!");
                Close();
            }
        }
    }



}
