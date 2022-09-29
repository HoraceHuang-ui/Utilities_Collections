using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using System.Collections;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Utilities_Fix.utilities_pages
{
    public sealed partial class freecalc : Page
    {
        int decimal_accuracy = 5;
        double mem = 0;
        SettingsData settings = new SettingsData();
        bool resVisible = false;
        int tipMode = 0;
        bool deg = false;

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
            "log  -  对数  -  log(10,100)=2\n",
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
            "log  -     Log       -  log(10,100)=2\n";
            funclist.PrimaryButtonText = "Got it";
        }

        /// <param name="eq">Full equation</param>
        /// <param name="i">Start index</param>
        private bool IsFunc(string eq, int i)
        {
            string[] funcs = { "sqr", "sin", "cos", "tan", "cot", "abs", "cei", "flo" };
            if (i >= eq.Length) return false;
            if (eq.Substring(i).Length < 3) return false;
            return funcs.Contains(eq.Substring(i, 3));
        }
        private int PriorTo(char tk1, char tk2)
        {
            Dictionary<char, int> priority = new Dictionary<char, int>
            {
                { '+', 1 },
                { '-', 1 },
                { '%', 2 },
                { '*', 3 },
                { '/', 3 },
                { '^', 4 }
                // { '(', 4 }
            };
            if (!priority.ContainsKey(tk1) || !priority.ContainsKey(tk2))
                return -2;

            if (priority[tk1] > priority[tk2])
                return 1;
            else if (priority[tk1] < priority[tk2])
                return -1;
            else
                return 0;
        }
        private bool IsOp(string op)
        {
            return "+-%*/^".Contains(op);
        }
        private bool IsConst(string eq, int i)
        {
            return "EPM".Contains(eq[i]);
        }
        private bool IsSpecialFunc(string eq, int i)
        {
            string[] funcs = { "log" };
            if (i >= eq.Length) return false;
            if (eq.Substring(i).Length < 3) return false;
            return funcs.Contains(eq.Substring(i, 3));
        }
        private bool IsNum(string eq, int i)
        {
            return eq[i] <= '9' && eq[i] >= '0';
        }
        private Stack TransformToRPN(string eq)
        {
            Stack s1 = new Stack();
            Stack s2 = new Stack();
            for (int i = 0; i < eq.Length; i++)
            {
                // Numbers
                if (IsNum(eq, i) ||
                    (eq[i] == '-' && (i == 0 || !(IsNum(eq, i-1) || eq[i - 1] == ')'))))
                {
                    int j;
                    for (j = i + 1; j < eq.Length && (IsNum(eq, j) || eq[j] == '.'); j++) ;
                    j--;
                    s2.Push(eq.Substring(i, j - i + 1));
                    i = j;
                }
                // Constants
                else if (IsConst(eq, i)) {
                    s2.Push(eq[i].ToString());
                }
                // Functions
                else if (IsFunc(eq, i))
                {
                    s1.Push(eq.Substring(i, 3));
                    i += 2;
                }
                // Special functions
                else if (IsSpecialFunc(eq, i))
                {
                    s1.Push(eq.Substring(i, 3));
                    i += 2;
                }
                // Operators
                else if (IsOp(eq[i].ToString()))
                {
                    // Higher priority than previous
                    if (s1.Count == 0 || s1.Peek().ToString() == "(" ||
                        (!IsFunc(s1.Peek().ToString(), 0) && !IsSpecialFunc(s1.Peek().ToString(), 0) &&
                        PriorTo(eq[i], s1.Peek().ToString()[0]) > 0))
                    {
                        s1.Push(eq[i].ToString());
                    }
                    // Lower or equal priority
                    else
                    {
                        while (s1.Count > 0 && (IsFunc(s1.Peek().ToString(), 0) || IsSpecialFunc(s1.Peek().ToString(), 0) ||
                            (s1.Peek().ToString() != "(" && PriorTo(s1.Peek().ToString()[0], eq[i]) >= 0)))
                        {
                            s2.Push(s1.Pop());
                        }
                        s1.Push(eq[i].ToString());
                    }
                }
                // Brackets
                else if (eq[i] == '(')
                {
                    s1.Push("(");
                }
                else if (eq[i] == ')')
                {
                    while (s1.Count > 0 && s1.Peek().ToString() != "(")
                    {
                        s2.Push(s1.Pop());
                    }
                    s1.Pop();
                }
                // spaces, ","
            }
            while (s1.Count > 0)
            {
                s2.Push(s1.Pop());
            }
            return s2;
        }

        private double calc(string eq)
        {
            // Transformation
            Stack s = TransformToRPN(eq);
            string[] tokens = new string[s.Count];

            for (int i = 0; i < tokens.Length; i++)
            {
                tokens[tokens.Length - i - 1] = s.Pop().ToString();
            }

            // Calculation
            s = new Stack();
            foreach (string token in tokens)
            {
                // Numbers
                if (IsNum(token, 0) || token[0] == '-' && token.Length > 1)
                {
                    s.Push(double.Parse(token));
                }
                // Operators
                else if (IsOp(token))
                {
                    double num1 = (double)s.Pop();
                    double num0 = (double)s.Pop();
                    switch (token)
                    {
                        case "+": s.Push(num0 + num1); break;
                        case "-": s.Push(num0 - num1); break;
                        case "%": s.Push(num0 % num1); break;
                        case "*": s.Push(num0 * num1); break;
                        case "/": s.Push(num0 / num1); break;
                        case "^": s.Push(Math.Pow(num0, num1)); break;
                    }
                }
                // Constants
                else if (IsConst(token, 0))
                {
                    switch (token)
                    {
                        case "E": s.Push(Math.E); break;
                        case "P": s.Push(Math.PI); break;
                        case "M": s.Push(mem); break;
                    }
                }
                // Functions
                else if (IsFunc(token, 0))
                {
                    double num = (double)s.Pop();
                    switch (token)
                    {
                        case "sqr": s.Push(Math.Sqrt(num)); break;
                        case "tan": s.Push(!deg ? Math.Tan(num) : Math.Tan(num / 180 * Math.PI)); break;
                        case "sin": s.Push(!deg ? Math.Sin(num) : Math.Sin(num / 180 * Math.PI)); break;
                        case "cot":
                            s.Push(!deg ? (Math.Cos(num) / Math.Sin(num)) :
                                (Math.Cos(num / 180 * Math.PI) / Math.Sin(num / 180 * Math.PI))); break;
                        case "cos": s.Push(!deg ? Math.Cos(num) : Math.Cos(num / 180 * Math.PI)); break;
                        case "abs": s.Push(num > 0 ? num : -num); break;
                        case "cei":
                            if ((int)num == num) s.Push(num);
                            else if (num > 0) s.Push((double)((int)num + 1));
                            else s.Push((double)(int)num);
                            break;
                        case "flo":
                            if ((int)num == num) s.Push(num);
                            else if (num > 0) s.Push((double)(int)num);
                            else s.Push((double)((int)num - 1));
                            break;
                    }
                }
                // Special functions
                else if (IsSpecialFunc(token, 0))
                {
                    switch (token)
                    {
                        case "log":
                            double num0 = (double)s.Pop();
                            double num1 = (double)s.Pop();
                            s.Push(Math.Log(num0) / Math.Log(num1));
                            break;
                    }
                }
            }
            return Math.Round(double.Parse(s.Pop().ToString()), decimal_accuracy);
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
                    tip8_consts.IsOpen = true;
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
                string s = eq_form.Text;
                for (int i = 0; i < s.Length; i++)
                {
                    if (s[i] == ' ')
                    {
                        s = s.Remove(i, 1);
                        i--;
                    }
                }
                res_form.Text = calc(s).ToString();
                if (settings.ovr_form)
                {
                    eq_form.Text = res_form.Text;
                }
                else
                {
                    eq_form.Translation = new Vector3(-100, 0, 0);
                    mBar.Translation = new Vector3(-100, 0, 0);
                    res_form.Visibility = Visibility.Visible;
                    res_form.Opacity = 1;
                }
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
            mBar.Translation = new Vector3(0, 0, 0);
            resVisible = false;
            if (tipMode == 9)
            {
                tip16_congrats.IsOpen = true;
                if (RequestedTheme == ElementTheme.Dark)
                    congratsPic.Source = new BitmapImage(new Uri("ms-appx:///Assets/math_dark.jpg"));
                else if (RequestedTheme == ElementTheme.Light)
                    congratsPic.Source = new BitmapImage(new Uri("ms-appx:///Assets/math_light.jpg"));
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
            decimal_accuracy = settings.decimal_accuracy;
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
                mBar.Translation = new Vector3(0, 0, 0);
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
            if (tipMode == 6) tipMode++;
            tipMode++;
        }

        //tip5 example
        private void tip5_funcs_CloseButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            eq_form.Text = "abs cos120+log(10,100)";
            tip6_submit.IsOpen = true;
        }
        //tip5 list
        private async void tip5_funcs_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            await funclist.ShowAsync();
        }

        private void tip8_consts_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            eq_form.Text = "E+P";
            tipMode = 9;
        }

        private void tip16_congrats_Closed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            eq_form.Text = String.Empty;
            tipMode = 0;
        }
        private void tip2_basicCalc_CloseButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {
            eq_form.Text = String.Empty;
            tipMode = 0;
        }

        private void tip1_Closed(Microsoft.UI.Xaml.Controls.TeachingTip sender, Microsoft.UI.Xaml.Controls.TeachingTipClosedEventArgs args)
        {
            tipMode = 0;
        }

        private void mPlus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (resVisible)
                    mem += double.Parse(res_form.Text);
                else
                {
                    string s = eq_form.Text;
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (s[i] == ' ')
                        {
                            s = s.Remove(i, 1);
                            i--;
                        }
                    }
                    mem += calc(eq_form.Text);
                    calc_error.IsOpen = false;
                }
                eq_form.Text = string.Empty;
                mValue.Text = "Mem = " + mem.ToString();
            }
            catch
            {
                calc_error.IsOpen = true;
            }
        }

        private void mMinus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (resVisible)
                    mem -= double.Parse(res_form.Text);
                else
                {
                    string s = eq_form.Text;
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (s[i] == ' ')
                        {
                            s = s.Remove(i, 1);
                            i--;
                        }
                    }
                    mem -= calc(eq_form.Text);
                    calc_error.IsOpen = false;
                }
                eq_form.Text = string.Empty;
                mValue.Text = "Mem = " + mem.ToString();
            }
            catch
            {
                calc_error.IsOpen = true;
            }
        }

        private void mc_Click(object sender, RoutedEventArgs e)
        {
            mem = 0;
            mValue.Text = "Mem = 0";
        }

        private void mCopy_Click(object sender, RoutedEventArgs e)
        {
            mValue.Text = mValue.Text.Substring(6);
            mValue.SelectAll();
            mValue.CopySelectionToClipboard();
            mValue.Text = "Mem = " + mValue.Text;
        }

        private void tip10_mBar_ActionButtonClick(Microsoft.UI.Xaml.Controls.TeachingTip sender, object args)
        {

        }
    }
}
