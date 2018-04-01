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

        List<StackPanel> SP;
        List<Image> Ava;
        List<TextBlock> NameOfPers;
        List<BitmapImage> image1;
        List<System.Windows.Controls.Label> KeepID;//невидимый лейбл, который будет хранить айди
        public Peoples(int id)
        {
            SqlAction sql = new SqlAction();
            ID = id;
            InitializeComponent();

            //Стоит сделать ее 1-й фнкцией т к повторяется 3 раза
            Pers[] Persons = sql.StateOf(ID,1);
            Array.Sort(Persons);
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
                image1 = new List<BitmapImage>();
                KeepID = new List<System.Windows.Controls.Label>();
            for (int i = 0; i < Persons.Length; i++)
            {
                //               

                if (Persons[i].id != ID)
                {
                    SP.Add(new StackPanel());
                    Ava.Add(new Image());
                    NameOfPers.Add(new TextBlock());
                    KeepID.Add(new System.Windows.Controls.Label());

                    NameOfPers[i].Text = " "+Persons[i].FullNameToStr;
                    try
                    { 
                    image1.Add(new BitmapImage(new Uri(sql.GetData(Convert.ToInt32(Persons[i].id))[5])));
                    }
                    catch
                    {
                        image1.Add(new BitmapImage(new Uri("pack://siteoforigin:,,,/Resources/Empty.png")));
                    }
                    Ava[i].Source = image1[i];
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
        }



        private void LBP_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if(LBP.SelectedIndex>=0)
            { 
            string GetId = (KeepID[LBP.SelectedIndex].Content).ToString();
            Messages msg = new Messages(ID, Convert.ToInt32(GetId));
            //MessageBox.Show("ID" + Convert.ToString(ID), Convert.ToString(LBP.SelectedIndex));
            msg.Show();
                //MessageBox.Show(GetId, GetId);
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
