using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Core
{
	using StatelessTask = Action<Engine, Time>;

	public class TaskSubSystem : SubSystem
    {
		public TaskSubSystem()
		{
			_tasks = new List<Tuple<int, StatelessTask>>();
			_nextTaskHandle = 1;
		}

		public override void Initialize(Engine engine)
		{

		}

		public override void Shutdown(Engine engine)
		{
			_tasks.Clear();
		}

		public override void Tick(Engine engine, Time elapsed)
		{
			foreach (Tuple<int, StatelessTask> entry in _tasks)
			{
				StatelessTask task = entry.Item2;
				task(engine, elapsed);
			}
		}

		public int Add(StatelessTask task)
		{
			int taskHandle = _nextTaskHandle++;
			Tuple<int, StatelessTask> entry = Tuple.Create(taskHandle, task);
			_tasks.Add(entry);
			return taskHandle;
		}

		public void Remove(int taskHandle)
		{
			_tasks.RemoveAll((Tuple<int, StatelessTask> entry) =>
			{
				return entry.Item1 == taskHandle;
			});
		}

		public bool Exist(int taskHandle)
		{
			return _tasks.Exists((Tuple<int, StatelessTask> entry) =>
			{
				return entry.Item1 == taskHandle;
			});
		}

		public int GetTaskCount()
		{
			return _tasks.Count;
		}

		private List<Tuple<int, StatelessTask>> _tasks;
		private int _nextTaskHandle;
		
	}
}
