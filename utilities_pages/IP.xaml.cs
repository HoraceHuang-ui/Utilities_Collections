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
using Windows.ApplicationModel.Resources;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Utilities_Fix.utilities_pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class IP : Page
    {
        public IP()
        {
            this.InitializeComponent();
        }
        SettingsData settings = new SettingsData();
        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await settings.GetLocalSettingsAsync();
        }

        int find(string s, string arg)
        {
            for (int i = 0; i < s.Length - arg.Length; i++)
                if (s.Substring(i, arg.Length) == arg)
                    return i;
            return -1;
        }

        string get_ip(string s)
        {
            string res = "a";
            int a = find(s, "\"ip\":");
            if (a == -1)
                res = "nf";
            else
                for (int i = a + 6; s[i] != '\"'; i++)
                    res += s[i];
            return res == "nf" ? res : a.ToString() + res;
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

        string request(string url, string domain)
        {
            url += settings.apikey + "&domain=" + domain;

            WebRequest request = WebRequest.Create(url);
            request.Method = "GET";

            WebResponse webResponse = request.GetResponse();
            Stream webStream = webResponse.GetResponseStream();

            StreamReader reader = new StreamReader(webStream);
            return reader.ReadToEnd();
        }

        private void url_textbox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView();
            try
            {
                ip_textbox.Text = resourceLoader.GetString("tianapi_loading");
                result.Text = "";
                string domain = url_textbox.Text;
                Task<string> request_data = Task.Run(() => request("http://api.tianapi.com/txapi/domain/index?key=", domain));
                string data = request_data.Result;

                result.Text = decode_json(data);
                string code = data.Substring(8, 3);
                if (code != "200")
                {
                    ip_textbox.Text = resourceLoader.GetString("ip_res_error");
                    ip_textbox.Text += resourceLoader.GetString("error" + code);
                }
                else
                {
                    string ips = "a";
                    string a = get_ip(data);
                    int coord, i;
                    while (a != "nf")
                    {
                        i = find(a, "a");
                        coord = int.Parse(a.Substring(0, i));
                        ips += a.Substring(i + 1) + " | ";
                        data = data.Substring(coord + 15);
                        a = get_ip(data);
                    }
                    ip_textbox.Text = ips.Substring(1, ips.Length - 4);
                }
            }
            catch (Exception ex)
            {
                ip_textbox.Text = resourceLoader.GetString("tianapi_error");
                result.Text = ex.Message;
                result.Text += "\n\n" + ex.GetType().Name;
            }
        }
    }
}
