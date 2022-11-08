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
        private string dataFilePath = string.Empty;

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
            minRPM.Text= "0";
            maxRPM.Text= "5000";
            minOtemp.Text= "0";
            maxOtemp.Text= "150";
            minOpres.Text = "300";
            maxOpres.Text = "700";
            minEtemp.Text = "0";
            maxEtemp.Text= "80";
            minFpres.Text = "600";
            maxFpres.Text= "700";
            minBattV.Text = "11";
            maxBattV.Text= "15";
            minFlevel.Text = "10";
            maxFlevel.Text= "100";
            minDepth.Text = "5";
            maxDepth.Text= "10000";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (getFilePath())
            {
                lineScroll.Maximum = File.ReadAllLines(dataFilePath).Length - 1;
                updateData();

                fileNameBox.Text = dataFilePath;
            }
        }

        private void lineScroll_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            updateData();
        }

        private void updateData()
        {
            if (dataFilePath == string.Empty)
            {
                outputBox.Text = "Please select a file.";
            } 
            else
            {
                //https://stackoverflow.com/a/30329207
                var lines = File.ReadLines(dataFilePath);
                string line = lines.ElementAtOrDefault((Index)(lineScroll.Value)); // null if there are less lines
                string[] splitData = line.Split(",");
                outputBox.Text = line;
                curLnNum.Text = (lineScroll.Value + 1).ToString();

                rpmBox.Text = splitData[0];
                etempBox.Text = splitData[1];
                otempBox.Text = splitData[2];
                opresBox.Text = splitData[3];
                fuelRateBox.Text = splitData[4];
                fpresBox.Text = splitData[5];
                flevelBox.Text = splitData[6];
                legTiltBox.Text = splitData[7];
                speedBox.Text = splitData[8];
                headingBox.Text = splitData[9];
                xteBox.Text = splitData[10];
                depthBox.Text = splitData[11];
                wtempBox.Text = splitData[12];
                battVBox.Text = splitData[13];
                ehoursBox.Text = splitData[14];
                gearBox.Text = splitData[15];
                latBox.Text = splitData[16];
                lonBox.Text = splitData[17];
                magVarBox.Text = splitData[18];
                timeStampBox.Text = splitData[19];

                if (double.Parse(rpmBox.Text) < double.Parse(minRPM.Text) || double.Parse(rpmBox.Text) > double.Parse(maxRPM.Text)) rpmBox.Background = new SolidColorBrush(Colors.Red);
                else rpmBox.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));

                if (double.Parse(etempBox.Text) < double.Parse(minEtemp.Text) || double.Parse(etempBox.Text) > double.Parse(maxEtemp.Text)) etempBox.Background = new SolidColorBrush(Colors.Red);
                else etempBox.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));

                if (double.Parse(otempBox.Text) < double.Parse(minOtemp.Text) || double.Parse(otempBox.Text) > double.Parse(maxOtemp.Text)) otempBox.Background = new SolidColorBrush(Colors.Red);
                else otempBox.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));

                if (double.Parse(opresBox.Text) < double.Parse(minOpres.Text) || double.Parse(opresBox.Text) > double.Parse(maxOpres.Text)) opresBox.Background = new SolidColorBrush(Colors.Red);
                else opresBox.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));

                if (double.Parse(fpresBox.Text) < double.Parse(minFpres.Text) || double.Parse(fpresBox.Text) > double.Parse(maxFpres.Text)) fpresBox.Background = new SolidColorBrush(Colors.Red);
                else fpresBox.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));

                if (double.Parse(battVBox.Text) < double.Parse(minBattV.Text) || double.Parse(battVBox.Text) > double.Parse(maxBattV.Text)) battVBox.Background = new SolidColorBrush(Colors.Red);
                else battVBox.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));

                if (double.Parse(flevelBox.Text) < double.Parse(minFlevel.Text) || double.Parse(flevelBox.Text) > double.Parse(maxFlevel.Text)) flevelBox.Background = new SolidColorBrush(Colors.Red);
                else flevelBox.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));

                if (double.Parse(depthBox.Text) < double.Parse(minDepth.Text) || double.Parse(depthBox.Text) > double.Parse(maxDepth.Text)) depthBox.Background = new SolidColorBrush(Colors.Red);
                else depthBox.Background = new SolidColorBrush(Color.FromRgb(200, 200, 200));
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
                if (lineScroll.Value >= lineScroll.Maximum)
                {
                    playbackEn = false;
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
                dataFilePath = dlg.FileName;
                if (dataFilePath != string.Empty)
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
            if (dataFilePath == string.Empty)
            {
                outputBox.Text = "Please select a file.";
            }
            else
            {
                var lines = File.ReadLines(dataFilePath);
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

        private void saveLimitsBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "csv files (*.csv)|*.csv";         // file type / extension

            if (dlg.ShowDialog() == true)
            {
                string limits = minRPM.Text.ToString() + ",";
                limits += maxRPM.Text.ToString() + ",";
                limits += minEtemp.Text.ToString() + ",";
                limits += maxEtemp.Text.ToString() + ",";
                limits += minOtemp.Text.ToString() + ",";
                limits += maxOtemp.Text.ToString() + ",";
                limits += minOpres.Text.ToString() + ",";
                limits += maxOpres.Text.ToString() + ",";
                limits += minFpres.Text.ToString() + ",";
                limits += maxFpres.Text.ToString() + ",";
                limits += minFlevel.Text.ToString() + ",";
                limits += maxFlevel.Text.ToString() + ",";
                limits += minBattV.Text.ToString() + ",";
                limits += maxBattV.Text.ToString() + ",";
                limits += minDepth.Text.ToString() + ",";
                limits += maxDepth.Text.ToString();
                File.WriteAllText(dlg.FileName, limits);
            }
        }

        private void loadLimitsBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            //dlg.InitialDirectory = "c:\\";
            dlg.Filter = "csv files (*.csv)|*.csv";
            dlg.FilterIndex = 1;

            if (dlg.ShowDialog() == true)
            {
                var limits = File.ReadAllText(dlg.FileName);
                string[] spiltLimits = limits.Split(",");
                minRPM.Text= spiltLimits[0];
                maxRPM.Text = spiltLimits[1];
                minEtemp.Text= spiltLimits[2];
                maxEtemp.Text= spiltLimits[3];
                minOtemp.Text= spiltLimits[4];
                maxOtemp.Text= spiltLimits[5];
                minOpres.Text= spiltLimits[6];
                maxOpres.Text= spiltLimits[7];
                minFpres.Text= spiltLimits[8];
                maxFpres.Text= spiltLimits[9];
                minFlevel.Text= spiltLimits[10];
                maxFlevel.Text= spiltLimits[11];
                minBattV.Text= spiltLimits[12];
                maxBattV.Text= spiltLimits[13];
                minDepth.Text= spiltLimits[14];
                maxDepth.Text= spiltLimits[15];
            }
        }
    }
}