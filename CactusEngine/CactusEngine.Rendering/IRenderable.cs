using CactusEngine.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Rendering
{
    public interface IRenderable
    {
        void Render(Engine engine, Renderer renderer);
    }
}
