using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
//using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameEngine
{
    public class Engine
    {
		[DllImport("user32.dll")]
		public static extern int PeekMessage(out NativeMessage message, IntPtr window, uint filterMin, uint filterMax, uint remove);

		[StructLayout(LayoutKind.Sequential)]
		public struct NativeMessage
		{
			public IntPtr Handle;
			public uint Message;
			public IntPtr WParameter;
			public IntPtr LParameter;
			public uint Time;
			public Point Location;
		}

		public Engine()
		{
			_fpsHistory = new TimeSpan[FPS_HISTORY_SIZE];
			_root = new GameObject();
			_gameObjects = new List<GameObject>();
			_subSystems = new List<SubSystem>();
		}

		public GameObject Root
		{
			get { return _root; }
		}

		public void Initialize()
		{
			//Application.SetCompatibleTextRenderingDefault(false);
			_updateTimer = Stopwatch.StartNew();
			_form = new GameForm();
			_viewport = new Viewport(_form);
			_viewport.OnMouseClick += OnViewportMouseClick;
			_viewport.OnPaint += OnViewportPaint;
		}

		public void Shutdown()
		{
			_viewport.OnMouseClick -= OnViewportMouseClick;
			_viewport.OnPaint -= OnViewportPaint;
			_viewport = null;
			_form = null;

			_gameObjects.ForEach(DestroyGameObject);
			_gameObjects.Clear();

			_subSystems.ForEach(StopSubSystem);
			_subSystems.Clear();
		}

		public void Run()
		{
			Application.Idle += HandleApplicationIdle;
			Application.Run(_form);
		}

		private const int FPS_HISTORY_SIZE = 1000;

		private void OnViewportMouseClick(MouseEventArgs e)
		{
			Vector2 worldPosition = new Vector2(e.X, e.Y);
			
			List<GameObject> zSortedGameObject = new List<GameObject>();
			_root.Traverse((GameObject gameObject) => {
				zSortedGameObject.Add(gameObject);
			});
			zSortedGameObject = zSortedGameObject.OrderBy(x => x.WorldTransform.ZOrder).ToList();

			foreach (GameObject gameObject in zSortedGameObject)
			{
				if(IsWorldPositionInGameObject(worldPosition, gameObject))
				{
					Vector2 localPosition = (worldPosition - gameObject.WorldTransform.Position);
					gameObject.Click(this, localPosition);
				}
			}
		}

		private bool IsWorldPositionInGameObject(Vector2 worldPosition, GameObject gameObject)
		{
			Vector2 topLeft = gameObject.WorldTransform.Position;
			Vector2 bottomRight = gameObject.WorldTransform.Position + gameObject.WorldTransform.Size;
			
			return worldPosition.X >= topLeft.X 
				&& worldPosition.X <= bottomRight.X
				&& worldPosition.Y >= topLeft.Y
				&& worldPosition.Y <= bottomRight.Y;
		}

		private void OnViewportPaint(PaintEventArgs e)
		{
			using (Renderer renderer = new Renderer(GetViewport(), e.Graphics))
			{
				renderer.Clear();

				_root.Traverse((GameObject gameObject) =>
				{
					gameObject.Render(this, renderer);
				});

				if (_fpsCount > 0)
				{
					int index = (_fpsCount -1) % FPS_HISTORY_SIZE;
					double fps = TimestampToFPS(_fpsHistory[index]);
					double averageFps = _fpsHistory.Take(index+1).Average(x => TimestampToFPS(x));
					double worstFps = _fpsHistory.Min(x => TimestampToFPS(x));
					string fpsStr = string.Format("fps:{0:0} avg:{1:0} worst:{2:0}", fps.ToString("000"), averageFps.ToString("000"), worstFps.ToString("000"));
					renderer.RenderString(Vector2.Zero, GetViewport().Size, fpsStr, 20, Color.White, Renderer.StringAlignment.TopLeft);
				}
			}
		}

		private float TimestampToFPS(TimeSpan timestamp)
		{
			return 1000 / (float)timestamp.TotalMilliseconds;
		}

		private void HandleApplicationIdle(object sender, EventArgs e)
		{
			while (IsApplicationIdle())
			{
				Tick();
			}
		}

		private void Tick()
		{
			TimeSpan storedNow = Now();
			TimeSpan dt = storedNow - _lastUpdate;
			_fpsHistory[_fpsCount++ % FPS_HISTORY_SIZE] = dt;
			UpdateWorldTransform(dt);
			Update(dt);
			Render(dt);
			_lastUpdate = storedNow;
		}

		public TimeSpan Now()
		{
			return _updateTimer.Elapsed;
		}

		public Viewport GetViewport()
		{
			return _viewport;
		}

		private void UpdateWorldTransform(TimeSpan dt)
		{
			_root.Traverse((GameObject gameObject, Transform parent) => 
			{
				gameObject.ComputeWorldTransform(parent);
				return gameObject.WorldTransform;
			}, Transform.Origin);
		}

		private void Update(TimeSpan dt)
		{
			_root.Traverse((GameObject gameObject) =>
			{
				gameObject.Update(this, dt);
			});
		}

		void Render(TimeSpan dt)
		{
			// Actual render will occurs in OnViewportPaint.
			// Since we use Windows Form, we must let paint
			// occurs at its own time.
			GetViewport().Invalidate();
		}

		public T CreateGameObject<T>(Action<T> preInitializationFunction = null) 
			where T : GameObject, new()
		{
			T gameObject = new T();
			if (preInitializationFunction != null)
			{
				preInitializationFunction(gameObject);
			}
			gameObject.Initialize(this);
			_gameObjects.Add(gameObject);
			return gameObject;
		}

		public void DestroyGameObject<T>(T gameObject)
			where T : GameObject, new()
		{
			gameObject.Shutdown(this);
			_root.Traverse((GameObject go) =>
			{
				if(go.HasChild(gameObject))
				{
					go.RemoveChild(gameObject);
				}
			});
			_gameObjects.Remove(gameObject);
		}

		public SubSystem StartSubSystem<T>(Action<T> preInitializationFunction = null)
			where T : SubSystem, new()
		{
			T subSystem = new T();
			if (preInitializationFunction != null)
			{
				preInitializationFunction(subSystem);
			}
			subSystem.Initialize(this);
			_subSystems.Add(subSystem);
			return subSystem;
		}

		public void StopSubSystem<T>(T subSystem)
			where T : SubSystem
		{
			subSystem.Shutdown(this);
			_subSystems.Remove(subSystem);
		}

		public T GetSubSystem<T>()
			where T : SubSystem
		{
			return (T)_subSystems.FirstOrDefault((SubSystem subsystem) =>
			{
				return subsystem is T;
			});
		}

		public Texture CreateTexture(string path)
		{
			Texture texture = new Texture(path);
			texture.Initialize();
			return texture;
		}

		public void ReleaseTexture(ref Texture texture)
		{
			texture.Shutdown();
			texture = null;
		}

		private bool IsApplicationIdle()
		{
			NativeMessage result;
			return PeekMessage(out result, IntPtr.Zero, (uint)0, (uint)0, (uint)0) == 0;
		}

		private GameForm _form;
		private Viewport _viewport;
		private Stopwatch _updateTimer;
		private TimeSpan _lastUpdate;
		private GameObject _root;
		private List<GameObject> _gameObjects;
		private List<SubSystem> _subSystems;
		private int _fpsCount;
		private TimeSpan[] _fpsHistory;
	}
}
