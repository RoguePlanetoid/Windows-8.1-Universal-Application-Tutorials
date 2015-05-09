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

namespace SlidePlayer
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            init();
        }
        public Shared Shared = new Shared();

        private void init()
        {
            Shared.Speed = (int)Speed.Value;
            Shared.Playing += (Windows.UI.Xaml.Media.Imaging.BitmapImage image, int index) =>
            {
                Display.Source = image;
                Position.Value = index;
            };
            Shared.Stopped += () =>
            {
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
                Display.Source = null;
                Position.Value = 0;
            };
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            Position.Maximum = Shared.Add(Value.Text);
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            Position.Maximum = Shared.Remove((int)Position.Value);
        }

        private void Play_Click(object sender, RoutedEventArgs e)
        {
            if (Shared.IsPlaying)
            {
                Shared.Pause();
                Play.Icon = new SymbolIcon(Symbol.Play);
                Play.Label = "Play";
            }
            else
            {
                Shared.Play();
                Play.Icon = new SymbolIcon(Symbol.Pause);
                Play.Label = "Pause";
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            Shared.Stop();
        }

        private void Go_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            Shared.Go(ref Display, Value.Text, e);
        }

        private void Position_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            Shared.Position = (int)Position.Value;
        }

        private void Speed_ValueChanged(object sender, RangeBaseValueChangedEventArgs e)
        {
            if (Speed != null)
            {
                Shared.Speed = (int)Speed.Value;
            }
        }
    }
}
