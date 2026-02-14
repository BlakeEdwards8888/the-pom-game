using System.Collections.Generic;
using UnityEngine;

namespace Pom.AnimationHandling.ScriptableObjects
{    
    [CreateAssetMenu(fileName = "New Idle State SO", menuName = "Animation States/Idle")]
    public class IdleStateSO : AnimationStateSO
    {
        [SerializeField] Vector2 loopsPerTwitchRange;

        public override void Enter(AnimationStateMachine stateMachine, ref Dictionary<string, object> context)
        {
            base.Enter(stateMachine, ref context);

            context["loopsPerTwitch"] = Random.Range(loopsPerTwitchRange.x, loopsPerTwitchRange.y + 1);
        }

        public override void Execute(AnimationStateMachine stateMachine, ref Dictionary<string, object> context)
        {
            if(context == null) Debug.Log("Context is null somehow");

            float loops = stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if(loops >= (float)context["loopsPerTwitch"])
            {
                stateMachine.SwitchState(AnimationTag.Twitch);
            }
        }
    }
}
