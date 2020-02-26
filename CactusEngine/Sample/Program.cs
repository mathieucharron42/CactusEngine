using CactusEngine.Core;
using CactusEngine.Rendering;
using System;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine engine = new Engine();

            engine.SetupTask<RenderingTask>();

            engine.StartSubSystem<RenderingSubSystem>();

            engine.Run();
        }
    }
}
