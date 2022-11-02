using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace NMEATrax_Replay_app
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool playbackEn;
        private double waitTime = 1.0;

        public MainWindow()
        {
            InitializeComponent();
            lineScroll.Maximum = File.ReadAllLines(@"../../../../data.csv").Length - 1;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            updateData();
        }

        private void lineScroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateData();
        }

        private void updateData()
        {
            //https://stackoverflow.com/a/30329207
            // open the file "data.csv" which is a CSV file with headers
            var lines = File.ReadLines(@"../../../../data.csv");
            string line = lines.ElementAtOrDefault((Index)lineScroll.Value); // null if there are less lines
            string[] splitData = line.Split(",");
            outputBox.Text = line;
            string valString = lineScroll.Value.ToString();
            curLnNum.Text = valString;

            rpmBox.Text = splitData[0];
            etempBox.Text = splitData[1];
            otempBox.Text = splitData[2];
            opresBox.Text = splitData[3];
            exhTempBox.Text = splitData[4];
            fuelRateBox.Text = splitData[5];
            fpresBox.Text = splitData[6];
            flevelBox.Text = splitData[7];
            legTiltBox.Text = splitData[8];
            ehoursBox.Text = splitData[9];
            gearBox.Text = splitData[10];
            latBox.Text = splitData[11];
            lonBox.Text = splitData[12];
            speedBox.Text = splitData[13];
            headingBox.Text = splitData[14];
            magVarBox.Text = splitData[15];
            xteBox.Text = splitData[16];
            depthBox.Text = splitData[17];
            wtempBox.Text = splitData[18];
            battVBox.Text = splitData[19];
            timeStampBox.Text = splitData[20];
        }

        private void incrementBtn_Click(object sender, RoutedEventArgs e)
        {
            lineScroll.Value++;
            updateData();
        }

        private void decrementBtn_Click(object sender, RoutedEventArgs e)
        {
            lineScroll.Value--;
            updateData();
        }

        private void lineScroll_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                lineScroll.Value++;
            } else if (e.Delta < 0)
            {
                lineScroll.Value--;
            }
        }

        private async void playBtn_Click(object sender, RoutedEventArgs e)
        {
            playbackEn = true;
            while (playbackEn)
            {
                try
                {
                    lineScroll.Value++;
                    await Task.Delay((int)((1.0/waitTime)*1000));
                }
                catch (Exception)
                {
                    outputBox.Text = e.ToString();
                    throw;
                }
                
            }
        }

        private void stopBtn_Click(object sender, RoutedEventArgs e)
        {
            playbackEn = false;
        }

        private void spdDec_Click(object sender, RoutedEventArgs e)
        {
            waitTime--;
        }

        private void spdInc_Click(object sender, RoutedEventArgs e)
        {
            waitTime++;
        }
    }
}