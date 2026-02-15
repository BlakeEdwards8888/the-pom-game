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
            currentState.Execute();
        }

        public void ReturnToDefaultState()
        {
            SwitchState(animationStates[0].Tag);
        }

        public void SwitchState(AnimationTag tag, (string key, object value)? stateArgs = null)
        {
            Debug.Log($"Switching state to {tag}");

            if(stateDict == null) BuildStateDict();

            if (!stateDict.ContainsKey(tag))
            {
                Debug.LogWarning($"{gameObject.name} does not have an animation state with the tag {tag}");
                return;
            }

            currentState?.Exit();
            currentState = stateDict[tag];
            currentState.Enter(stateArgs);

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
