using CactusEngine.Core;
using CactusEngine.Rendering;
using CactusEngine.SMFLUtilities;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace CactusEngine.Window
{
    public class WindowSubSystem : SubSystem
    {
        public Vector2u WindowSize
        {
            get { return _windowSize; }
            set { _windowSize = value; }
        }

        public override void Initialize(Engine engine)
        {
            _engine = engine;
            _window = new RenderWindow(new VideoMode(_windowSize.X, _windowSize.Y), "Test");
            _window.Closed += OnWindowClosed;
            _eventTaskHandle = engine.Get<TaskSubSystem>().Add(DispatchEventTask);
            _updateTextureTaskHandle = engine.Get<TaskSubSystem>().Add(DrawOnWindowTask);
        }

        public override void Shutdown(Engine engine)
        {
            engine.Get<TaskSubSystem>().Remove(_eventTaskHandle);
            engine.Get<TaskSubSystem>().Remove(_updateTextureTaskHandle);
        }

        private void DispatchEventTask(Engine engine, Time elapsed)
        {
            _window.DispatchEvents();
        }

        private void DrawOnWindowTask(Engine engine, Time elapsed)
        {
            RenderingSubSystem rendering = engine.Get<RenderingSubSystem>();
            _window.Clear();
            Sprite sprite = new Sprite(rendering.RenderTarget.Texture);
            if (sprite.Texture.Size != _window.Size)
            {
                Vector2f windowSize = (Vector2f)_window.Size;
                Vector2f spriteSize = (Vector2f)sprite.Texture.Size;
                sprite.Scale = windowSize.Divide(spriteSize);
            }
            _window.Draw(sprite);
            _window.Display();
        }

        private void OnWindowClosed(object sender, EventArgs e)
        {
            _window.Close();
            _engine.Stop();
        }

        private Engine _engine;
        private RenderWindow _window;
        private int _eventTaskHandle;
        private int _updateTextureTaskHandle;
        private Vector2u _windowSize;
    }
}
