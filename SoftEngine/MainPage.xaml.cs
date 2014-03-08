using SharpDX;
using SoftEngine.Model;
using SoftEngine.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en http://go.microsoft.com/fwlink/?LinkId=234238

namespace SoftEngine
{

    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent(); 
            //Initialize objects
            // Source of the XAML img => our Writeable bitmap
            frontBuffer.Source = MainViewModel.Current.Engine.Bitmap;
            // Registering to the XAML rendering loop
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            // Registering keyboard events

        }



        private void CompositionTarget_Rendering(object sender, object e)
        {
            MainViewModel.Current.Engine.Clear(0, 0, 0, 255);
            Quaternion p = new Quaternion(new Vector3(1.0f, 1.0f, 1.0f), 50f);
            p.Normalize();
            MainViewModel.Current.Cube.Rotation = MainViewModel.Current.Cube.Rotation * p;
            MainViewModel.Current.Tetra.Rotation = MainViewModel.Current.Tetra.Rotation * p;
            // Doing the various matrix operations
            MainViewModel.Current.Engine.Render(MainViewModel.Current.Camera, MainViewModel.Current.Cube);
            MainViewModel.Current.Engine.Render(MainViewModel.Current.Camera, MainViewModel.Current.Tetra);
            // Flushing the back buffer into the front buffer
            MainViewModel.Current.Engine.Present();
        }
    }
}
