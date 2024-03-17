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

        private void ButtonCheck_Click(object sender, RoutedEventArgs e)
        {
            TextBlockCheck.Text = "";
            var input = TextBoxInput.Text;
            if (IsEmpty(input)) return;
            try
            {
                // Json形式でない場合、シリアライザーが例外を返す
                JsonConvert.DeserializeObject<JObject>(input);
                TextBlockCheck.Text = "Success !!!";
            }
            catch (Exception ex)
            {
                TextBlockCheck.Text = ex.Message;
            }
        }

        private void ButtonIndentAdd_Click(object sender, RoutedEventArgs e) => RunJsonHelper(input =>
        {
            // 文字列をそのままシリアライズすると、escapeが入るだけ。文字列を一旦オブジェクトに戻す。
            //TextBoxOutput.Text = JsonConvert.SerializeObject(input, Formatting.Indented);
            var jObject = JsonConvert.DeserializeObject<JObject>(input);
            TextBoxOutput.Text = JsonConvert.SerializeObject(jObject, Formatting.Indented);
        });

        private void ButtonIndentDel_Click(object sender, RoutedEventArgs e) => RunJsonHelper(input =>
        {
            var jObject = JsonConvert.DeserializeObject<JObject>(input);
            TextBoxOutput.Text = JsonConvert.SerializeObject(jObject, Formatting.None);
        });

        private void ButtonEscapeAdd_Click(object sender, RoutedEventArgs e) => RunJsonHelper(input =>
        {
            TextBoxOutput.Text = JsonConvert.SerializeObject(input, Formatting.None);
        });

        private void ButtonEscapeDel_Click(object sender, RoutedEventArgs e) => RunJsonHelper(input =>
        {
            var jToken = JsonConvert.DeserializeObject<JToken>(input);
            TextBoxOutput.Text = jToken.ToString();
        });

        private void ButtonBase64_Click(object sender, RoutedEventArgs e) => RunJsonHelper(input =>
        {
            // 必要な処理を定義
            TextBoxOutput.Text = input;
        });

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            TextBoxInput.Text = "";
            TextBoxOutput.Text = "";
        }

        private void ButtonCopy_Click(object sender, RoutedEventArgs e)
        {
            var output = TextBoxOutput.Text;
            if (IsEmpty(output)) return;
            Clipboard.SetText(output);
        }

        private void ButtonSwap_Click(object sender, RoutedEventArgs e)
        {
            (TextBoxInput.Text, TextBoxOutput.Text) = (TextBoxOutput.Text, TextBoxInput.Text);
        }

        // ---------------------------------------------------------------------------------------------

        private void RunJsonHelper(Action<string> action)
        {
            var input = TextBoxInput.Text;
            if (!IsValid(input)) return;
            try
            {
                action.Invoke(input);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                ShowWarnMessage("変換に失敗しました" + Environment.NewLine + ex.Message);
            }
        }

        private bool IsValid(string value)
        {
            if (IsEmpty(value))
            {
                ShowWarnMessage("入力データがありません");
                return false;
            }
            return true;
        }

        private bool IsEmpty(string value)
        {
            if (value == null) return true;
            return value.Length == 0;
        }

        private void ShowWarnMessage(string msg)
        {
            MessageBox.Show(msg, "JsonHelper", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
