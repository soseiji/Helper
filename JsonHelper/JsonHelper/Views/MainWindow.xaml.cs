using JsonHelper.Models;
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

namespace JsonHelper.Views
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
            (var isSuccess, var errMessage) = jsonService.IsValidJson(TextBoxInput.Text);
            TextBlockCheck.Text = isSuccess ? "Success !!!" : errMessage;
        }

        private void ButtonAddIndent_Click(object sender, RoutedEventArgs e) => RunAction(input => TextBoxOutput.Text = jsonService.AddIndent(input));
        private void ButtonDelIndent_Click(object sender, RoutedEventArgs e) => RunAction(input => TextBoxOutput.Text = jsonService.DelIndent(input));
        private void ButtonAddEscape_Click(object sender, RoutedEventArgs e) => RunAction(input => TextBoxOutput.Text = jsonService.AddEscapeStr(input));
        private void ButtonDelEscape_Click(object sender, RoutedEventArgs e) => RunAction(input => TextBoxOutput.Text = jsonService.DelEscapeStr(input));
        private void ButtonConvertGzipHexStr_Click(object sender, RoutedEventArgs e) => RunAction(input => TextBoxOutput.Text = jsonService.ConvertFromGzipHexStr(input));
        private void ButtonConvertGzipBase64_Click(object sender, RoutedEventArgs e) => RunAction(input => TextBoxOutput.Text = jsonService.ConvertFromGzipBase64(input));

        private void ButtonClear_Click(object sender, RoutedEventArgs e) => (TextBoxInput.Text, TextBoxOutput.Text) = ("", "");
        private void ButtonSwap_Click(object sender, RoutedEventArgs e) => (TextBoxInput.Text, TextBoxOutput.Text) = (TextBoxOutput.Text, TextBoxInput.Text);
        private void ButtonInputCopy_Click(object sender, RoutedEventArgs e) => SetClipboard(TextBoxInput.Text);
        private void ButtonOutputCopy_Click(object sender, RoutedEventArgs e) => SetClipboard(TextBoxOutput.Text);

        // ---------------------------------------------------------------------------------------------



        private void RunAction(Action<string> action)
        {
            var input = TextBoxInput.Text;
            if (!IsValid(input)) return;
            try
            {
                action.Invoke(input);
            }
            catch (Exception ex)
            {
                ShowWarnMessage("変換に失敗しました" + Environment.NewLine + ex.Message);
            }
        }

        private bool IsValid(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                ShowWarnMessage("入力データがありません");
                return false;
            }
            return true;
        }

        private void ShowWarnMessage(string msg)
        {
            MessageBox.Show(msg, "JsonHelper", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void SetClipboard(string text)
        {
            if (string.IsNullOrEmpty(text)) return;
            Clipboard.SetText(text);
        }

        private readonly JsonService jsonService = new();
    }
}
