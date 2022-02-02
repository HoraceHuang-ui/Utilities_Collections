using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Utilities_Fix.utilities_pages
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class freecalc : Page
    {
        int decimal_accuracy = 5;
        SettingsData settings = new SettingsData();

        ContentDialog funclist = new ContentDialog
        {
            Title = "Function List",
            Content =
            "You can find this list at any time in the settings.\n\n" +
            "sqr  -  平方根  -  sqr9=3\n" +
            "sin  -  正弦  -  sin45=0.707\n" +
            "cos  -  余弦  -  co45=0.707\n" +
            "tan  -  正切  -  tan45=1\n" +
            "cot  -  余切  -  cot45=1\n" +
            "abs  -  绝对值  -  abs(-10)=10\n" +
            "cei  -  向上取整  -  cei4.3=5\n" +
            "flo  -  向下取整  -  flo4.7=4\n" +
            "log  -  对数  -  log[10,100]=2\n",
            PrimaryButtonText = "明白",
            DefaultButton = ContentDialogButton.Primary
        };

        void allEng()
        {
            eq_form.PlaceholderText = "Enter your expression here...";

            funclist.Title = "Function List";
            funclist.Content =
            "You can find this list at any time in the settings.\n\n" +
            "sqr  -  Square root  -  sqr9=3\n" +
            "sin  -     Sine      -  sin45=0.707\n" +
            "cos  -    Cosine     -  co45=0.707\n" +
            "tan  -    Tangent    -  tan45=1\n" +
            "cot  -   Cotangent   -  cot45=1\n" +
            "abs  -   Absolute    -  abs(-10)=10\n" +
            "cei  -     Ceil      -  cei4.3=5\n" +
            "flo  -     Floor     -  flo4.7=4\n" +
            "log  -     Log       -  log[10,100]=2\n";
            funclist.PrimaryButtonText = "Got it";
        }

        bool resVisible = false;
        int tipMode = 0;

        string[] funcs = { "sqr", "sin", "cos", "tan", "cot", "log", "abs", "cei", "flo" };
        const int sqrDEF = 0;
        const int sinDEF = 1;
        const int cosDEF = 2;
        const int tanDEF = 3;
        const int cotDEF = 4;
        const int logDEF = 5;
        const int absDEF = 6;
        const int ceiDEF = 7;
        const int floDEF = 8;
        const double E = 2.718281828;
        double Pi = Math.Atan(1) * 4;
        bool deg = false;

        int find_first_of(string str, string arg)
        {
            for (int i = 0; i < str.Length; i++)
            {
                for (int j = 0; j < arg.Length; j++)
                {
                    if (arg[j] == str[i])
                        return i;
                }
            }
            return -1;
        }
        int Sfind(string str, string arg)
        {
            for (int i = 0; i < str.Length - arg.Length + 1; i++)
            {
                if (str.Substring(i, arg.Length) == arg) return i;
            }
            return -1;
        }
        int findConst(string s)
        {
            int pos;
            for (int i = 0; i < 6; i++)
            {
                pos = find_first_of(s, "Ep");
                if (pos == -1) continue;
                else return pos;
            }
            return -1;
        }
        int findFunc(string s)
        {
            int pos;
            for (int i = 0; i < 9; i++)
            {
                pos = Sfind(s, funcs[i]);
                if (pos == -1) continue;
                else return pos;
            }
            return -1;
        }
        double Functions(string a)
        {
            int func = 6;
            string f = a.Substring(0, 3);

            if (f == "sqr") func = sqrDEF;
            else if (f == "tan") func = tanDEF;
            else if (f == "cos") func = cosDEF;
            else if (f == "sin") func = sinDEF;
            else if (f == "cot") func = cotDEF;
            else if (f == "log") func = logDEF;
            else if (f == "abs") func = absDEF;
            else if (f == "cei") func = ceiDEF;
            else if (f == "flo") func = floDEF;
            double num = func == logDEF ? 0 : double.Parse(a.Substring(3, a.Length - 3));
            switch (func)
            {
                case sqrDEF: return Math.Sqrt(num);
                case tanDEF: return deg ? Math.Tan(num / 180 * Pi) : Math.Tan(num);
                case sinDEF: return deg ? Math.Sin(num / 180 * Pi) : Math.Sin(num);
                case cotDEF: return deg ? (Math.Cos(num / 180 * Pi) / Math.Sin(num / 180 * Pi)) : (Math.Cos(num) / Math.Sin(num));
                case cosDEF: return deg ? Math.Cos(num / 180 * Pi) : Math.Cos(num);
                case logDEF:
                    double l, r;
                    int i, j;
                    i = Sfind(a, ","); j = a.Length - 1;
                    l = double.Parse(a.Substring(4, i - 4));
                    r = double.Parse(a.Substring(i + 1, j - i - 1));
                    return Math.Log(r) / Math.Log(l);
                case absDEF: return num > 0 ? num : -num;
                case ceiDEF:
                    if ((int)num == num) return num;
                    else if (num > 0) return (int)num + 1;
                    else return (int)num;
                case floDEF:
                    if ((int)num == num) return num;
                    else if (num > 0) return (int)num;
                    else return (int)num - 1;
                default:
                    return 0;
            }
        }
        int find(string str, char c)
        {
            for (int i = 0; i < str.Length; i++)
                if (str[i] == c)
                    return i;
            return -1;
        }
        bool nonOpe(char c)
        {
            if ((c <= '9' && c >= '0') || c == '.' || (c <= 'z' && c >= 'a') || c == '[' || c == ']' || c == ',')
                return true;
            else return false;
        }
        int findFirstof_2(string source, string args)
        {
            for (int i = 0; i < source.Length; i++)
                if (source[i] == args[0] || source[i] == args[1])
                    return i;
            return -1;
        }

        string calc(string e1)
        {
            string e2, res; int i, j, k; string left, right; double num_L, num_R;
            int brace_count = 0;
        Check_Again:
            i = findConst(e1);
            if (i == -1) goto No_ep;
            if (e1[i] == 'E')
            {
                e1 = e1.Remove(i, 1);
                e1 = e1.Insert(i, "2.718281828459");
            }
            else
            {
                e1 = e1.Remove(i, 1);
                e1 = e1.Insert(i, Pi.ToString());
            }
            goto Check_Again;
        No_ep:
            j = find(e1, '(');
            if (j == -1) goto NoKH;
            for (i = j + 1; brace_count != -1; i++)
            {
                if (e1[i] == '(') brace_count += 1;
                if (e1[i] == ')') brace_count -= 1;
            }
            k = i - 1;
            e2 = e1.Substring(j + 1, k - j - 1);
            res = calc(e2);
            e1 = e1.Remove(j, k - j + 1);
            e1 = e1.Insert(j, res);
            goto No_ep;
        NoKH:
            i = findFunc(e1);
            if (i == -1) goto NoFunc;
            brace_count = 0;          //此处brace_count用来标记是否有负号，有为1
            if (e1[i + 3] == '-') brace_count = 1;
            for (j = i; ; j++)
            {
                if (j == i + 3 && brace_count == 1) continue;
                if (j >= e1.Length || !nonOpe(e1[j])) break;
            }
            e2 = e1.Substring(i, j - i);
            res = Functions(e2).ToString();
            e1 = e1.Remove(i, j - i);
            e1 = e1.Insert(i, res);
            goto NoKH;
        NoFunc:
            i = find(e1, '^');
            if (i == -1) goto NoPW;
            //getnum!!!在这儿！！！！！
            for (j = i - 1; ; j--) if (j < 0 || !nonOpe(e1[j]))
                {
                    if (j < 0)
                        break;
                    if (j == 0)
                    {
                        j -= 1;
                        break;
                    }
                    if (e1[j] == '-' && !nonOpe(e1[j - 1]))
                    {
                        j -= 1;
                        break;
                    }
                    if (e1[j] != '-' || (e1[j] == '-' && nonOpe(e1[j - 1])))
                        break;
                }
            for (k = i + 1; ; k++)
                if (k >= e1.Length || !nonOpe(e1[k]))
                {
                    if (k < e1.Length && k == i + 1 && e1[k] == '-') ; //符号后紧接减号则为负，跳过符号继续读取
                    else break;
                }
            left = e1.Substring(j + 1, i - j - 1);
            right = e1.Substring(i + 1, k - i - 1);
            num_L = double.Parse(left);
            num_R = double.Parse(right);
            //以上是getnum！！！
            res = Math.Pow(num_L, num_R).ToString();
            e1 = e1.Remove(j + 1, k - j - 1);
            e1 = e1.Insert(j + 1, res);
            goto NoFunc;
        NoPW:
            i = findFirstof_2(e1, "*/");
            if (i == -1) goto NoTD;
            //getnum！！！！
            for (j = i - 1; ; j--)
                if (j < 0 || !nonOpe(e1[j]))
                {
                    if (j < 0)
                        break;
                    if (j == 0)
                    {
                        j -= 1;
                        break;
                    }
                    if (e1[j] == '-' && !nonOpe(e1[j - 1]))
                    {
                        j -= 1;
                        break;
                    }
                    if (e1[j] != '-' || (e1[j] == '-' && nonOpe(e1[j - 1])))
                        break;
                }
            for (k = i + 1; ; k++)
                if (k >= e1.Length || !nonOpe(e1[k]))
                {
                    if (!(k < e1.Length && k == i + 1 && e1[k] == '-')) break; //符号后紧接减号则为负，跳过符号继续读取
                }
            left = e1.Substring(j + 1, i - j - 1);
            right = e1.Substring(i + 1, k - i - 1);
            num_L = double.Parse(left);
            num_R = double.Parse(right);
            //以上是getnum！！！
            if (e1[i] == '*') res = (num_L * num_R).ToString();
            else res = (num_L / num_R).ToString();
            e1 = e1.Remove(j + 1, k - j - 1);
            e1 = e1.Insert(j + 1, res);
            goto NoPW;
        NoTD:
            i = findFirstof_2(e1, "+-");
            if (i == -1) goto NoPM;
            if (i == 0)
            {
                i = findFirstof_2(e1.Substring(1, e1.Length - 1), "+-");
                i++;
            }
            if (i == 0) goto NoPM;
            //getnum!!!!!!
            for (j = i - 1; ; j--) if (j < 0 || !nonOpe(e1[j]))
                {
                    if (j < 0)
                        break;
                    if (j == 0)
                    {
                        j -= 1;
                        break;
                    }
                    if (e1[j] == '-' && !nonOpe(e1[j - 1]))
                    {
                        j -= 1;
                        break;
                    }
                    if (e1[j] != '-' || (e1[j] == '-' && nonOpe(e1[j - 1])))
                        break;
                }
            for (k = i + 1; ; k++)
                if (k >= e1.Length || !nonOpe(e1[k]))
                {
                    if (!(k < e1.Length && k == i + 1 && e1[k] == '-')) break; //符号后紧接减号则为负，跳过符号继续读取
                }
            left = e1.Substring(j + 1, i - j - 1);
            right = e1.Substring(i + 1, k - i - 1);
            num_L = double.Parse(left);
            num_R = double.Parse(right);
            //以上是getnum！！！
            if (e1[i] == '+') res = (num_L + num_R).ToString();
            else res = (num_L - num_R).ToString();
            e1 = e1.Remove(j + 1, k - j - 1);
            e1 = e1.Insert(j + 1, res);
            goto NoTD;
        NoPM:
            num_L = double.Parse(e1);   //此处num_L用来精确到指定小数位
            return Math.Round(num_L, decimal_accuracy).ToString();
        }
        public freecalc()
        {
            this.InitializeComponent();
        }
        private void eq_form_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (tipMode != 0)
            {
                if (tip3_submit.IsOpen)
                {
                    tip3_submit.IsOpen = false;
                    tip4_AnsFill.IsOpen = true;
                }
                else if (tip6_submit.IsOpen)
                {
                    tip6_submit.IsOpen = false;
                    tip7_logWarning.IsOpen = true;
                }
                else if (tip9_submit.IsOpen)
                {
                    tip9_submit.IsOpen = false;
                    //tip10_settings.IsOpen = true;
                }
                else if (tipMode == 15)
                {
                    tip16_congrats.IsOpen = true;
                    if (RequestedTheme == ElementTheme.Dark)
                        congratsPic.Source = new BitmapImage(new Uri("ms-appx:///Assets/math_dark.jpg"));
                    else
                        congratsPic.Source = new BitmapImage(new Uri("ms-appx:///Assets/math_light.png"));
                }
            }
            try
            {
                res_form.Text = calc(eq_form.Text);
                eq_form.Translation = new Vector3(-100, 0, 0);
                res_form.Visibility = Visibility.Visible;
                res_form.Opacity = 1;
                resVisible = true;
                calc_error.IsOpen = false;
            }
            catch
            {
                calc_error.IsOpen = true;
            }
        }

        private void res_form_GotFocus(object sender, RoutedEventArgs e)
        {
            eq_form.Text = res_form.Text;
            res_form.Text = "";
            res_form.Opacity = 0;
            eq_form.Translation = new Vector3(0, 0, 0);
            resVisible = false;
            if (tipMode == 9)
            {
                tip16_congrats.IsOpen = true;
                if (RequestedTheme == ElementTheme.Dark)
                    congratsPic.Source = new BitmapImage(new Uri("ms-appx:///Assets/math_dark.jpg"));
                else
                    congratsPic.Source = new BitmapImage(new Uri("ms-appx:///Assets/math_light.png"));
            }
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await settings.GetLocalSettingsAsync();
            if (eq_form.Height <= 20)
            {
                eq_form.FontSize = 20;
                res_form.FontSize = 20;
            }

            if (settings.language_eng) allEng();
            if (settings.degree) deg = true;
            decimal_accuracy = settings.decimal_accuration;
        }

        private void eq_form_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (tipMode != 0)
            {
                if (tip2_basicCalc.IsOpen)
                {
                    tip2_basicCalc.IsOpen = false;
                    tip3_submit.IsOpen = true;
                }
                else if (tip4_AnsFill.IsOpen)
                {
                    tip4_AnsFill.IsOpen = false;
                    tip5_funcs.IsOpen = true;
                }
                else if (tip7_logWarning.IsOpen)
                {
                    tip7_logWarning.IsOpen = false;
                    tip8_consts.IsOpen = true;
                    res_form.Text = "2";
                    return;
                }
                else if (tip8_consts.IsOpen)
                {
                    tip8_consts.IsOpen = false;
                    tip9_submit.IsOpen = true;
                }
                else if (tipMode == 11)
                {
                    tipMode++;
                }
            }

            if (resVisible)
            {
                res_form.Opacity = 0;
                eq_form.Translation = new Vector3(0, 0, 0);
                resVisible = false;
            }
        }

        private void help_Click(object sender, RoutedEventArgs e)
        {
            tipMode = 1;
            tip1.IsOpen = true;
        }

        private void tip1_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            tip1.IsOpen = false;
            tipMode++;
            tip2_basicCalc.IsOpen = true;
        }

        private void tip2_basicCalc_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            eq_form.Text = "(10*(5+3-1)/7)^2";
            tipMode++;
        }

        //tip5 example
        private void tip5_funcs_CloseButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            eq_form.Text = "abs(cos120)+log[10,100]";
            tip6_submit.IsOpen = true;
        }
        //tip5 list
        private async void tip5_funcs_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            await funclist.ShowAsync();
        }

        private void tip7_logWarning_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            eq_form.Text = "log[(5+5),100]";
        }

        private void tip8_consts_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            eq_form.Text = "E+p";
            tipMode = 9;
        }

        private void tip16_congrats_Closed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            eq_form.Text = "\0";
            tipMode = 0;
        }
        private void tip2_basicCalc_CloseButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            eq_form.Text = "\0";
            tipMode = 0;
        }

        private void tip1_Closed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            tipMode = 0;
        }
    }
}
