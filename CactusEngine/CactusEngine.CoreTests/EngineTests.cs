using Microsoft.VisualStudio.TestTools.UnitTesting;
using CactusEngine.Core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SFML.System;

namespace CactusEngine.CoreTests
{
    [TestClass()]
    public class EngineTests
    {
        class TestSubSystem1 : SubSystem
        {
            public bool HasInitialized { get; private set; }

            public bool HasShutdowned { get; private set; }

            public bool HasTicked { get; private set; }

            public int SomeMember { get; set; }

            public override void Initialize(Engine engine)
            {
                HasInitialized = true;
            }

            public override void Shutdown(Engine engine)
            {
                HasShutdowned = true;
            }

            public override void Tick(Engine engine, Time elapsed)
            {
                HasTicked = true;
            }
        }

        [TestMethod()]
        public void ConstructionTest()
        {
            Engine engine = new Engine();
            Assert.IsNull(engine.Get<TestSubSystem1>());
        }


        [TestMethod()]
        public void EngineSubSystemTest()
        {
            Engine engine = new Engine();
            TestSubSystem1 subSystem1 = engine.StartSubSystem<TestSubSystem1>((TestSubSystem1 subSystem) =>
            {
                subSystem.SomeMember = 42;
            });
            Assert.IsTrue(subSystem1.HasInitialized);
            Assert.IsFalse(subSystem1.HasShutdowned);
            Assert.IsFalse(subSystem1.HasTicked);
            Assert.AreEqual(42, subSystem1.SomeMember);

            // engine.Run();
        }
    }
}
