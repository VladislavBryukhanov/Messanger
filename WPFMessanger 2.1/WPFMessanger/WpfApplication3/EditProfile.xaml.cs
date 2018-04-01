using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для EditProfile.xaml
    /// </summary>
    public partial class EditProfile : Window
    {
        string name, about, country, photo;
        int age, ID;

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Profile myProfile = new Profile(ID);
            myProfile.Show();
        }

        public EditProfile(string name, string about, string country, string photo,int age,int ID)
        {
            this.ID = ID;
            this.name = name;
            this.about = about;
            this.country = country;
            this.photo = photo;
            this.age = age;
            InitializeComponent();
            Name.Text = name;
            About.Text = about;
            Age.Text = age.ToString();
            try//если существует
            { 
            BitmapImage image1 = new BitmapImage(new Uri(photo));
            Avatar.Width = 200;
            Avatar.Height = 200;
            Avatar.Source = image1;
            }
            catch
            {

            }
            Country.Text = country;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SqlAction sql = new SqlAction();
            sql.EditData(Name.Text, Country.Text, About.Text,Convert.ToInt32( Age.Text), photo, ID);
            this.Close();
        }

        private void Avatar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SqlAction sql = new SqlAction();
            System.Windows.Forms.OpenFileDialog open = new System.Windows.Forms.OpenFileDialog();
            open.Filter = "Image Files(*.BMP;*.JPG;*.GIF)|*.BMP;*.JPG;*.GIF|All files (*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamReader sr = new StreamReader(open.FileName);
                this.photo = open.FileName;
                //sql.LoadPicture(fileName, ID);
                BitmapImage image1 = new BitmapImage(new Uri(photo));
                Avatar.Width = 200;
                Avatar.Height = 200;
                Avatar.Source = image1;
            }
        }





    }
}
