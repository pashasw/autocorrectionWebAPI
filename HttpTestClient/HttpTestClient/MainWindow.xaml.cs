using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HttpTestClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            corrComboBox.Visibility  = Visibility.Hidden;
        }
        private void FindIndexOfLastWord(string text,out int start)
        {
            text = Regex.Replace(text, @"[^\w\s]", "");
            if (text.EndsWith(' '))
                text = text.Remove(text.Length - 1);
            var words = text.Split(' ');
            var word = words[words.Length - 1];
            start = text.IndexOf(word);
        }
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var text = textBox.Text;
            if (text.EndsWith(' ') || Char.IsPunctuation(text.ToCharArray()[text.ToCharArray().Length -1]))
            {
                text = Regex.Replace(text, @"[^\w\s]", "");
                if (text.EndsWith(' '))
                    text = text.Remove(text.Length-1);
                var words = text.Split(' ');
                var word = words[words.Length -1];
                var autoCorr = SendToAutoCorrection(word);
                if (autoCorr.Any(word.Contains))
                    return;
                corrComboBox.Visibility = Visibility.Visible;
                corrComboBox.ItemsSource = autoCorr.Distinct();
                corrComboBox.IsDropDownOpen = true;
            }
        }
        private string[] SendToAutoCorrection(string input)
        {
            var endpoint = ConfigurationManager.AppSettings.Get("EndPoint").ToString();
            var request = (HttpWebRequest)WebRequest.Create(endpoint  +"?input=" + input);
            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                var words = responseString.Split(' ');
                return words;
            }
            catch (Exception exc)
            { 
                return new string[] { exc.Message}; 
            }
        }
        private string ChangeLastWord(string text, string word, int start)
        {
            char lastChar = text.ToCharArray()[text.Length-1];
            string output = "";
            for (int i = 0; i < start; i++)
                output += text.ToCharArray()[i];
            for (int i = 0; i < word.Length; i++)
                output += word.ToCharArray()[i];
            output += lastChar;
            return output;
        }
        private void corrComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var text = textBox.Text;
            int start = 0;
            FindIndexOfLastWord(text, out  start);
            if (corrComboBox.SelectedItem != null)
                textBox.Text = ChangeLastWord(text, corrComboBox.SelectedItem.ToString(), start);
            corrComboBox.Visibility = Visibility.Hidden;
            corrComboBox.ItemsSource = null;
        }
    }
}
