using System.Linq;
using Core.UpdateService;
using System.Collections.Generic;

namespace Core.HSM
{
    public abstract class StackBasedHSM<HSMRoot> : HierarchicalStateMachine<HSMRoot>
        where HSMRoot : IHSMRoot
    {
        private Stack<IState> _currStacks;

        public override void Initialize(HSMRoot manager, IState initState = null)
        {
            base.Initialize(manager);

            _currStacks = new Stack<IState>();
        }

        public override void Dispose()
        {
            foreach (IState state in GetCurrStack_SelfAndParentStates().Reverse<IState>()) state.Dispose();
            _currStacks = null;

            base.Dispose();
        }

        public void ChangeState(IState[] states) // Add
        {
            for (int i = 0; i < states.Length; i++)
            {
                if (states[i] == null || _currStacks.Contains(states[i]) || IsStopTransition) return;

                List<IState> beforStates = GetCurrStack_SelfAndParentStates();

                _currStacks.Push(states[i]);
                CurrState = _currStacks.Peek();

                ChangeState(beforStates, GetCurrStack_SelfAndParentStates());
            }
        }
        public void ChangeState(bool isClear, int count = 1) // Remove
        {
            for (int i = 0; i < count; i++)
            {
                if (_currStacks.Count <= 0 || IsStopTransition) return;

                List<IState> beforStates = GetCurrStack_SelfAndParentStates();

                if (isClear) _currStacks.Clear();
                else _currStacks.Pop();

                CurrState = _currStacks.Count > 0 ? _currStacks.Peek() : null;

                ChangeState(beforStates, GetCurrStack_SelfAndParentStates());
            }
        }
        private void ChangeState(List<IState> beforStates, List<IState> afterStates) // Call
        {
            foreach (IState state in beforStates.Except(afterStates).Reverse()) state.Exit();
            foreach (IState state in afterStates.Except(beforStates)) state.Enter();
        }


        public override void Tick()
        {
            if (CurrState == null || IsStopUpdate) return;

            ExecuteUpdate(UpdateType.Update, GetCurrStack_SelfAndParentStates());
        }

        public override void FixedTick()
        {
            if (CurrState == null || IsStopUpdate) return;

            ExecuteUpdate(UpdateType.FixedUpdate, GetCurrStack_SelfAndParentStates());
        }

        public override void LateTick()
        {
            if (CurrState == null || IsStopUpdate) return;

            ExecuteUpdate(UpdateType.LateUpdate, GetCurrStack_SelfAndParentStates());
        }

        // Return: Parent -> Self
        private List<IState> GetCurrStack_SelfAndParentStates()
        {
            List<IState> states = new List<IState>();

            if (_currStacks == null) return states;

            foreach (IState target in _currStacks)
            {
                if (states.Contains(target)) continue;

                foreach (IState state in GetSelfAndParentStates(target))
                {
                    if (states.Contains(state)) continue;

                    states.Add(state);
                }
            }

            return states;
        }
    }
}