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
    /// Логика взаимодействия для Peoples.xaml
    /// </summary>
    public partial class Peoples : Window
    {
        int ID;
        SqlAction sql;
        System.Windows.Forms.Timer tm;
        List<StackPanel> SP;
        List<Image> Ava;
        List<TextBlock> NameOfPers;
        List<BitmapImage> image;
        List<System.Windows.Controls.Label> KeepID;//невидимый лейбл, который будет хранить айди
        Pers[] Persons;

        List<StackPanel> SP2;
        List<Image> ChatAva;
        List<TextBlock> ChatName;
        List<System.Windows.Controls.Label> ChatId;//невидимый лейбл, который будет хранить айди
        Chat[] chats;
        public Peoples(int id)
        {
            sql = new SqlAction();
            ID = id;
            InitializeComponent();
            Refresh(null, null);
            tm = new System.Windows.Forms.Timer();
            tm.Interval = 5000;
            //tm.Tick += new EventHandler(MessageBox.Show("HI"));
            tm.Tick += Refresh;
            tm.Enabled = true;

        }
         void Refresh(object sender, EventArgs e)
        {
            LBC.Items.Clear();
            LBP.Items.Clear();
            Persons = sql.StateOf(ID, 1);
            Array.Sort(Persons);
            chats = sql.GetChats(ID);
            Array.Sort(chats);
            //for (int i = 0; i < Persons.Length; i++)
            //{
            //    if(Persons[i].id!= ID)
            //    { 
            //    LBP.Items.Add(Persons[i]);
            //    //Persons[i].FullNameToStr = Persons[i].FirstName + " " + Persons[i].LastName;
            //    LBP.DisplayMemberPath = "FullNameToStr";
            //    }
            //}
            Ava = new List<Image>();
            NameOfPers = new List<TextBlock>();
            SP = new List<StackPanel>();
            image = new List<BitmapImage>();
            KeepID = new List<System.Windows.Controls.Label>();

            ChatAva = new List<Image>();
            ChatName = new List<TextBlock>();
            SP2 = new List<StackPanel>();
            ChatId = new List<System.Windows.Controls.Label>();
            for (int i = 0; i < Persons.Length; i++)
            {
                //               

                if (Persons[i].id != ID)
                {
                    SP.Add(new StackPanel());
                    Ava.Add(new Image());
                    NameOfPers.Add(new TextBlock());
                    KeepID.Add(new System.Windows.Controls.Label());

                    NameOfPers[i].Text = " " + Persons[i].FullNameToStr;
                    try
                    {
                        image.Add(new BitmapImage(new Uri(sql.GetData(Convert.ToInt32(Persons[i].id))[5])));
                    }
                    catch
                    {
                        image.Add(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Empty.png")));
                    }
                    Ava[i].Source = image[i];
                    KeepID[i].Content = Convert.ToString(Persons[i].id);

                    SP[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                    Ava[i].Height = 30;
                    Ava[i].Width = 50;
                    Ava[i].Margin = new Thickness(0, 0, 0, 0);
                    NameOfPers[i].Width = 200;
                    NameOfPers[i].Height = Double.NaN;
                    NameOfPers[i].TextWrapping = TextWrapping.Wrap;
                    //time[i].Foreground = new SolidColorBrush(Colors.Red);

                    //time[i].HorizontalAlignment = Left;
                    //time[i].Color = "grey";

                    SP[i].Children.Add(Ava[i]);
                    SP[i].Children.Add(NameOfPers[i]);
                    LBP.Items.Add(SP[i]);

                    //LBP.Items.Add(Persons[i]);
                    // LBP.DisplayMemberPath = "FullNameToStr";

                }
                //for (int i = 0; i < Persons.Length; i++)
                //    {
                //        LBS.Items.Add(Persons[i]);
                //        LBS.DisplayMemberPath = "FullNameToStr";
                //    }

            }
            //ListCollectionView view = new ListCollectionView(Persons);
            //view.SortDescriptions.Add(new System.ComponentModel.SortDescription("FirstName",
            // System.ComponentModel.ListSortDirection.Ascending));
            //view.SortDescriptions.Add(new System.ComponentModel.SortDescription("LastName",
            //  System.ComponentModel.ListSortDirection.Ascending));

            //view.Refresh();
            SP2.Add(new StackPanel());
            ChatAva.Add(new Image());
            ChatName.Add(new TextBlock());
            ChatId.Add(new System.Windows.Controls.Label());
            image = new List<BitmapImage>();
            for (int i = 0; i < chats.Length; i++)
            {
                ChatName[i].Text = " " + chats[i].Name;
                try
                {
                    image.Add(new BitmapImage(new Uri(chats[i].PathToPhoto)));
                }
                catch
                {
                    image.Add(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Empty.png")));
                }
                ChatAva[i].Source = image[i];
                ChatId[i].Content = Convert.ToString(chats[i].ID);
                SP2[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                ChatAva[i].Height = 30;
                ChatAva[i].Width = 50;
                ChatAva[i].Margin = new Thickness(0, 0, 0, 0);
                ChatName[i].Width = 200;
                ChatName[i].Height = Double.NaN;
                ChatName[i].TextWrapping = TextWrapping.Wrap;
                SP2[i].Children.Add(ChatAva[i]);
                SP2[i].Children.Add(ChatName[i]);
                LBC.Items.Add(SP2[i]);
            }
        }
        private void LBP_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (LBP.SelectedIndex >= 0)
            {
                string GetId = (KeepID[LBP.SelectedIndex].Content).ToString();
                Messages msg = new Messages(ID, Convert.ToInt32(GetId));
                //MessageBox.Show("ID" + Convert.ToString(ID), Convert.ToString(LBP.SelectedIndex));
                msg.Show();
                //MessageBox.Show(GetId, GetId);
            }
        }


        private void NewChat_Click(object sender, RoutedEventArgs e)
        {
                    PeopleInChat PIC = new PeopleInChat(ID);
                    PIC.ShowDialog();
        }




        private void LBC_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LBC.SelectedIndex >= 0  )
            {
                if (ID == chats[LBC.SelectedIndex].IdOfCreator)
                {
                    PeopleInChat PIC = new PeopleInChat(chats[LBC.SelectedIndex]);
                    PIC.ShowDialog();
                }
                else MessageBox.Show("You aren't owner of this chat", "Access denied");
            }
        }

        private void LBC_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (LBC.SelectedIndex >= 0)
            {
                Messages msg = new Messages(ID, Convert.ToInt32(ChatId[LBC.SelectedIndex].Content), true);
                msg.Show();
            }
        }




        /*
        private void LBP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show(listBox1.SelectedIndex.ToString());
            Messages msg = new Messages(ID, LBP.SelectedIndex + 1);
            msg.Show();

        }
        */

    }
}
