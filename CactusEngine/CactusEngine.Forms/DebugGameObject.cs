using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class DebugGameObject : GameObject
    {
        public override void Initialize(Engine engine)
        {
        }

        public override void Shutdown(Engine engine)
        {
        }

        public override void Update(Engine engine, TimeSpan timespan)
        {
        }

        public override void Render(Engine engine, Renderer renderer)
        {
            Vector2 from = new Vector2(0, 0);
            Vector2 size = new Vector2(200);
            Vector2 to = from + size - Vector2.One;

            Vector2 from2 = new Vector2(from.X, to.Y);
            Vector2 to2 = new Vector2(to.X, from.Y);

            renderer.RenderRectangle(from, to, Color.Blue);
            renderer.RenderTexture(engine.CreateTexture("Assets/test.png"), from, to);
            renderer.RenderLine(from2, to2, 1, Color.Green);
            renderer.RenderLine(from, to, 1, Color.Green);
            renderer.RenderTexture(engine.CreateTexture("Assets/character.png"), from, to);
            renderer.RenderString(from, to, "D", 100, Color.WhiteSmoke, Renderer.StringAlignment.Centered);
        }
    }
}
