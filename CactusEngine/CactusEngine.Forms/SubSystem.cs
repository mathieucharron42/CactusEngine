using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class SubSystem
    {
        public virtual void Initialize(Engine engine) { }
        public virtual void Shutdown(Engine engine) { }
    }
}
