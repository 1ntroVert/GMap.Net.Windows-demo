using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Device.Location;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp3
{
    class Human : MapObject
    {
        private PointLatLng Point { get; set; }

        public Human(string title, PointLatLng startPoint) : base(title)
        {
            Point = startPoint;
        }

        public override double GetDistance(PointLatLng point)
        {
            GeoCoordinate p1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate p2 = new GeoCoordinate(Point.Lat, Point.Lng);

            return p1.GetDistanceTo(p2);
        }
    
        public override PointLatLng Focus => Point;

        public override GMapMarker Marker
        {
            get
            {
                return new GMapMarker(Point)
                {
                    Shape = new Image
                    {
                        Width = 64,
                        Height = 64,
                        ToolTip = Title,
                        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/human.png"))
                    }
                };
            }
        }
    }
}
