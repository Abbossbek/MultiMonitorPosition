using MaterialDesignThemes.Wpf;

using MultiMonitorPosition.Models;
using MultiMonitorPosition.Utils;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiMonitorPosition.Pages
{
    /// <summary>
    /// Логика взаимодействия для LoadPositionsPage.xaml
    /// </summary>
    public partial class LoadPositionsPage : Page
    {
        public List<ScreenModel> Screens { get; set; }
        public LoadPositionsPage()
        {
            InitializeComponent();
            lbItems.ItemsSource = DataHelper.GetValue<List<ScreensPositionModel>>("screenPositions");
        }

        private void Item_Click(object sender, RoutedEventArgs e)
        {
            var item = (ScreensPositionModel)((Button)sender).DataContext;
            Screens = item.Screens;
            DialogHost.CloseDialogCommand.Execute(((MainWindow)Window.GetWindow(this)).dialogHost, null);
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            DataHelper.Clear();
            lbItems.ItemsSource = null;
        }
    }
}
