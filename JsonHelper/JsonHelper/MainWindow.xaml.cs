using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace JsonHelper
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

        private void ButtonIndentAdd_Click(object sender, RoutedEventArgs e)
        {
            var input = TextBoxInput.Text;
            if (!isValid(input)) return;
            try
            {
                // 文字列をそのままシリアライズすると、escapeが入るだけ。文字列を一旦オブジェクトに戻す。
                //TextBoxOutput.Text = JsonConvert.SerializeObject(input, Formatting.Indented);
                var jObject = JsonConvert.DeserializeObject<JObject>(input);
                TextBoxOutput.Text = JsonConvert.SerializeObject(jObject, Formatting.Indented);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                showWarnMessage("変換に失敗しました" + Environment.NewLine + ex.Message);
            }
        }

        private void ButtonIndentDel_Click(object sender, RoutedEventArgs e)
        {
            var input = TextBoxInput.Text;
            if (!isValid(input)) return;
            try
            {
                var jObject = JsonConvert.DeserializeObject<JObject>(input);
                TextBoxOutput.Text = JsonConvert.SerializeObject(jObject, Formatting.None);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                showWarnMessage("変換に失敗しました" + Environment.NewLine + ex.Message);
            }
        }

        private void ButtonEscapeAdd_Click(object sender, RoutedEventArgs e)
        {
            var input = TextBoxInput.Text;
            if (!isValid(input)) return;
            try
            {
                TextBoxOutput.Text = JsonConvert.SerializeObject(input, Formatting.None);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                showWarnMessage("変換に失敗しました" + Environment.NewLine + ex.Message);
            }
        }

        private void ButtonEscapeDel_Click(object sender, RoutedEventArgs e)
        {
            var input = TextBoxInput.Text;
            if (!isValid(input)) return;
            try
            {
                var jToken = JsonConvert.DeserializeObject<JToken>(input);
                TextBoxOutput.Text = jToken.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                showWarnMessage("変換に失敗しました" + Environment.NewLine + ex.Message);
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            TextBoxInput.Text = "";
            TextBoxOutput.Text = "";
        }

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            var output = TextBoxOutput.Text;
            if (isEmpty(output)) return;
            Clipboard.SetText(output);
        }

        // ---------------------------------------------------------------------------------------------

        private bool isValid(string value)
        {
            if (isEmpty(value))
            {
                showWarnMessage("入力データがありません");
                return false;
            }
            return true;
        }

        private bool isEmpty(string value)
        {
            if (value == null) return true;
            return value.Length == 0;
        }

        private void showWarnMessage(string msg)
        {
            MessageBox.Show(msg, "JsonHelper", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
