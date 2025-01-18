using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public class MonoPool<T> : MonoSingleton<MonoPool<T>> where T : MonoBehaviour, IPoolable
    {
        [SerializeField] private int initialSize;
        [SerializeField] private T prefab;
        [SerializeField] private Transform parent;
        private Stack<T> _available;
        private string _poolName;

        private void Awake()
        {
            _available = new Stack<T>();
            AddItemsToPool();
        }

        public void Initialize(GameObject prefabObject, int size, Transform parentTransform)
        {
            this.prefab = prefabObject.GetComponent<T>();
            _poolName = prefabObject.name;
            this.initialSize = size;
            this.parent = parentTransform;

            _available = new Stack<T>();
            AddItemsToPool();
        }
        public string GetPoolName()
        {
            return _poolName;
        }

        public T Get()
        {
            if (_available.Count == 0)
                AddItemsToPool();

            var obj = _available.Pop();
            obj.gameObject.SetActive(true);
            obj.Reset();
            return obj;
        }

        public void Return(T obj)
        {
            obj.gameObject.SetActive(false);
            _available.Push(obj);
        }

        private void AddItemsToPool()
        {
            for (int i = 0; i < initialSize; i++)
            {
                var obj = Instantiate(prefab, parent, true);
                obj.gameObject.SetActive(false);
                _available.Push(obj);
            }
        }
    }
}