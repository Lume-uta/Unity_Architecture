using System.Linq;
using Core.UpdateService;
using System.Collections.Generic;

namespace Core.HSM
{
    public interface IHSM { }
    public interface IHSMRoot { void Initialize(); void Dispose(); void Stop(bool isStop); }
    public interface IMainCtx { IHSMRoot HSMRoot { get; } }

    public abstract class HierarchicalStateMachine<HSMRoot> : IUpdateRegistry, IFixedUpdateRegistry, ILateUpdateRegistry, IHSM
        where HSMRoot : IHSMRoot
    {
        public HSMRoot Root { get; private set; }

        protected IState CurrState;
        public virtual void Initialize(HSMRoot manager, IState initState = null)
        {
            Root = manager;

            IsStopUpdate = false;
            IsStopTransition = false;

            UpdateManager.AddUpdate(this);
            UpdateManager.AddFixedUpdate(this);
            UpdateManager.AddLateUpdate(this);

            //Debug.Log($"Initialize: {GetType().FullName}");
        }

        public virtual void Dispose()
        {
            CurrState = null;

            UpdateManager.RemoveUpdate(this);
            UpdateManager.RemoveFixedUpdate(this);
            UpdateManager.RemoveLateUpdate(this);

            //Debug.Log($"Dispose: {GetType().FullName}");
        }

        protected bool IsStopUpdate;
        protected bool IsStopTransition;
        public void UpdateStop(bool isStop)
        {
            IsStopUpdate = isStop;

            //if (isStop) Debug.LogWarning($"Stop Update: {GetType().FullName}");
            //else Debug.Log($"Start Update: {GetType().FullName}");
        }

        private int _currTransitionPriority;
        public void TransitionStop(bool isStop, int priority)
        {
            if (_currTransitionPriority > priority) return;

            IsStopTransition = isStop;

            if (isStop) _currTransitionPriority = priority;
            else _currTransitionPriority = 0;

            //if (isStop) Debug.LogWarning($"Stop Transition: {GetType().FullName}");
            //else Debug.Log($"Start Transition: {GetType().FullName}");
        }

        public abstract void Tick();
        public abstract void FixedTick();
        public abstract void LateTick();

        protected IState CheckTransition(List<IState> states)
        {
            foreach (IState state in states.ToList())
            {
                IState nextState = state.GetNextTransition();
                if (nextState != null && nextState != CurrState) return nextState;
            }

            return null;
        }

        protected void ExecuteUpdate(UpdateType type, List<IState> states)
        {
            if (type == UpdateType.Update) foreach (IState state in states.ToArray()) state.Tick();
            else if (type == UpdateType.LateUpdate) foreach (IState state in states.ToArray()) state.LateTick();
            else if (type == UpdateType.FixedUpdate) foreach (IState state in states.ToArray()) state.FixedTick();
        }

        // Return: Parent -> Target
        protected List<IState> GetSelfAndParentStates(IState target)
        {
            List<IState> states = new List<IState>();

            if (target == null) return states;

            IState tempCurrState = target;
            while (true)
            {
                if (tempCurrState == null) break;

                states.Add(tempCurrState);
                tempCurrState = tempCurrState.Parent;
            }

            states.Reverse();
            return states;
        }
    }
}