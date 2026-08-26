using UnityEngine;

namespace Core.Utility
{
    public sealed class TimeCheck : StateMachineBehaviour
    {
        [Header("Reference"), SerializeField] private MethodData _data;

        [Header("Setting"), Range(0, 1), SerializeField] private float _invokeTime;

        private bool _isReturn;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _isReturn = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_isReturn) return;

            if (stateInfo.normalizedTime >= _invokeTime)
            {
                _isReturn = true;
                _data.Execute();
            }
        }
    }
}