using UnityEngine;

namespace Core.HSM
{
    public interface IState
    {
        IState Parent { get; }

        void Enter();
        void Tick();
        IState GetNextTransition();
        void LateTick();
        void FixedTick();
        void Exit();
        void Dispose();
    }


    public abstract class HSMState<HSM> : IState
        where HSM : IHSM
    {
        public IState Parent { get; private set; }
        protected HSM Mgr { get; private set; }

        public HSMState(HSM manager, IState parent = null)
        {
            Parent = parent;
            Mgr = manager;

            IsDispose = false;
        }

        protected bool IsDispose { get; private set; }
        public virtual void Dispose() => IsDispose = true;

        public virtual void Enter() { }
        public virtual IState GetNextTransition() => null;
        public virtual void Tick() { }
        public virtual void LateTick() { }
        public virtual void FixedTick() { }
        public virtual void Exit() { }
    }
}