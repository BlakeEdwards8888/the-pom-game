using System;
using System.Collections.Generic;
using Pom.AnimationHandling;
using UnityEngine;

namespace Pom.AnimationHandling.ScriptableObjects
{
    [CreateAssetMenu(fileName = "New Animation State SO", menuName = "Animation States/Default State SO")]
    public class AnimationStateSO : ScriptableObject
    {
        [SerializeField] protected string[] animations;

        public virtual void Enter(AnimationStateMachine stateMachine, ref Dictionary<string, object> context)
        {
            int randomValue = UnityEngine.Random.Range(0, animations.Length);

            stateMachine.Animator.Play(animations[randomValue]);
        }

        public virtual void Execute(AnimationStateMachine stateMachine, ref Dictionary<string, object> context) { }
        public virtual void Exit(AnimationStateMachine stateMachine, ref Dictionary<string, object> context)
        {
            if (context.ContainsKey("finished"))
            {
                Action finishedAction = context["finished"] as Action;
                finishedAction?.Invoke();
            }
        }
    }
}
