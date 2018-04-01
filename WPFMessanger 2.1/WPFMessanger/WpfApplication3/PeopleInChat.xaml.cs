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

namespace WpfApplication3
{
    /// <summary>
    /// Логика взаимодействия для PeopleInChat.xaml
    /// </summary>
    public partial class PeopleInChat : Window
    {
        int ID;
        SqlAction sql;
        Chat ChatData;
        Pers[] Tmp;
        List<Pers> AddedPersons;
        List<Pers> Persons;
        List<StackPanel> SP;
        List<Image> AvaOfPers;
        List<TextBlock> NameOfPers;
        List<System.Windows.Controls.Label> IDofExistMembers, IDofMembers;
        string PathToPhoto;
        bool isCreated;
        public PeopleInChat(Chat ChatData)
        {
            this.ChatData = ChatData;
            this.ID = ChatData.ID;
            isCreated = true;
            InitializeComponent();
            try
            {
                this.ChatAva.Source = new BitmapImage(new Uri(ChatData.PathToPhoto));
            }
            catch
            {
                this.ChatAva.Source = new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Empty.png"));
            }
            name.Text = ChatData.Name;

            Refresh(null, null);

        }
        public PeopleInChat(int ID)
        {
            this.ID =ID;
            InitializeComponent();
            this.ChatAva.Source = new BitmapImage(new Uri(PathToPhoto = "pack://siteoforigin:,,,/Resources/Empty.png"));
            name.Text = "NewChatName";
            isCreated = false;
            newChat(null, null);

        }
        public void newChat(object sender, EventArgs e)//обновление окна сообщений
        {

            LBA.Items.Clear();
            sql = new SqlAction();
            Persons = new List<Pers>();
            IDofExistMembers = new List<System.Windows.Controls.Label>();
            Tmp = sql.StateOf(ID, 2);
            //sql.GetPeople();
            for (int i = 0; i < Tmp.Length; i++)
            {
                    Persons.Add(Tmp[i]);
            }
            Array.Sort(Tmp);
            AvaOfPers = new List<Image>();
            NameOfPers = new List<TextBlock>();
            SP = new List<StackPanel>();
            IDofMembers = new List<System.Windows.Controls.Label>();
            for (int i = 0; i < Persons.Count; i++)
            {
                if (sql.isBlackList(ID, Persons[i].id) || sql.isBlackList(Persons[i].id, ID))
                    sql.Action(ID, Persons[i].id, 2, false);

                if (Persons[i].id != ID && (!sql.isBlackList(ID, Persons[i].id) && !sql.isBlackList(Persons[i].id, ID)))
                {
                    SP.Add(new StackPanel());
                    AvaOfPers.Add(new Image());
                    NameOfPers.Add(new TextBlock());
                    IDofMembers.Add(new System.Windows.Controls.Label());

                    NameOfPers[i].Text = " " + Persons[i].FullNameToStr;
                    try
                    {
                        AvaOfPers[i].Source = (new BitmapImage(new Uri(sql.GetData(Convert.ToInt32(Persons[i].id))[5])));
                    }
                    catch
                    {
                        AvaOfPers[i].Source = (new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Empty.png")));
                    }
                    IDofMembers[i].Content = Convert.ToString(Persons[i].id);
                    IDofMembers[i].Visibility = Visibility.Hidden;
                    SP[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                    AvaOfPers[i].Height = 30;
                    AvaOfPers[i].Width = 50;
                    AvaOfPers[i].Margin = new Thickness(0, 0, 0, 0);
                    NameOfPers[i].Width = 200;
                    NameOfPers[i].Height = Double.NaN;
                    NameOfPers[i].TextWrapping = TextWrapping.Wrap;
                    SP[i].Children.Add(AvaOfPers[i]);
                    SP[i].Children.Add(NameOfPers[i]);
                    SP[i].Children.Add(IDofMembers[i]);
                    LBA.Items.Add(SP[i]);
                }
            }
        }
        //_____________________________________
        public void Refresh(object sender, EventArgs e)//обновление окна сообщений
        {
            Persons = new List<Pers>();
            AddedPersons = new List<Pers>();
            LBD.Items.Clear();
            sql = new SqlAction();
            //Persons = sql.StateOf(ID, 2);
            Tmp = sql.GetPeople();
            for (int i = 0; i < Tmp.Length; i++)
            {
                for (int j = 0; j < ChatData.Members.Count; j++)
                {
                    if (Tmp[i].id == ChatData.Members[j])
                        AddedPersons.Add(Tmp[i]);
                }
            }
            //Array.Sort(Persons);
            AddedPersons.Sort();

            AvaOfPers = new List<Image>();
            NameOfPers = new List<TextBlock>();
            SP = new List<StackPanel>();
            IDofExistMembers = new List<System.Windows.Controls.Label>();
            for (int i = 0; i < AddedPersons.Count; i++)
            {
                if (sql.isBlackList(ID, AddedPersons[i].id) || sql.isBlackList(AddedPersons[i].id, ID))
                    sql.Action(ID, AddedPersons[i].id, 2, false);

                if (AddedPersons[i].id != ChatData.IdOfCreator && (!sql.isBlackList(ID, AddedPersons[i].id) && !sql.isBlackList(AddedPersons[i].id, ID)))
                {
                    SP.Add(new StackPanel());
                    AvaOfPers.Add(new Image());
                    NameOfPers.Add(new TextBlock());
                    IDofExistMembers.Add(new System.Windows.Controls.Label());

                    NameOfPers[i].Text = " " + AddedPersons[i].FullNameToStr;
                    try
                    {
                        AvaOfPers[i].Source = (new BitmapImage(new Uri(sql.GetData(Convert.ToInt32(AddedPersons[i].id))[5])));
                    }
                    catch
                    {
                        AvaOfPers[i].Source = (new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Empty.png")));
                    }
                    IDofExistMembers[i].Content = Convert.ToString(AddedPersons[i].id);
                    IDofExistMembers[i].Visibility = Visibility.Hidden;
                    SP[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                    AvaOfPers[i].Height = 30;
                    AvaOfPers[i].Width = 50;
                    AvaOfPers[i].Margin = new Thickness(0, 0, 0, 0);
                    NameOfPers[i].Width = 200;
                    NameOfPers[i].Height = Double.NaN;
                    NameOfPers[i].TextWrapping = TextWrapping.Wrap;
                    SP[i].Children.Add(AvaOfPers[i]);
                    SP[i].Children.Add(NameOfPers[i]);
                    SP[i].Children.Add(IDofExistMembers[i]);
                    LBD.Items.Add(SP[i]);
                }
            }
            //______________
            LBA.Items.Clear();
            sql = new SqlAction();
            Tmp = sql.StateOf(ID, 2);
            //sql.GetPeople();
            bool isNotMember = true;
            for (int i = 0; i < Tmp.Length; i++)
            {
                isNotMember = true;
                for (int j = 0; j < AddedPersons.Count; j++)
                {
                    if (Tmp[i].id == AddedPersons[j].id)
                        isNotMember = false;
                }
                if(isNotMember)
                    Persons.Add(Tmp[i]);
            }
            Array.Sort(Tmp);
            AvaOfPers = new List<Image>();
            NameOfPers = new List<TextBlock>();
            SP = new List<StackPanel>();
            IDofMembers = new List<System.Windows.Controls.Label>();
            for (int i = 0; i < Persons.Count; i++)
            {
                if (sql.isBlackList(ID, Persons[i].id) || sql.isBlackList(Persons[i].id, ID))
                    sql.Action(ID, Persons[i].id, 2, false);

                if (Persons[i].id != ID && (!sql.isBlackList(ID, Persons[i].id) && !sql.isBlackList(Persons[i].id, ID)))
                {
                    SP.Add(new StackPanel());
                    AvaOfPers.Add(new Image());
                    NameOfPers.Add(new TextBlock());
                    IDofMembers.Add(new System.Windows.Controls.Label());

                    NameOfPers[i].Text = " " + Persons[i].FullNameToStr;
                    try
                    {
                        AvaOfPers[i].Source = (new BitmapImage(new Uri(sql.GetData(Convert.ToInt32(Persons[i].id))[5])));
                    }
                    catch
                    {
                        AvaOfPers[i].Source = (new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Empty.png")));
                    }
                    IDofMembers[i].Content = Convert.ToString(Persons[i].id);
                    IDofMembers[i].Visibility = Visibility.Hidden;
                    SP[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                    AvaOfPers[i].Height = 30;
                    AvaOfPers[i].Width = 50;
                    AvaOfPers[i].Margin = new Thickness(0, 0, 0, 0);
                    NameOfPers[i].Width = 200;
                    NameOfPers[i].Height = Double.NaN;
                    NameOfPers[i].TextWrapping = TextWrapping.Wrap;
                    SP[i].Children.Add(AvaOfPers[i]);
                    SP[i].Children.Add(NameOfPers[i]);
                    SP[i].Children.Add(IDofMembers[i]);
                    LBA.Items.Add(SP[i]);
                }
            }
        }

        private void LBA_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (LBA.SelectedIndex >= 0)
            {
                object LBI = LBA.Items[LBA.SelectedIndex];
                IDofExistMembers.Add(IDofMembers[LBA.SelectedIndex]);
                IDofMembers.Remove(IDofMembers[LBA.SelectedIndex]);
                LBA.Items.Remove(LBA.Items[LBA.SelectedIndex]);
                LBD.Items.Add(LBI);
            }
                //string GetId = (KeepIDOfPers[LBA.SelectedIndex].Content).ToString();
        }

        private void Apply_Click(object sender, RoutedEventArgs e)
        {
            if(isCreated)
            { 
            string MembersString= IDofExistMembers[0].Content.ToString();
            for (int i = 1; i < IDofExistMembers.Count; i++)
                MembersString +=' '+ IDofExistMembers[i].Content.ToString()  ;
            sql.EditChats(ID, MembersString, name.Text, ChatData.PathToPhoto);
            this.Close();
            }
            else
            {
                string MembersString = IDofExistMembers[0].Content.ToString();
                for (int i = 1; i < IDofExistMembers.Count; i++)
                    MembersString += ' ' + IDofExistMembers[i].Content.ToString();
                sql.newChat(name.Text,PathToPhoto,ID, MembersString );
                this.Close();
            }
        }

        private void ChatAva_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            SqlAction sql = new SqlAction();
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
            open.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(ChatData.PathToPhoto=open.FileName);
                BitmapImage image1 = new BitmapImage(new Uri(open.FileName));
                ChatAva.Source = image1;
            }
        }

        private void LBD_PreviewMouseUp(object sender, MouseButtonEventArgs e)//Существующие в беседе юзеры, клик- удалить
        {
            if (LBD.SelectedIndex >= 0)
            {
                object LBI = LBD.Items[LBD.SelectedIndex];
                IDofMembers.Add(IDofExistMembers[LBD.SelectedIndex] );
                IDofExistMembers.Remove(IDofExistMembers[LBD.SelectedIndex]);
                LBD.Items.Remove(LBD.Items[LBD.SelectedIndex]);
                LBA.Items.Add(LBI);
            }
        }
    }
}

