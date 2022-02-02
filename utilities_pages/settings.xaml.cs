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
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Utilities_Fix.utilities_pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class settings : Page
    {
        bool done = false;
        SettingsData preferences = new SettingsData();

        ContentDialog func_const_list = new ContentDialog
        {
            Title = "支持的函数与常量",
            Content =
                "sqr  -  平方根  -  sqr9=3\n" +
                "sin  -  正弦  -  sin45=0.707\n" +
                "cos  -  余弦  -  co45=0.707\n" +
                "tan  -  正切  -  tan45=1\n" +
                "cot  -  余切  -  cot45=1\n" +
                "abs  -  绝对值  -  abs(-10)=10\n" +
                "cei  -  向上取整  -  cei4.3=5\n" +
                "flo  -  向下取整  -  flo4.7=4\n" +
                "log  -  对数  -  log[10,100]=2\n\n" +
                "E  -  自然对数的底数  -  2.7182818...\n" +
                "p  -  圆周率  -  3.1415926...",
            PrimaryButtonText = "明白",
            DefaultButton = ContentDialogButton.Primary,
            FontFamily = new FontFamily("segoe ui variable display")
        };
        ContentDialog reset_confirm = new ContentDialog
        {
            Title = "确定重置吗？",
            Content =
                "所有设置都将回归默认。重启应用即可生效。",
            PrimaryButtonText = "确定",
            SecondaryButtonText = "取消",
            DefaultButton = ContentDialogButton.Primary,
            FontFamily = new FontFamily("segoe ui variable display")
        };
        ContentDialog welcome = new ContentDialog()
        {
            Title = "本次更新内容：",
            Content = "欢迎使用 hyy 小工具大杂烩 V2.5.0！\n\n" +
                "修复了大量闪退问题，程序运行更稳定\n" +
                "必应每日壁纸会显示加载中，反馈更直观\n" +
                "更新后现可同步上一版本设置项\n" +
                "采用 Windows 11 风格图标，好康！\n\n" +
                "上一版本 V2.0.0 更新内容：\n" +
                "自由计算器 — 混合使用混合运算、函数、常量，计算器新物种\n" +
                "行列式计算器 - 线代考生必备！！！\n" +
                "BV / av 号转换 — 新老样式及链接互转，帮你刻入新 DNA\n" +
                "域名解析 — 输入域名即可解析出 IP 等要素，高端网民必备\n" +
                "IP 地址查询 — 输入 IP 即可解析出地区等要素，钓鱼网站无所遁形\n" +
                "号码归属地查询 — 什么？你的手机不支持显示归属地？小工具来帮你\n" +
                "必应每日壁纸 - 新的一天，新的美景，就在必应每日壁纸\n" +
                "命令提示符 — 没什么用但是好高端的亚子 :)",
            PrimaryButtonText = "芜湖起飞！!",
            DefaultButton = ContentDialogButton.Primary
        };
        public settings()
        {
            this.InitializeComponent();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await preferences.GetLocalSettingsAsync();

            if (preferences.display_theme == 0) rb_light.IsChecked = true;
            else if (preferences.display_theme == 1) rb_dark.IsChecked = true;
            else rb_default.IsChecked = true;
            if (preferences.language_eng)
            {
                english_switch.IsOn = true;

                func_const_list.Title = "Available Functions & Constants";
                func_const_list.Content =
                    "sqr  -  Square root  -  sqr9=3\n" +
                    "sin  -  Sine  -  sin45=0.707\n" +
                    "cos  -  Cosine  -  co45=0.707\n" +
                    "tan  -  Tangent  -  tan45=1\n" +
                    "cot  -  Cotangent  -  cot45=1\n" +
                    "abs  -  Absolute  -  abs(-10)=10\n" +
                    "cei  -  Ceiling  -  cei4.3=5\n" +
                    "flo  -  Floor  -  flo4.7=4\n" +
                    "log  -  logarithm  -  log[10,100]=2\n\n" +
                    "E  -  Euler's number  -  2.7182818...\n" +
                    "p  -  Pi  -  3.1415926...";
                func_const_list.PrimaryButtonText = "OK";

                reset_confirm.Title = "Are you sure?";
                reset_confirm.Content = "All your se/ttings will be reset to default. Restart the app to take effect.";
                reset_confirm.PrimaryButtonText = "OK";
                reset_confirm.SecondaryButtonText = "Cancel";

                welcome.Title = "What's New:";
                welcome.Content = "Welcome to Utilities by Hyy V2.5.0！\n\n" +
                "Fixed lots of force close problems, making the app more stable\n" +
                "Bing Daily Wallpaper now displays \"Loading...\"\n" +
                "Your settings from the previous version is now synced\n" +
                "Added a Windows 11 styled icon!\n\n" +
                "What's new in previous version V2.0.0:\n" +
                "FreeCalc - Combine basic and advanced calculations\n" +
                "Determinant Calculator - A necessity for Linear Algebra students\n" +
                "BV / av Converter - Shuffle through BV / av numbers and URL\n" +
                "Domain Analyzer - Extract key informations from a URL\n" +
                "IP Request - Extract key informations from an IP address\n" +
                "Phone Number Region - Extract key informations from a Chinese phone number\n" +
                "Bing Daily Wallpaper - Refresh yourself with daily random sceneries\n" +
                "Command Prompt - Nothing useful but looks advanced :)";
                welcome.PrimaryButtonText = "Woo-Hoo!!";
            }
            if (preferences.pane_top)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                panemode_ddb_text.Text = resourceLoader.GetString("toppane");
                panemode_ddb_icon.Glyph = "";
            }
            if (preferences.degree)
                degree_switch.IsOn = false;
            else
                degree_switch.IsOn = true;
            decimal_slider.Value = preferences.decimal_accuration;
            if (preferences.custom_apikey_switch)
            {
                custom_apikey_switch.IsOn = true;
                custom_apikey_box.Text = preferences.apikey;
                apikey_textblock.Opacity = 1;
            }
            if (preferences.custom_mkt_switch)
            {
                var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                custom_mkt_switch.IsOn = true;
                mkt_ddb.Content = resourceLoader.GetString(preferences.mkt.Substring(3, 2).ToLower());
                mkt_ddb.IsEnabled = true;
                mkt_textblock.Opacity = 1;
            }
            switch (preferences.resolution)
            {
                case 0: rb_1366.IsChecked = true; break;
                case 1: rb_1920.IsChecked = true; break;
                case 2: rb_3840.IsChecked = true; break;
                default: rb_1920.IsChecked = true; break;
            }
            done = true;
        }
        private async void english_switch_Toggled(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                if (english_switch.IsOn == true)
                {
                    Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US";
                    preferences.language_eng = true;
                    await preferences.RefreshLocalFileAsync();
                }
                else
                {
                    Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "zh-CN";
                    preferences.language_eng = false;
                    await preferences.RefreshLocalFileAsync();
                }
            }
        }

        private async void rb_light_Checked(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                preferences.display_theme = 0;
                await preferences.RefreshLocalFileAsync();
            }
        }

        private async void rb_dark_Checked(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                preferences.display_theme = 1;
                await preferences.RefreshLocalFileAsync();
            }
        }

        private async void rb_default_Checked(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                preferences.display_theme = 2;
                await preferences.RefreshLocalFileAsync();
            }
        }
        private async void degree_switch_Toggled(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                preferences.degree = !preferences.degree;
                await preferences.RefreshLocalFileAsync();
            }
        }

        private async void decimal_slider_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (done)
            {
                preferences.decimal_accuration = (int)decimal_slider.Value;
                await preferences.RefreshLocalFileAsync();
            }
        }

        private async void show_func_btn_Click(object sender, RoutedEventArgs e)
        {
            await func_const_list.ShowAsync();
        }

        private async void reset_all_btn_Click(object sender, RoutedEventArgs e)
        {
            ContentDialogResult res = await reset_confirm.ShowAsync();
            if (res == ContentDialogResult.Primary)
            {
                SettingsData a = new SettingsData();
                preferences.CopyDataFrom(a);
                preferences.first_time_open = false;
                await preferences.RefreshLocalFileAsync();
            }
        }

        //private void edit_settingsVal_hypertext_Click(object sender, RoutedEventArgs e)
        //{
        //    quick_edit_box.Visibility = Visibility.Visible;
        //    quick_edit_box.Text = settingsVal;
        //}

        //private async void quick_edit_box_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        //{
        //    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
        //    StorageFolder storageFolder = Windows.Storage.KnownFolders.DocumentsLibrary;
        //    StorageFile File = await storageFolder.GetFileAsync("utilities\\datav2_1_0.txt");
        //    settingsVal = quick_edit_box.Text;
        //    quick_edit_box.Text = resourceLoader.GetString("quick_edit_box");
        //    await FileIO.WriteTextAsync(File, settingsVal);
        //}

        private async void welcome_again_btn_Click(object sender, RoutedEventArgs e)
        {
            await welcome.ShowAsync();
        }

        private async void custom_apikey_switch_Toggled(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                if (custom_apikey_switch.IsOn)
                {
                    custom_apikey_box.IsEnabled = true;
                    apikey_textblock.Opacity = 1;
                    preferences.custom_apikey_switch = true;
                    await preferences.RefreshLocalFileAsync();
                }
                else
                {
                    apikey_textblock.Opacity = 0.4;
                    preferences.custom_apikey_switch = false;
                    preferences.apikey = "a9c1ad156691364ebfe8b3f1ff4eb153";
                    custom_apikey_box.Text = "a9c1ad156691364ebfe8b3f1ff4eb153";
                    custom_apikey_box.IsEnabled = false;
                    apikey_textblock.Opacity = 0.4;
                    await preferences.RefreshLocalFileAsync();
                }
            }
        }

        private async void custom_apikey_box_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (done)
            {
                preferences.apikey = custom_apikey_box.Text;
                await preferences.RefreshLocalFileAsync();
                custom_apikey_box.Text = english_switch.IsOn ? "Saved." : "已保存。";
            }
        }

        private async void custom_mkt_switch_Toggled(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                if (custom_mkt_switch.IsOn)
                {
                    mkt_textblock.Opacity = 1;
                    mkt_ddb.IsEnabled = true;
                    preferences.custom_mkt_switch = true;
                }
                else
                {
                    var resourceLoader = Windows.ApplicationModel.Resources.ResourceLoader.GetForCurrentView();
                    mkt_textblock.Opacity = 0.4;
                    mkt_ddb.IsEnabled = false;
                    preferences.custom_mkt_switch = false;
                    preferences.mkt = "zh-CN";
                    mkt_ddb.Content = resourceLoader.GetString("mkt_zh.Text");
                }
                await preferences.RefreshLocalFileAsync();
            }
        }

        private async void rb_1366_Checked(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                preferences.resolution = 0;
                await preferences.RefreshLocalFileAsync();
            }
        }

        private async void rb_1920_Checked(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                preferences.resolution = 1;
                await preferences.RefreshLocalFileAsync();
            }
        }

        private async void rb_3840_Checked(object sender, RoutedEventArgs e)
        {
            if (done)
            {
                preferences.resolution = 2;
                await preferences.RefreshLocalFileAsync();
            }
        }
        private async void cn_mkt_Click(object sender, RoutedEventArgs e)
        {
            preferences.mkt = "zh-CN";
            mkt_ddb.Content = cn_mkt.Text;
            await preferences.RefreshLocalFileAsync();
        }

        private async void us_mkt_Click(object sender, RoutedEventArgs e)
        {
            preferences.mkt = "en-US";
            mkt_ddb.Content = us_mkt.Text;
            await preferences.RefreshLocalFileAsync();
        }

        private async void uk_mkt_Click(object sender, RoutedEventArgs e)
        {
            preferences.mkt = "en-UK";
            mkt_ddb.Content = uk_mkt.Text;
            await preferences.RefreshLocalFileAsync();
        }

        private async void nz_mkt_Click(object sender, RoutedEventArgs e)
        {
            preferences.mkt = "en-NZ";
            mkt_ddb.Content = nz_mkt.Text;
            await preferences.RefreshLocalFileAsync();
        }

        private async void au_mkt_Click(object sender, RoutedEventArgs e)
        {
            preferences.mkt = "en-AU";
            mkt_ddb.Content = au_mkt.Text;
            await preferences.RefreshLocalFileAsync();
        }

        private async void jp_mkt_Click(object sender, RoutedEventArgs e)
        {
            preferences.mkt = "ja_JP";
            mkt_ddb.Content = jp_mkt.Text;
            await preferences.RefreshLocalFileAsync();
        }

        private async void de_mkt_Click(object sender, RoutedEventArgs e)
        {
            preferences.mkt = "de-DE";
            mkt_ddb.Content = de_mkt.Text;
            await preferences.RefreshLocalFileAsync();
        }

        private async void pane_left_Click(object sender, RoutedEventArgs e)
        {
            preferences.pane_top = false;
            await preferences.RefreshLocalFileAsync();
            panemode_ddb_icon.Glyph = "";
            panemode_ddb_text.Text = pane_left.Text;
        }

        private async void pane_top_Click(object sender, RoutedEventArgs e)
        {
            preferences.pane_top = true;
            await preferences.RefreshLocalFileAsync();
            panemode_ddb_icon.Glyph = "";
            panemode_ddb_text.Text = pane_top.Text;
        }
    }
}
