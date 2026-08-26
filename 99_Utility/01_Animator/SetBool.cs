using System.Linq;
using UnityEngine;

namespace Core.Utility
{
    public sealed class SetBool : StateMachineBehaviour
    {
        [Header("Setting"), SerializeField] private bool _setValue;
        [SerializeField] private bool _isEnter = true;
        [SerializeField] private string[] _params;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (!_isEnter) return;

            foreach (string name in _params.ToList()) animator.SetBool(name, _setValue);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_isEnter) return;

            foreach (string name in _params.ToList()) animator.SetBool(name, _setValue);
        }
    }
}