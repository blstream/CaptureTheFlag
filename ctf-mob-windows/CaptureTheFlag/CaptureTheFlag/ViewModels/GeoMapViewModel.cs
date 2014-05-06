﻿namespace CaptureTheFlag.ViewModels
{
    using Caliburn.Micro;
    using CaptureTheFlag.Models;
    using CaptureTheFlag.Services;
    using Microsoft.Phone.Maps.Controls;
    using Microsoft.Phone.Maps.Toolkit;
    using System;
    using System.Device.Location;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Ink;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using Windows.Devices.Geolocation;
    public class GeoMapViewModel : Screen
    {
        private readonly ICommunicationService communicationService;
        private readonly ILocationService locationService;
        private readonly IGlobalStorageService globalStorageService;

        public GeoMapViewModel(ILocationService locationService, ICommunicationService communicationService, IGlobalStorageService globalStorageService)
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            this.communicationService = communicationService;
            this.locationService = locationService;
            this.globalStorageService = globalStorageService;


            
            ZoomLevel = 18.2;
            MapCenter = new GeoCoordinate(53.432806, 14.548033);
            MapCenter.Altitude = 0.0;
            Markers = MarkerHelper.makeMarkers(10);


            MapLayer MapLayer = new MapLayer();
            
            Area = new Ellipse();
            Area.Height = 600;
            Area.Width = 600;
            Area.Opacity = 0.5;
            
            SolidColorBrush fillSolidColorBrush = new SolidColorBrush();
            fillSolidColorBrush.Color = Colors.Red;
            Area.Fill = fillSolidColorBrush;
            
            
            Fill = fillSolidColorBrush;
            MapOverlay overlay = new MapOverlay();
            overlay.Content = Area;
            overlay.GeoCoordinate = MapCenter;
            overlay.PositionOrigin = new Point(0.5, 0.5);
            MapLayer.Add(overlay);
            MapLayers = new BindableCollection<MapLayer>();
            MapLayers.Add(MapLayer);

            //Area.Visibility = System.Windows.Visibility.Visible;
            //Area.Opacity = 1.0;
            //// Create a SolidColorBrush with a red color to fill the  
            //// Ellipse with.
            
            //SolidColorBrush borderSolidColorBrush = new SolidColorBrush();

            //// Describes the brush's color using RGB values.  
            //// Each value has a range of 0-255.
            
            
            //Area.StrokeThickness = 2;
            //borderSolidColorBrush.Color = Colors.Black;
            //Area.Stroke = borderSolidColorBrush;

            Map = new Map();
            Map.ZoomLevel = ZoomLevel;
            Map.Center = MapCenter;
            
            RefreshAppBarItemText = "refresh";
            RefreshIcon = new Uri("/Images/refresh.png", UriKind.Relative);
        }

        #region ViewModel States
        protected override void OnActivate()
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            base.OnActivate();
            Authenticator = globalStorageService.Current.Authenticator;
        }
        #endregion

        public async void UpdateMarkersAction()
        {
            GeoCoordinate position = await locationService.getCurrentGeoCoordinateAsync();
            Game game = new Game() { url = "http://78.133.154.39:8888/api/games/2/" };
            if (Authenticator.IsValid(Authenticator))
            {
                communicationService.RegisterPosition(game, position, Authenticator.token,
                    responseMarkers =>
                    {
                        DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), "Successful marker response: {0}", responseMarkers);
                        MoveAndShowMarkers();
                    },
                    serverErrorMessage =>
                    {
                        DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), "Failed marker response: {0}", serverErrorMessage);
                        MoveAndShowMarkers();
                    }
                );
            }
        }

        public void MoveAndShowMarkers()
        {
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod());
            MarkerHelper.moveMarkers(Markers);
            string formattedLocations = "><";
            foreach(Marker marker in Markers)
            {
                formattedLocations += String.Format("t {0} : lat {1}, lon {2} ><", marker.type, marker.location.lat, marker.location.lon);
            }
            DebugLogger.WriteLine(this.GetType(), MethodBase.GetCurrentMethod(), formattedLocations);
        }

        public void RefreshAction()
        {
            UpdateMarkersAction();
        }

        private SolidColorBrush fill;

        public SolidColorBrush Fill
        {
            get { return fill; }
            set { fill = value; NotifyOfPropertyChange(() => Fill); }
        }

        private BindableCollection<MapLayer> mapLayers;

        public BindableCollection<MapLayer> MapLayers
        {
            get { return mapLayers; }
            set { mapLayers = value; NotifyOfPropertyChange(() => MapLayers); }
        }

        private MapLayer mapLayer;

        public MapLayer MapLayer
        {
            get { return mapLayer; }
            set { mapLayer = value; NotifyOfPropertyChange(() => MapLayer); }
        }

        private Ellipse area;
        public Ellipse Area
        {
            get { return area; }
            set
            {
                if (area != value)
                {
                    area = value;
                    NotifyOfPropertyChange(() => Area);
                }
            }
        }

        private Map map;
        public Map Map
        {
            get { return map; }
            set
            {
                if (map != value)
                {
                    map = value;
                    NotifyOfPropertyChange(() => Map);
                }
            }
        }

        private BindableCollection<Pushpin> pins;
        public BindableCollection<Pushpin> Pins
        {
            get { return pins; }
            set
            {
                if (pins != value)
                {
                    pins = value;
                    NotifyOfPropertyChange(() => Pins);
                }
            }
        }

        private BindableCollection<Marker> markers;
        public BindableCollection<Marker> Markers
        {
            get { return markers; }
            set
            {
                if (markers != value)
                {
                    markers = value;
                    NotifyOfPropertyChange(() => Markers);
                }
            }
        }

        private GeoCoordinate mapCenter;
        public GeoCoordinate MapCenter
        {
            get { return mapCenter; }
            set
            {
                if (mapCenter != value)
                {
                    mapCenter = value;
                    NotifyOfPropertyChange(() => MapCenter);
                }
            }
        }

        private Authenticator authenticator;
        public Authenticator Authenticator
        {
            get { return authenticator; }
            set
            {
                if (authenticator != value)
                {
                    authenticator = value;
                    NotifyOfPropertyChange(() => Authenticator);
                }
            }
        }

        private Uri refreshIcon;
        public Uri RefreshIcon
        {
            get { return refreshIcon; }
            set
            {
                if (refreshIcon != value)
                {
                    refreshIcon = value;
                    NotifyOfPropertyChange(() => RefreshIcon);
                }
            }
        }

        private string refreshAppBarItemText;
        public string RefreshAppBarItemText
        {
            get { return refreshAppBarItemText; }
            set
            {
                if (refreshAppBarItemText != value)
                {
                    refreshAppBarItemText = value;
                    NotifyOfPropertyChange(() => RefreshAppBarItemText);
                }
            }
        }

        public static double MetersToPixels(double meters, double latitude, double zoomLevel)
        {
            var pixels = meters / (156543.04 * Math.Cos(latitude) / (Math.Pow(2, zoomLevel)));
            return Math.Abs(pixels);
        }

        //TODO: Draw visibility range
        //TODO: Draw action range

        private double zoomLevel = 15;
        public double ZoomLevel
        {
            get { return zoomLevel; }
            set
            {
                if (zoomLevel != value)
                {
                    zoomLevel = value;
                    if (Area != null)
                    {
                        Area.Height = MetersToPixels(150, Map.Center.Latitude, ZoomLevel); //TODO: Calculate from Map radius
                        Area.Width = MetersToPixels(150, Map.Center.Latitude, ZoomLevel); //TODO: Calculate from Map radius
                    }
                    NotifyOfPropertyChange(() => ZoomLevel);
                }
            }
        }
    }
}