using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Pom.AnimationHandling.ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements.Experimental;

namespace Pom.AnimationHandling
{
    [System.Serializable]
    public class AnimationState
    {
        [SerializeField] string label;  //This is just to display the mappings better in the inspector
        [field: SerializeField] public AnimationTag Tag {get; private set;}

        [SerializeField] AnimationStateMachine stateMachine;
        [SerializeField] AnimationStateSO stateSO;

        Dictionary<string, object> stateContext = new Dictionary<string, object>();

        public void Enter((string key, object value)? stateArgs)
        { 
            if(stateArgs != null) stateContext[stateArgs?.key] = stateArgs?.value;

            stateSO.Enter(stateMachine, ref stateContext); 
        }

        public void Execute(){ stateSO.Execute(stateMachine, ref stateContext); }
        public void Exit(){ stateSO.Exit(stateMachine, ref stateContext); }
    }
}
