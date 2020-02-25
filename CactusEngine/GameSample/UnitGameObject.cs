using GameEngine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace TestForms
{
    class UnitGameObject : GameObject
    {
        public UnitGameObject()
        {
            _selected = false;
        }

        public string SpritePath
        {
            get { return _spritePath; }
            set { _spritePath = value; }
        }

        public string AnimatedSpritePattern
        {
            get { return _animatedSpritePattern; }
            set { _animatedSpritePattern = value; }
        }

        public int AnimatedSpriteCount
        {
            get { return _animatedSpriteCount; }
            set { _animatedSpriteCount = value; }
        }

        public TimeSpan AnimatedSpriteTotalTime
        {
            get { return _animatedSpriteTotalTime; }
            set { _animatedSpriteTotalTime = value; }
        }

        public bool Selected
        {
            get { return _selected; }
            set 
            { 
                _selected = value;
            }
        }

        public override void Initialize(Engine engine)
        {
            _texture = engine.CreateTexture(_spritePath);
            _animatedSpriteTextures = new List<Texture>();
            for (int i = 0; i < _animatedSpriteCount; ++i)
            {
                string path = string.Format(_animatedSpritePattern, i);
                _animatedSpriteTextures.Add(engine.CreateTexture(path));
            }
        }

        public override void Shutdown(Engine engine)
        {
            engine.ReleaseTexture(ref _texture);
        }

        public override void Render(Engine engine, Renderer renderer)
        {
            if(_selected)
            {
                renderer.RenderRectangle(WorldTransform, Color.Yellow);
            }
            renderer.RenderTexture(_texture, WorldTransform);
            renderer.RenderString(WorldTransform, WorldTransform.Angle.ToString(), 20, Color.Red, Renderer.StringAlignment.Centered);
        }

        public override void Update(Engine engine, TimeSpan timespan)
        {
            UpdateAnimatedSprite(engine);

            //Transform transform = LocalTransform;
            //transform.Angle = ((transform.Angle - (float)timespan.TotalMilliseconds * 0.01f) % (2 * (float)Math.PI));
            //LocalTransform = transform;
        }

        public void UpdateAnimatedSprite(Engine engine)
        {
            if (_animatedSpriteTextures.Count > 0)
            {
                TimeSpan now = engine.Now();
                if (now >= _animatedSpriteNextUpdate)
                {
                    _texture = _animatedSpriteTextures[_animatedSpriteIndex];
                    _animatedSpriteIndex = (_animatedSpriteIndex+1) % _animatedSpriteCount;
                    _animatedSpriteNextUpdate = now + ComputeAnimatedSpriteChangeFrequency();
                }
            }
        }

        public TimeSpan ComputeAnimatedSpriteChangeFrequency()
        {
            return new TimeSpan(_animatedSpriteTotalTime.Ticks / _animatedSpriteCount);
        }

        private string _spritePath;
        private Texture _texture;
        private string _animatedSpritePattern;
        private int _animatedSpriteCount;
        private List<Texture> _animatedSpriteTextures;
        private int _animatedSpriteIndex;
        private TimeSpan _animatedSpriteTotalTime;
        private TimeSpan _animatedSpriteNextUpdate;
        private bool _selected;
    }
}
