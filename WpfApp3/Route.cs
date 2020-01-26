using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;

namespace WpfApp3
{
    class Route : MapObject
    {
        private List<PointLatLng> Points { get; set; }

        public Route(string title, List<PointLatLng> points) : base(title)
        {
            Points = points;
        }

        public override double GetDistance(PointLatLng point)
        {
            return double.MaxValue;
        }

        public override PointLatLng Focus => Points[0];

        public override GMapMarker Marker => new GMapRoute(Points)
        {
            Shape = new Path()
            {
                Stroke = Brushes.DarkBlue,
                Fill = Brushes.DarkBlue,
                StrokeThickness = 4
            }
        };
    }
}
