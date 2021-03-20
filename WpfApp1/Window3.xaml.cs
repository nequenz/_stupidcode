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
    /// Логика взаимодействия для Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        public ListBox FolderList;
        public Window3(ListBox LB)
        {
            FolderList = LB;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (FolderNameBox.Text == "")
            {
                MessageBox.Show("Введите имя рабочей папки");
                return;
            }

            foreach(Folder F in Folder.FolderList)
            {

                if (F.getName() == FolderNameBox.Text)
                {
                    MessageBox.Show("Название такой папки уже существует!");
                    return;
                }

            }

            Folder.FolderList.Add(new Folder(FolderNameBox.Text));
            Folder.loadFolderListTo(FolderList);

            Close();

        }
    }
}
