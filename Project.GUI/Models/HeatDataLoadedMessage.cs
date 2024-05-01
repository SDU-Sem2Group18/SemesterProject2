using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Threading;

namespace Project.GUI.Models
{
    public class HeatDataLoadedMessage
    {
        public string Name { get; }
        public List<double> xData { get ; }
        public List<double> yData { get ; }
        public string title { get ; }
        public string xName { get ; }
        public string yName { get ; }
        public HeatDataLoadedMessage(string name, (List<double> xData, List<double> yData, string title, string xName, string yName) args) {
            Name = name;
            xData = args.xData;
            yData = args.yData;
            title = args.title;
            xName = args.xName;
            yName = args.yName;
        }
    }
}