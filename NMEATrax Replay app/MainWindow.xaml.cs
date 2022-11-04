using Microsoft.VisualBasic.FileIO;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        private int multiCount = 0;
        private readonly int []multiplier = { 1, 2, 4, 8, 16, 32, 64 };
        private string filePath = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
            string[] variables = { "RPM", "Engine Temp", "Oil Temp", "Oil Pressure", "Exhaust Temp", 
                "Fuel Rate", "Fuel Pressure", "Fuel Level", "Leg Tilt", "Speed", "Heading", "Cross Track Error", 
                "Depth", "Water Temp", "Battery Voltage" };
            analyzeOptBox.ItemsSource = variables;
            analyzeOptBox.SelectedIndex = 0;
            minBox.Text = "0";
            maxBox.Text = "100";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (getFilePath())
            {
                lineScroll.Maximum = File.ReadAllLines(filePath).Length - 1;
                updateData();
            }
        }

        private void lineScroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateData();
        }

        private void updateData()
        {
            if (filePath == string.Empty)
            {
                outputBox.Text = "Please select a file.";
            } else
            {
                //https://stackoverflow.com/a/30329207
                var lines = File.ReadLines(filePath);
                string line = lines.ElementAtOrDefault((Index)(lineScroll.Value+1)); // null if there are less lines
                string[] splitData = line.Split(",");
                outputBox.Text = line;
                double temp = lineScroll.Value + 1;
                string valString = temp.ToString();
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
                speedBox.Text = splitData[9];
                headingBox.Text = splitData[10];
                xteBox.Text = splitData[11];
                depthBox.Text = splitData[12];
                wtempBox.Text = splitData[13];
                battVBox.Text = splitData[14];
                ehoursBox.Text = splitData[15];
                gearBox.Text = splitData[16];
                latBox.Text = splitData[17];
                lonBox.Text = splitData[18];
                magVarBox.Text = splitData[19];
                timeStampBox.Text = splitData[20] + splitData[21];
            }
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

        private async void playStopBtn_Click(object sender, RoutedEventArgs e)
        {
            playbackEn = !playbackEn;
            while (playbackEn)
            {
                try
                {
                    lineScroll.Value++;
                    await Task.Delay((int)((1.0 / multiplier[multiCount])*1000));
                }
                catch (Exception)
                {
                    outputBox.Text = e.ToString();
                    throw;
                }
            }
        }

        private void spdDec_Click(object sender, RoutedEventArgs e)
        {
            if (multiCount > 0)
            {
                multiCount--;
                playbackSpeedBox.Text = multiplier[multiCount].ToString();
            } else{}
        }

        private void spdInc_Click(object sender, RoutedEventArgs e)
        {
            if (multiCount < 6)
            {
                multiCount++;
                playbackSpeedBox.Text = multiplier[multiCount].ToString();
            }
        }

        bool getFilePath()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = "c:\\";
            dlg.Filter = "csv files (*.csv)|*.csv";
            dlg.FilterIndex = 1;
            
            if (dlg.ShowDialog() == true)
            {
                filePath = dlg.FileName;
                if (filePath != string.Empty)
                {
                    return (true);
                } else
                {
                    return (false);
                }
            }
            return (false);
        }

        void analyzeData(int item, double min, double max)
        {
            analyzeBox.Text = string.Empty;
            if (filePath == string.Empty)
            {
                outputBox.Text = "Please select a file.";
            }
            else
            {
                var lines = File.ReadLines(filePath);
                double count = 0;
                foreach (var line in lines)
                {
                    count++;
                    string[] splitData = line.Split(",");
                    double result = double.Parse(splitData[item]);
                    if (result > max)
                    {
                        analyzeBox.Text += "Upper Limit Exceeded @ line: " + count + "\r\n";
                    }
                    if (result < min)
                    {
                        analyzeBox.Text += "Lower Limit Exceeded @ line: " + count + "\r\n";
                    }
                }
                if (analyzeBox.Text == string.Empty)
                {
                    analyzeBox.Text = "No limits exceeded.";
                }
            }
        }

        private void analyzeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (minBox.Text == string.Empty || maxBox.Text == string.Empty)
            {
                analyzeBox.Text = "One or both limits are missing!";
            } 
            else
            {
                double min = double.Parse(minBox.Text);
                double max = double.Parse(maxBox.Text);
                analyzeData(analyzeOptBox.SelectedIndex, min, max);
            }
        }

        private void curLnNum_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (curLnNum.Text != string.Empty)
                {
                    lineScroll.Value = double.Parse(curLnNum.Text) - 1;
                    updateData();
                }
            }
        }
    }
}