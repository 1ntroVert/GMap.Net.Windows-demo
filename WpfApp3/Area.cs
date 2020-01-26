using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp3
{
    class Area : MapObject
    {
        private List<PointLatLng> Points { get; set; }

        public Area(string title, List<PointLatLng> points) : base(title)
        {
            Points = points;
        }

        public override double GetDistance(PointLatLng point)
        {
            return double.MaxValue;
        }

        public override PointLatLng Focus => Points[0];

        public override GMapMarker Marker => new GMapPolygon(Points)
        {
            Shape = new Path
            {
                Stroke = Brushes.Black,
                Fill = Brushes.Violet,
                Opacity = 0.7
            }
        };
    }
}
