using SFML.System;
using System;

namespace CactusEngine.SMFLUtilities
{
    public static class VectorExtensions
    {
        public static Vector2f Divide(this Vector2f vector1, Vector2f vector2)
        {
            return new Vector2f(vector1.X / vector2.X, vector1.Y / vector2.Y);
        }
    }
}
