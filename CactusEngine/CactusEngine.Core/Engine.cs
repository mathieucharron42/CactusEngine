using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Core
{
    class Engine
    {
		public void SetupTask<TaskType>()
			where TaskType : ITask, new()
		{
			TaskType task = new TaskType();
			_task.Add(task);
		}

		private List<ITask> _task;

		private void Tick()
		{
			foreach(ITask task in _task)
			{
				task.Execute(this);
			}
		}
	}
}
