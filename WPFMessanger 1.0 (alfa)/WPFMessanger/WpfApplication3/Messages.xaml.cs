using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;

namespace WpfApplication3
{
    /// <summary>
    /// Логика взаимодействия для Messages.xaml
    /// </summary>
    public partial class Messages : Window
    {
        public static RoutedCommand Send = new RoutedCommand();

        string[] TextToListBox;
        string FullTextOfDialog;
        int ID, TO;
        SqlAction sql;
        System.Windows.Forms.Timer tm;

        List<StackPanel> SP;
        List<Image> Ava;
        List<Image> FilePic;
        List<TextBlock> Msg;
        List<BitmapImage> image1;
        List<System.Windows.Controls.Label> time;

        bool ban = false;//ВНИМАНИЕ!!! ВРЕМЕННЫЙ КОСТЫЛЬ, будет запрещать конекшн во время копирования файла, фиксану когда сделаю асинхронный режим подключений
        public Messages(int id, int to)
        {
            sql = new SqlAction();
            ID = id;
            TO = to;
            InitializeComponent();
            //var scrollViewer = myListBox.GetFirstDescendantOfType<ScrollViewer>();
            FullTextOfDialog = sql.DownloadMessages(ID, TO);


            tm = new System.Windows.Forms.Timer();
            tm.Interval = 1000;
            //tm.Tick += new EventHandler(MessageBox.Show("HI"));
            tm.Tick += Refresh;
            tm.Enabled = true;

            //<StckPanel x:Name="SP" Orientation = "Horizontal">
            //    <Image x:Name="Ava" Margin= "0,0,0,0"  Height="52" Width="48"/>
            //    <TextBlock x:Name="Msg" TextWrapping="Wrap" Margin= "3" Text= "" Width="293" />
            //    <Label x:Name="label" Content="" HorizontalAlignment="Left" Margin="0,0,0,0" VerticalAlignment="Top" Height="53" Width="42" />
            //</StckPanel>


            /*
            SP = new StackPanel();
            Msg.Text = "Hello, World!";
            BitmapImage image1 = new BitmapImage(new Uri("D:\\Desktop\\Picture\\Null.png"));
            Ava.Source = image1;
            //SP.Children.Add(TB);
            //SP.Children.Add(img);
            LBDialog.Items.Add(SP);
            */

            Ava = new List<Image>();
            FilePic = new List<Image>();
            Msg = new List<TextBlock>();
            SP = new List<StackPanel>();
            image1 = new List<BitmapImage>();
            time = new List<System.Windows.Controls.Label>();
            if (FullTextOfDialog != "")
            {
                TextToListBox = FullTextOfDialog.Split('\n');
                ToListBox();
            }
        }

        //private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}

        void ToListBox()
        {
            int j = 0;
            for (int i = 0; i < TextToListBox.Length; i++)
            {
                //
                SP.Add(new StackPanel());
                Ava.Add(new Image());
                Msg.Add(new TextBlock());
                time.Add(new System.Windows.Controls.Label());
                int LI = TextToListBox[i].LastIndexOf("&");
                int IO = TextToListBox[i].IndexOf("&");

                // System.Windows.MessageBox.Show(Convert.ToString(TextToListBox[i].Length), TextToListBox[i][TextToListBox[i].Length-1]+"a");
                string GetTime = TextToListBox[i].Substring(LI + 1, TextToListBox[i].Length - (LI + 1));//время и дата

                bool hasFile;
                string GetId;
                if (TextToListBox[i].IndexOf("|") == -1)
                    hasFile = false;
                else hasFile = true;

                if (!hasFile)
                    GetId = TextToListBox[i].Substring(0, IO);
                else
                    GetId = TextToListBox[i].Substring(TextToListBox[i].IndexOf("|") + 1, IO - (TextToListBox[i].IndexOf("|") + 1));
                //GetId = TextToListBox[i].Substring(0,TextToListBox[i].IndexOf("|"));


                time[i].Content = GetTime;//DateTime.Now;
                TextToListBox[i] = TextToListBox[i].Remove(TextToListBox[i].LastIndexOf('&'));
                TextToListBox[i] = TextToListBox[i].Substring(TextToListBox[i].IndexOf('&') + 1);//идентификатор-аватарка

                Msg[i].Text = " " + TextToListBox[i];
                image1.Add(new BitmapImage(new Uri(sql.GetData(Convert.ToInt32(GetId))[5])));
                Ava[i].Source = image1[i];

                //


                //
                SP[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                Ava[i].Height = Double.NaN;
                Ava[i].Width = 50;
                Ava[i].Margin = new Thickness(0, 0, 0, 0);
                Msg[i].Width = 250;
                Msg[i].Height = Double.NaN;
                Msg[i].TextWrapping = TextWrapping.Wrap;
                //time[i].Foreground = new SolidColorBrush(Colors.Red);
                time[i].Foreground = Brushes.Gray;
                //time[i].HorizontalAlignment = Left;
                //time[i].Color = "grey";


                SP[i].Children.Add(Ava[i]);
                if (hasFile == true)
                {
                    FilePic.Add(new Image());
                    FilePic[j].Source = new BitmapImage(new Uri("Resources/file.png", UriKind.RelativeOrAbsolute));
                    FilePic[j].Height = 40;
                    FilePic[j].Width = Double.NaN;
                    FilePic[j].Margin = new Thickness(0, 0, 0, 0);
                    SP[i].Children.Add(FilePic[j]);
                    j++;
                }
                SP[i].Children.Add(Msg[i]);
                SP[i].Children.Add(time[i]);
                LBDialog.Items.Add(SP[i]);

                //
                //LBDialog.Items.Add(TextToListBox[i]);



                //LBDialog.SelectedIndex = LBDialog.Items.Count - 1;//Проскролить в конец
                //LBDialog.ScrollIntoView(LBDialog.SelectedItem);
                LBDialog.ScrollIntoView(LBDialog.Items[LBDialog.Items.Count - 1]);
            }
        }
        private void Refresh(object sender, EventArgs e)//обновление окна сообщений
        {
            if (ban == false)
            {
                if (FullTextOfDialog != sql.DownloadMessages(ID, TO))// && FullTextOfDialog!=""
                {
                    FullTextOfDialog = sql.DownloadMessages(ID, TO);

                    Ava = new List<Image>();
                    Msg = new List<TextBlock>();
                    SP = new List<StackPanel>();
                    image1 = new List<BitmapImage>();
                    FilePic = new List<Image>();
                    time = new List<System.Windows.Controls.Label>();
                    TextToListBox = FullTextOfDialog.Split('\n');
                    LBDialog.Items.Clear();
                    ToListBox();
                }
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            tm.Enabled = false;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            ban = true;
            /*System.Windows.MessageBox.Show("4to-to poslo ne tak: ili razrab ne dopisal etu function or u vas o4en bad computer, scoree vsego 2 variant but you can send message on number 0990867398 and uznat 4to je na samom dele proizishlo", ":c");
              System.Windows.MessageBox.Show("No ti ne univay- lovi frostmourne", ":3");

             StreamResourceInfo sriCurs = System.Windows.Application.GetResourceStream(
             new Uri("Frostmourne.cur", UriKind.Relative));
             this.Cursor = new System.Windows.Input.Cursor(sriCurs.Stream);
             */
            //int GCP = System.Diagnostics.Process.GetCurrentProcess().Threads.Count;


            OpenFileDialog dowbloadFile = new OpenFileDialog();
            dowbloadFile.Filter = "All files (*.*)|*.*";
            dowbloadFile.ShowDialog();
            string filename = dowbloadFile.FileName;

            Thread FileThread = new Thread(delegate () { sql.SaveFileToDatabase(filename); });
            FileThread.Priority = ThreadPriority.Normal;
            FileThread.Start();

            /*

            FolderBrowserDialog Op = new FolderBrowserDialog();
             Op.ShowDialog();
             Thread FileThread = new Thread(delegate () { sql.ReadFileFromDatabase(Op.SelectedPath); });
             FileThread.Priority = ThreadPriority.Normal;
             FileThread.Start();
*/

            //while(GCP!= System.Diagnostics.Process.GetCurrentProcess().Threads.Count)
            FileThread.Join();//ожидать завершение потока(Остановка всеъ тредов до завершения этого)
            if (!FileThread.IsAlive)
                ban = false;


            string FileName = filename.Substring(filename.LastIndexOf('\\') + 1);
            Guid FID = sql.GetStreamID(FileName);


            if (FullTextOfDialog != "")
                sql.SendMess(FullTextOfDialog + "\n" + FID.ToString() + "|" + ID + "&" + FileName + "&" + (DateTime.Now), ID, TO);
            else
                sql.SendMess(FID.ToString() + "|" + ID + "&" + FileName + "&" + (DateTime.Now), ID, TO);

            YourMessage.Clear();
            Refresh(sender, e);
        }



        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }





        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void LBDialog_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (LBDialog.SelectedIndex >= 0)
            {
                string[] LocalTextFRomLB = FullTextOfDialog.Split('\n');
                if (LocalTextFRomLB[LBDialog.SelectedIndex].IndexOf("|") != -1)// TextToListBox[LBDialog.SelectedIndex].IndexOf("|") != -1
                {
                    ban = true;
                    string path;
                    if (!System.IO.File.Exists("SaveDefaultPath"))
                    { 
                    FolderBrowserDialog Op = new FolderBrowserDialog();
                    Op.ShowDialog();
                        path = Op.SelectedPath;
                    System.IO.File.WriteAllText(@"SaveDefaultPath", path);
                    }
                    else
                    path= System.IO.File.ReadAllText(@"SaveDefaultPath");
                    Guid StrmID=new Guid(LocalTextFRomLB[LBDialog.SelectedIndex].Substring(0, LocalTextFRomLB[LBDialog.SelectedIndex].IndexOf("|")));
                    Thread FileThread = new Thread(delegate () { sql.DownloadFileFromDB(StrmID, path); });

                    FileThread.Priority = ThreadPriority.Normal;
                    FileThread.Start();

                    FileThread.Join();//ожидать завершение потока(Остановка всеъ тредов до завершения этого)
                    if (!FileThread.IsAlive)
                        ban = false;
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!ban)
            {
                if (!sql.isBlackList(ID, TO) && !sql.isBlackList(TO, ID))//!!! как вариант можно дать возможность тому кто дал БЛ писать человеку, который не сможет ему ответить( но это беспреел, но как фишка может зайти)
                {
                    if (YourMessage.Text != "" && YourMessage.Text != " ")
                    {
                        sql = new SqlAction();
                        if (FullTextOfDialog != "")
                            sql.SendMess(FullTextOfDialog + "\n" + ID + "&" + YourMessage.Text + "&" + (DateTime.Now), ID, TO);
                        else
                            sql.SendMess(ID + "&" + YourMessage.Text + "&" + (DateTime.Now), ID, TO);
                        YourMessage.Clear();
                        Refresh(sender, e);
                    }

                }
                else
                    System.Windows.MessageBox.Show("You can't write to this user because he added you in a black list", "Warning", MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }
    }
}
