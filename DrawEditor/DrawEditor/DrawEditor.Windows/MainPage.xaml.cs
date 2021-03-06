﻿using System;
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

namespace DrawEditor
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

        private void New_Click(object sender, RoutedEventArgs e)
        {
            Shared.New(Display);
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Shared.Opened += (List<Shared.Ink> value) =>
            {
                Shared.Set(ref Display, value);
            };
            Shared.Open();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            Shared.Save();
        }

        private void Display_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            Shared.Draw(ref Display, ref Size, ref Colour, e);
        }
    }
}
