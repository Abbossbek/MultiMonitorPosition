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

namespace MultiMonitorPosition
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<ScreenModel> screens = new List<ScreenModel>();
        public MainWindow()
        {
            InitializeComponent();
            Refresh();
        }
        void Refresh()
        {
            mainCanvas.Children.Clear();
            screens.Clear();
            spMain.DataContext = new ScreenModel();
            foreach (var item in Forms.Screen.AllScreens)
            {
                var screen = new ScreenModel()
                {
                    Name = item.DeviceName,
                    Height = item.Bounds.Height,
                    Width = item.Bounds.Width,
                    X = item.WorkingArea.X,
                    Y = item.WorkingArea.Y,
                    IsPrimary = item.Primary
                };
                screens.Add(screen);
                var screenView = new ScreenView() { DataContext = screens.Last() };
                screenView.PreviewMouseLeftButtonDown += (s, e) =>
                {
                    spMain.DataContext = ((ScreenView)s).DataContext;
                };
                mainCanvas.Children.Add(screenView);
                Canvas.SetLeft(screenView, screen.X);
                Canvas.SetTop(screenView, screen.Y);
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void btnApply_Click(object sender, RoutedEventArgs e)
        {
            for(int i=0;i<screens.Count();i++)
            {
                var processInfo = new ProcessStartInfo("DPEdit.exe", $"{screens[i].Name.Last()} {screens[i].X} {screens[i].Y}");
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
    }
}
