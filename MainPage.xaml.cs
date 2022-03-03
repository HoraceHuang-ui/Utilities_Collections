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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Utilities_Fix
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        ContentDialog welcome_cn = new ContentDialog()
        {
            Title = "本次更新内容：",
            Content = "欢迎使用 hyy 小工具大杂烩 V2.7.0！\n\n" +
                "加入 PPI 计算器，大果粒无处遁形！\n\n" +
                "上一版本 V2.6.0 更新内容：\n" +
                "修复了大量闪退问题，程序运行更稳定\n" +
                "必应每日壁纸会显示加载中，反馈更直观\n" +
                "更新后现可同步上一版本设置项\n" +
                "采用 Windows 11 风格图标，好康！\n" +
                "自由计算器代码完全重写，运算效率更高限制更少！",
            PrimaryButtonText = "芜湖起飞！!",
            DefaultButton = ContentDialogButton.Primary
        };
        ContentDialog welcome_en = new ContentDialog()
        {
            Title = "What's New:",
            Content = "Welcome to Utilities by Hyy V2.7.0！\n\n" +
                "Added a PPI Calculator and sense the sharpness of your screen!\n\n" +
                "What was new in the previous version V2.6.0:\n" +
                "Fixed lots of force close problems, making the app more stable\n" +
                "Bing Daily Wallpaper now displays \"Loading...\"\n" +
                "Your settings from the previous version is now synced\n" +
                "Added a Windows 11 styled icon!\n" +
                "Enjoy a more consistent experience with the brand new FreeCalc!",
            PrimaryButtonText = "Woo-hoo!!",
            DefaultButton = ContentDialogButton.Primary
        };

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            nvView.SelectedItem = utilities_home;
            SettingsData settingsData = new SettingsData();
            await settingsData.GetLocalSettingsAsync();

            if (!settingsData.language_eng)
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "zh-CN";
            else
                Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US";

            if (settingsData.display_theme == 0)
                RequestedTheme = ElementTheme.Light;
            else if (settingsData.display_theme == 1)
                RequestedTheme = ElementTheme.Dark;
            else
                RequestedTheme = ElementTheme.Default;

            if (settingsData.pane_top)
                nvView.PaneDisplayMode = NavigationViewPaneDisplayMode.Top;

            if (settingsData.first_time_open)
            {
                settingsData.first_time_open = false;
                await settingsData.RefreshLocalFileAsync();
                if (settingsData.language_eng)
                    await welcome_en.ShowAsync();
                else await welcome_cn.ShowAsync();
            }
        }

        private void nvView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(utilities_pages.settings));
                return;
            }
            if (args.SelectedItem == utilities_home)
                contentFrame.Navigate(typeof(utilities_pages.utilitiesHome));
            else if (args.SelectedItem == avbv)
                contentFrame.Navigate(typeof(utilities_pages.avBV));
            else if (args.SelectedItem == bing_wp)
                contentFrame.Navigate(typeof(utilities_pages.bing_wp));
            else if (args.SelectedItem == num_rand)
                contentFrame.Navigate(typeof(utilities_pages.Randomizer));
            else if (args.SelectedItem == calc)
                contentFrame.Navigate(typeof(utilities_pages.freecalc));
            else if (args.SelectedItem == detcalc)
                contentFrame.Navigate(typeof(utilities_pages.determinant));
            else if (args.SelectedItem == PPICalc)
                contentFrame.Navigate(typeof (utilities_pages.PPI_Calc));
            else if (args.SelectedItem == ip_url)
                contentFrame.Navigate(typeof(utilities_pages.IP));
            else if (args.SelectedItem == ip_request)
                contentFrame.Navigate(typeof(utilities_pages.IP_request));
            else if (args.SelectedItem == number_region)
                contentFrame.Navigate(typeof(utilities_pages.number_region));
            else if (args.SelectedItem == cmd)
                contentFrame.Navigate(typeof(utilities_pages.cmd));
            else if (args.SelectedItem == text_comparator)
                contentFrame.Navigate(typeof(utilities_pages.textDiff));
        }
    }
}
