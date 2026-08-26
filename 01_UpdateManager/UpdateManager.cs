using System;
using System.Linq;
using UnityEngine;

namespace Core.UpdateService
{
    public enum UpdateType
    {
        Update,
        LateUpdate,
        FixedUpdate
    }

    public sealed class UpdateManager : MonoBehaviour
    {
        private static UpdateManager _instance;

        private static Action _updates;
        private static Action _lateUpdates;
        private static Action _fixedUpdates;

        #region// Update
        private void Update() => _updates?.Invoke();
        public static void AddUpdate(IUpdateRegistry method)
        {
            if (_updates != null && _updates.GetInvocationList().Contains((Action)method.Tick)) return;
            else _updates += method.Tick;
        }
        public static void RemoveUpdate(IUpdateRegistry method) => _updates -= method.Tick;
        #endregion

        #region// Late Update
        private void LateUpdate() => _lateUpdates?.Invoke();
        public static void AddLateUpdate(ILateUpdateRegistry method)
        {
            if (_lateUpdates != null && _lateUpdates.GetInvocationList().Contains((Action)method.LateTick)) return;
            else _lateUpdates += method.LateTick;
        }
        public static void RemoveLateUpdate(ILateUpdateRegistry method) => _lateUpdates -= method.LateTick;
        #endregion

        #region// Fixed Update
        private void FixedUpdate() => _fixedUpdates?.Invoke();
        public static void AddFixedUpdate(IFixedUpdateRegistry method)
        {
            if (_fixedUpdates != null && _fixedUpdates.GetInvocationList().Contains((Action)method.FixedTick)) return;
            else _fixedUpdates += method.FixedTick;
        }
        public static void RemoveFixedUpdate(IFixedUpdateRegistry method) => _fixedUpdates -= method.FixedTick;
        #endregion

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);

                Debug.Log($"Initialize: {GetType().Name}");
            }
            else Destroy(gameObject);
        }
    }
}