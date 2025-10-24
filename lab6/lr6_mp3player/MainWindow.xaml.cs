using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Microsoft.Win32;
using System.Windows.Media;

namespace lr6_mp3player
{
    public partial class MainWindow : Window
    {
        private MediaPlayer mediaPlayer = new MediaPlayer();
        private List<string> playlist = new List<string>();
        private int currentIndex = -1;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            SetupTimer();
            mediaPlayer.Volume = 0.5;
        }

        private void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += TimerTick;
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (mediaPlayer.Source != null && mediaPlayer.NaturalDuration.HasTimeSpan)
            {
                txtTime.Text = $"{mediaPlayer.Position:mm\\:ss} / {mediaPlayer.NaturalDuration.TimeSpan:mm\\:ss}";
            }
        }

        private void BtnSelectFiles_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "MP3 files|*.mp3";
            dialog.Multiselect = true;

            if (dialog.ShowDialog() == true)
            {
                playlist.Clear();
                lstPlaylist.Items.Clear();

                foreach (string file in dialog.FileNames)
                {
                    playlist.Add(file);
                    lstPlaylist.Items.Add(Path.GetFileName(file));
                }

                if (playlist.Count > 0)
                {
                    currentIndex = 0;
                }
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count == 0) return;

            if (currentIndex == -1) currentIndex = 0;

            PlayCurrent();
        }

        private void BtnPause_Click(object sender, RoutedEventArgs e)
        {
            if (mediaPlayer.CanPause)
            {
                mediaPlayer.Pause();
            }
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {
            mediaPlayer.Stop();
            timer.Stop();
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (playlist.Count == 0) return;

            currentIndex = (currentIndex + 1) % playlist.Count;
            PlayCurrent();
        }

        private void PlayCurrent()
        {
            if (currentIndex < 0 || currentIndex >= playlist.Count) return;

            mediaPlayer.Open(new Uri(playlist[currentIndex]));
            mediaPlayer.Play();
            timer.Start();

            txtCurrentTrack.Text = $"Playing: {Path.GetFileName(playlist[currentIndex])}";
            lstPlaylist.SelectedIndex = currentIndex;
        }

        private void sldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mediaPlayer.Volume = sldVolume.Value;
        }
    }
}