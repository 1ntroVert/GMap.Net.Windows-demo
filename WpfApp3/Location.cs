using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Device.Location;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp3
{
    class Location : MapObject
    {
        private PointLatLng Point { get; set; }

        public Location(string title, PointLatLng point) : base(title)
        {
            Point = point;
        }

        public override double GetDistance(PointLatLng point)
        {
            GeoCoordinate p1 = new GeoCoordinate(point.Lat, point.Lng);
            GeoCoordinate p2 = new GeoCoordinate(Point.Lat, Point.Lng);

            return p1.GetDistanceTo(p2);
        }

        public override PointLatLng Focus => Point;

        public override GMapMarker Marker => new GMapMarker(Point)
        {
            Shape = new Image
            {
                Width = 32,
                Height = 32,
                ToolTip = Title,
                Source = new BitmapImage(new Uri("pack://application:,,,/Resources/marker.png"))
            }
        };
    }
}
