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
    public sealed partial class textDiff : Page
    {
        public textDiff()
        {
            this.InitializeComponent();
        }

        private void compare_Click(object sender, RoutedEventArgs e)
        {
            title.Text = right_box.TextDocument.ToString();
        }

        static string[,] LCS(string str1, string str2)
        {
            int[,] s = new int[str1.Length + 1, str2.Length + 1];
            string[,] str = new string[str1.Length + 1, str2.Length + 1];
            for (int i = 0; i <= str1.Length; i++)
            {
                for (int j = 0; j <= str2.Length; j++)
                {
                    if (i == 0 || j == 0)
                    {
                        s[i, j] = 0;
                        str[i, j] = string.Empty;
                    }
                    else
                    {
                        if (str1.Substring(i - 1, 1) == str2.Substring(j - 1, 1))
                        {
                            s[i, j] = s[i - 1, j - 1] + 1;
                            str[i, j] = str[i - 1, j - 1] + str1.Substring(i - 1, 1);
                        }
                        else
                        {
                            s[i, j] = s[i - 1, j] > s[i, j - 1] ? s[i - 1, j] : s[i, j - 1];
                            str[i, j] = s[i - 1, j] > s[i, j - 1] ? str[i - 1, j] : str[i, j - 1];
                        }
                    }
                }
            }
            return str;
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            left_box = new RichEditBox();
            right_box = new RichEditBox();
            //left_box.Text = string.Empty;
            //right_box.Text = string.Empty;
        }
    }
}
