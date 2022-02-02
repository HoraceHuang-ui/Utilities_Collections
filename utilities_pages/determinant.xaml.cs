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
namespace Utilities_Fix.utilities_pages
{
    public sealed partial class determinant : Page
    {
        int status = 0;
        int dimension = 0;
        List<List<int>> matrix = new List<List<int>>();
        string desc = "";

        SettingsData preferences = new SettingsData();
        bool eng = false;

        public determinant()
        {
            this.InitializeComponent();
        }
        // Calculates the determinant of a given matrix.
        private int CalculateDetMatrix(List<List<int>> m)
        {
            int dim = m[0].Count;  // 矩阵阶数
            int res = 0;

            if (dim > 1)
            {
                int sign = 1;

                for (int i = 0; i < dim; i++)
                {
                    List<List<int>> matrix2 = new List<List<int>>();
                    // 余子式赋值
                    for (int j = 1; j < dim; j++)  // 跳过第 1 行
                    {
                        List<int> row = new List<int>();
                        for (int k = 0; k < dim; k++)
                        {
                            if (k != i)  // 跳过第 i 列
                                row.Add(m[j][k]);
                        }
                        matrix2.Add(row);
                    }
                    // 计算代数余子式
                    res += m[0][i] * sign * CalculateDetMatrix(matrix2);
                    sign *= -1;
                }
            }
            else
                res = m[0][0];
            return res;
        }
        private List<int> GetListIntsFromString(string s)
        {
            List<int> arr = new List<int>();
            int n = 0;
            bool negative = false;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] <= '9' && s[i] >= '0')
                    n = n * 10 + (s[i] - '0');
                else if (s[i] == '-')
                    negative = true;
                else
                {
                    int t = negative ? -n : n;
                    arr.Add(t);
                    n = 0;
                    negative = false;
                }
            }
            return arr;
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await preferences.GetLocalSettingsAsync();
            eng = preferences.language_eng;
            description.Text = eng ? "Enter the dimension of the matrix\n" : "请输入矩阵阶数\n";
            desc = description.Text;
            tip.Message = eng ? "Please input an integer" : "请输入一个整数";
        }

        private void dyn_input_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (status == 0)
            {
                try
                {
                    dimension = int.Parse(dyn_input.Text);
                    description.Text = "dim A = " + dyn_input.Text + "\n";
                    desc = description.Text;
                    dyn_input.Text = "";
                    status++;
                    tip.Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Informational;
                    tip.Message = eng ? "Row 1: Separate numbers by spaces" : "第 1 行：请用空格分隔";
                }
                catch
                {
                    dyn_input.Text = "";
                    tip.Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error;
                    tip.IsOpen = true;
                    tip.Message = eng ? "Please enter one SINGLE number for the dimension." : "请输入 *单个* 整数代表阶数。";
                }
            }
            else
            {
                if (status == dimension) // Complete
                {
                    List<int> r = GetListIntsFromString(dyn_input.Text + " ");
                    if (r.Count() != dimension)
                    {
                        tip.Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error;
                        tip.IsOpen = true;
                        tip.Message = eng ?
                            ("Please ensure you've input " + dimension.ToString() + " numbers.") :
                            ("请确保您输入了 " + dimension.ToString() + " 个数。");
                        return;
                    }
                    
                    matrix.Add(r);
                    description.Text += dyn_input.Text + "\n";
                    // desc += eng ? "Press Enter to reset" : "按回车即可重置";
                    // description.Text = desc;
                    tip.Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Success;
                    tip.IsOpen = true;
                    tip.Message = eng ? "Press Enter to reset" : "按回车即可重置";
                    status++;
                    dyn_input.Text = "det A = " + CalculateDetMatrix(matrix);
                }
                else if (status > dimension) // Post-complete
                {
                    status = 0;
                    description.Text = eng ? "Enter the dimension of the matrix\n" : "请输入矩阵阶数\n";
                    desc = description.Text;
                    dyn_input.Text = "";
                    tip.Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Informational;
                    tip.Message = eng ? "Please input an integer" : "请输入一个整数";
                    matrix.Clear();
                }
                else // Pending
                {
                    List<int> r = GetListIntsFromString(dyn_input.Text + " ");
                    if (r.Count() != dimension)
                    {
                        tip.Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error;
                        tip.IsOpen = true;
                        tip.Message = eng ?
                            ("Please ensure you've input " + dimension.ToString() + " numbers.") :
                            ("请确保您输入了 " + dimension.ToString() + " 个数。");
                        return;
                    }

                    matrix.Add(r);
                    desc += dyn_input.Text + "\n";
                    description.Text = desc;
                    status++;
                    dyn_input.Text = "";
                    tip.Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Informational;
                    tip.Message = eng ?
                        ("Row " + status.ToString() + ": Separate numbers by spaces") :
                        ("第 " + status.ToString() + " 行：请用空格分隔");
                }
            }
        }
    }
}
