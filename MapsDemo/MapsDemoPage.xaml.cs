using System;
using System.Collections.ObjectModel;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace MapsDemo
{
    public partial class MapsDemoPage : ContentPage
    {





        public MapsDemoPage()
        {
            InitializeComponent();


			


            loadPosition();

            BindingContext = this;
        }


        async void loadPosition(){
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 5000;
            locator.PositionChanged+= Locator_PositionChanged;
            locator.StartListeningAsync(10000,5000,true);
			var position = await locator.GetPositionAsync(10000);
			var pin = new Pin
			{
				Type = PinType.SearchResult,
                Position = new Position(position.Latitude, position.Longitude),
                Label = string.Format("{0}, {1}", position.Latitude, position.Longitude)
			};

            map.Pins.Add(pin);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(position.Latitude,position.Longitude),Distance.FromKilometers(3)));
		}
		
		void Locator_PositionChanged(object sender, Plugin.Geolocator.Abstractions.PositionEventArgs e)
        {
			var pin = new Pin
			{
				Type = PinType.SearchResult,
                Position = new Position(e.Position.Latitude, e.Position.Longitude),
                Label = string.Format("{0}, {1}", e.Position.Latitude, e.Position.Longitude)
			};

			map.Pins.Add(pin);
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(e.Position.Latitude, e.Position.Longitude), Distance.FromKilometers(3)));

		}

        void Handle_ValueChanged(object sender, Xamarin.Forms.ValueChangedEventArgs e)
        {
            
			var zoomLevel = e.NewValue; // between 1 and 18
			var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
			map.MoveToRegion(new MapSpan(map.VisibleRegion.Center, latlongdegrees, latlongdegrees));
        }
    }
}
