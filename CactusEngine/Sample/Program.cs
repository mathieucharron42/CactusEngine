using CactusEngine.Core;
using CactusEngine.Object;
using CactusEngine.Rendering;
using CactusEngine.Window;
using SFML.System;
using SFML.Window;
using System;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            Vector2u kRenderSize = new Vector2u(500, 500);

            Engine engine = new Engine();

            TaskSubSystem taskSubsystem = engine.StartSubSystem<TaskSubSystem>();
            GameObjectSubSystem gameObjectSubsystem = engine.StartSubSystem<GameObjectSubSystem>();

            RenderingSubSystem renderingSubSystem = engine.StartSubSystem<RenderingSubSystem>(renderingSubSystem =>
            {
                renderingSubSystem.RenderSize = kRenderSize;
                renderingSubSystem.ViewportSize = new Vector2f(100, 100);
                renderingSubSystem.ViewportPosition = new Vector2f(0, 0);
            });
            WindowSubSystem windowSubSystem = engine.StartSubSystem<WindowSubSystem>(windowSubSystem =>
            {
                windowSubSystem.WindowSize = kRenderSize;
            });

            taskSubsystem.Add(UpdateViewportTask);

            gameObjectSubsystem.Create<DummyGameObject>(gameObject =>
            {
                gameObject.Size = 50;
            });
            gameObjectSubsystem.Create<DummyGameObject2>();

            engine.Run();
        }

        static void UpdateViewportTask(Engine engine, Time elapsed)
        {
            RenderingSubSystem renderingSubSystem = engine.Get<RenderingSubSystem>();

            const float speed = 50f;
            float offset = elapsed.AsSeconds() * speed;

            if (Keyboard.IsKeyPressed(Keyboard.Key.A))
            {
                renderingSubSystem.ViewportPosition -= new Vector2f(offset, 0);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.W))
            {
                renderingSubSystem.ViewportPosition -= new Vector2f(0, offset);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.D))
            {
                renderingSubSystem.ViewportPosition += new Vector2f(offset, 0);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.S))
            {
                renderingSubSystem.ViewportPosition += new Vector2f(0, offset);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.PageDown))
            {
                renderingSubSystem.ViewportSize -= new Vector2f(offset, offset);
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.PageUp))
            {
                renderingSubSystem.ViewportSize += new Vector2f(offset, offset);
            }
            
            //if (Keyboard.IsKeyPressed(Keyboard.Key.I))
            //{
            //    Console.WriteLine("Center: {0}", view.Center);
            //    Console.WriteLine("Size: {0}", view.Size);
            //}
        }
    }
}
