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
            double w = double.Parse(width.Text);
            double h = double.Parse(height.Text);
            double diag = double.Parse(size.Text);
            res.Text = Math.Round(Math.Sqrt(Math.Pow(w, 2) + Math.Pow(h, 2)) / diag * discount, 2).ToString();
        }

        private bool CheckEmpty()
        {
            return width.Text == "" || height.Text == "" || size.Text == "";
        }

        private void type_rgb_Click(object sender, RoutedEventArgs e)
        {
            screen_type.Content = "RGB (100%)";
            discount = 1;
            if (!CheckEmpty())
                CalcPPI();
        }

        private void type_delta_Click(object sender, RoutedEventArgs e)
        {
            screen_type.Content = "RGB Delta (70%)";
            discount = 0.7;
            if (!CheckEmpty())
                CalcPPI();
        }

        private void type_pentile_Click(object sender, RoutedEventArgs e)
        {
            screen_type.Content = "PenTile (81.6%)";
            discount = 0.816;
            if (!CheckEmpty())
                CalcPPI();
        }

        private void type_diamond_Click(object sender, RoutedEventArgs e)
        {
            screen_type.Content = settings.language_eng ? "Diamond (83%)" : "钻石排列 (83%)";
            discount = 0.83;
            if (!CheckEmpty())
                CalcPPI();
        }

        private void type_zdy_Click(object sender, RoutedEventArgs e)
        {
            screen_type.Content = settings.language_eng ? "BOE Delta (68%)" : "周冬雨排列 (68%)";
            discount = 0.68;
            if (!CheckEmpty())
                CalcPPI();
        }

        private async void Grid_Loaded(object sender, RoutedEventArgs e)
        {

            await settings.GetLocalSettingsAsync();
        }
    }
}
