using System.Collections.Generic;
using UnityEngine;

namespace Pom.AnimationHandling
{
    public class AnimationStateMachine : MonoBehaviour
    {
        [field:SerializeField] public Animator Animator {get; private set;}

        [SerializeField] AnimationState[] animationStates;
        Dictionary<AnimationTag, AnimationState> stateDict;

        AnimationState currentState;

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

        public void SwitchState(AnimationTag tag)
        {
            if(stateDict == null) BuildStateDict();

            if (!stateDict.ContainsKey(tag))
            {
                Debug.LogWarning($"{gameObject.name} does not have an animation state with the tag {tag}");
                return;
            }

            currentState?.Exit();
            currentState = stateDict[tag];
            currentState.Enter();
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
