using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngine
{
    class QuaternionEngine
    {

        public static Matrix rotationMatrix(Quaternion q)
        {
            SharpDX.Matrix matrix = new SharpDX.Matrix();
            // This is the arithmetical formula optimized to work with unit quaternions.
            // |1-2y²-2z²        2xy-2zw         2xz+2yw       0|
            // | 2xy+2zw        1-2x²-2z²        2yz-2xw       0|
            // | 2xz-2yw         2yz+2xw        1-2x²-2y²      0|
            // |    0               0               0          1|

            // And this is the code.
            // First Column
            matrix[0] = 1 - 2 * (q.Y * q.Y + q.Z * q.Z);
            matrix[1] = 2 * (q.X * q.Y + q.Z * q.W);
            matrix[2] = 2 * (q.X * q.Z - q.Y * q.W);
            matrix[3] = 0;

            // Second Column
            matrix[4] = 2 * (q.X * q.Y - q.Z * q.W);
            matrix[5] = 1 - 2 * (q.X * q.X + q.Z * q.Z);
            matrix[6] = 2 * (q.Y * q.Z + q.X * q.W);
            matrix[7] = 0;

            // Third Column
            matrix[8] = 2 * (q.X * q.Z + q.Y * q.W);
            matrix[9] = 2 * (q.Y * q.Z - q.X * q.W);
            matrix[10] = 1 - 2 * (q.X * q.X + q.Y * q.Y);
            matrix[11] = 0;

            // Fourth Column
            matrix[12] = 0;
            matrix[13] = 0;
            matrix[14] = 0;
            matrix[15] = 1;
            return matrix;
        }

        public static Quaternion obtainRotationQuaternion(Vector3 vec, float angle)
        {
            // The new quaternion variable.
            Quaternion q = new Quaternion();

            // Converts the angle in degrees to radians.
            double radians = (angle*Math.PI)/360;

            // Finds the Sin and Cosin for the half angle.
            float sin = (float)Math.Sin(radians);
            float cos = (float)Math.Cos(radians);

            // Formula to construct a new Quaternion based on direction and angle.
            q.W = cos;
            q.X = vec.X * sin;
            q.Y = vec.Y * sin;
            q.Z = vec.Z * sin;

            return q;
        }
    }
}
