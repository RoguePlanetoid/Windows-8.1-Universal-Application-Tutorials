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

namespace Agent
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            toggleButton(Shared.Init());
        }
        public Shared Shared = new Shared();

        private void toggleButton(bool value)
        {
            if (value)
            {
                Toggle.Icon = new SymbolIcon(Symbol.Stop);
                Toggle.Label = "Stop Agent";
            }
            else
            {
                Toggle.Icon = new SymbolIcon(Symbol.Play);
                Toggle.Label = "Start Agent";
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Shared.Save(Value.Text);
        }

        private async void Toggle_Click(object sender, RoutedEventArgs e)
        {
            toggleButton(await Shared.Toggle());
        }
    }
}
