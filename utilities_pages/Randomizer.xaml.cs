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
    public sealed partial class Randomizer : Page
    {
        public Randomizer()
        {
            this.InitializeComponent();
        }

        SettingsData settings = new SettingsData();

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            await settings.GetLocalSettingsAsync();
        }

        private int[] GenerateUniqueRandom(int minValue, int maxValue, int n)
        {
            if (maxValue < minValue)
                throw new ArgumentException(settings.language_eng ? "Invalid bounds." : "无效边界值。");
            if (n > maxValue - minValue + 1)
                throw new ArgumentException(settings.language_eng ? "Count value out of range." : "生成数量过多。");

            int maxIndex = maxValue - minValue + 2;
            int[] indexArr = new int[maxIndex];
            for (int i = 0; i < maxIndex; i++)
            {
                indexArr[i] = minValue - 1;
                minValue++;
            }

            Random ran = new Random();
            int[] randNum = new int[n];
            int index;
            for (int j = 0; j < n; j++)
            {
                index = ran.Next(1, maxIndex - 1);
                randNum[j] = indexArr[index];
                indexArr[index] = indexArr[maxIndex - 1];
                maxIndex--;
            }
            return randNum;
        }

        private int[] GenerateRandom(int minValue, int maxValue, int n)
        {
            if (maxValue < minValue)
                throw new ArgumentException(settings.language_eng ? "Invalid bounds." : "无效边界值。");

            int[] rand = new int[n];
            Random random = new Random();
            for (int i = 0; i < n; i++)
            {
                rand[i] = random.Next(minValue, maxValue);
            }
            return rand;
        }

        private void randomize_Click(object sender, RoutedEventArgs e)
        {
            result.Text = string.Empty;
            int min = int.Parse(min_textbox.Text);
            int max = int.Parse(max_textbox.Text);
            int cnt = int.Parse(cnt_textbox.Text);
            string res = string.Empty;
            try
            {
                if (no_dup.IsChecked == true)
                {
                    int[] rand = GenerateUniqueRandom(min, max+1, cnt);
                    Array.Sort(rand);
                    foreach (int num in rand)
                    {
                        res += num.ToString() + ", ";
                    }
                } 
                else
                {
                    int[] rand = GenerateRandom(min, max+1, cnt);
                    Array.Sort(rand);
                    foreach (int num in rand)
                    {
                        res += num.ToString() + ", ";
                    }
                }
                result.Text = res.Substring(0, res.Length - 2);
            }
            catch (Exception ex)
            {
                result.Text = ex.Message;
            }
        }

        private void reset_Click(object sender, RoutedEventArgs e)
        {
            min_textbox.Text = string.Empty;
            max_textbox.Text = string.Empty;
            cnt_textbox.Text = string.Empty;
            result.Text = string.Empty;
        }
    }
}
