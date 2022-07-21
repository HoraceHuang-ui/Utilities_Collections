using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Utilities_Fix.utilities_pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class physcalc : Page
    {
        public physcalc()
        {
            this.InitializeComponent();
        }

        List<string> strings = new List<string>();
        List<double> datas = new List<double>();
        SettingsData settings = new SettingsData();
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await settings.GetLocalSettingsAsync();
            strings.Add(settings.language_eng ? "Undo" : "撤销");
            data_trace.ItemsSource = new List<string>(strings);
            data_trace.SelectedIndex = 0;
        }
        private void data_trace_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int n = data_trace.SelectedIndex;
            for (int i = strings.Count() - 1; i > n; i--)
            {
                strings.RemoveAt(i);
                if (i != 0)
                    datas.RemoveAt(i - 1);
            }
            data_input.Text = string.Empty;
            data_trace.ItemsSource = new List<string>(strings);
        }
        private void data_input_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            datas.Add(double.Parse(sender.Text));
            strings.Add("(" + strings.Count().ToString() + ") " + sender.Text);
            data_trace.ItemsSource = strings;
            if (settings.keep_integers)
            {
                int i;
                for (i = 0; i < sender.Text.Length && sender.Text[i] != '.'; i++) ;
                sender.Text = sender.Text.Substring(0, Math.Min(i+1, sender.Text.Length));
            } 
            else
                sender.Text = string.Empty;

            if (!data_trace.IsEnabled)
                data_trace.IsEnabled = true;
            double average = datas.Count() == 1 ? datas[0] : datas.Average();
            double uncertainty = 0.0;
            foreach (double v in datas)
            {
                uncertainty += Math.Pow(v - average, 2);
            }
            uncertainty /= (datas.Count()*(datas.Count() - 1));
            uncertainty = Math.Sqrt(uncertainty);

            int auto_accuracy = DetectDecimalAccuracy();

            average_box.Text = (settings.language_eng ? "Average = " : "平均值：") + 
                Math.Round(average, settings.auto_rounding ? auto_accuracy : settings.physex_accuracy)
                .ToString();

            uncertainty_box.Text = (settings.language_eng ? "Uncertainty = " : "不确定度：") + 
                Math.Round(uncertainty, settings.auto_rounding ? auto_accuracy : settings.physex_accuracy)
                .ToString();
            
            relative_uncertainty_box.Text = (settings.language_eng ? "Relative uncertainty = " : "相对不确定度：") + 
                Math.Round(uncertainty / average * 100, settings.auto_rounding ? auto_accuracy : settings.physex_accuracy)
                .ToString() + " %";

            data_trace.ItemsSource = new List<string>(strings);
        }

        private int DetectDecimalAccuracy()
        {
            int res = 0;
            foreach (string s in strings)
            {
                int i;
                for (i = 0; i < s.Length && s[i] != '.'; i++) ;
                if (s.Length - i - 1 > res) 
                    res = s.Length - i - 1;
            }
            return res < 0 ? 0 : res;
        }
    }
}
