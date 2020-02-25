using GameEngine;
using System;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace TestForms
{
	class BouncyGameObject : GameObject
    {
		float _speed;
		Texture _sprite;

		public float Speed
		{
			get { return _speed; }
			set { _speed = value; }
		}

		public BouncyGameObject()
		{
			_speed = 1f;
		}

		public override void Initialize(Engine engine)
		{
			_sprite = engine.CreateTexture(null); 
		}

		public override void Shutdown(Engine engine)
		{
			_sprite = null;
		}

		public override void Update(Engine engine, TimeSpan dt)
		{
			Vector2 viewportSize = engine.GetViewport().Size;

			Vector2? intersectionNormal = GetIntersectionNormal(viewportSize);

			//Transform newTransform = new Transform();
			//if (intersectionNormal != null)
			//{
			//	newTransform.Orientation = WorldTransform.Orientation - (2 * Vector2.Dot(WorldTransform.Orientation, intersectionNormal.Value) * intersectionNormal.Value);
			//}

			//newTransform.Position += (float)dt.TotalMilliseconds * Speed * WorldTransform.Orientation;
			//newTransform.Position = Vector2.Clamp(WorldTransform.Position, new Vector2(0, 0), viewportSize - WorldTransform.Size);
			//LocalTransform = newTransform;
		}

		public override void Render(Engine engine, Renderer renderer)
		{
			renderer.RenderTexture(_sprite, WorldTransform.Position, WorldTransform.Position + WorldTransform.Size);
		}

		Vector2? GetIntersectionNormal(Vector2 viewportSize)
		{
			Vector2 topLeft = WorldTransform.Position;
			Vector2 buttomRight = WorldTransform.Position + WorldTransform.Size;

			if (topLeft.X <= 0)
			{
				return new Vector2(1, 0);
			}
			else if (topLeft.Y <= 0)
			{
				return new Vector2(0, 1);
			}
			else if (buttomRight.X >= viewportSize.X)
			{
				return new Vector2(-1, 0);
			}
			else if (buttomRight.Y >= viewportSize.Y)
			{
				return new Vector2(0, -1);
			}
			return null;
		}
	}
}
