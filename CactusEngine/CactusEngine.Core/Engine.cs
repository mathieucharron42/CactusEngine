using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;


namespace CactusEngine.Core
{
	public class Engine
    {
		public Engine()
		{
			_stopRequested = false;
			_subSystems = new Dictionary<Type, SubSystem>();
			_clock = new Clock();
		}

		public T StartSubSystem<T>(Action<T> preInitializationFunction = null)
			where T : SubSystem, new()
		{
			T subSystem = new T();
			if (preInitializationFunction != null)
			{
				preInitializationFunction(subSystem);
			}
			subSystem.Initialize(this);
			_subSystems[typeof(T)] = subSystem;
			return subSystem;
		}

		public void StopSubSystem<T>(T subSystem)
			where T : SubSystem
		{
			subSystem.Shutdown(this);
			_subSystems.Remove(typeof(T));
		}

		public T Get<T>()
			where T : SubSystem
		{
			SubSystem subSystem;
			if (_subSystems.TryGetValue(typeof(T), out subSystem))
			{
				return (T)subSystem;
			}
			else
			{
				return default(T);
			}
		}

		public void Run()
		{
			while (!_stopRequested)
			{
				Tick();
			}
		}

		public void Stop()
		{
			_stopRequested = true;
		}

		private void Tick()
		{
			Time elapsed = _clock.Restart();
			foreach (KeyValuePair<Type, SubSystem> subSystemEntry in _subSystems)
			{
				SubSystem subSystem = subSystemEntry.Value;
				subSystem.Tick(this, elapsed);
			}
		}

		private bool _stopRequested;
		private Dictionary<Type, SubSystem> _subSystems;
		private Clock _clock;
	}
}
