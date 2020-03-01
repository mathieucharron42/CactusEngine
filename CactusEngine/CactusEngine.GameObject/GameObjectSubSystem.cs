using CactusEngine.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace CactusEngine.Object
{
    public class GameObjectSubSystem : SubSystem
    {
		public GameObjectSubSystem()
		{
			_gameObjects = new List<GameObject>();
			_nextGameObjectId = 1;
		}
		public override void Initialize(Engine engine)
		{
			_engine = engine;
		}

		public override void Shutdown(Engine engine)
		{
			_gameObjects.Clear();
			_engine = null;
		}

		public GameObjectType Create<GameObjectType>(Action<GameObjectType> preInitializationFunction = null)
			where GameObjectType : GameObject, new()
		{
			GameObjectType gameObject = new GameObjectType();
			gameObject.Id = _nextGameObjectId++;
			if (preInitializationFunction != null)
			{
				preInitializationFunction(gameObject);
			}
			gameObject.Initialize(_engine);
			_gameObjects.Add(gameObject);
			return gameObject;
		}

		public void Destroy<GameObjectType>(GameObjectType gameObject)
			where GameObjectType : GameObject
		{
			gameObject.Shutdown(_engine);
			_gameObjects.Remove(gameObject);
		}

		public GameObject Get(int id)
		{
			return GetAll().Find((GameObject gameObject) =>
			{
				return gameObject.Id == id;
			});
		}

		public GameObjectType Get<GameObjectType>(int id)
			where GameObjectType : GameObject
		{
			return GetAll<GameObjectType>().Find((GameObjectType gameObject) =>
			{
				return gameObject.Id == id;
			});
		}

		public List<GameObject> GetAll()
		{
			return _gameObjects;
		}

		public List<GameObjectType> GetAll<GameObjectType>()
			where GameObjectType : class
		{
			List<GameObjectType> elements = new List<GameObjectType>();

			foreach (GameObject gameObject in _gameObjects)
			{
				if (gameObject is GameObjectType)
				{
					elements.Add(gameObject as GameObjectType);
				}
			}

			return elements;
		}

		private Engine _engine;
		private List<GameObject> _gameObjects;
        private int _nextGameObjectId;
    }
}
