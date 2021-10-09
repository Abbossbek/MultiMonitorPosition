using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Forms = System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Text.RegularExpressions;
using MaterialDesignThemes.Wpf;
using MultiMonitorPosition.Pages;

namespace MultiMonitorPosition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();
        public MainWindow()
        {
            InitializeComponent();
            Refresh();
        }
        void Refresh()
        {
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
            //spMain.DataContext = new ScreenModel();
            foreach (var item in Forms.Screen.AllScreens)
            {
                var screenView = mainCanvas.Children.Cast<ScreenView>().FirstOrDefault(x => ((ScreenModel)x.DataContext)?.Name == item.DeviceName);
                ScreenModel screen = (ScreenModel)screenView?.DataContext;
                if (screen is null)
                {
                    screen = new ScreenModel()
                    {
                        Name = item.DeviceName,
                        Height = item.Bounds.Height,
                        Width = item.Bounds.Width,
                        X = item.WorkingArea.X,
                        Y = item.WorkingArea.Y,
                        IsPrimary = item.Primary
                    };
                    screenView = new ScreenView() { DataContext = screen };
                    screenView.PreviewMouseLeftButtonDown += (s, e) =>
                    {
                        spMain.DataContext = ((ScreenView)s).DataContext;
                    };
                    mainCanvas.Children.Add(screenView);
                    Canvas.SetLeft(screenView, screen.X);
                    Canvas.SetTop(screenView, screen.Y);
                }
                else
                {
                    Canvas.SetLeft(screenView, item.WorkingArea.X);
                    Canvas.SetTop(screenView, item.WorkingArea.Y);
                }
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnApply_Click(object sender, RoutedEventArgs e)
        {
            foreach (var screen in mainCanvas.Children.Cast<ScreenView>().Select(x => (ScreenModel)x.DataContext))
            {
                var processInfo = new ProcessStartInfo("DPEdit.exe", $"{screen.Name.Last()} {screen.X} {screen.Y}");
                processInfo.CreateNoWindow = true;
                Process.Start(processInfo);
            }
            await Task.Delay(3000);
            Refresh();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            dialogHostFrame.NavigationService.RemoveBackEntry();
            dialogHostFrame.NavigationService.Navigate(new SavePositionsPage(mainCanvas.Children.Cast<ScreenView>().Select(x => (ScreenModel)x.DataContext).ToList()));
            dialogHost.IsOpen = true;
        }

        private async void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            dialogHostFrame.NavigationService.RemoveBackEntry();
            var loadPage = new LoadPositionsPage();
            dialogHostFrame.NavigationService.Navigate(loadPage);
            await DialogHost.Show(dialogHostFrame, new DialogClosingEventHandler((sender, args) =>
            {
                if (loadPage.Screens is not null)
                    foreach (var item in loadPage.Screens)
                    {
                        var screenView = mainCanvas.Children.Cast<ScreenView>().FirstOrDefault(x => ((ScreenModel)x.DataContext)?.Name == item.Name);
                        if (screenView is not null)
                        {
                            Canvas.SetLeft(screenView, item.X);
                            Canvas.SetTop(screenView, item.Y);
                        }
                    }
            }));
        }
    }
}
