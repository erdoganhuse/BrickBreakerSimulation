using System.Collections.Generic;
using Library.CoroutineSystem;
using UnityEngine;
using UnityEngine.Assertions;

namespace Library.Patterns.ObjectPool
{
    public class GenericPool<T> where T : MonoBehaviour
    {
        #region Variables

        private const int DefaultIncreaseAmount = 3;

        public int Capacity => _availableItems.Count;
        
        private readonly T _prefab;
        private readonly Transform _container;

        private readonly Queue<T> _availableItems;
        private readonly List<T> _busyItems;

        #endregion

        #region Class Methods

        public GenericPool(T prefab, int capacity, Transform container)
        {
            _prefab = prefab;
            _container = container;

            _availableItems = new Queue<T>();
            _busyItems = new List<T>();

            Create(capacity);
        }

        private void Create(int capacity)
        {
            for (int i = 0; i < capacity; i++)
            {
                T instantiatedItem = UnityEngine.Object.Instantiate(_prefab, _container) as T;
                instantiatedItem.gameObject.SetActive(false);
                _availableItems.Enqueue(instantiatedItem);
            }
        }

        private void IncreaseCapacity(int increaseAmount)
        {
            for (int i = 0; i < increaseAmount; i++)
            {
                T instantiatedItem = Object.Instantiate(_prefab, _container) as T;
                instantiatedItem.gameObject.SetActive(false);
                _availableItems.Enqueue(instantiatedItem);
            }
        }

        public T Spawn()
        {
            return Spawn(_container);
        }

        public T Spawn(Transform parent, Vector3? position = null, Quaternion? rotation = null, Vector3? scale = null)
        {
            Assert.IsTrue(_availableItems != null);

            if (Capacity <= 0)
            {
                IncreaseCapacity(DefaultIncreaseAmount);
            }

            T item = _availableItems.Dequeue();
            item.transform.SetParent(parent, false);
            
            if(position != null) item.transform.localPosition = position.Value;
            if(rotation != null) item.transform.localRotation = rotation.Value;
            if(scale != null) item.transform.localScale = scale.Value;
                
            _busyItems.Add(item);

            return item;
        }

        public void Recycle(T item)
        {
            Assert.IsTrue(item != null);

            item.gameObject.SetActive(false);
            item.transform.SetParent(_container);

            if (_busyItems.Contains(item)) _busyItems.Remove(item);

            _availableItems.Enqueue(item);
        }

        public void Recycle(T item, float recycleDelay = 0f)
        {
            CoroutineManager.DoAfterGivenTime(recycleDelay, () =>
            {
                Recycle(item);
            });
        }

        #endregion
    }
}
