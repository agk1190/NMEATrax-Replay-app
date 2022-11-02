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
            lineScroll.Maximum = File.ReadAllLines(@"../../../../data.csv").Length;
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
            outputBox.Text = line;
            string valString = lineScroll.Value.ToString();
            curLnNum.Text = valString;
        }
    }
}