using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    /// 

    /*
    public partial class Peoples : Window
    {
        public Peoples(int id)
        {
            SqlAction sql = new SqlAction();
            ID = id;
            InitializeComponent();
            string[] PeopleName = sql.GetPeople(ID);
            for (int i = 0; i < PeopleName.Length; i++)
                LBP.Items.Add(PeopleName[i]);
        }
    }*/
    public partial class Search : Window
    {
        int ID;
        SqlAction sql;
        List<StackPanel> SP;
        List<Image> Ava;
        List<TextBlock> NameOfPers;
        List<BitmapImage> image1;
        List<System.Windows.Controls.Label> KeepID;//невидимый лейбл, который будет хранить айди
        public Search(int id)
        {
            sql = new SqlAction();
            ID = id;
            InitializeComponent();


            //string[] PeopleName = sql.GetPeople();
            //for (int i = 0; i < PeopleName.Length; i++)
            //    LBS.Items.Add(PeopleName[i]);


            //Стоит сделать ее 1-й фнкцией т к повторяется 3 раза
            Pers[] Persons = sql.GetPeople();
            Array.Sort(Persons);

            Ava = new List<Image>();
            NameOfPers = new List<TextBlock>();
            SP = new List<StackPanel>();
            image1 = new List<BitmapImage>();
            KeepID = new List<System.Windows.Controls.Label>();
            int j = 0;
            for (int i = 0; i < Persons.Length; i++)
            {
                if (Persons[i].id != ID)
                {
                    SP.Add(new StackPanel());
                    Ava.Add(new Image());
                    NameOfPers.Add(new TextBlock());
                    KeepID.Add(new System.Windows.Controls.Label());

                    NameOfPers[j].Text = " " + Persons[i].FullNameToStr;
                    try
                    {
                        image1.Add(new BitmapImage(new Uri(sql.GetData(Convert.ToInt32(Persons[i].id))[5])));
                    }
                    catch
                    {
                        image1.Add(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Empty.png")));
                    }
                    Ava[j].Source = image1[j];
                    KeepID[j].Content = Convert.ToString(Persons[i].id);

                    SP[j].Orientation = System.Windows.Controls.Orientation.Horizontal;
                    Ava[j].Height = 30;
                    Ava[j].Width = 50;
                    Ava[j].Margin = new Thickness(0, 0, 0, 0);
                    NameOfPers[j].Width = 200;
                    NameOfPers[j].Height = Double.NaN;
                    NameOfPers[j].TextWrapping = TextWrapping.Wrap;
                    //time[i].Foreground = new SolidColorBrush(Colors.Red);

                    //time[i].HorizontalAlignment = Left;
                    //time[i].Color = "grey";

                    SP[j].Children.Add(Ava[j]);
                    SP[j].Children.Add(NameOfPers[j]);
                    LBS.Items.Add(SP[j]);

                    //LBP.Items.Add(Persons[i]);
                    // LBP.DisplayMemberPath = "FullNameToStr";

                    //for (int i = 0; i < Persons.Length; i++)
                    //    {
                    //        LBS.Items.Add(Persons[i]);
                    //        LBS.DisplayMemberPath = "FullNameToStr";
                    //    }
                    j++;
                }
            }
            //for (int i = 0; i < Persons.Length; i++)
            //{
            //    LBS.Items.Add(Persons[i]);
            //    //Persons[i].FullNameToStr = Persons[i].FirstName + " " + Persons[i].LastName;
            //    LBS.DisplayMemberPath = "FullNameToStr";
            //}
            //ListCollectionView view = new ListCollectionView(Persons);
            //view.SortDescriptions.Add(new System.ComponentModel.SortDescription("FirstName",
            // System.ComponentModel.ListSortDirection.Ascending));
            //view.SortDescriptions.Add(new System.ComponentModel.SortDescription("LastName",
            //  System.ComponentModel.ListSortDirection.Ascending));

            //view.Refresh();

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            string ToSearch = Name.Text;
            int[] id = (sql.Search(ToSearch)).ToArray();

            // MessageBox.Show(Convert.ToString(ID), Convert.ToString(ID));

            LBS.Items.Clear();
            Pers[] Persons = sql.GetPeople(id);
            Array.Sort(Persons);
            //for (int i = 0; i < Persons.Length; i++)
            ////LBS.Items.Add(Persons[i].FirstName+" "+ Persons[i].LastName);
            //{
            //    LBS.Items.Add(Persons[i]);
            //    //Persons[i].FullNameToStr = Persons[i].FirstName +" "+ Persons[i].LastName;
            //    LBS.DisplayMemberPath = "FullNameToStr";
            //    //LBS.SelectedValuePath = "FirstName";
            //    //LBS.SelectedValuePath = Persons[i].LastName;
            //}
            //LBS.Items.Insert(2,PeopleName[2]+"!");
            Ava = new List<Image>();
            NameOfPers = new List<TextBlock>();
            SP = new List<StackPanel>();
            image1 = new List<BitmapImage>();
            KeepID = new List<System.Windows.Controls.Label>();
            int j = 0;
            for (int i = 0; i < Persons.Length; i++)
            {
                if (Persons[i].id != ID)
                {
                    SP.Add(new StackPanel());
                    Ava.Add(new Image());
                    NameOfPers.Add(new TextBlock());
                    KeepID.Add(new System.Windows.Controls.Label());

                    NameOfPers[j].Text = " " + Persons[i].FullNameToStr;
                    try
                    {
                        image1.Add(new BitmapImage(new Uri(sql.GetData(Convert.ToInt32(Persons[i].id))[5])));
                    }
                    catch
                    {
                        image1.Add(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Empty.png")));
                    }
                    Ava[j].Source = image1[j];
                    KeepID[j].Content = Convert.ToString(Persons[i].id);

                    SP[j].Orientation = System.Windows.Controls.Orientation.Horizontal;
                    Ava[j].Height = 30;
                    Ava[j].Width = 50;
                    Ava[j].Margin = new Thickness(0, 0, 0, 0);
                    NameOfPers[j].Width = 200;
                    NameOfPers[j].Height = Double.NaN;
                    NameOfPers[j].TextWrapping = TextWrapping.Wrap;
                    //time[i].Foreground = new SolidColorBrush(Colors.Red);

                    //time[i].HorizontalAlignment = Left;
                    //time[i].Color = "grey";

                    SP[j].Children.Add(Ava[j]);
                    SP[j].Children.Add(NameOfPers[j]);
                    LBS.Items.Add(SP[j]);

                    //LBP.Items.Add(Persons[i]);
                    // LBP.DisplayMemberPath = "FullNameToStr";

                    //for (int i = 0; i < Persons.Length; i++)
                    //    {
                    //        LBS.Items.Add(Persons[i]);
                    //        LBS.DisplayMemberPath = "FullNameToStr";
                    //    }
                    j++;
                }
            }


            //ListCollectionView view = new ListCollectionView(Persons);
            //view.SortDescriptions.Add(new System.ComponentModel.SortDescription("FirstName",
            // System.ComponentModel.ListSortDirection.Ascending));
            //view.SortDescriptions.Add(new System.ComponentModel.SortDescription("LastName",
            //  System.ComponentModel.ListSortDirection.Ascending));

            //view.Refresh();
            //LBS.Items.SortDescriptions.Add(new SortDescription("", ListSortDirection.Ascending));
            //MessageBox.Show(Convert.ToString(LBS.Items[0]), Convert.ToString(LBS.Items[0]));
        }

        private void listBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //Messages msg = new Messages(ID, LBS.SelectedIndex + 1);
            //MessageBox.Show("ID" + Convert.ToString(ID), Convert.ToString(LBP.SelectedIndex));
            //msg.Show();
            if(LBS.SelectedIndex>=0)
            { 
            string GetId = (KeepID[LBS.SelectedIndex].Content).ToString();
            //string GetId = ((LBS.SelectedItem) as Pers).id.ToString();
            //MessageBox.Show(GetId, GetId);
            StrangerPage SP = new StrangerPage(ID, Convert.ToInt32(GetId));//LBS.SelectedIndex
            SP.Show();
            }
        }
    }
}






