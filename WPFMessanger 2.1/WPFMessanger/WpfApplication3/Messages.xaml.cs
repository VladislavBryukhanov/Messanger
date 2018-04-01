using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

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
        int ID, TO, ChatID;
        bool isChat;
        private SqlAction sql;
        private System.Windows.Forms.Timer tm;

        int mediaIndex;//!!!!
        int j;
        int i;
        private List<bool> state;
        private List<bool> TimerState;
        int CurrentMediaElement;
        private List<MediaElement> media;
        private List<System.Windows.Forms.Timer> MediaTimer;
        private List<System.Windows.Controls.Button> Restart;
        private List<System.Windows.Controls.Button> PauseResume;
        private List<System.Windows.Controls.Slider> rewind;
        private List<StackPanel> mediaPanel;
        private bool isUserDragging = false;//если в данный момент юзер тянет за слайдер

        private List<StackPanel> SP;
        private List<Image> Ava;
        private List<Image> FilePic;
        private List<TextBlock> Msg;
        private List<BitmapImage> image1;
        private List<System.Windows.Controls.Label> time;

        private bool ban = false;//ВНИМАНИЕ!!! ВРЕМЕННЫЙ КОСТЫЛЬ, будет запрещать конекшн во время копирования файла, фиксану когда сделаю асинхронный режим подключений
        public Messages(int id, int to)
        {
            CurrentMediaElement = 0;
            //state = false;
            //TimerState = false;
            sql = new SqlAction();
            ID = id;
            TO = to;

            mediaIndex = 0;
            j = 0;
            i = 0;
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
            media = new List<MediaElement>();
            Restart = new List<System.Windows.Controls.Button>();
            PauseResume = new List<System.Windows.Controls.Button>();
            rewind = new List<System.Windows.Controls.Slider>();
            mediaPanel = new List<StackPanel>();
            Msg = new List<TextBlock>();
            SP = new List<StackPanel>();
            image1 = new List<BitmapImage>();
            time = new List<System.Windows.Controls.Label>();
            MediaTimer = new List<System.Windows.Forms.Timer>();
            TimerState = new List<bool>();
            state = new List<bool>();
            if (FullTextOfDialog != "")
            {
                TextToListBox = FullTextOfDialog.Split('\n');
                ToListBox();
            }

        }
        public Messages(int id, int ChatID, bool isChat)
        {
            CurrentMediaElement = 0;
            sql = new SqlAction();
            ID = id;
            this.ChatID = ChatID;
            this.isChat = isChat;
            mediaIndex = 0;
            j = 0;
            i = 0;
            InitializeComponent();
            FullTextOfDialog = sql.DownloadChat(ChatID);

            tm = new System.Windows.Forms.Timer();
            tm.Interval = 1000;
            tm.Tick += RefreshCh;
            tm.Enabled = true;

            Ava = new List<Image>();
            FilePic = new List<Image>();
            media = new List<MediaElement>();
            Restart = new List<System.Windows.Controls.Button>();
            PauseResume = new List<System.Windows.Controls.Button>();
            rewind = new List<System.Windows.Controls.Slider>();
            mediaPanel = new List<StackPanel>();
            Msg = new List<TextBlock>();
            SP = new List<StackPanel>();
            image1 = new List<BitmapImage>();
            time = new List<System.Windows.Controls.Label>();
            MediaTimer = new List<System.Windows.Forms.Timer>();
            TimerState = new List<bool>();
            state = new List<bool>();
            if (FullTextOfDialog != "")
            {
                TextToListBox = FullTextOfDialog.Split('\n');
                ToListBox();
            }
        }

        void ToListBox()
        {
            for (; i < TextToListBox.Length; i++)
            {
                System.IO.Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory+"temp");

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

                string extend = null;
                Guid StrmID = new Guid();
                if (!hasFile)
                    GetId = TextToListBox[i].Substring(0, IO);
                else
                {
                    GetId = TextToListBox[i].Substring(TextToListBox[i].IndexOf("|") + 1, IO - (TextToListBox[i].IndexOf("|") + 1));
                    //-узнаем расширение, чтобы если этот файл окажется изображением, музыкой или видео прогрузить соответствующую пиктограмму
                    extend = TextToListBox[i].Substring(TextToListBox[i].IndexOf("&"), TextToListBox[i].LastIndexOf("&") - TextToListBox[i].IndexOf("|"));
                    extend = extend.Substring(extend.LastIndexOf("."), (extend.LastIndexOf("&")) - (extend.LastIndexOf(".")));
                    StrmID = new Guid(TextToListBox[i].Substring(0, TextToListBox[i].IndexOf("|")));
                    //
                }
                time[i].Content = GetTime;
                TextToListBox[i] = TextToListBox[i].Remove(TextToListBox[i].LastIndexOf('&'));
                TextToListBox[i] = TextToListBox[i].Substring(TextToListBox[i].IndexOf('&') + 1);//идентификатор-аватарка

                Msg[i].Text = " " + TextToListBox[i];
                try
                {

                    image1.Add(new BitmapImage(new Uri(sql.GetData(Convert.ToInt32(GetId))[5])));
                }
                catch (Exception)
                {
                    image1.Add(new BitmapImage(new Uri(@"file:///D:/Desktop/WPFMessanger/WpfApplication3/Resources/Empty.png")));
                }
                Ava[i].Source = image1[i];

                SP[i].Orientation = System.Windows.Controls.Orientation.Horizontal;
                Ava[i].Height = Double.NaN;
                Ava[i].Width = 50;
                Msg[i].Width = 260;
                Msg[i].Height = Double.NaN;
                Msg[i].TextWrapping = TextWrapping.Wrap;
                //time[i].Foreground = new SolidColorBrush(Colors.Red);
                time[i].Foreground = Brushes.Gray;
                //time[i].HorizontalAlignment = Left;
                //time[i].Color = "grey";
                //


                SP[i].Children.Add(Ava[i]);
                if (hasFile == true)
                {
                    FilePic.Add(new Image());
                    MediaTimer.Add(new System.Windows.Forms.Timer());

                    if (extend == ".jpg" || extend == ".png" || extend == ".bmp" || extend == ".jepg")
                    {
                        try
                        {
                            System.IO.MemoryStream stream = new System.IO.MemoryStream(sql.GetPicture(StrmID));
                            var image = new BitmapImage();
                            image.BeginInit();
                            image.StreamSource = stream;
                            image.EndInit();
                            FilePic[j].Source = image;
                        }
                        catch (System.ArgumentNullException)
                        {
                            FilePic[j].Source = new BitmapImage(new Uri(@"file:///D:/Desktop/WPFMessanger/WpfApplication3/Resources/Null.png"));
                        }

                        //GIF не работает :с
                        //FilePic[i].SetAnimatedSource(img, image);
                        //ImageBehavior.SetRepeatBehavior(img, new RepeatBehavior(0));
                        //ImageBehavior.SetRepeatBehavior(img, RepeatBehavior.Forever);
                        FilePic[j].Height = Double.NaN;
                        FilePic[j].Width = 260;
                        Msg[i].Text = " ";
                        Msg[i].Width = 0;
                        FilePic[j].Margin = new Thickness(10, 0, 0, 0);
                    }
                    else if (extend == ".mp3" || extend == ".m4a" || extend == ".aud" || extend == ".aif" || extend == ".amr" || extend == ".wma" || extend == ".wav" || extend == ".mid")
                    {
                        try
                        {
                            MediaTimer[mediaIndex].Interval = 400;
                            MediaTimer[mediaIndex].Tick += timer_Tick;
                            TimerState.Add(false);
                            state.Add(false);

                            media.Add(new MediaElement());
                            PauseResume.Add(new System.Windows.Controls.Button());
                            Restart.Add(new System.Windows.Controls.Button());
                            rewind.Add(new Slider());
                            mediaPanel.Add(new StackPanel());

                            media[mediaIndex].MediaOpened += new RoutedEventHandler(media_MediaOpened);
                            //media[mediaIndex].MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(media_MediaFailed);


                            System.IO.MemoryStream stream = new System.IO.MemoryStream(sql.GetPicture(StrmID));
                            FilePic[j].Source = new BitmapImage(new Uri("Resources/music.png", UriKind.RelativeOrAbsolute)); ;

                            System.IO.File.WriteAllBytes("temp/temp" + mediaIndex + extend, sql.GetPicture(StrmID));
                            media[mediaIndex].Source = (new Uri(AppDomain.CurrentDomain.BaseDirectory + "temp/temp" + mediaIndex + extend));
                            media[mediaIndex].LoadedBehavior = MediaState.Manual;
                            media[mediaIndex].UnloadedBehavior = MediaState.Close;
                            media[mediaIndex].Play();
                            media[mediaIndex].Stop();
                            // media.Pause();


                            media[mediaIndex].Name = "Media_" + mediaIndex;
                            rewind[mediaIndex].Name = "Slider_" + mediaIndex;
                            Restart[mediaIndex].Name = "Restart_" + mediaIndex;
                            PauseResume[mediaIndex].Name = "Pause_" + mediaIndex;
                            Restart[mediaIndex].Content = "Restart";
                            PauseResume[mediaIndex].Content = "Pause";
                            Restart[mediaIndex].Click += new RoutedEventHandler(ToRestart);
                            PauseResume[mediaIndex].Click += new RoutedEventHandler(ToResumePause);

                            //Thumb thumb = this.GetTemplateChild(rewind[mediaIndex].Name) as Thumb;
                            //thumb.DragStarted += new DragStartedEventHandler(sld_DragStarted);
                            //thumb.DragCompleted += new DragCompletedEventHandler(sld_DragCompleted);
                            //rewind[mediaIndex].OnThumbDragStarted += new RoutedEventHandler(sld_DragStarted);
                            //rewind[mediaIndex].DragLeave += new System.Windows.RoutedEventHandler(sld_DragCompleted);
                            //rewind[mediaIndex].ValueChanged += new RoutedPropertyChangedEventHandler<double>(sld_ValueChanged);

                            media[mediaIndex].MediaOpened += new RoutedEventHandler(media_MediaOpened);

                            mediaPanel[mediaIndex].Children.Add(media[mediaIndex]);
                            mediaPanel[mediaIndex].Children.Add(rewind[mediaIndex]);
                            mediaPanel[mediaIndex].Children.Add(PauseResume[mediaIndex]);
                            mediaPanel[mediaIndex].Children.Add(Restart[mediaIndex]);


                            FilePic[j].Height = Double.NaN;
                            FilePic[j].Width = 0;
                            Msg[i].Text = " ";
                            Msg[i].Width = 0;

                            //media.Width = 250;
                            //media.Height = Double.NaN;
                            mediaPanel[mediaIndex].Orientation = System.Windows.Controls.Orientation.Vertical;
                            PauseResume[mediaIndex].Width = 50;
                            PauseResume[mediaIndex].Height = Double.NaN;
                            Restart[mediaIndex].Width = 50;
                            Restart[mediaIndex].Height = Double.NaN;
                            mediaPanel[mediaIndex].Width = 260;
                            mediaPanel[mediaIndex].Height = Double.NaN;
                            mediaPanel[mediaIndex].Margin = new Thickness(10, 0, 0, 0);
                            rewind[mediaIndex].Width = 260;


                            FilePic[j].Height = 40;
                            FilePic[j].Width = Double.NaN;
                            Msg[i].Width = 220;
                            FilePic[j].Margin = new Thickness(10, 0, 0, 0);

                            //System.IO.MemoryStream stream = new System.IO.MemoryStream(sql.GetPicture(StrmID));
                            media[mediaIndex].Width = 0;
                            rewind[mediaIndex].IsEnabled = false;

                            SP[i].Children.Add(mediaPanel[mediaIndex]);
                            mediaIndex++;
                        }
                        catch (System.ArgumentNullException)
                        {
                            FilePic[j].Source = new BitmapImage(new Uri(@"file:///D:/Desktop/WPFMessanger/WpfApplication3/Resources/Null.png"));
                            FilePic[j].Height = Double.NaN;
                            FilePic[j].Width = 260;
                            Msg[i].Text = " ";
                            Msg[i].Width = 0;
                            FilePic[j].Margin = new Thickness(10, 0, 0, 0);
                        }
                    }
                    else if (extend == ".mp4" || extend == ".3gp" || extend == ".3g2" || extend == ".asf" || extend == ".asx" || extend == ".avi" || extend == ".flv" || extend == ".mpg" || extend == ".webm" || extend == ".wmv" || extend == ".gif")
                    {
                        MediaTimer[mediaIndex].Interval = 400;
                        MediaTimer[mediaIndex].Tick += timer_Tick;
                        TimerState.Add(false);
                        state.Add(false);

                        media.Add(new MediaElement());
                        PauseResume.Add(new System.Windows.Controls.Button());
                        Restart.Add(new System.Windows.Controls.Button());
                        rewind.Add(new Slider());
                        mediaPanel.Add(new StackPanel());
                        try
                        {
                            //media[mediaIndex].MediaFailed += new EventHandler<ExceptionRoutedEventArgs>(media_MediaFailed);

                            //System.IO.MemoryStream stream = new System.IO.MemoryStream(sql.GetPicture(StrmID));




                            System.IO.File.WriteAllBytes("temp/temp" + mediaIndex + extend, sql.GetPicture(StrmID));
                            //var mediaPlayer = new MediaPlayer();
                            //mediaPlayer.Open(new Uri(@"\\Desktop - 3e0rm1s\mssqlserver\ChatNetwork\SendFile\odessa.mp4", UriKind.RelativeOrAbsolute));
                            //mediaPlayer.Play();
                            media[mediaIndex].Source = (new Uri(AppDomain.CurrentDomain.BaseDirectory + "temp/temp" + mediaIndex + extend));
                            media[mediaIndex].LoadedBehavior = MediaState.Manual;
                            media[mediaIndex].UnloadedBehavior = MediaState.Close;
                            media[mediaIndex].ScrubbingEnabled = true;
                            media[mediaIndex].Play();
                            media[mediaIndex].Stop();
                            //media.Pause();4


                            media[mediaIndex].Name = "Media_" + mediaIndex;
                            rewind[mediaIndex].Name = "Slider_" + mediaIndex;
                            Restart[mediaIndex].Name = "Restart_" + mediaIndex;
                            PauseResume[mediaIndex].Name = "Pause_" + mediaIndex;
                            Restart[mediaIndex].Content = "Restart";
                            PauseResume[mediaIndex].Content = "Pause";
                            Restart[mediaIndex].Click += new RoutedEventHandler(ToRestart);
                            PauseResume[mediaIndex].Click += new RoutedEventHandler(ToResumePause);

                            //rewind[mediaIndex].DragEnter += new System.Windows.DragEventHandler(sld_DragStarted);
                            //rewind[mediaIndex].DragLeave += new System.Windows.DragEventHandler(sld_DragCompleted);
                            //rewind[mediaIndex].ValueChanged += new RoutedPropertyChangedEventHandler<double>(sld_ValueChanged);

                            media[mediaIndex].MediaOpened += new RoutedEventHandler(media_MediaOpened);

                            mediaPanel[mediaIndex].Children.Add(media[mediaIndex]);
                            mediaPanel[mediaIndex].Children.Add(rewind[mediaIndex]);
                            mediaPanel[mediaIndex].Children.Add(PauseResume[mediaIndex]);
                            mediaPanel[mediaIndex].Children.Add(Restart[mediaIndex]);


                            FilePic[j].Height = Double.NaN;
                            FilePic[j].Width = 0;
                            Msg[i].Text = " ";
                            Msg[i].Width = 0;

                            //media.Width = 250;
                            //media.Height = Double.NaN;
                            mediaPanel[mediaIndex].Orientation = System.Windows.Controls.Orientation.Vertical;
                            PauseResume[mediaIndex].Width = 50;
                            PauseResume[mediaIndex].Height = Double.NaN;
                            Restart[mediaIndex].Width = 50;
                            Restart[mediaIndex].Height = Double.NaN;
                            media[mediaIndex].Width = 260;
                            media[mediaIndex].Height = Double.NaN;
                            mediaPanel[mediaIndex].Width = 260;
                            mediaPanel[mediaIndex].Height = Double.NaN;
                            mediaPanel[mediaIndex].Margin = new Thickness(10, 0, 0, 0);
                            rewind[mediaIndex].Width = 260;
                            rewind[mediaIndex].IsEnabled = false;

                            SP[i].Children.Add(mediaPanel[mediaIndex]);//media
                            mediaIndex++;

                        }
                        catch (System.ArgumentNullException)
                        {
                            FilePic[j].Source = new BitmapImage(new Uri(@"file:///D:/Desktop/WPFMessanger/WpfApplication3/Resources/Null.png"));
                            FilePic[j].Height = Double.NaN;
                            FilePic[j].Width = 260;
                            Msg[i].Text = " ";
                            Msg[i].Width = 0;
                            FilePic[j].Margin = new Thickness(10, 0, 0, 0);
                        }
                    }

                    else
                    {
                        FilePic[j].Source = new BitmapImage(new Uri("Resources/file.png", UriKind.RelativeOrAbsolute));
                        FilePic[j].Height = 40;
                        FilePic[j].Width = Double.NaN;
                        Msg[i].Width = 210;
                        FilePic[j].Margin = new Thickness(10, 0, 0, 0);
                    }
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
        public void ToRestart(object sender, System.EventArgs e)
        {
            string IndexOfCurrentMediaElement = ((System.Windows.Controls.Button)sender).Name.Substring(((System.Windows.Controls.Button)sender).Name.IndexOf("_") + 1);
            int index = Convert.ToInt32(IndexOfCurrentMediaElement);
            CurrentMediaElement = index;
            MediaTimer[CurrentMediaElement].Start();

            rewind[CurrentMediaElement].Value = 0;
            //System.Windows.MessageBox.Show(IndexOfCurrentMediaElement + "", "");
            //if (media[Convert.ToInt32(IndexOfCurrentMediaElement)].Position > TimeSpan.FromSeconds(5))
            {

                media[index].Pause();
                media[index].Position -= TimeSpan.FromSeconds(5);
                media[index].Stop();
                state[index] = false;
                MediaTimer[CurrentMediaElement].Stop();
                TimerState[index] = false;
            }
            //media[Convert.ToInt32(IndexOfCurrentMediaElement)].Stop();
            //    MediaTimer.Stop();

        }
        public void ToResumePause(object sender, System.EventArgs e)
        {



            string IndexOfCurrentMediaElement = ((System.Windows.Controls.Button)sender).Name.Substring(((System.Windows.Controls.Button)sender).Name.IndexOf("_") + 1);
            int index = Convert.ToInt32(IndexOfCurrentMediaElement);

            if (CurrentMediaElement != index)
            {
                media[CurrentMediaElement].Pause();//Поставить на паузу преыдущий запущенный элемент(Музыку или видео)
                MediaTimer[CurrentMediaElement].Stop();
                state[CurrentMediaElement] = false;
                TimerState[CurrentMediaElement] = true;
            }

            CurrentMediaElement = index;
            MediaTimer[CurrentMediaElement].Start();
            if (state[CurrentMediaElement] == true)
            {
                media[CurrentMediaElement].Pause();
                MediaTimer[CurrentMediaElement].Stop();
                state[CurrentMediaElement] = false;
                TimerState[CurrentMediaElement] = true;
            }
            else
            {
                media[CurrentMediaElement].Play();
                state[CurrentMediaElement] = true;
                TimerState[CurrentMediaElement] = false;
            }
        }
        private void Refresh(object sender, EventArgs e)//обновление окна сообщений    //Затратный алгоритм в плане  реурсов и трафика, гораздо более эффективно было бы, если вместо очистки списка добавлять новые элементы в конец
        {
            if (ban == false)
            {
                if (FullTextOfDialog != sql.DownloadMessages(ID, TO))// && FullTextOfDialog!=""
                {
                    FullTextOfDialog = sql.DownloadMessages(ID, TO);
                    /*
                    Ava = new List<Image>();
                    FilePic = new List<Image>();
                    media = new List<MediaElement>();
                    Restart = new List<System.Windows.Controls.Button>();
                    PauseResume = new List<System.Windows.Controls.Button>();
                    rewind = new List<System.Windows.Controls.Slider>();
                    mediaPanel = new List<StackPanel>();
                    Msg = new List<TextBlock>();
                    SP = new List<StackPanel>();
                    image1 = new List<BitmapImage>();
                    time = new List<System.Windows.Controls.Label>();
                    MediaTimer = new List<System.Windows.Forms.Timer>();
                    TimerState = new List<bool>();
                    state = new List<bool>();
                    LBDialog.Items.Clear();*/
                    TextToListBox = FullTextOfDialog.Split('\n');
                    ToListBox();

                }
            }
        }
        private void RefreshCh(object sender, EventArgs e)//обновление окна сообщений    //Затратный алгоритм в плане  реурсов и трафика, гораздо более эффективно было бы, если вместо очистки списка добавлять новые элементы в конец
        {
            if (ban == false)
            {
                if (FullTextOfDialog != sql.DownloadChat(ChatID))// && FullTextOfDialog!=""
                {
                    FullTextOfDialog = sql.DownloadChat(ChatID);
                    /*
                    Ava = new List<Image>();
                    FilePic = new List<Image>();
                    media = new List<MediaElement>();
                    Restart = new List<System.Windows.Controls.Button>();
                    PauseResume = new List<System.Windows.Controls.Button>();
                    rewind = new List<System.Windows.Controls.Slider>();
                    mediaPanel = new List<StackPanel>();
                    Msg = new List<TextBlock>();
                    SP = new List<StackPanel>();
                    image1 = new List<BitmapImage>();
                    time = new List<System.Windows.Controls.Label>();
                    MediaTimer = new List<System.Windows.Forms.Timer>();
                    TimerState = new List<bool>();
                    state = new List<bool>();
                    LBDialog.Items.Clear();*/
                    TextToListBox = FullTextOfDialog.Split('\n');
                    ToListBox();

                }
            }
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

            if (isChat)
            {
                if (FullTextOfDialog != "")
                    sql.SendChatMess(FullTextOfDialog + "\n" + FID.ToString() + "|" + ID + "&" + FileName + "&" + (DateTime.Now), ID);
                else
                    sql.SendChatMess(FID.ToString() + "|" + ID + "&" + FileName + "&" + (DateTime.Now), ID);
            }
            else
            { 
            if (FullTextOfDialog != "")
                sql.SendMess(FullTextOfDialog + "\n" + FID.ToString() + "|" + ID + "&" + FileName + "&" + (DateTime.Now), ID, TO);
            else
                sql.SendMess(FID.ToString() + "|" + ID + "&" + FileName + "&" + (DateTime.Now), ID, TO);
            }
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

        private void DownloadFile(object sender, MouseButtonEventArgs e)
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
                        path = System.IO.File.ReadAllText(@"SaveDefaultPath");
                    //AsyncCallback callback=new AsyncCallback(sql.DownloadFileFromDB(StrmID, path));
                    Guid StrmID = new Guid(LocalTextFRomLB[LBDialog.SelectedIndex].Substring(0, LocalTextFRomLB[LBDialog.SelectedIndex].IndexOf("|")));
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
            if (isChat)
            {
                if (!ban)
                {
                        if (YourMessage.Text != "" && YourMessage.Text != " ")
                        {
                            sql = new SqlAction();
                            if (FullTextOfDialog != "")
                                sql.SendChatMess(FullTextOfDialog + "\n" + ID + "&" + YourMessage.Text + "&" + (DateTime.Now), ID);
                            else
                                sql.SendChatMess(ID + "&" + YourMessage.Text + "&" + (DateTime.Now), ID);
                            YourMessage.Clear();
                            Refresh(sender, e);
                        }
                }
            }
            else
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

        private void Window_Closed(object sender, EventArgs e)
        {
            try
            {
                Thread FileThread = new Thread(delegate () { System.IO.Directory.Delete((AppDomain.CurrentDomain.BaseDirectory + "temp"), true); });
                FileThread.Priority = ThreadPriority.Normal;
                FileThread.Start();
                //System.IO.Directory.Delete((AppDomain.CurrentDomain.BaseDirectory + "temp"), true);//так как стоит тру, должна удалятся папка со всем содержимым, но почему-то вылетает эксепшн,что папка не пуста, однако перед эксепшеном все файлы в папки удаляются=> результат работы меня все-таки утраивает
            }
            catch (Exception)
            { }
            tm.Enabled = false;
        }

        public void timer_Tick(object sender, EventArgs e)
        {

            if ((media[CurrentMediaElement].Source != null) && (media[CurrentMediaElement].NaturalDuration.HasTimeSpan) && (!isUserDragging))
            {
                rewind[CurrentMediaElement].Value = media[CurrentMediaElement].Position.TotalSeconds;

            }
            //     throw new NotImplementedException();
        }
        //Возможность перематывать видео, потянув за ползунок. Не работает, для работы нужно присвоить собыитие ~OnThumbDragStarted~
        /*
        public void sld_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
           // LbTime.Content = TimeSpan.FromSeconds(rewind[CurrentMediaElement].Value).ToString(@"hh\:mm\:ss");
        }

        public void sld_DragStarted(object sender, RoutedEventArgs e)
        {
            isUserDragging = true;

        }

        public void sld_DragCompleted(object sender, RoutedEventArgs e)
        {
            media[CurrentMediaElement].Pause();
            media[CurrentMediaElement].Position = TimeSpan.FromSeconds(rewind[CurrentMediaElement].Value);
            isUserDragging = false;
            media[CurrentMediaElement].Play();
            if (TimerState[CurrentMediaElement] == false)
            {
                MediaTimer[CurrentMediaElement].Start();
                TimerState[CurrentMediaElement] = true;
            }
        }
        */
        private void media_MediaOpened(object sender, RoutedEventArgs e)//файл загружен
        {
            string IndexOfCurrentMediaElement = ((System.Windows.Controls.MediaElement)sender).Name.Substring(((System.Windows.Controls.MediaElement)sender).Name.IndexOf("_") + 1);
            int index = Convert.ToInt32(IndexOfCurrentMediaElement);
            //CurrentMediaElement = index;
            if (!media[index].HasAudio && !media[index].HasVideo)
            {
                System.Windows.MessageBox.Show("Format mismatch", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (media[index].NaturalDuration.HasTimeSpan)
                rewind[index].Maximum = (int)media[index].NaturalDuration.TimeSpan.TotalSeconds;
        }

        //private void media_MediaFailed(object sender, ExceptionRoutedEventArgs e)//медиаэлемент не бросает исключения а отправляет их в этот event
        //{
        //    media[CurrentMediaElement].Stop();
        //    System.Windows.MessageBox.Show("Error in playing format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
    }
}


