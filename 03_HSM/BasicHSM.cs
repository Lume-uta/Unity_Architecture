using System.Linq;
using Core.UpdateService;
using System.Collections.Generic;

namespace Core.HSM
{
    public abstract class BasicHSM<HSMRoot> : HierarchicalStateMachine<HSMRoot>
        where HSMRoot : IHSMRoot
    {
        public override void Initialize(HSMRoot manager, IState initState)
        {
            base.Initialize(manager);

            ChangeState(initState);
        }

        public override void Dispose()
        {
            foreach (IState state in GetSelfAndParentStates(CurrState).Reverse<IState>()) state.Dispose();

            base.Dispose();
        }

        public void ChangeState(IState nextState)
        {
            if (nextState == null || nextState == CurrState || IsStopTransition) return;

            List<IState> beforStates = GetSelfAndParentStates(CurrState);
            CurrState = nextState;
            List<IState> afterStates = GetSelfAndParentStates(CurrState);

            foreach (IState state in beforStates.Except(afterStates).Reverse()) state.Exit();
            foreach (IState state in afterStates.Except(beforStates)) state.Enter();

            //Debug.Log($"Change HSM State: {nextState.GetType().FullName}");
        }

        public override void Tick()
        {
            if (CurrState == null || IsStopUpdate) return;

            if (!IsStopTransition)
            {
                IState nextState = CheckTransition(GetSelfAndParentStates(CurrState));
                if (nextState != null) ChangeState(nextState);
            }

            ExecuteUpdate(UpdateType.Update, GetSelfAndParentStates(CurrState));
        }

        public override void FixedTick()
        {
            if (CurrState == null || IsStopUpdate) return;

            ExecuteUpdate(UpdateType.FixedUpdate, GetSelfAndParentStates(CurrState));
        }

        public override void LateTick()
        {
            if (CurrState == null || IsStopUpdate) return;

            ExecuteUpdate(UpdateType.LateUpdate, GetSelfAndParentStates(CurrState));
        }
    }
}