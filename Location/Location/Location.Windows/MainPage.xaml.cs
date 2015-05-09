using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Location
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }
        private async void Location_Click(object sender, RoutedEventArgs e)
        {
            Windows.Devices.Geolocation.Geopoint position = await Shared.Position();
            Bing.Maps.Location location = new Bing.Maps.Location(position.Position.Latitude, position.Position.Longitude);
            UIElement marker = Shared.Marker();
            Display.Children.Add(marker);
            Bing.Maps.MapLayer.SetPosition(marker, location);
            Bing.Maps.MapLayer.SetPositionAnchor(marker, new Point(0.5, 0.5));
            Display.ZoomLevel = 12;
            Display.Center = location;
        }
    }
}
