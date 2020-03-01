using CactusEngine.Core;
using CactusEngine.Object;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Rendering
{
    public class RenderingSubSystem : SubSystem
    {
        public RenderTexture RenderTarget
        {
            get
            {
                return _renderTexture;
            }
        }

        public Vector2u RenderSize
        {
            get { return _renderSize; }
            set { _renderSize = value; }
        }

        public Vector2f ViewportSize
        {
            get { return _viewportSize; }
            set { _viewportSize = value; }
        }

        public Vector2f ViewportPosition
        {
            get { return _viewportPosition; }
            set { _viewportPosition = value; }
        }

        public override void Initialize(Engine engine)
        {
            _renderTexture = new RenderTexture(RenderSize.X, RenderSize.Y);
            _renderingTaskHandle = engine.Get<TaskSubSystem>().Add(Render);
        }

        public override void Shutdown(Engine engine)
        {
            engine.Get<TaskSubSystem>().Remove(_renderingTaskHandle);
        }

        private void Render(Engine engine, Time elapsed)
        {
            View view = new View(ViewportPosition + ViewportSize / 2, ViewportSize);
            Renderer renderer = new Renderer(_renderTexture, view);
            renderer.Begin();
            foreach (IRenderable renderable in engine.Get<GameObjectSubSystem>().GetAll<IRenderable>())
            {
                renderable.Render(engine, renderer);
            }
            renderer.End();
        }

        private RenderTexture _renderTexture;

        private Vector2f _viewportSize;
        private Vector2f _viewportPosition;

        private int _renderingTaskHandle;
        private Vector2u _renderSize;
    }
}
