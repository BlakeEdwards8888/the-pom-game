using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Pom.AnimationHandling
{
    public class AnimationStateMachine : MonoBehaviour
    {
        [field:SerializeField] public Animator Animator {get; private set;}

        [Tooltip("Whichever state is set to the first element will be considered the 'default state' ")]
        [SerializeField] AnimationState[] animationStates;
        Dictionary<AnimationTag, AnimationState> stateDict;

        AnimationState currentState;

        public event Action onCurrentAnimationFinished;

        void Start()
        {
            ReturnToDefaultState();
        }

        void Update()
        {
            if(currentState == null) return;

            currentState.Execute();
        }

        public void ReturnToDefaultState()
        {
            if(animationStates.Length == 0) return;
            
            SwitchState(animationStates[0].Tag);
        }

        public void SwitchState(AnimationTag tag, Dictionary<string, object> context = null)
        {
            if(animationStates.Length == 0)
            {
                if(context == null) return;
                if(!context.ContainsKey("finished")) return;

                Action finished = context["finished"] as Action;
                finished.Invoke();
                return;
            }

            if(stateDict == null) BuildStateDict();

            if (!stateDict.ContainsKey(tag))
            {
                Debug.LogWarning($"{gameObject.name} does not have an animation state with the tag {tag}");
                return;
            }

            currentState?.Exit();
            currentState = stateDict[tag];
            currentState.Enter(context);

            onCurrentAnimationFinished?.Invoke();
            onCurrentAnimationFinished = null;
        }

        void BuildStateDict()
        {
            stateDict = new Dictionary<AnimationTag, AnimationState>();

            foreach(AnimationState state in animationStates)
            {
                stateDict[state.Tag] = state;
            }
        }
    }
}
