using SFML.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Rendering
{
    class Renderer
    {
        public Renderer(RenderWindow target)
        {

        }

        public void Clear()
        {
            _graphics.Clear(Color.Green);
        }
    }
}
