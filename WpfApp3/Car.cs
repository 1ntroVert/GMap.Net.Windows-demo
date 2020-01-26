using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace WpfApp3
{
    class Car : MapObject
    {
        private PointLatLng Point { get; set; }

        public Car(string title, PointLatLng startPoint) : base(title)
        {
            Point = startPoint;
        }

        public override double GetDistance(PointLatLng point)
        {
            GDirections direction = new GDirections();
            GMapProviders.GoogleMap.GetDirections(out direction, Point, point, false, false, true, false, false);
            var d = direction.DistanceValue;

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
                        Width = 96,
                        Height = 96,
                        ToolTip = Title,
                        Source = new BitmapImage(new Uri("pack://application:,,,/Resources/car.png"))
                    }
                };
            }
        }
    }
}
