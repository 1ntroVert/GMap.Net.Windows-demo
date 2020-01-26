using GMap.NET;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace WpfApp3
{
    abstract class MapObject
    {
        public string Title { get; private set; }
        public DateTime CreationDate { get; private set; }

        public abstract PointLatLng Focus { get; }

        public abstract GMapMarker Marker { get; }

        public abstract double GetDistance(PointLatLng point);

        protected MapObject(string title)
        {
            Title = title;
            CreationDate = DateTime.Now;
        }
    }
}
