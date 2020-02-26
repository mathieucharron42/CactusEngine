using CactusEngine.Core;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Rendering
{
    public class RenderingSubSystem : ISubSystem
    {
        public void Initialize(Engine engine)
        {
            _window = new Window(VideoMode.DesktopMode, "test");
        }

        public void Shutdown(Engine engine)
        {
            
        }

        private Window _window;
    }
}
