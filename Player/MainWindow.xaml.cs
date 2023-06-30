using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

namespace Player
{
    public partial class MainWindow : Window
    {

        ObservableCollection<string> _elements = new ObservableCollection<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void NewElements()
        {
            musicList.Items.Clear();
            foreach (string element in _elements)
            {
                musicList.Items.Add(TakeNameOfElement(element));
            }
        }

        private string TakeNameOfElement(string e)
        {
            string[] strings = e.Split(new char[] { ' ', '/', '.', ',', '-' }, StringSplitOptions.RemoveEmptyEntries);
            return strings[strings.Length - 2];
        }

        private void openFolderBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == true)
            {
                _elements.Add(openFileDialog.FileName);
            }
            NewElements();
        }

        private void playBtn_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Play();
        }

        private void pauseBtn_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Pause();
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            mediaElement.Stop();
        }

        private void musicList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mediaElement.Source = new Uri(_elements[musicList.SelectedIndex]);
        }

        private void mediaElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            TimeSpan duration = mediaElement.NaturalDuration.TimeSpan;
            musicSlider.Maximum = duration.TotalSeconds;
            wholeTime.Text = $"{duration.Hours:D2}:{duration.Minutes:D2}:{duration.Seconds:D2}";
            UpdatePositionText();
        }

        private void UpdatePositionText()
        {
            TimeSpan currentPosition = mediaElement.Position;
            currentPos.Text = $"{currentPosition.Hours:D2}:{currentPosition.Minutes:D2}:{currentPosition.Seconds:D2}";
        }

        private void musicSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TimeSpan newPosition = TimeSpan.FromSeconds(musicSlider.Value);
            mediaElement.Position = newPosition;
            UpdatePositionText();
        }
    }
}