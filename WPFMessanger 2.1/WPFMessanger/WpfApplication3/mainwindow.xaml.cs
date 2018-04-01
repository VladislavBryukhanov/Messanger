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

using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace WpfApplication3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class Auentification : Window
    {
         public static RoutedCommand LogIn = new RoutedCommand();


        int Key;
        public Auentification()
        {
            InitializeComponent();
           // StreamResourceInfo sriCurs = System.Windows.Application.GetResourceStream(
           //new Uri("Frostmourne.cur", UriKind.Relative));
           //this.Cursor = new System.Windows.Input.Cursor(sriCurs.Stream);
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SqlAction sql = new SqlAction();
            Key = sql.Auentific(TB.Text, PB.Password);
            if (Key != 0)
            {

                Profile myProfile = new Profile(Key);
                myProfile.Owner = this;
                myProfile.Show();
                this.Visibility = Visibility.Collapsed;//Visibility.Visible;///
            }
            else
                System.Windows.Forms.MessageBox.Show("You are input invalid login or password", "Confirmation", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);
            //    this.Hide();
          
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
                 Registration reg = new Registration();
                 reg.ShowDialog();
        }

        private void Auentific_Closing(object sender, CancelEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();

        }
    }
}
