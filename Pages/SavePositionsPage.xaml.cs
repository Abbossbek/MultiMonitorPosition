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
    /// Логика взаимодействия для SavePositionsPage.xaml
    /// </summary>
    public partial class SavePositionsPage : Page
    {
        public SavePositionsPage(List<ScreenModel> screens)
        {
            InitializeComponent();
            DataContext = new ScreensPositionModel() { Screens = screens};
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var spm = (ScreensPositionModel)DataContext;
            var list = DataHelper.GetValue<List<ScreensPositionModel>>("screenPositions");
            if(list is null)
            {
                DataHelper.SetValue("screenPositions", new List<ScreensPositionModel> { spm });
                DialogHost.CloseDialogCommand.Execute(((MainWindow)Window.GetWindow(this)).dialogHost, null);
            }
            else if(list.Any(x=>x.Name == spm.Name))
            {
                MessageBox.Show("An object with this name exists in the saved ones!");
            }
            else
            {
                list.Add(spm);
                DataHelper.SetValue("screenPositions", list);
                DialogHost.CloseDialogCommand.Execute(((MainWindow)Window.GetWindow(this)).dialogHost, null);
            }
        }
    }
}
