using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.AnimationHandling.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Directional Animation State", menuName = "Animation States/Directional State")]
    public class DirectionalAnimationState : AnimationStateSO
    {
        [Tooltip("This is the actual tag indicated in the Animator component")]
        [SerializeField] string animationTag;

        public override void Enter(AnimationStateMachine stateMachine, ref Dictionary<string, object> context)
        {
            Vector2 direction = (Vector2)context["direction"];

            stateMachine.Animator.Play(GetDirectionalAnimation(direction, stateMachine));
        }

        public override void Execute(AnimationStateMachine stateMachine, ref Dictionary<string, object> context)
        {
            AnimatorStateInfo currentAnimatorStateInfo = stateMachine.Animator.GetCurrentAnimatorStateInfo(0);
            
            if(currentAnimatorStateInfo.IsTag(animationTag) && currentAnimatorStateInfo.normalizedTime >= 1) stateMachine.ReturnToDefaultState();
        }

        private string GetDirectionalAnimation(Vector2 direction, AnimationStateMachine stateMachine)
        {
            if(direction == Vector2.zero)
            {
                Debug.LogWarning($"{stateMachine.gameObject} tried to use a directional animation with an invalid direction: {direction}");
                return animations[0];
            }

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            if(angle < 0) angle += 360;

            int index = Mathf.FloorToInt(angle / (360/animations.Length));
            
            return animations[index];
        }
    }
}
