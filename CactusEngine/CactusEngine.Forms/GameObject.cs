using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
	public class GameObject
    {
		public GameObject()
		{
			_children = new List<GameObject>();
			_localTransform = Transform.Origin;
			_worldTransform = Transform.Origin;
			_isWorldTransformDirty = false;
		}

		public Transform LocalTransform
		{
			get 
			{ 
				return _localTransform; 
			}
			set 
			{
				_localTransform = value;
				_isWorldTransformDirty = true;
			}
		}

		public Transform WorldTransform
		{
			get
			{ 
				return _worldTransform; 
			}
		}

		public bool IsWorldTransformDirty
		{
			get { return _isWorldTransformDirty; }
		}

		public IReadOnlyList<GameObject> Children
		{
			get { return _children; }
		}

		public virtual void Initialize(Engine engine) { }

		public virtual void Shutdown(Engine engine) { }

		public virtual void Update(Engine engine, TimeSpan timespan) { }

		public virtual void Render(Engine engine, Renderer renderer) { }

		public virtual void Click(Engine engine, Vector2 where) { }

		public void ComputeWorldTransform(Transform parent)
		{
			_worldTransform = LocalTransform.Combine(parent);
			_isWorldTransformDirty = false;
		}

		public void Traverse(Action<GameObject> operation)
		{
			operation(this);
			foreach (GameObject child in _children)
			{
				child.Traverse(operation);
			}
		}

		public ReturnType Traverse<ReturnType>(Func<GameObject, ReturnType, ReturnType> operation, ReturnType parentValue)
		{
			ReturnType rootValue = operation(this, parentValue);
			foreach (GameObject child in _children)
			{
				child.Traverse(operation, rootValue);
			}
			return rootValue;
		}

		public void AddChild(GameObject child)
		{
			if (!HasChild(child))
			{
				_children.Add(child);
			}
		}

		public void RemoveChild(GameObject child)
		{
			_children.Remove(child);
		}

		public bool HasChild(GameObject child)
		{
			return _children.Contains(child);
		}

		private List<GameObject> _children;
		private Transform _localTransform;
		private Transform _worldTransform;
		private bool _isWorldTransformDirty;
	}
}
