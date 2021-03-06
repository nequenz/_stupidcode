﻿using System;
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
        public static List<Folder>  FolderList;      //V2

        public static Folder[]      MyNameFilters  = null;
        public static readonly int  FilterMaxCount = 100;

        public Folder(string _Name="Нет названия")
        {
            Name        = _Name;
            NameFilters = new List<string>();
        }

        public string       getName()
        {
            return Name;
        }

        public List<string> getNameFilters()
        {
            return NameFilters;
        }

        public int          getCount()
        {
            return Count;
        }

        public void         setCount(int C)
        {
            Count = C;
        }


        //V2
        public static void      setStandartFolders()
        {
            FolderList  = new List<Folder>();
            Folder F    = new Folder("Все");
            FolderList.Add(F);
        }

        public static void      loadFolderListTo(ListBox LB)
        {
            LB.Items.Clear();

            for (int i = 0; i < FolderList.Count; i++)
            {
                LB.Items.Add(FolderList[i].getName());
            }
        }
        
        public static Folder    getFolderByName(string name)
        {
            for(int i = 0; i < FolderList.Count; i++)
            {
                if (FolderList[i].getName() == name)
                {
                    return FolderList[i];
                }
            }

            return null;
        }

        //--


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

        public static int               AccountID       = 0;
        public static readonly string[] RootsStrings    = { "Роль [Пользователь]", "Роль [Администратор]" };
        public static bool              AdminRoot       = false;


        public MainWindow()
        {
            InitializeComponent();
        }


        private void Window_Loaded(object Sender,RoutedEventArgs e)
        {
            int RootID                  = Int32.Parse(getAccountByID(AccountID)[5]);
            AccountNameLabel.Content    = "Вы зашли под "+getAccountByID(AccountID)[3]+" "+getAccountByID(AccountID)[4]+" "+ RootsStrings[RootID];

            if (RootID == 1)
            {
                AdminRoot = true;
                AdminRootLabel.Content = "Вы обладаете правами администратора";
            }
            else
            {
                AdminRoot               = false;
                AdminRootLabel.Content  = "Вы не обладаете правами администратора";
            }


            Folder.setStandartFolders();
            Folder.loadFolderListTo(FoldersList);
        }



        public static String    GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["conString"].ToString();
        }

        public static void      openConnection()
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

        public static void      closeConnection()
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

        public static void      executeSQL(string Code)
        {
            MainWindow.openConnection();

            MainWindow.DT.Clear();

            MainWindow.SQL = Code;
            MainWindow.cmd.CommandType = System.Data.CommandType.Text;
            MainWindow.cmd.CommandText = MainWindow.SQL;

            MainWindow.Adapter.SelectCommand = MainWindow.cmd;
            MainWindow.Adapter.Fill(MainWindow.DT);

            MainWindow.closeConnection();
        }

        public static string    getStringFromSql(string str)
        {
            int SIndex = str.IndexOf(' ');
            if (SIndex == -1) SIndex = str.Length;

            return str.Substring(0, SIndex);
        }

        public static string[]  getAccountByID(int ID)
        {
            executeSQL("SELECT [ID],[EmailName],[Name],[Surname],[Roots],[Password] FROM [Account]");

            string[] Acc = new string[6];

            Acc[0] = "Undefined";
            Acc[1] = "Undefined";
            Acc[2] = "Undefined";
            Acc[3] = "Undefined";
            Acc[4] = "Undefined";
            Acc[5] = "Undefined";

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string LID  = getStringFromSql(DT.Rows[i].ItemArray[2].ToString());

                if (ID.ToString() == LID)
                {

                    Acc[0]          = getStringFromSql(DT.Rows[i].ItemArray[0].ToString());
                    Acc[1]          = getStringFromSql(DT.Rows[i].ItemArray[1].ToString());
                    Acc[2]          = getStringFromSql(DT.Rows[i].ItemArray[2].ToString());
                    Acc[3]          = getStringFromSql(DT.Rows[i].ItemArray[3].ToString());
                    Acc[4]          = getStringFromSql(DT.Rows[i].ItemArray[4].ToString());
                    Acc[5]          = getStringFromSql(DT.Rows[i].ItemArray[5].ToString());

                    return Acc;
                }

            }

            return Acc;

        }

        public static string[]  getAccountByLogin(string Login)
        {
            executeSQL("SELECT [ID],[EmailName],[Name],[Surname],[Roots],[Password] FROM [Account]");

            string[] Acc = new string[6];

            Acc[0] = "Undefined";
            Acc[1] = "Undefined";
            Acc[2] = "Undefined";
            Acc[3] = "Undefined";
            Acc[4] = "Undefined";
            Acc[5] = "Undefined";

            for (int i = 0; i < DT.Rows.Count; i++)
            {
                string LID = getStringFromSql(DT.Rows[i].ItemArray[0].ToString());

                if (Login == LID)
                {

                    Acc[0] = getStringFromSql(DT.Rows[i].ItemArray[0].ToString());
                    Acc[1] = getStringFromSql(DT.Rows[i].ItemArray[1].ToString());
                    Acc[2] = getStringFromSql(DT.Rows[i].ItemArray[2].ToString());
                    Acc[3] = getStringFromSql(DT.Rows[i].ItemArray[3].ToString());
                    Acc[4] = getStringFromSql(DT.Rows[i].ItemArray[4].ToString());
                    Acc[5] = getStringFromSql(DT.Rows[i].ItemArray[5].ToString());

                    return Acc;
                }

            }

            return Acc;

        }

        public void             setNameFilters(int M)
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

        public void             chooseSendOrReceiveMode(int Mode)
        {
            if(FoldersList.SelectedItem == null) return;

            if((string)FoldersList.SelectedItem == "Все")
            {
                //send
                //receive
                string[] SqlMode    = {  "SELECT [Sender],[Recipient],[Text],[DateSend],[DataReceive] FROM [Messages] WHERE([Sender]='" + AccountID.ToString() + "')"
                                        ,"SELECT [Sender],[Recipient],[Text],[DateSend],[DataReceive] FROM [Messages] WHERE([Recipient]='" + AccountID.ToString() + ")"
                                        ,""};

                executeSQL(SqlMode[Mode]);

                for(int i = 0; i < DT.Rows.Count; i++)
                {

                        //to-do11111

                }

                return;

            }





            string Logins           = "SELECT [ID],[EmailName],[Name],[Surname] FROM [Account] WHERE(";
            Folder SelectedFolder   = Folder.getFolderByName(FoldersList.SelectedItem.ToString());

            for (int i = 0; i < SelectedFolder.getNameFilters().Count; i++)
            {
                if (i == 0)
                {
                    Logins += "[EmailName]='" + SelectedFolder.getNameFilters()[i] + "' ";
                }
                else
                {
                    Logins += " && [EmailName]='" + SelectedFolder.getNameFilters()[i] + "' ";
                }
                

                if (i == (SelectedFolder.getNameFilters().Count - 1))
                {
                    Logins += ")";
                }
            }

            executeSQL(Logins);


            Console.WriteLine(Logins);
        }

        public void             getSQLBaseFromMail()
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

        public static void      sendMessage(string _From,string _To,string _Theme,string _Text)
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

            chooseSendOrReceiveMode(0);

            /*
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
            */
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (FoldersList.SelectedItem == null) return;

            //EXCEPTION

            if (FoldersList.SelectedItem.ToString() == "Все")
            {
                MessageBox.Show("Нельзя удалить данную папку");
                return;
            }

            var Result = MessageBox.Show("Вы уверены что хотите удалить рабочую папку?", "Удаление папки",MessageBoxButton.YesNoCancel,MessageBoxImage.Question);

            if (Result == MessageBoxResult.Yes)
            {
                for(int i = 0; i < Folder.FolderList.Count; i++)
                {
                    if (Folder.FolderList[i].getName()==FoldersList.SelectedItem.ToString())
                    {
                        Folder.FolderList.RemoveAt(i);
                        Folder.loadFolderListTo(FoldersList);
                        return;
                    }
                    
                }
            }

        }
    }
}
