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

namespace ImageRotate
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
        public Shared Shared = new Shared();

        private void Go_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                Display.Source = new Windows.UI.Xaml.Media.Imaging.BitmapImage(new Uri(Value.Text));
            }
        }

        private void Pitch_Click(object sender, RoutedEventArgs e)
        {
            Shared.Rotate("X", ref Display);
        }

        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            Shared.Rotate("Y", ref Display);
        }

        private void Yaw_Click(object sender, RoutedEventArgs e)
        {
            Shared.Rotate("Z", ref Display);
        }
    }
}
