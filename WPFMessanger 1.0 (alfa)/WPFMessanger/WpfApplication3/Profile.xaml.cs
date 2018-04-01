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
    public partial class Profile : Window
    {
        Auentification main;
        String fileName;
        string Name, About, Country, Photo;
        int age, ID;
        public Profile(int id)
        {
            ID = id;
            SqlAction sql = new SqlAction();
            sql.GetData(ID);
            InitializeComponent();
            fileName = null;
            LBN.Content += Name=sql.GetData(ID)[0] + " " + sql.GetData(ID)[1];
            LBA.Content += Convert.ToString(age =Convert.ToInt32( sql.GetData(ID)[2]));
            LBC.Content += Country=sql.GetData(ID)[3];
            AbMe.Text = About=sql.GetData(ID)[4];
            //if (sql.GetData(Fname, Lname, Descript, age, Photo, Country, ID)[5] != "NULL")
            SetPict(Photo=sql.GetData(ID)[5]);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            main = (Auentification)this.Owner;
            main.Visibility = Visibility.Visible;
            this.Hide();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Search src = new Search(ID);
            src.Show();
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
                //System.Windows.Forms.MessageBox.Show("Picture isn't found", "Confirmation", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Friends fr = new Friends(ID);
            fr.Show();
        }

        private void Window_Closing_1(object sender, CancelEventArgs e)
        {
            //main = (Auentification)this.Owner;
            //main.Visibility = Visibility.Visible;
            // main.Close();
            System.Windows.Application.Current.Shutdown();

        }

        private void aPicture_MouseDown(object sender, MouseButtonEventArgs e)
        {
            EditProfile EP = new EditProfile(Name,About,Country,Photo,age, ID);
            EP.Show();
            this.Hide();
            /*
            SqlAction sql = new SqlAction();
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader sr = new StreamReader(open.FileName);
                //  String textRead = sr.ReadToEnd();
                this.fileName = open.FileName;
                sql.LoadPicture(fileName, ID);
                SetPict(fileName);
            }
            */
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
              Peoples people = new Peoples(ID);
              people.Show();
        }




    }
}
