using CactusEngine.Core;
using CactusEngine.Object;
using CactusEngine.Rendering;
using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    class DummyGameObject : GameObject, IRenderable
    {
        public int Size { get; set; }
        public void Render(Engine engine, Renderer renderer)
        {
            renderer.DrawCircle(Size, Color.Red);
        }
    }
}
