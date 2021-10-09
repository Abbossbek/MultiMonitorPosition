using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MultiMonitorPosition
{
    /// <summary>
    /// Логика взаимодействия для MoveThumb.xaml
    /// </summary>
    public partial class ScreenView : UserControl
    {
        public ScreenView()
        {
            InitializeComponent();
            
            thumb.DragDelta += new DragDeltaEventHandler(this.MoveThumb_DragDelta);

        }

        private void MoveThumb_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var screen = (ScreenModel)DataContext;
            if (!screen.IsPrimary)
            {
                screen.X += (int)e.HorizontalChange;
                screen.Y += (int)e.VerticalChange;
                Canvas.SetLeft(this, screen.X);
                Canvas.SetTop(this, screen.Y);
            }
        }
    }
}
