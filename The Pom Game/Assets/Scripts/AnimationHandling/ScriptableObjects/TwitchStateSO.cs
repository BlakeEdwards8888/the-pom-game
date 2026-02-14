using System.Collections.Generic;
using UnityEngine;
namespace Pom.AnimationHandling.ScriptableObjects
{    
    [CreateAssetMenu(fileName = "New Twitch State SO", menuName = "Animation States/Twitch")]
    public class TwitchStateSO : AnimationStateSO
    {
        public override void Execute(AnimationStateMachine stateMachine, ref Dictionary<string, object> context)
        {
            float loops = stateMachine.Animator.GetCurrentAnimatorStateInfo(0).normalizedTime;

            if(loops >= 1)
            {
                stateMachine.ReturnToDefaultState();
            }
        }
    }
}