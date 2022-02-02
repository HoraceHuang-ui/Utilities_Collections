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
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Utilities_Fix.utilities_pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class IP_request : Page
    {
        public IP_request()
        {
            this.InitializeComponent();
        }

        SettingsData settings = new SettingsData();
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await settings.GetLocalSettingsAsync();
        }
        string decode_json(string s)
        {
            int tab = 0;
            int i;
            string res = "";
            bool nl = false;
            for (i = 0; i < s.Length; i++)
            {
                if (nl)
                {
                    for (int j = 0; j < tab; j++)
                    {
                        res += "    ";
                        nl = false;
                    }
                }
                switch (s[i])
                {
                    case '{':
                        tab++;
                        res += "{\n";
                        nl = true;
                        break;
                    case '}':
                        tab--;
                        res += "\n";
                        for (int j = 0; j < tab; j++)
                        {
                            res += "    ";
                            nl = false;
                        }
                        res += "}";
                        break;
                    case ',':
                        res += ",\n";
                        nl = true;
                        break;
                    case '[':
                        res += "[\n";
                        nl = true;
                        tab++;
                        break;
                    case ']':
                        tab--;
                        res += "\n";
                        for (int j = 0; j < tab; j++)
                        {
                            res += "    ";
                            nl = false;
                        }
                        res += "]";
                        break;
                    default:
                        res += s[i];
                        break;
                }
            }
            return res;
        }

        string request(string url, string ip)
        {
            url += settings.apikey + "&ip=" + ip;

            var request = WebRequest.Create(url);
            request.Method = "GET";

            var webResponse = request.GetResponse();
            var webStream = webResponse.GetResponseStream();

            var reader = new StreamReader(webStream);
            return reader.ReadToEnd();
        }

        private void ip_textbox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
            try
            {
                info_textbox.Text = resourceLoader.GetString("tianapi_loading");
                result.Text = "";
                var url = "http://api.tianapi.com/txapi/ipquery/index?key=";
                string ip = ip_textbox.Text;
                Task<string> request_ip = Task.Run(() => request(url, ip));
                string data = request_ip.Result;
                result.Text = decode_json(data);
                string code = data.Substring(8, 3);
                if (code != "200")
                {
                    info_textbox.Text = resourceLoader.GetString("ip_res_error");
                    info_textbox.Text += resourceLoader.GetString("error" + code);
                }

                else
                {
                    info_textbox.Text = resourceLoader.GetString("ip_res_success");
                }
            }
            catch (Exception ex)
            {
                info_textbox.Text = resourceLoader.GetString("tianapi_error");
                result.Text = ex.Message;
                result.Text += "\n\n" + ex.GetType().Name;
            }
        }
    }
}
