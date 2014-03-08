using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SharpDX;
using Windows.UI.Xaml.Media.Imaging;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;

namespace SoftEngine
{
    class Device
    {
        private byte[] backBuffer;
        private WriteableBitmap wbm;

        public Device(WriteableBitmap wbm)
        {
            this.wbm = wbm;
            this.backBuffer = new byte[this.wbm.PixelHeight * this.wbm.PixelWidth * 4];
        }

        public void Clear(byte r, byte g, byte b, byte a)
        {
            for (var index = 0; index < backBuffer.Length; index += 4)
            {
                // BGRA is used by Windows instead of RGBA
                backBuffer[index] = b;
                backBuffer[index + 1] = g;
                backBuffer[index + 2] = r;
                backBuffer[index + 3] = a;
            }
        }

        public void Present()
        {
            //the method in the tutorial doesn't exist, test this!
            using (Stream stream = this.wbm.PixelBuffer.AsStream())
            {
                // writing our back buffer into our WriteableBitmap stream
                stream.Write(this.backBuffer, 0, backBuffer.Length);
            }
            // request a redraw of the entire bitmap
            this.wbm.Invalidate();
        }

        // Called to put a pixel on screen at a specific X,Y coordinates
        public void PutPixel(int x, int y, Color4 color)
        {
            int index = (x + y * this.wbm.PixelWidth) * 4;
            backBuffer[index] = (byte)(color.Blue * 255);
            backBuffer[index + 1] = (byte)(color.Green * 255);
            backBuffer[index + 2] = (byte)(color.Red * 255);
            backBuffer[index + 3] = (byte)(color.Alpha * 255);
        }

        //Transforms the 3D coordinates into 2D and then changes the reference system using (0,0) as the center of the screen instead
        //of the top left corner
        public Vector2 Project(Vector3 coord, Matrix transMat)
        {
            // transforming the coordinates
            var point = Vector3.TransformCoordinate(coord, transMat);
            // The transformed coordinates will be based on coordinate system
            // starting on the center of the screen. But drawing on screen normally starts
            // from top left. We then need to transform them again to have x:0, y:0 on top left.
            var x = point.X * this.wbm.PixelWidth + this.wbm.PixelWidth / 2.0f;
            var y = -point.Y * this.wbm.PixelHeight + this.wbm.PixelHeight / 2.0f;
            return (new Vector2(x, y));
        }

        // DrawPoint calls PutPixel but does the clipping operation before (eliminates what cannot be be seen on screen)
        public void DrawPoint(Vector2 point)
        {
            // Clipping what's visible on screen
            if (point.X >= 0 && point.Y >= 0 && point.X < this.wbm.PixelWidth && point.Y < this.wbm.PixelHeight)
            {
                // Drawing a yellow point
                PutPixel((int)point.X, (int)point.Y, new Color4(1.0f, 1.0f, 1.0f, 1.0f));
            }
        }
        // The main method of the engine that re-compute each vertex projection
        // during each frame
        public void Render(Camera camera, Mesh mesh)
        {
            //First, we create the camera matrix. 
            var viewMatrix = Matrix.LookAtLH(camera.Position, camera.Target, Vector3.UnitY);
            //Then, the projection matrix to add some perspective
            var projectionMatrix = Matrix.PerspectiveFovRH(0.78f,
                                                           (float)this.wbm.PixelWidth / this.wbm.PixelHeight,
                                                           0.01f, 1.0f);

            //Then the worldmatrix. Beware to apply rotation before translation 
            //var worldMatrix = Matrix.RotationYawPitchRoll(mesh.Rotation.Y,
            //                                              mesh.Rotation.X, mesh.Rotation.Z) *
            //                  Matrix.Translation(mesh.Position);

            //Let's play with quaternions!
            var worldMatrix = QuaternionEngine.rotationMatrix(mesh.Rotation) *
                              Matrix.Translation(mesh.Position);
            //Final transformMatrix, remember to multiply backwards
            var transformMatrix = worldMatrix * viewMatrix * projectionMatrix;
            // This is where magic happens: transfomed vertex are projected on the screen. First we identify each face, then transform its vertex and later we draw them
            //and the lines that join them.
            foreach (var face in mesh.Faces)
            {
                var vertexA = mesh.Vertices[face.A];
                var vertexB = mesh.Vertices[face.B];
                var vertexC = mesh.Vertices[face.C];

                var pixelA = Project(vertexA, transformMatrix);
                var pixelB = Project(vertexB, transformMatrix);
                var pixelC = Project(vertexC, transformMatrix);

                DrawLine(pixelA, pixelB);
                DrawLine(pixelB, pixelC);
                DrawLine(pixelC, pixelA);
            }


        }
        //Bresenham's algorythm for drawing lines. The simpler midpoint algorythm can also be used, but it's less efficient. This is a straight copy-paste, 
        //I don't know HTF it works, but it does.
        public void DrawLine(Vector2 point0, Vector2 point1)
        {
            int x0 = (int)point0.X;
            int y0 = (int)point0.Y;
            int x1 = (int)point1.X;
            int y1 = (int)point1.Y;

            var dx = Math.Abs(x1 - x0);
            var dy = Math.Abs(y1 - y0);
            var sx = (x0 < x1) ? 1 : -1;
            var sy = (y0 < y1) ? 1 : -1;
            var err = dx - dy;

            while (true)
            {
                DrawPoint(new Vector2(x0, y0));

                if ((x0 == x1) && (y0 == y1)) break;
                var e2 = 2 * err;
                if (e2 > -dy) { err -= dy; x0 += sx; }
                if (e2 < dx) { err += dx; y0 += sy; }
            }
        }
    }
}
