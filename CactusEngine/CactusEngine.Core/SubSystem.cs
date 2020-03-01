using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Core
{
    public class SubSystem
    {
        public virtual void Initialize(Engine engine)
        {

        }
        public virtual void Shutdown(Engine engine)
        {

        }
        public virtual void Tick(Engine engine, Time elapsed)
        {

        }
    }
}
