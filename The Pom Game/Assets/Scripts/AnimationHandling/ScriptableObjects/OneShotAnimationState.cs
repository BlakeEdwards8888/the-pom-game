using System.Collections.Generic;
using UnityEngine;
namespace Pom.AnimationHandling.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New One Shot Animation State", menuName = "Animation States/One Shot Animation State")]
    public class OneShotAnimationState : AnimationStateSO
    {
        [Tooltip("This is the actual tag indicated in the Animator component")]
        [SerializeField] string animationTag;

        public override void Execute(AnimationStateMachine stateMachine, ref Dictionary<string, object> context)
        {
            AnimatorStateInfo currentAnimatorStateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);

            if(currentAnimatorStateInfo.IsTag(animationTag) && currentAnimatorStateInfo.normalizedTime >= 1) stateMachine.ReturnToDefaultState();
        }
    }
}
