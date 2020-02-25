using System;
using System.Numerics;

namespace GameEngine
{
	public struct Transform
	{
		public static Transform Origin
		{
			get { return new Transform(Vector2.Zero, Vector2.One, 1, 0, Vector2.One/2); }
		}

		public Transform(Vector2 position, Vector2 size, int zOrder, float angle, Vector2 pivot)
		{
			_position = position;
			_size = size;
			_zOrder = zOrder;
			_angle = angle;
			_pivot = pivot;
		}

		public Vector2 Size
		{
			get { return _size; }
			set { _size = value; }
		}

		public Vector2 Position
		{
			get { return _position; }
			set { _position = value; }
		}

		public Vector2 RotationPivot
		{
			get { return _pivot; }
			set { _pivot = value; }
		}

		public float Angle
		{
			get { return _angle; }
			set { _angle = value; }
		}

		public int ZOrder
		{
			get { return _zOrder; }
			set { _zOrder = value; }
		}

		public Vector2 GetTopLeftPosition()
		{
			return MathHelper.Rotate(Position, RotationPivot, Angle);
		}

		public Vector2 GetTopRightPosition()
		{
			Vector2 topRight = Position + MathHelper.Projection(Size, Vector2.UnitX);
			return MathHelper.Rotate(topRight, RotationPivot, Angle);
		}

		public Vector2 GetBottomLeftPosition()
		{
			Vector2 bottomLeft = Position + MathHelper.Projection(Size, Vector2.UnitY);
			return MathHelper.Rotate(bottomLeft, RotationPivot, Angle);
		}

		public Vector2 GetBottomRightPosition()
		{
			Vector2 bottomRight = Position + Size;
			return MathHelper.Rotate(bottomRight, RotationPivot, Angle);
		}


		public Transform Combine(Transform other)
		{
			Transform combined = new Transform();
			combined.Position = (Position * other.Size) + other.Position;
			combined.Size = Size * other.Size;
			combined.ZOrder = ZOrder + other.ZOrder;
			combined.Angle = Angle + other.Angle;
			combined.RotationPivot = RotationPivot + other.RotationPivot;
			return combined;
		}

		private Vector2 _position;
		private float _angle;
		private Vector2 _pivot;
		private Vector2 _size;
		private int _zOrder;
	}
}
