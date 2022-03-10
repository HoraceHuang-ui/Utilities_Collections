using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Automation;

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

        SettingsData settingsData = new SettingsData();

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            nvView.SelectedItem = utilities_home;
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
                contentFrame.Navigate(typeof(utilities_pages.PPI_Calc));
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

        const int HOME = 0;
        const int FREECALC = 1;
        const int DETCALC = 2;
        const int PPICALC = 3;
        const int NUMRAND = 4;
        const int BINGWP = 5;
        const int DOMAIN = 6;
        const int IPREQUEST = 7;
        const int NUMREGION = 8;
        const int BVAV = 9;
        const int TEXTCOMP = 10;
        const int CMD = 11;

        public class SearchItem
        {
            public string formal_eng;
            public string formal_chn;
            public string[] fuzzy;
            public int num;

            public SearchItem(int n, string fe, string fc, string[] fz)
            {
                num = n;
                formal_chn = fc;
                formal_eng = fe;
                fuzzy = fz.ToArray();
            }
        }
        List<SearchItem> items = new List<SearchItem>()
        {
            new SearchItem(0, "Home", "主页", new string[]{"zy"}),

            new SearchItem(1, "Free Calculator", "自由计算器", new string[]{"zyjsq", "freecalc"}),
            new SearchItem(2, "Determinant Calculator", "行列式计算器", new string[] {"hlsjsq", "detcalc"}),
            new SearchItem(3, "Pixel Density Calculator", "像素密度计算器", new string[]{"xsmdjsq", "ppicalculator", "ppicalc", "ppijsq"}),
            new SearchItem(4, "Number Randomizer", "随机数生成器", new string[]{"sjsscq"}),

            new SearchItem(5, "Bing Daily Wallpapers", "必应每日壁纸", new string[]{"bymrbz"}),
            new SearchItem(6, "Domain Analyzer", "域名解析", new string[]{"ymjx"}),
            new SearchItem(7, "IP Request", "IP地址查询", new string[]{"ipdzcx", "iprequest"}),
            new SearchItem(8, "Number Region", "号码归属地", new string[]{"hmgsd"}),

            new SearchItem(9, "BV / av Converter", "BV / av 号转换", new string[]{"bvavhzhq", "avbvhzhq", "bvavzhq", "avbvzhq", "bvavconverter", "avbvconverter"}),
            new SearchItem(10, "Text Comparator", "文字比较", new string[]{"wzbj", "wenzibijiao"}),
            new SearchItem(11, "Command Prompt", "命令提示符", new string[]{"cmd", "mltsf"}),
        };

        private void nav_view_search_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Since selecting an item will also change the text,
            // only listen to changes caused by user entering text.
            List<string> suitableItems = new List<string>();
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                var splitText = sender.Text.ToLower().Split(" ");
                foreach (var item in items)
                {
                    var found = splitText.All((string key) =>
                    {
                        if (item.formal_eng.ToLower().Contains(key) || item.formal_chn.Contains(key))
                            return true;
                        foreach (string s in item.fuzzy)
                            if (s.Contains(key))
                                return true;
                        return false;
                    });
                    if (found)
                    {
                        suitableItems.Add(settingsData.language_eng ? item.formal_eng : item.formal_chn);
                    }
                }

                if (suitableItems.Count == 0)
                {
                    suitableItems.Add(settingsData.language_eng ? "No results found." : "未找到结果。");
                }
                nav_view_search.ItemsSource = suitableItems;
            }
        }
        private void nav_view_search_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            if (nav_view_search.Items[0].ToString() == (settingsData.language_eng ? "No results found." : "未找到结果。"))
            {
                nav_view_search = sender;
                return;
            }
            NavigateToSearch(args.SelectedItem.ToString());
        }

        private void nav_view_search_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion == null)
            {
                if (nav_view_search.Items[0].ToString() == (settingsData.language_eng ? "No results found." : "未找到结果。"))
                    return;
                NavigateToSearch(nav_view_search.Items[0].ToString());
            }
        }

        private void NavigateToSearch(string pageName)
        {
            int i = 0;
            foreach (SearchItem item in items)
            {
                if (pageName == item.formal_eng || pageName == item.formal_chn)
                    break;
                i++;
            }
            switch (i)
            {
                case HOME: contentFrame.Navigate(typeof(utilities_pages.utilitiesHome)); nvView.SelectedItem = utilities_home; break;
                case FREECALC: contentFrame.Navigate(typeof(utilities_pages.freecalc)); nvView.SelectedItem = calc; break;
                case DETCALC: contentFrame.Navigate(typeof(utilities_pages.determinant)); nvView.SelectedItem = detcalc; break;
                case PPICALC: contentFrame.Navigate(typeof(utilities_pages.PPI_Calc)); nvView.SelectedItem = PPICalc; break;
                case NUMRAND: contentFrame.Navigate(typeof(utilities_pages.Randomizer)); nvView.SelectedItem = num_rand; break;
                case BINGWP: contentFrame.Navigate(typeof(utilities_pages.bing_wp)); nvView.SelectedItem = bing_wp; break;
                case DOMAIN: contentFrame.Navigate(typeof(utilities_pages.IP)); nvView.SelectedItem = ip_url; break;
                case IPREQUEST: contentFrame.Navigate(typeof(utilities_pages.IP_request)); nvView.SelectedItem = ip_request; break;
                case NUMREGION: contentFrame.Navigate(typeof(utilities_pages.number_region)); nvView.SelectedItem = number_region; break;
                case BVAV: contentFrame.Navigate(typeof(utilities_pages.avBV)); nvView.SelectedItem = avbv; break;
                case TEXTCOMP: contentFrame.Navigate(typeof(utilities_pages.textDiff)); nvView.SelectedItem = text_comparator; break;
                case CMD: contentFrame.Navigate(typeof(utilities_pages.cmd)); nvView.SelectedItem = cmd; break;
            }
        }
    }
}
