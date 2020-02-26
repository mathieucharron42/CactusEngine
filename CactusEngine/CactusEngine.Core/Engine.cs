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
			_task = new List<ITask>();
			_subSystems = new Dictionary<Type, ISubSystem>();
			_clock = new Clock();
		}

		public void SetupTask<TaskType>()
			where TaskType : ITask, new()
		{
			TaskType task = new TaskType();
			_task.Add(task);
		}

		public ISubSystem StartSubSystem<T>(Action<T> preInitializationFunction = null)
			where T : ISubSystem, new()
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
			where T : ISubSystem
		{
			subSystem.Shutdown(this);
			_subSystems.Remove(typeof(T));
		}

		public T GetSubSystem<T>()
			where T : ISubSystem
		{
			ISubSystem subSystem;
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
			_clock.Restart();
			while (!_stopRequested)
			{
				Tick();
			}
		}

		private bool _stopRequested;
		private List<ITask> _task;
		private Dictionary<Type, ISubSystem> _subSystems;
		private Clock _clock;

		private void Tick()
		{
			Time elapsed = _clock.Restart();
			foreach (ITask task in _task)
			{
				task.Execute(this, elapsed);
			}
		}
	}
}
