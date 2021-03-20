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
    /// 
    public class Folder
    {
        //Экземляр рабочей папки
        private string          Name;
        private List<string>    NameFilters;

        private int             Count=0;

        //статичные данные
        public static Folder[]   MyNameFilters;
        public static readonly int          FilterMaxCount =100;

        public Folder(string _Name="Нет названия")
        {
            Name        = _Name;
            NameFilters = new List<string>();
        }

        public string getName()
        {
            return Name;
        }

        public List<string> getNameFilters()
        {
            return NameFilters;
        }

        public void setCount(int C)
        {
            Count = C;
        }

        public int getCount()
        {
            return Count;
        }

        public static void resetStandartNameFilters(ListBox LB)
        {
            MyNameFilters       = new Folder[FilterMaxCount];

            MyNameFilters[0]    = new Folder("Начальство");

            MyNameFilters[0].NameFilters.Add("Директор И.И А*****");
            MyNameFilters[0].NameFilters.Add("Начальник сектора 01 А.П Р*****");
            MyNameFilters[0].NameFilters.Add("Начальник сектора 02 H.T Е*****");
            MyNameFilters[0].NameFilters.Add("Заместитель начальника сектора 02 О.О Л*****");



            MyNameFilters[1] = new Folder("Рабочая рассылка");

            MyNameFilters[1].NameFilters.Add("Рассылка от сектора 01");
            MyNameFilters[1].NameFilters.Add("Рассылка от сектора 02");
            MyNameFilters[1].NameFilters.Add("Рассылка от турагента 01");


            MyNameFilters[2] = new Folder("Спам");

            MyNameFilters[2].NameFilters.Add("Anywhere@box.ru");
            MyNameFilters[2].NameFilters.Add("NOTSPAM");
            MyNameFilters[2].NameFilters.Add("NOSPAMTOO");

            MyNameFilters[3] = new Folder("Все");

            MyNameFilters[3].NameFilters.Add("ALL");
 

            if (LB!=null)
            {
                LB.Items.Clear();

                for(int i = 0; i < MyNameFilters.Length; i++)
                {
                    if (MyNameFilters[i] != null)
                    {
                        LB.Items.Add(MyNameFilters[i].Name);
                    }
                    
                }


            }

        }

        public static Folder getFilterByName(string name)
        {
            for (int i = 0; i < MyNameFilters.Length; i++)
            {
                if (MyNameFilters[i] != null && MyNameFilters[i].getName() == name)
                {
                    return MyNameFilters[i];
                }
            }

            return null;
        }

        public static List<string> getNamesFromFilter(string name)
        {
            List<string> RetList = new List<string>();

            if (name == "")
            {
                return RetList;
            }

            for (int i = 0; i < MyNameFilters.Length; i++)
            {
                if (MyNameFilters[i] != null)
                {

                    for(int j = 0; j < MyNameFilters[i].getNameFilters().Count; j++)
                    {


                        string NameFromFilter = MyNameFilters[i].getNameFilters()[j];
                        byte IsIt = 1;

                        int Len = NameFromFilter.Length - name.Length;

                        if (Len >= 0)
                        {
                            Len = name.Length;
                        }
                        else
                        {
                            Len = NameFromFilter.Length;
                        }

                        for(int c = 0; c < Len; c++)
                        {
                            if (name[c] == NameFromFilter[c])
                            {
                                IsIt *= 1;
                            }
                            else
                            {
                                IsIt *= 0;
                            }

                        }

                        if (IsIt == 1)
                        {
                            RetList.Add(NameFromFilter);
                            continue;
                        }



                    }

                }

            }

            return RetList;
        }


    }

    public partial class MainWindow : Window
    {

        public static string            SQL;
        public static SqlDataReader     SqlReader;
        public static SqlConnection     SQLC            = new SqlConnection();
        public static SqlCommand        cmd             = new SqlCommand("", SQLC);
        public static DataTable         DT              = new DataTable();
        public static SqlDataAdapter    Adapter         = new SqlDataAdapter();
        public static bool              Emulation       = false;
        public static string            YourMailAddress = "Студент Бонча";
        public static Window1           WindowFolderSettting = null;
        public static string            MailSQLCode     = "";
        private object                  PrevSelected    = null;



        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object Sender,RoutedEventArgs e)
        {
            Folder.resetStandartNameFilters(FoldersList);

        }

        public static bool isWindowOpened(Type WType)
        {
            foreach (Window openWin in System.Windows.Application.Current.Windows)
            {
                if (openWin.GetType() == WType)
                    return true;
            }

            return false;
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

        public void setNameFilters(int M)
        {
            string Where = "";

            //exception

            if (FoldersList.SelectedItem == null) return;

            if (FoldersList.SelectedItem.ToString()=="Все")
            {
                if (M == 0)
                {
                    MailSQLCode = "SELECT [From], [To], Text, ReceiveData, SendData, ServiceInfo FROM Mail WHERE ([To] <> '" + YourMailAddress + "')";
                }
                else
                {
                    MailSQLCode = "SELECT [From], [To], Text, ReceiveData, SendData, ServiceInfo FROM Mail WHERE ([From] <> '"+YourMailAddress+"')";
                }

                //MailSQLCode="SELECT [From], [To], Text, ReceiveData, SendData, ServiceInfo FROM Mail";
                return;
            }



            if (M == 0)
            {
                Where = "WHERE([From]='" + YourMailAddress + "' AND";
                Folder Filter = Folder.getFilterByName(FoldersList.SelectedItem.ToString());

                for(int i=0;i< Filter.getNameFilters().Count; i++)
                {
                    Where+=" [To]='"+Filter.getNameFilters()[i]+"'";

                    if (i < (Filter.getNameFilters().Count - 1))
                    {
                        Where += " OR";
                    }
                }

                Where += ")";

                MailSQLCode = "SELECT [From], [To], Text, ReceiveData, SendData, ServiceInfo FROM Mail " + Where;
            }
            else if(M == 1)
            {
                Where = "WHERE([To]='" + YourMailAddress + "' AND";
                Folder Filter = Folder.getFilterByName(FoldersList.SelectedItem.ToString());

                for (int i = 0; i < Filter.getNameFilters().Count; i++)
                {
                    Where += " [From]='" + Filter.getNameFilters()[i] + "'";

                    if(i< (Filter.getNameFilters().Count-1))
                    {
                        Where += " OR";
                    }

                }

                Where += ")";

                
            }


            MailSQLCode = "SELECT [From], [To], Text, ReceiveData, SendData, ServiceInfo FROM Mail " + Where;

        }


        

        public void getSQLBaseFromMail()
        {
            openConnection();

            DT.Clear();
            MessageListBox.Items.Clear();

            SQL = MailSQLCode;
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = SQL;

            Adapter.SelectCommand = cmd;
            Adapter.Fill(DT);

            

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string Data     = DT.Rows[i].ItemArray[3].ToString();
                string From     = DT.Rows[i].ItemArray[0].ToString();
                string To       = DT.Rows[i].ItemArray[1].ToString();
                string Theme    = DT.Rows[i].ItemArray[2].ToString();

                int Index=Theme.IndexOf('@');

                if (Index == -1) {
                    if (Theme.Length >= 15)
                    {
                        Index = 15;
                    }
                    else
                    {
                        Index = Theme.Length;
                    }
                } 

                Theme = Theme.Substring(0, Index);
                



                string Result   =(i+1) + ".     "+Data + "  "+From+"  "+Theme;
   
               

                MessageListBox.Items.Add(Result);
            }


            closeConnection();

        }

        public static void sendMessage(string _From,string _To,string _Theme,string _Text)
        {
            string From = _From;
            string To   = _To;
            string Text = _Theme + "@" + _Text;
            string ReceiveData  = DateTime.Now.ToString();
            string SendData     = DateTime.Now.ToString();
            string ServiceInfo  = "NULL";

            openConnection();

            SQL = "use MailSQL; insert into Mail([From],[To],Text,ReceiveData,SendData,ServiceInfo) values ('" + From + "','" + To + "','" + Text + "','" + ReceiveData + "','" + SendData + "','" + ServiceInfo + "');";
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = SQL;
            Adapter.SelectCommand = cmd;


            int LCount = cmd.ExecuteNonQuery();

            closeConnection();


        }

   


        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            //--
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            //---
        }

        private void SelfSend_Click(object sender, RoutedEventArgs e)
        { 

            Random R            = new Random();
            List<int> Findex    = new List<int>();

            for(int i=0;i< Folder.MyNameFilters.Length; i++)
            {
                if (Folder.MyNameFilters[i] != null)
                {
                    Findex.Add(i);
                }

            }
            Console.WriteLine(Findex.ToString());
            int     RandomFolderIndex   = R.Next(0, Findex.Count);
            int     RandomFolderAddress = R.Next(0, Folder.MyNameFilters[Findex[RandomFolderIndex]].getNameFilters().Count);

            if (Folder.MyNameFilters[Findex[RandomFolderIndex]].getNameFilters().Count == 0)
            {
                return;
            }

            string  RandomAddress       =           Folder.MyNameFilters[Findex[RandomFolderIndex]].getNameFilters()[RandomFolderAddress];
            



            sendMessage(RandomAddress,YourMailAddress, "Случайное сообщение", "Тестовое сообщение от " + RandomAddress + "!@ Привет, это было случайное сообщение!");


        }

        private void SettingMailFolder_Click(object sender, RoutedEventArgs e)
        {
            if (FoldersList.SelectedItem != null)
            {
                WindowFolderSettting = new Window1();
                WindowFolderSettting.Show();
                WindowFolderSettting.loadFolder(FoldersList.SelectedItem.ToString());
            }
            

        }

        private void FoldersList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        private void SendAndReceiveButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton R0 = (RadioButton)MailModeBox.Children[0];
            RadioButton R1 = (RadioButton)MailModeBox.Children[1];
            RadioButton R2 = (RadioButton)MailModeBox.Children[2];

            if (FoldersList.SelectedItem == null) return;

            if (R0.IsChecked==true)
            {
                setNameFilters(1);
                getSQLBaseFromMail();
                MessageCountButton.Content ="Сообщения: "+ MessageListBox.Items.Count.ToString();
            }

            if (R1.IsChecked == true)
            {
                setNameFilters(0);
                getSQLBaseFromMail();
                MessageCountButton.Content = "Сообщения: "+ MessageListBox.Items.Count.ToString();
            }
        }

        private void SendMailButton_Click(object sender, RoutedEventArgs e)
        {
            Window2 W = new Window2();
            W.Show();
        }

        private void MessageListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MessageShow.Document.Blocks.Clear();

            if (MessageListBox.SelectedIndex == -1)
            {
                return;
            }

            string  Text    = DT.Rows[MessageListBox.SelectedIndex].ItemArray[2].ToString();
            int     Index   = Text.IndexOf('@');

            if (Index == -1)
            {
                MessageShow.AppendText(Text);
            }
            else
            {
                MessageShow.AppendText(Text.Substring(Index+1));
            }

        }

        //Delete
        private void SendMailButton_Copy_Click(object sender, RoutedEventArgs e)
        {

        }

        private void MessageShow_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddEmailFolder_Click(object sender, RoutedEventArgs e)
        {
            Window3 W= new Window3(FoldersList);
            W.Show();

        }
    }
}
