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

        public Folder CurrentFF; 




        public Window1()
        {
            InitializeComponent();
        }


        public void reloadFolder()
        {
            FolderDestinations.Items.Clear();

            for (int i = 0; i < CurrentFF.getNameFilters().Count; i++)
            {
                FolderDestinations.Items.Add(CurrentFF.getNameFilters()[i]);
            }

        }

        public void loadFolder(string FolderName)
        {

            CurrentFolder.Content   = FolderName;
            CurrentFF               = Folder.getFolderByName(FolderName);

            reloadFolder();
        }

        public bool searchDuplicates()
        {
            for(int i = 0; i < CurrentFF.getNameFilters().Count; i++)
            {
                if(NameDestination.Text == CurrentFF.getNameFilters()[i])
                {
                    return true;
                }
            }

            return false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < CurrentFF.getNameFilters().Count; i++)
            {
                if (CurrentFF.getNameFilters()[i] == FolderDestinations.SelectedItem.ToString())
                {
                    CurrentFF.getNameFilters().RemoveAt(i);
                    break;
                }
            }

            reloadFolder();

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (NameDestination.Text == "")
            {
                MessageBox.Show("Пустое имя. Введите полное имя адресата.");
                return;

            }else if(searchDuplicates())
            {
                MessageBox.Show("Данное имя уже используется. Введите другое имя адресата.");
                return;
            }
            else
            {
                CurrentFF.getNameFilters().Add(NameDestination.Text);
                reloadFolder();
                MessageBox.Show("Адресант добавлен.");
                return;
            }

        }
    }
}
