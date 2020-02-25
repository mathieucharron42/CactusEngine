using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
	static class MathHelper
    {
		public static Vector2 Rotate(Vector2 point, Vector2 origin, double angle)
		{
			float cos_angle = (float)Math.Cos(angle);
			float sin_angle = (float)Math.Sin(angle);

			Vector2 translatedPoint = point - origin;

			Vector2 translatedRotatedPoint = new Vector2(
				translatedPoint.X * cos_angle - translatedPoint.Y * sin_angle,
				translatedPoint.X * sin_angle + translatedPoint.Y * cos_angle
			);

			Vector2 pointRotated = translatedRotatedPoint + origin;

			return pointRotated;
		}

		public static float Dot(Vector2 vector1, Vector2 vector2)
		{
			return vector1.X * vector2.X + vector1.Y * vector2.Y;
		}

		public static float ScalarProjection(Vector2 vector, Vector2 onto)
		{
			return Dot(vector, onto) / onto.Length();
		}

		public static Vector2 Projection(Vector2 vector, Vector2 onto)
		{
			return onto * ScalarProjection(vector, onto);
		}

		public static float ToDegree(float radAngle)
		{
			return radAngle * (180 / (float)Math.PI);
		}
	}
}
