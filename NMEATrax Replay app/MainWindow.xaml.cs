using Microsoft.Win32;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

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
            string[] variables = { "RPM", "Engine Temp", "Oil Temp", "Oil Pressure", 
                "Fuel Rate", "Fuel Pressure", "Fuel Level", "Leg Tilt", "Speed", "Heading", 
                "Depth", "Water Temp", "Battery Voltage" };
            analyzeOptBox.ItemsSource = variables;      // set the variables available for analyzing
            analyzeOptBox.SelectedIndex = 0;
            minBox.Text = "0";      // set the default live limits
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
                try
                {
                    lineScroll.Maximum = File.ReadAllLines(dataFilePath).Length - 1;        // detect and set the maximum lines avalible to scroll
                    updateData();

                    fileNameBox.Text = dataFilePath;        // display the file opened
                }
                catch (System.IO.IOException)
                {
                    outputBox.Text = "Please close the file you are trying to open.";
                }
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
                // reference https://stackoverflow.com/a/30329207
                var lines = File.ReadLines(dataFilePath);
                string line = lines.ElementAtOrDefault((Index)(lineScroll.Value)); // retrieve data from the csv file at the line requested by the scroll bar
                string[] splitData = line.Split(",");
                outputBox.Text = line;
                curLnNum.Text = (lineScroll.Value + 1).ToString();

                rpmBox.Text = splitData[0];     // populate the live data boxes with the data
                etempBox.Text = splitData[1];
                otempBox.Text = splitData[2];
                opresBox.Text = splitData[3];
                fuelRateBox.Text = splitData[4];
                fpresBox.Text = splitData[5];
                flevelBox.Text = splitData[6];
                legTiltBox.Text = splitData[7];
                speedBox.Text = splitData[8];
                headingBox.Text = splitData[9];
                depthBox.Text = splitData[10];
                wtempBox.Text = splitData[11];
                battVBox.Text = splitData[12];
                ehoursBox.Text = splitData[13];
                gearBox.Text = splitData[14];
                latBox.Text = splitData[15];
                lonBox.Text = splitData[16];
                magVarBox.Text = splitData[17];
                timeStampBox.Text = splitData[20];
                if (splitData[18] == "1")
                {
                    etempLabel.Content = "Engine Temp (°F)";
                    otempLabel.Content = "Oil Temp (°F)";
                    wtempLabel.Content = "Water Temp (°F)";
                } else
                {
                    etempLabel.Content = "Engine Temp (°C)";
                    otempLabel.Content = "Oil Temp (°C)";
                    wtempLabel.Content = "Water Temp (°C)";
                }
                if (splitData[19] == "1")
                {
                    depthLabel.Content = "Water Depth (m)";
                }
                else
                {
                    depthLabel.Content = "Water Depth (ft)";
                }

                // highlight the corresponding live data box when the data exceeds the limit specified
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
            if (e.Delta > 0)        // move the scroll bar when using scroll wheel
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
                    //throw;
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
                playbackSpeedBox.Text = multiplier[multiCount].ToString();      // display current speed number
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