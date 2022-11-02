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
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (TextFieldParser parser = new TextFieldParser(@"..\..\..\..\data.csv"))
            {
                //https://stackoverflow.com/a/30329207
                // open the file "data.csv" which is a CSV file with headers
                var lines = File.ReadLines(@"../../../../data.csv");
                double.TryParse(lnNum.Text, out double lineNum);
                string line = lines.ElementAtOrDefault((Index)lineNum); // null if there are less lines
                outputBox.Text = line;
            }
        }
    }
}