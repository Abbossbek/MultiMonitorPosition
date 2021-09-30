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
            foreach (var item in Forms.Screen.AllScreens)
            {
                var screen = new ScreenModel()
                {
                    Name = item.DeviceName,
                    Height = item.WorkingArea.Height,
                    Width = item.WorkingArea.Width,
                    X = item.WorkingArea.X,
                    Y = item.WorkingArea.Y
                };
                screens.Add(screen);
                var screenView = new ScreenView() { DataContext = screens.Last() };
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
                var processInfo = new ProcessStartInfo("DPEdit.exe", $"{i + 1} {screens[i].X} {screens[i].Y}");
                processInfo.CreateNoWindow = true;
                Process.Start(processInfo);
            }
            await Task.Delay(3000);
            Refresh();
        }
    }
}
