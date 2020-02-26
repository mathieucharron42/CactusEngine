using CactusEngine.Core;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Rendering
{
    public class RenderingTask : ITask
    {
        void ITask.Execute(Engine engine, Time elapsed)
        {
            Renderer renderer = new Renderer();
            Console.WriteLine("render stuff! {0}", elapsed.AsMicroseconds());
        }
    }
}
