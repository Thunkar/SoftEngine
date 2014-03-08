using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;

namespace SoftEngine.Model
{

    class Mesh
    {
        public string Name { get; set; }
        public Vector3[] Vertices { get; private set; }
        public Vector3 Position { get; set; }
        public Quaternion Rotation { get; set; }
        public Face[] Faces { get; set; }

        public Mesh(string name, int verticesCount, int facesCount)
        {
            this.Vertices = new Vector3[verticesCount];
            this.Faces = new Face[facesCount];
            this.Name = name;
            this.Rotation = Quaternion.Identity;
        }
    }
}
