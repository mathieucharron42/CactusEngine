using Microsoft.VisualStudio.TestTools.UnitTesting;
using CactusEngine.Core;
using System;
using System.Collections.Generic;
using System.Text;
using SFML.System;

namespace CactusEngine.Core.Tests
{
    [TestClass()]
    public class TaskSubSystemTests
    {
        [TestMethod()]
        public void ConstructionTest()
        {
            TaskSubSystem subSystem = new TaskSubSystem();
            Assert.IsNotNull(subSystem);
            Assert.AreEqual(0, subSystem.GetTaskCount());
        }

        [TestMethod()]
        public void AddTaskTest()
        { 
            TaskSubSystem subSystem = new TaskSubSystem();
            {
                int handle1 = subSystem.Add((Engine engine, Time elapsed) => { });
                Assert.AreEqual(1, subSystem.GetTaskCount());
                Assert.IsTrue(subSystem.Exist(handle1));
            }
            {
                int handle2 = subSystem.Add((Engine engine, Time elapsed) => { });
                Assert.AreEqual(2, subSystem.GetTaskCount());
                Assert.IsTrue(subSystem.Exist(handle2));
            }
        }

        [TestMethod()]
        public void RemoveTaskTest()
        {
            TaskSubSystem subSystem = new TaskSubSystem();
            {
                int handle1 = subSystem.Add((Engine engine, Time elapsed) => { });
                subSystem.Remove(handle1);
                Assert.AreEqual(0, subSystem.GetTaskCount());
                Assert.IsFalse(subSystem.Exist(handle1));
            }
            {
                int handle1 = subSystem.Add((Engine engine, Time elapsed) => { });
                int handle2 = subSystem.Add((Engine engine, Time elapsed) => { });
                subSystem.Remove(handle1);
                Assert.AreEqual(1, subSystem.GetTaskCount());
                Assert.IsFalse(subSystem.Exist(handle1));
                Assert.IsTrue(subSystem.Exist(handle2));
            }
        }

        [TestMethod()]
        public void TickTest()
        {
            bool hasTicked1 = false;
            bool hasTicked2 = false;
            bool hasTicked3 = false;
            bool controlTick = false;

            TaskSubSystem subSystem = new TaskSubSystem();

            int handle1 = subSystem.Add((Engine engine, Time elapsed) => { hasTicked1 = true; });
            int handle2 = subSystem.Add((Engine engine, Time elapsed) => { hasTicked2 = true; });
            int handle3 = subSystem.Add((Engine engine, Time elapsed) => { hasTicked3 = true; });

            subSystem.Remove(handle2);

            Time time = new Time();
            subSystem.Tick(null, time);

            Assert.IsTrue(hasTicked1);
            Assert.IsFalse(hasTicked2);
            Assert.IsTrue(hasTicked3);
            Assert.IsFalse(controlTick);
        }

    }
}