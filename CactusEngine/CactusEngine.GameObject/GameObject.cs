using CactusEngine.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Object
{
    public class GameObject
    {
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public GameObject()
        {

        }

        public virtual void Initialize(Engine engine) { }

        public virtual void Shutdown(Engine engine) { }

        private int _id;
    }
}
