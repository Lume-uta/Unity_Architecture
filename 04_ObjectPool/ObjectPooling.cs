using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Core.ObjectPool
{
    public sealed class ObjectPooling<T>
        where T : class
    {
        private string _name;

        private Action<T> _getEvt;
        private Action<T> _releaseEvt;

        private ObjectPool<T> _pooling;
        public void Initialize(string name, int initSpawn, int maxSize,
            Func<T> createEvt,
            Action<T> getEvt,
            Action<T> releaseEvt)
        {
            _name = name;

            // Init
            _pooling = new ObjectPool<T>(
           createFunc: createEvt,
           collectionCheck: true,
           actionOnGet: GetEvent,
           actionOnRelease: ReleaseEvent,
           defaultCapacity: initSpawn,
           maxSize: maxSize);

            // Init Spawn
            T[] objs = new T[initSpawn];

            for (int i = 0; i < initSpawn; i++) objs[i] = Get();
            for (int i = 0; i < initSpawn; i++) Release(objs[i]);

            _getEvt = getEvt;
            _releaseEvt = releaseEvt;

            Debug.Log($"Initialize: ObjectPooling({_name})");
        }

        public void Dispose()
        {
            _pooling.Dispose();
            Debug.Log($"Dispose: ObjectPooling({_name})");
        }


        public T Get() => _pooling.Get();
        private void GetEvent(T obj) => _getEvt?.Invoke(obj);

        public void Release(T obj) => _pooling.Release(obj);
        private void ReleaseEvent(T obj) => _releaseEvt?.Invoke(obj);


        public Transform CreateParentGameObject(string name)
        {
            Transform parent = new GameObject($"ObjectPooling({name})").transform;
            parent.parent = GameObject.FindWithTag("ObjectPool").transform;

            return parent;
        }
    }
}