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
    public sealed partial class avBV : Page
    {
        private static String table = "fZodR9XQDSUm21yCkr6zBqiveYah8bt4xsWpHnJE7jL5VG3guMTKNPAwcF";
        private static Dictionary<String, int> mp = new Dictionary<string, int>();
        static int[] ss = { 9, 8, 1, 6, 2, 4, 0, 7, 3, 5 };
        static long xor = 177451812;
        static long add = 8728348608;
        public avBV()
        {
            initbv2av();
            this.InitializeComponent();
        }


        private void bv_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (bv.Text.Length == 12) bv.Text = bv.Text.Substring(2);
            av.Text = "av" + bv2av(bv.Text);
            bv.Text = "BV" + bv.Text;
            link.Text = "https://www.bilibili.com/video/" + bv.Text;
        }

        private void av_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (av.Text[0] == 'a') av.Text = av.Text.Substring(2);
            bv.Text = "BV" + av2bv(av.Text);
            av.Text = "av" + av.Text;
            link.Text = "https://www.bilibili.com/video/" + bv.Text;
        }

        public static void initbv2av()
        {
            mp.Clear();
            for (int i = 0; i < 58; i++)
            {
                String s1 = table.Substring(i, 1);
                mp.Add(s1, i);
            }
        }
        public static String bv2av(String s)
        {
            long r = 0;
            for (int i = 0; i < 6; i++)
            {
                r = r + mp[s.Substring(ss[i], 1)] * (long)Math.Pow(58, i);      //查表，转数字，转回10进制
            }
            return Convert.ToString((r - add) ^ xor);
        }
        public static String av2bv(String st)
        {
            long s = Convert.ToInt64(st);
            char[] av_ch = "1  4 1 7  ".ToCharArray();
            s = (s ^ xor) + add;
            for (int i = 0; i < 6; i++)
            {
                int temp = (int)(s / Math.Pow(58, i) % 58);             //进制转换，10→58
                String r = table.Substring(temp, 1);                    //查字符表
                av_ch[ss[i]] = r.ToCharArray()[0];                      //放入对应位
            }
            return new String(av_ch);
        }

        private void link_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            string str = "\0";
            if (link.Text == "\0") return;
            int quePos = -1;
            for (int i = 0; i < link.Text.Length; i++)
                if (link.Text[i] == '?')
                {
                    quePos = i;
                    break;
                }
            if (quePos == -1)
                str = link.Text.Substring(31);
            else str = link.Text.Substring(31, quePos - 32);
            if (str[0] == 'B')
            {
                bv.Text = str;
                av.Text = "av" + bv2av(str.Substring(2));
            }
            else
            {
                av.Text = str;
                bv.Text = "BV" + av2bv(str.Substring(2));
            }
        }
    }
}
