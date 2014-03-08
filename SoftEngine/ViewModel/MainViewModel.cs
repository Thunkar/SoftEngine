using SharpDX;
using SoftEngine.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine.ViewModel
{
    class MainViewModel
    {
        public static MainViewModel Current;
        public Engine Engine { get; set; }
        public Mesh Cube { get; set; }
        public Mesh Tetra { get; set; }
        public Camera Camera { get; set; }

        public MainViewModel()
        {
            Current = this;
            Engine = new Engine(900,600);
            Cube = new Mesh("Cube", 8, 12);
            Tetra = new Mesh("Tetraedrum", 4, 4);
            Camera = new Camera();
            CreateMeshes();
        }

        public void CreateMeshes()
        {
            Tetra.Vertices[0] = new Vector3(0, 1, 0);
            Tetra.Vertices[1] = new Vector3(1, 0, 1);
            Tetra.Vertices[2] = new Vector3(-1, 0, 1);
            Tetra.Vertices[3] = new Vector3(-1, 0, 0);

            Tetra.Faces[0] = new Face { A = 0, B = 1, C = 2 };
            Tetra.Faces[1] = new Face { A = 0, B = 1, C = 3 };
            Tetra.Faces[2] = new Face { A = 1, B = 2, C = 3 };
            Tetra.Faces[3] = new Face { A = 0, B = 2, C = 3 };


            Cube.Vertices[0] = new Vector3(-1, 1, 1);
            Cube.Vertices[1] = new Vector3(1, 1, 1);
            Cube.Vertices[2] = new Vector3(-1, -1, 1);
            Cube.Vertices[3] = new Vector3(1, -1, 1);
            Cube.Vertices[4] = new Vector3(-1, 1, -1);
            Cube.Vertices[5] = new Vector3(1, 1, -1);
            Cube.Vertices[6] = new Vector3(1, -1, -1);
            Cube.Vertices[7] = new Vector3(-1, -1, -1);

            Cube.Faces[0] = new Face { A = 0, B = 1, C = 2 };
            Cube.Faces[1] = new Face { A = 1, B = 2, C = 3 };
            Cube.Faces[2] = new Face { A = 1, B = 3, C = 6 };
            Cube.Faces[3] = new Face { A = 1, B = 5, C = 6 };
            Cube.Faces[4] = new Face { A = 0, B = 1, C = 4 };
            Cube.Faces[5] = new Face { A = 1, B = 4, C = 5 };

            Cube.Faces[6] = new Face { A = 2, B = 3, C = 7 };
            Cube.Faces[7] = new Face { A = 3, B = 6, C = 7 };
            Cube.Faces[8] = new Face { A = 0, B = 2, C = 7 };
            Cube.Faces[9] = new Face { A = 0, B = 4, C = 7 };
            Cube.Faces[10] = new Face { A = 4, B = 5, C = 6 };
            Cube.Faces[11] = new Face { A = 4, B = 6, C = 7 };

            Camera.Position = new Vector3(0, 0, 10.0f);
            Camera.Target = Vector3.Zero;
            Cube.Position = new Vector3(1.5f, 0f, 0f);
            Tetra.Position = new Vector3(-1.5f, 0f, 0f);
        }

    }
}
