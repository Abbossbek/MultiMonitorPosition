using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiMonitorPosition.Models
{
    public class ScreensPositionModel
    {
        public string Name { get; set; }
        public List<ScreenModel> Screens { get; set; }
    }
}
