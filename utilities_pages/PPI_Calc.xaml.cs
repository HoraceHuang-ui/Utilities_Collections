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
    public sealed partial class PPI_Calc : Page
    {
        public PPI_Calc()
        {
            this.InitializeComponent();
        }

        double discount = 1;
        SettingsData settings = new SettingsData();

        private void size_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            CalcPPI();
        }

        private void CalcPPI()
        {
            if (CheckEmpty()) return;
            double w = double.Parse(width.Text);
            double h = double.Parse(height.Text);
            double diag = double.Parse(size.Text);
            res.Text = Math.Round(Math.Sqrt(Math.Pow(w, 2) + Math.Pow(h, 2)) / diag * discount, 2).ToString();
        }

        private bool CheckEmpty()
        {
            return width.Text == "" || height.Text == "" || size.Text == "";
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            await settings.GetLocalSettingsAsync();
        }

        private void screen_type_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            const int RGB = 0;
            const int DELTA = 1;
            const int ZDY = 2;
            const int PENTILE = 3;
            const int DIAMOND = 4;

            switch (screen_type.SelectedIndex)
            {
                case RGB: discount = 1; break;
                case DELTA: discount = 0.7; break;
                case ZDY: discount = 0.68; break;
                case PENTILE: discount = 0.816; break;
                case DIAMOND: discount = 0.83; break;
                default: discount = 1; break;
            }

            CalcPPI();
        }

        private void R720p_Click(object sender, RoutedEventArgs e)
        {
            height.Text = "720";
        }
        private void R1080p_Click(object sender, RoutedEventArgs e)
        {
            height.Text = "1080";
        }
        private void R1440p_Click(object sender, RoutedEventArgs e)
        {
            height.Text = "1440";
        }
        private void R1644p_Click(object sender, RoutedEventArgs e)
        {
            height.Text = "1644";
        }
        private void R2160p_Click(object sender, RoutedEventArgs e)
        {
            height.Text = "2160";
        }
        private void B16x10_Click(object sender, RoutedEventArgs e)
        {
            if (height.Text != "")
            {
                double h = double.Parse(height.Text);
                width.Text = ((int)(h / 10 * 16)).ToString();
            }
            else if (width.Text != "")
            {
                double w = double.Parse(width.Text);
                height.Text = ((int)(w / 16 * 10)).ToString();
            }
            CalcPPI();
        }
        private void B16x9_Click(object sender, RoutedEventArgs e)
        {
            if (height.Text != "")
            {
                double h = double.Parse(height.Text);
                width.Text = ((int)(h / 9 * 16)).ToString();
            }
            else if (width.Text != "")
            {
                double w = double.Parse(width.Text);
                height.Text = ((int)(w / 16 * 9)).ToString();
            }
            CalcPPI();
        }
        private void B18x9_Click(object sender, RoutedEventArgs e)
        {
            if (height.Text != "")
            {
                double h = double.Parse(height.Text);
                width.Text = ((int)(h * 2)).ToString();
            }
            else if (width.Text != "")
            {
                double w = double.Parse(width.Text);
                height.Text = ((int)(w / 2)).ToString();
            }
            CalcPPI();
        }
        private void B21x9_Click(object sender, RoutedEventArgs e)
        {
            if (height.Text != "")
            {
                double h = double.Parse(height.Text);
                if (h == 1644)
                {
                    width.Text = "3840";
                    CalcPPI();
                    return;
                }
                width.Text = ((int)(h / 9 * 21)).ToString();
            }
            else if (width.Text != "")
            {
                double w = double.Parse(width.Text);
                if (w == 3840)
                {
                    height.Text = "1644";
                    CalcPPI();
                    return;
                }
                height.Text = ((int)(w / 21 * 9)).ToString();
            }
            CalcPPI();
        }
        private void Reset_Click(object sender, RoutedEventArgs e)
        {
            width.Text = "";
            height.Text = "";
            res.Text = "";
            size.Text = "";
            screen_type.SelectedIndex = 0;
        }
    }
}
