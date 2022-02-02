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
using Windows.Storage;
using System.Net;
using Windows.UI.Xaml.Media.Imaging;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Utilities_Fix.utilities_pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class bing_wp : Page
    {
        public bing_wp()
        {
            this.InitializeComponent();
        }

        SettingsData settings = new SettingsData();
        int idx = 0;
        string url = "";
        string res = "1920";
        int find(string s, string arg)
        {
            for (int i = 0; i < s.Length - arg.Length; i++)
                if (s.Substring(i, arg.Length) == arg)
                    return i;
            return -1;
        }

        string get_str(string s, string arg)
        {
            string res = "";
            int a = find(s, "\"" + arg + "\":");
            if (a == -1)
                res = "nf";
            else
                for (int i = a + arg.Length + 4; s[i] != '\"'; i++)
                    res += s[i];
            return res;
        }
        string reload_image()
        {
            try
            {
                url = "";
                var request = WebRequest.Create("https://bing.biturl.top/?resolution=" + res + "&format=json&index=" + idx.ToString() + "&mkt=" + settings.mkt);
                request.Method = "GET";

                WebResponse webResponse = request.GetResponse();
                Stream webStream = webResponse.GetResponseStream();

                StreamReader reader = new StreamReader(webStream);
                return reader.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        void SetValues(string data)
        {
            url = get_str(data, "url");

            string date = get_str(data, "start_date");
            imageInfo_flyout_text.Text = date.Substring(0, 4) + "." + date.Substring(4, 2) + "." + date.Substring(6);
            imageInfo_flyout_text.Text += "   " + settings.mkt;
            switch (res)
            {
                case "1366": imageInfo_flyout_text.Text += "   " + "1366 x 768"; break;
                case "1920": imageInfo_flyout_text.Text += "   " + "1920 x 1080"; break;
                case "3840": imageInfo_flyout_text.Text += "   " + "3840 x 2160"; break;
                default: imageInfo_flyout_text.Text += "   " + "1920 x 1080"; break;
            }
            imageInfo_flyout_text.Text += "\n";

            imageInfo_flyout_text.Text += get_str(data, "copyright");

            loading_sign.Visibility = Visibility.Collapsed;
            wp_image.Source = new BitmapImage(new Uri(url));
            save_image.IsEnabled = true;
            image_scroll.Visibility = Visibility.Visible;
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await settings.GetLocalSettingsAsync();
            switch (settings.resolution)
            {
                case 0: res = "1366"; break;
                case 1: res = "1920"; break;
                case 2: res = "3840"; break;
                default: res = "1920"; break;
            }
            loading_sign.Visibility = Visibility.Visible;
            try
            {
                image_scroll.Visibility = Visibility.Collapsed;
                Task<string> load_image = Task.Run(reload_image);
                SetValues(load_image.Result);
            }
            catch
            {
                image_scroll.Visibility = Visibility.Collapsed;
                loading_sign.Visibility = Visibility.Collapsed;
                wallpaper_failed.IsOpen = true;
                save_image.IsEnabled = false;
            }
        }

        private async void save_image_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }

        private void size_slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            image_scroll.ChangeView(null, null, (float)size_slider.Value / 50);
        }

        private void next_image_Click(object sender, RoutedEventArgs e)
        {
            if (idx == 7) prev_image.IsEnabled = true;
            idx -= 1;
            if (idx == 0) next_image.IsEnabled = false;
            loading_sign.Visibility = Visibility.Visible;
            try
            {
                image_scroll.Visibility = Visibility.Collapsed;
                Task<string> load_image = Task.Run(reload_image);
                SetValues(load_image.Result);
            }
            catch
            {
                image_scroll.Visibility = Visibility.Collapsed;
                loading_sign.Visibility = Visibility.Collapsed;
                wallpaper_failed.IsOpen = true;
                save_image.IsEnabled = false;
            }
        }

        private void prev_image_Click(object sender, RoutedEventArgs e)
        {
            if (idx == 0) next_image.IsEnabled = true;
            idx += 1;
            if (idx == 7) prev_image.IsEnabled = false;
            loading_sign.Visibility = Visibility.Visible;
            try
            {
                image_scroll.Visibility = Visibility.Collapsed;
                Task<string> load_image = Task.Run(reload_image);
                SetValues(load_image.Result);
            }
            catch
            {
                image_scroll.Visibility = Visibility.Collapsed;
                loading_sign.Visibility = Visibility.Collapsed;
                wallpaper_failed.IsOpen = true;
                save_image.IsEnabled = false;
            }
        }

        private void refresh_wallpaper_Click(object sender, RoutedEventArgs e)
        {
            wallpaper_failed.IsOpen = false;
            loading_sign.Visibility = Visibility.Visible;
            try
            {
                image_scroll.Visibility = Visibility.Collapsed;
                Task<string> load_image = Task.Run(reload_image);
                SetValues(load_image.Result);
            }
            catch
            {
                image_scroll.Visibility = Visibility.Collapsed;
                loading_sign.Visibility = Visibility.Collapsed;
                wallpaper_failed.IsOpen = true;
                save_image.IsEnabled = false;
            }
        }
    }
}
