using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Core
{
    public interface ISubSystem
    {
        void Initialize(Engine engine);
        void Shutdown(Engine engine);
    }
}
