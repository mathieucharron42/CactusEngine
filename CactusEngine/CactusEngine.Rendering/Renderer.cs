using CactusEngine.Core;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Rendering
{
    public class Renderer
    {
        public Renderer(RenderTexture target, View view)
        {
            _target = target;
            _target.SetView(view);
        }

        public void Begin()
        {
            _target.Clear(Color.Green);
        }

        public void End()
        {
            _target.Display();
        }

        public void DrawCircle(float size, Color color)
        {
            CircleShape shape = new CircleShape(size);
            shape.FillColor = color;
            DrawShape(shape);
        }

        public void DrawShape(Shape shape)
        {
            _target.Draw(shape);
        }

        private RenderTexture _target;
    }
}
