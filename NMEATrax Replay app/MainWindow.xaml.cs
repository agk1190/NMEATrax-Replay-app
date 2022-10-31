using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "json files (*.json)|*json";

            //if (openFileDialog.ShowDialog() == true)
            //{
            //    FileStream fs = new FileStream(openFileDialog.FileName, FileMode.Open);
            //    outputBox.Text = fs.ToString();
            //    fs.Close();
            //}
            var json = System.IO.File.ReadAllText(@"..\..\..\..\data1.json");
            var objects = JArray.Parse(json); // parse as array  
            
            foreach (JObject root in objects)
            {
                foreach (KeyValuePair<String, JToken> app in root)
                {
                    var appName = app.Key;
                    var value = (String)app.Value["value"];

                    Console.WriteLine(appName);
                    Console.WriteLine(value);
                    Console.WriteLine("\n");

                    outputBox.Text += appName;
                    outputBox.Text += ",";
                    outputBox.Text += value;
                    outputBox.Text += "\n";

                    if (appName == "rpm")
                    {
                        rpmBox.Text = value;
                    }
                }
            }
        }
    }
}
