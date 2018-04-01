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
using System.Windows.Threading;
namespace WpfApplication3
{
    /// <summary>
    /// Логика взаимодействия для Friends.xaml
    /// </summary>
    public partial class Friends : Window
    {
        int ID;

        SqlAction sql;
        DispatcherTimer tm;
        List<StackPanel> SP;
        List<Image> Ava;
        List<TextBlock> NameOfPers;
        List<BitmapImage> image1;
        List<System.Windows.Controls.Label> KeepID;//невидимый лейбл, который будет хранить айди
        public Friends(int id)
        {
            ID = id;
            InitializeComponent();
            Refresh(null,null);

            tm = new DispatcherTimer();


            tm.Tick += new EventHandler(Refresh);
            tm.Interval = new TimeSpan(0, 0, 1);
            tm.Start();
        }
        //Стоит сделать ее 1-й фнкцией т к повторяется 4 раза
        public void Refresh(object sender, EventArgs e)//обновление окна сообщений
        {

            LBF.Items.Clear();
            sql = new SqlAction();
            Pers[] Persons = sql.StateOf(ID, 2);
            Array.Sort(Persons);
            Ava = new List<Image>();
            NameOfPers = new List<TextBlock>();
            SP = new List<StackPanel>();
            image1 = new List<BitmapImage>();
            KeepID = new List<System.Windows.Controls.Label>();
            for (int i = 0; i < Persons.Length; i++)
            {
                if(sql.isBlackList(ID,Persons[i].id) || sql.isBlackList(Persons[i].id, ID))
                    sql.Action(ID, Persons[i].id, 2, false);

                if (Persons[i].id != ID && (!sql.isBlackList(ID, Persons[i].id) && !sql.isBlackList(Persons[i].id,ID)))
                {
                    SP.Add(new StackPanel());
                    Ava.Add(new Image());
                    NameOfPers.Add(new TextBlock());
                    KeepID.Add(new System.Windows.Controls.Label());

                    NameOfPers[i].Text = " " + Persons[i].FullNameToStr;
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
                    SP[i].Children.Add(Ava[i]);
                    SP[i].Children.Add(NameOfPers[i]);
                    LBF.Items.Add(SP[i]);


                }
            }
        }


        private void LBP_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (LBF.SelectedIndex >= 0)
            { 
                string GetId = (KeepID[LBF.SelectedIndex].Content).ToString();
            StrangerPage SP = new StrangerPage(ID, Convert.ToInt32(GetId));//LBS.SelectedIndex

            SP.Show();
            }
        }
    }
}
