using Microsoft.VisualStudio.TestTools.UnitTesting;
using CactusEngine.Core;
using CactusEngine.Object;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using SFML.System;

namespace CactusEngine.CoreTests
{
    [TestClass()]
    public class TaskSubSystemTests
    {
        class TestGameObject1 : GameObject
        {
            public int SomeMember { get; set; }
        }

        class TestGameObject2 : GameObject { }

        class TestGameObject3 : TestGameObject1 { }

        [TestMethod()]
        public void ConstructionTest()
        {
            GameObjectSubSystem subSystem = new GameObjectSubSystem();
            Assert.IsNotNull(subSystem);
            Assert.AreEqual(0, subSystem.GetAll().Count);
        }

        [TestMethod()]
        public void CreateTest()
        {
            GameObjectSubSystem subSystem = new GameObjectSubSystem();

            GameObject gameObject1 = subSystem.Create<GameObject>();
            Assert.IsNotNull(gameObject1);
            TestGameObject1 gameObject2 = subSystem.Create<TestGameObject1>((TestGameObject1 gameObject) =>
            {
                gameObject.SomeMember = 42;
            });
            Assert.IsNotNull(gameObject2);
            Assert.AreEqual(gameObject2.SomeMember, 42);
        }

        [TestMethod()]
        public void DestroyTest()
        {
            GameObjectSubSystem subSystem = new GameObjectSubSystem();

            GameObject gameObject1 = subSystem.Create<GameObject>();
            GameObject gameObject2 = subSystem.Create<GameObject>();
            GameObject gameObject3 = subSystem.Create<GameObject>();
            subSystem.Destroy(gameObject2);
            Assert.IsNotNull(subSystem.Get(gameObject1.Id));
            Assert.IsNull(subSystem.Get(gameObject2.Id));
            Assert.IsNotNull(subSystem.Get(gameObject3.Id));
        }


        [TestMethod()]
        public void GetTests()
        {
            GameObjectSubSystem subSystem = new GameObjectSubSystem();

            GameObject gameObject1_0 = subSystem.Create<TestGameObject1>();
            GameObject gameObject1_1 = subSystem.Create<TestGameObject1>();
            GameObject gameObject2_0 = subSystem.Create<TestGameObject2>();
            GameObject gameObject2_1 = subSystem.Create<TestGameObject2>();
            GameObject gameObject3_0 = subSystem.Create<TestGameObject3>();
            GameObject gameObject3_1 = subSystem.Create<TestGameObject3>();

            Assert.AreEqual(gameObject1_0, subSystem.Get(gameObject1_0.Id));
            Assert.AreNotEqual(gameObject1_0, subSystem.Get(gameObject1_1.Id));

            Assert.AreEqual(gameObject2_0, subSystem.Get<TestGameObject2>(gameObject2_0.Id));
            Assert.AreNotEqual(gameObject2_0, subSystem.Get<TestGameObject2>(gameObject2_1.Id));
            Assert.AreNotEqual(gameObject2_0, subSystem.Get<TestGameObject3>(gameObject2_0.Id));
        }

        [TestMethod()]
        public void GetAllTests()
        {
            GameObjectSubSystem subSystem = new GameObjectSubSystem();

            GameObject gameObject1_0 = subSystem.Create<TestGameObject1>();
            GameObject gameObject1_1 = subSystem.Create<TestGameObject1>();
            GameObject gameObject2_0 = subSystem.Create<TestGameObject2>();
            GameObject gameObject2_1 = subSystem.Create<TestGameObject2>();
            GameObject gameObject3_0 = subSystem.Create<TestGameObject3>();
            GameObject gameObject3_1 = subSystem.Create<TestGameObject3>();

            CollectionAssert.AreEqual(new List<GameObject>() { gameObject1_0, gameObject1_1, gameObject3_0, gameObject3_1 }, subSystem.GetAll<TestGameObject1>());
            CollectionAssert.AreEqual(new List<GameObject>() { gameObject2_0, gameObject2_1 }, subSystem.GetAll<TestGameObject2>());
            CollectionAssert.AreEqual(new List<GameObject>() { gameObject3_0, gameObject3_1 }, subSystem.GetAll<TestGameObject3>());
        }
    }
}
