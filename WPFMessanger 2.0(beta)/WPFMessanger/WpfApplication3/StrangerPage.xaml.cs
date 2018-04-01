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

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
namespace WpfApplication3
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class StrangerPage : Window
    {
        Auentification main;
        String fileName;
        string Fname, Lname, Descript, Country, Photo;
        SqlAction sql;
        Friends FriendWindow;
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            sql = new SqlAction();
            sql.Action(mID, tID,1,true);
            Messages msg = new Messages(mID,tID);
            //MessageBox.Show("ID" + Convert.ToString(ID), Convert.ToString(LBP.SelectedIndex));
            msg.Show();
        }

        private void button_Click_2(object sender, RoutedEventArgs e)
        {
            if (sql.isBlackList(mID, tID))
            {
                sql.Action(mID, tID, 3, false);
                BL.Content = "Add to black list";
            }
            else
            {
                sql.Action(mID, tID, 3, true);
                sql.Action(mID, tID, 2, false);
                BL.Content = "Remove from black List";
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sql.isFriend(mID, tID))
            { 
                sql.Action(mID, tID, 2, false);
                ToFriend.Content = "Add to friend";

            }
            else
            {
                sql.Action(mID, tID, 2,true);
                ToFriend.Content = "Remove from friend";

            }

        }

        int age, mID,tID;//main id & this id
        public StrangerPage(int mid,int tid)
        {
            mID = mid;
            tID = tid;
            sql = new SqlAction();
            sql.GetData(tID);
            InitializeComponent();
            fileName = null;
            LBN.Content += sql.GetData(tID)[0] + " " + sql.GetData(tID)[1];
            LBA.Content += Convert.ToString(sql.GetData(tID)[2]);
            LBC.Content += sql.GetData(tID)[3];
            AbMe.Text = sql.GetData(tID)[4];
            //if (sql.GetData(Fname, Lname, Descript, age, Photo, Country, ID)[5] != "NULL")
            SetPict(sql.GetData(tID)[5]);
            if(sql.isFriend(mID,tID))
                ToFriend.Content = "Remove from friend";
            if (sql.isBlackList(mID, tID))
                BL.Content = "Remove from black List";

            if (sql.isBlackList(tID, mID))
            {
                MSG.IsEnabled = false;
                ToFriend.IsEnabled = false;

                ToFriend.Content = "Add to friend";
            }
        }




        void SetPict(string Path)
        {
            try
            {
                BitmapImage image1 = new BitmapImage(new Uri(Path));
                //pictureBox1.Size = new System.Drawing.Size(100, 100);
                // pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                //pictureBox1.BorderStyle = BorderStyle.Fixed3D;
                //pictureBox1.Image = image1;
                Avatar.Width = 200;
                Avatar.Height = 200;
                Avatar.Source = image1;
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show("User haven't avatar", "Confirmation", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

      

    }
}
