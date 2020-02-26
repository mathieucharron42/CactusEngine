using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Core
{
    public interface ITask
    {
        void Execute(Engine engine, Time time);
    }
}
