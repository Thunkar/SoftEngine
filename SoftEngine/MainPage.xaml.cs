using SharpDX;
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
        private Device device;
        Mesh mesh = new Mesh("Cube", 8, 12);
        Mesh tetra = new Mesh("Tetraedrum", 4, 4);
        Camera camera = new Camera();

        public MainPage()
        {
            this.InitializeComponent();
            // Choose the back buffer resolution here
            WriteableBitmap bmp = new WriteableBitmap(900, 600);
            //Initialize objects
            this.device = new Device(bmp);
            // Source of the XAML img => our Writeable bitmap
            frontBuffer.Source = bmp;


            tetra.Vertices[0] = new Vector3(0, 1, 0);
            tetra.Vertices[1] = new Vector3(1, 0, 1);
            tetra.Vertices[2] = new Vector3(-1, 0, 1);
            tetra.Vertices[3] = new Vector3(-1, 0, 0);

            tetra.Faces[0] = new Face { A = 0, B = 1, C = 2 };
            tetra.Faces[1] = new Face { A = 0, B = 1, C = 3 };
            tetra.Faces[2] = new Face { A = 1, B = 2, C = 3 };
            tetra.Faces[3] = new Face { A = 0, B = 2, C = 3 };


            mesh.Vertices[0] = new Vector3(-1, 1, 1);
            mesh.Vertices[1] = new Vector3(1, 1, 1);
            mesh.Vertices[2] = new Vector3(-1, -1, 1);
            mesh.Vertices[3] = new Vector3(1, -1, 1);
            mesh.Vertices[4] = new Vector3(-1, 1, -1);
            mesh.Vertices[5] = new Vector3(1, 1, -1);
            mesh.Vertices[6] = new Vector3(1, -1, -1);
            mesh.Vertices[7] = new Vector3(-1, -1, -1);

            mesh.Faces[0] = new Face { A = 0, B = 1, C = 2 };
            mesh.Faces[1] = new Face { A = 1, B = 2, C = 3 };
            mesh.Faces[2] = new Face { A = 1, B = 3, C = 6 };
            mesh.Faces[3] = new Face { A = 1, B = 5, C = 6 };
            mesh.Faces[4] = new Face { A = 0, B = 1, C = 4 };
            mesh.Faces[5] = new Face { A = 1, B = 4, C = 5 };

            mesh.Faces[6] = new Face { A = 2, B = 3, C = 7 };
            mesh.Faces[7] = new Face { A = 3, B = 6, C = 7 };
            mesh.Faces[8] = new Face { A = 0, B = 2, C = 7 };
            mesh.Faces[9] = new Face { A = 0, B = 4, C = 7 };
            mesh.Faces[10] = new Face { A = 4, B = 5, C = 6 };
            mesh.Faces[11] = new Face { A = 4, B = 6, C = 7 };

            camera.Position = new Vector3(0, 0, 10.0f);
            camera.Target = Vector3.Zero;
            mesh.Position = new Vector3(1.5f, 0f, 0f);
            tetra.Position = new Vector3(-1.5f, 0f, 0f);

            // Registering to the XAML rendering loop
            CompositionTarget.Rendering += CompositionTarget_Rendering;
            // Registering keyboard events

        }



        private void CompositionTarget_Rendering(object sender, object e)
        {
            device.Clear(0, 0, 0, 255);
            Quaternion p = new Quaternion(new Vector3(1.0f, 1.0f, 1.0f), 50f);
            p.Normalize();
            mesh.Rotation = mesh.Rotation * p;
            tetra.Rotation = tetra.Rotation * p;
            // Doing the various matrix operations
            device.Render(camera, mesh);
            device.Render(camera, tetra);
            // Flushing the back buffer into the front buffer
            device.Present();
        }



    }
}
