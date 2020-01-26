using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace WpfApp3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<MapObject> MapObjectCollection = new List<MapObject>();
        private List<MapObject> FoundedObject = new List<MapObject>();
        private Dictionary<PointLatLng, GMapMarker> PointsCollection = new Dictionary<PointLatLng, GMapMarker>();

        public MainWindow()
        {
            InitializeComponent();
            combobox1.ItemsSource = new string[] {
                "Область",
                "Маршрут",
                "Местоположение",
                "Автомобиль",
                "Человек"
            };
            combobox1.SelectedIndex = 0;
        }

        private void MapLoaded(object sender, RoutedEventArgs e)
        {
            // настройка доступа к данным
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            // установка провайдера карт
            Map.MapProvider = OpenStreetMapProvider.Instance;

            // установка зума карты
            Map.MinZoom = 2;
            Map.MaxZoom = 17;
            Map.Zoom = 15;

            // установка фокуса карты
            Map.Position = new PointLatLng(55.012823, 82.950359);

            // настройка взаимодействия с картой
            Map.MouseWheelZoomType = MouseWheelZoomType.MousePositionAndCenter;
            Map.CanDragMap = true;
            Map.DragButton = MouseButton.Left;

            InitMapObjects();

            dsf();
        }

        private void Map_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            PointLatLng point = Map.FromLocalToLatLng((int)e.GetPosition(Map).X, (int)e.GetPosition(Map).Y);

            if (radiobutton1.IsChecked == true)
            {
                var pointMarker = GetPointMarker(point);
                Map.Markers.Add(pointMarker);
                PointsCollection.Add(point, pointMarker);
            }
            else if (radiobutton2.IsChecked == true)
            {
                FoundedObject = new List<MapObject>(MapObjectCollection);

                var besidedObjects = MapObjectCollection
                    .ToDictionary(mapObject => mapObject, mapObject => mapObject.GetDistance(point))
                    .OrderBy(mapObject => mapObject.Value);

                listBox1.Items.Clear();
                foreach (KeyValuePair<MapObject, double> mapObjectAndDistance in besidedObjects)
                {
                    string mapObjectAndDistanceString = new StringBuilder()
                        .Append(mapObjectAndDistance.Key.Title)
                        .Append(" - ")
                        .Append(mapObjectAndDistance.Value.ToString("0.##"))
                        .Append(" м.")
                        .ToString();
                    listBox1.Items.Add(mapObjectAndDistanceString);
                }    
            }
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ClearPoints();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (PointsCollection.Count == 0)
            {
                MessageBox.Show("Выберите точки");
                return;
            }

            string title = textBox1.Text;
            MapObject mapObject = null;
            switch (combobox1.SelectedIndex)
            {
                case 0:
                    mapObject = new Area(title, PointsCollection.Keys.ToList());
                    break;
                case 1:
                    mapObject = new Route(title, PointsCollection.Keys.ToList());
                    break;
                case 2:
                    mapObject = new Location(title, PointsCollection.Keys.Last());
                    break;
                case 3:
                    mapObject = new Car(title, PointsCollection.Keys.Last());
                    break;
                case 4:
                    mapObject = new Human(title, PointsCollection.Keys.Last());
                    break;
                default:
                    break;
            }
            MapObjectCollection.Add(mapObject);
            Map.Markers.Add(mapObject.Marker);

            ClearPoints();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string text = textBox2.Text;
            FoundedObject.Clear();
            foreach (MapObject mapObject in MapObjectCollection)
            {
                if (mapObject.Title.Contains(text))
                {
                    FoundedObject.Add(mapObject);
                }
            }

            listBox1.Items.Clear();
            if (FoundedObject.Count == 0)
            {
                MessageBox.Show("Объекты не найдены");
            }
            else if (FoundedObject.Count == 1)
            {
                MoveTo(FoundedObject[0]);
            }
            else if (FoundedObject.Count > 1)
            {
                foreach (MapObject foundObject in FoundedObject)
                {
                    listBox1.Items.Add(foundObject.Title);
                }
            }
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = listBox1.SelectedIndex;
            if (index == -1) return;
            else MoveTo(FoundedObject[index]);
        }

        private void InitMapObjects()
        {
            MapObjectCollection.Add(new Car("Honda CR-V", new PointLatLng(55.013743, 82.956879)));
            MapObjectCollection.Add(new Car("Reno Logan", new PointLatLng(55.009706, 82.951371)));
            MapObjectCollection.Add(new Area("Октябрьский рынок", new PointLatLng[] {
                new PointLatLng(55.016351, 82.950650),
                new PointLatLng(55.017021, 82.951484),
                new PointLatLng(55.015795, 82.954526),
                new PointLatLng(55.015129, 82.953586),
            }.ToList()));
            MapObjectCollection.Add(new Area("Сквер ГПНТБ", new PointLatLng[] {
                new PointLatLng(55.016104, 82.946914),
                new PointLatLng(55.016639, 82.947559),
                new PointLatLng(55.016178, 82.948765),
                new PointLatLng(55.016334, 82.949068),
                new PointLatLng(55.016556, 82.949220),
                new PointLatLng(55.016708, 82.949652),
                new PointLatLng(55.016356, 82.950388),
                new PointLatLng(55.015216, 82.949008),
                new PointLatLng(55.015173, 82.948742),
                new PointLatLng(55.015956, 82.946823)
            }.ToList()));
            MapObjectCollection.Add(new Route("ул. Восход", new PointLatLng[] {
                new PointLatLng(55.010637, 82.938550),
                new PointLatLng(55.012421, 82.940781),
                new PointLatLng(55.014613, 82.943497),
                new PointLatLng(55.016214, 82.945469),
            }.ToList()));
            MapObjectCollection.Add(new Route("ул. Гурьевская", new PointLatLng[] {
                new PointLatLng(55.007988, 82.944863),
                new PointLatLng(55.011489, 82.949283),
                new PointLatLng(55.013507, 82.951762),
                new PointLatLng(55.018785, 82.958424),
            }.ToList()));
            MapObjectCollection.Add(new Location("Coober, Спортивный центр", new PointLatLng(55.015104, 82.948034)));
            MapObjectCollection.Add(new Location("СибГУТИ", new PointLatLng(55.013104, 82.950663)));
            MapObjectCollection.Add(new Human("Я", new PointLatLng(55.016511, 82.946152)));
            MapObjectCollection.Add(new Human("Он", new PointLatLng(55.011696, 82.953658)));

            foreach (MapObject mapObject in MapObjectCollection) Map.Markers.Add(mapObject.Marker);
        }

        private void ClearPoints()
        {
            foreach (GMapMarker pointMarker in PointsCollection.Values)
                Map.Markers.Remove(pointMarker);

            PointsCollection.Clear();
        }

        private GMapMarker GetPointMarker(PointLatLng point)
        {
            return new GMapMarker(point)
            {
                Shape = new Image
                {
                    Width = 16,
                    Height = 16,
                    Source = new BitmapImage(new Uri("pack://application:,,,/Resources/circle.png"))
                }
            };
        }

        private void MoveTo(MapObject mapObject)
        {
            Map.Position = mapObject.Focus;
        }

        private void dsf()
        {
            PointLatLng a = new PointLatLng(54.996576, 82.995545);
            PointLatLng b = new PointLatLng(55.010605, 83.053137);
            PointLatLng c = new PointLatLng(54.978028, 83.051269);

            double result = new DistanceCalculator().GetMinDistance(
                new PointLatLng(54.996576, 82.995545),
                 new PointLatLng(55.010605, 83.053137),
                 new PointLatLng(54.978028, 83.051269));

            result = new DistanceCalculator().GetMinDistance(
               new PointLatLng(53.337864, 83.691411),
               new PointLatLng(53.351514, 83.900452),
               new PointLatLng(54.978028, 83.051269));
        }
    }
}

