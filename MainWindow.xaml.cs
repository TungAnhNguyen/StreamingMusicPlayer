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
using System.Windows.Threading;
using System.Media;
using System.Diagnostics;
//NTA import
using System.IO;
using System.Reflection;
using WMPLib;

namespace WPF_Scratch
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public TimeSpan timeSpan1 = new TimeSpan(0, 0, 0);
        public TimeSpan timeSpan2 = new TimeSpan();
        public TimeSpan timeSpan3 = new TimeSpan(0, 0, 1); //serves as the counter to increment.
        DispatcherTimer timer1 = new DispatcherTimer();

        string[] listSong1 = Directory.GetFiles("E:\\Tung Anh\\Music\\Taylor Swift\\1989", "*.flac", SearchOption.AllDirectories);
        int songCounter = 0;
        WindowsMediaPlayer media1 = new WindowsMediaPlayer(); //to play the music
        double endTime;
        string notPress = "First Time";
        string dummystring1 = "YES";

        public MainWindow()
        {
            InitializeComponent();
        }

        //void dispatcher1_Tick(object sender, EventArgs e)
        //{
        //    progressBar1.Value = progressBar1.Value + 1;
        //}
        public string[,] songPropertyMethod1()
        {
            string[] hello = new string[] { "hello" };
            string[,] songListProperty = new string[listSong1.Length, 2];
            for (int i = 0; i < listSong1.Length; i++)
            {
                songListProperty[i, 0] = media1.currentPlaylist.getItemInfo("Title") + media1.currentMedia.getItemInfo("Album");
                songListProperty[i, 1] = media1.currentMedia.durationString;
            }
            return songListProperty;
        }

        //public void myButton_Click_1(object sender, RoutedEventArgs e)
        //{

        //}

        public void playButton_Click(object sender, RoutedEventArgs e)
        {
            playHelperMethod();
            timer1.Stop();
            timer1.Tick -= timer1_Tick;
            //System.Threading.Thread.Sleep(100);
            playHelperMethod2();

        }

        public void Next_Click(object sender, RoutedEventArgs e)
        {
            songCounter++;
            timer1.Stop();
            timer1.Tick -= timer1_Tick;
            System.Threading.Thread.Sleep(100);
            playHelperMethod();
        
            playHelperMethod2();
        }

        public void previousButton_Click(object sender, RoutedEventArgs e)
        {
            songCounter = songCounter - 1;
            timer1.Stop();
            timer1.Tick -= timer1_Tick;
            System.Threading.Thread.Sleep(100);
            playHelperMethod();
            playHelperMethod2();
        }

        public void playHelperMethod()
        {
            if (songCounter < 0)
            {
                songCounter = listSong1.Length - 1;
            }
            else if (songCounter > listSong1.Length - 1)
            {
                songCounter = 0;
            }
        }

        public void playHelperMethod2() //to assist buttons "Play", "Next", "Previous".
        {
            
            media1.URL = listSong1[songCounter];
            //media1.controls.play() ;
            if (dummystring1 == "YES")
            {
                //System.Threading.Thread.Sleep(10000);
                dummystring1 = "NO";
            }
            string songNameTextBoxString = media1.currentMedia.getItemInfo("Title") + " - " + media1.currentMedia.getItemInfo("Album");

            //endTimeLabel.Content = songPropertyList1[songCounter, 1];
            //System.Threading.Thread.Sleep(10000);//delay time
            songNameTextBox.Text = songNameTextBoxString;
            //endTime = media1.currentMedia.duration;
            timeSpan1 = new TimeSpan(0, 0, 0); //must reset  the timespan 1 everyrtime the function is fired.
            TimeSpan timeSpan2 = TimeSpan.FromSeconds(endTime);
            TimeSpan timeSpan3 = new TimeSpan(0, 0, 1); //serves as the counter to increment.
            //it doesn't hurt to leave timeSpan3 over there. 

            testTextBlock.Text = endTime.ToString();

            //Timer and loading bar
            progressBar1.Maximum = endTime;
            progressBar1.Minimum = 0;
            progressBar1.Value = 0;

            //DispatcherTimer timer1 = new DispatcherTimer();

            timer1.Tick += timer1_Tick;
            timer1.Interval = new TimeSpan(0, 0, 1);
            timer1.Start();
            endTimeLabel.Content = TimeSpan.FromSeconds(endTime);
            currentTimeLabel.Content = timeSpan1;
            //System.Threading.Thread.Sleep(1000);

            media1.controls.play();

        }

        //public void songLoadingMethod1()
        //{
        //    //media1.URL = listSong1[songCounter];

        //    //copy from hardwork section in tick event
        //    endTime = media1.currentMedia.duration;
        //    endTimeLabel.Content = TimeSpan.FromSeconds(endTime);
        //    songNameTextBox.Text = media1.currentMedia.getItemInfo("Title") + " - " + media1.currentMedia.getItemInfo("Album");
        //    progressBar1.Maximum = endTime;
        //    //progressBar1.Minimum = 0;
        //}

        void timer1_Tick(object sender, EventArgs e)
        {
            currentTimeLabel.Content = timeSpan1;
            timeSpan1 = timeSpan1 + timeSpan3;
            progressBar1.Value = progressBar1.Value + 1;

            if (progressBar1.Value == progressBar1.Maximum && progressBar1.Value > 5)
            {
                songCounter++;
                timer1.Stop();
                playHelperMethod();
                playHelperMethod2();
            }

            //Hard work
            endTime = media1.currentMedia.duration;
            endTimeLabel.Content = TimeSpan.FromSeconds(endTime);
            songNameTextBox.Text = media1.currentMedia.getItemInfo("Title") + " - " + media1.currentMedia.getItemInfo("Album");
            progressBar1.Maximum = endTime;
            progressBar1.Minimum = 0;
        }

        public void pauseButton_Click(object sender, RoutedEventArgs e)
        {
            playHelperMethod();
            double playTime1 = media1.controls.currentPosition;
            //double playTime1 = 0;

            if (notPress=="First Time")
            {
                //media1.controls.play();
                playHelperMethod();
                playHelperMethod2();
                notPress = "Not Pause";
                pauseButton.Content = "Pause Me!";
            }
            else if (notPress == "Not Pause")
            {
                playTime1 = media1.controls.currentPosition;
                media1.controls.pause();
                notPress = "Being Paused";
                testTextBlock.Text = "Being Paused";
                pauseButton.Content = "Continue Me";
                timer1.Stop();
            }
            
            else if (notPress == "Being Paused")
            {
                notPress = "Not Pause";
                testTextBlock.Text = "Not Pause";
                pauseButton.Content = "Pause Me";
                media1.controls.currentPosition = playTime1;
                media1.controls.play();
                timer1.Start();
            }
        }


    }

}
