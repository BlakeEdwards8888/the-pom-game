using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pom.Objectives
{
    [Serializable]
    public class Objective
    {
        [field: SerializeField] public string Tag { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public bool MustBeCompleted { get; private set; }
        
        public ObjectiveState State { get; private set; } = ObjectiveState.InProgress;

        public UnityEvent onComplete;
        public UnityEvent onFailed;

        public event Action onStateChanged;

        public void SetState(ObjectiveState state)
        {
            if (state == State) return;

            State = state;

            switch (state)
            {
                case ObjectiveState.Complete:
                    onComplete?.Invoke();
                    break;
                case ObjectiveState.Failed:
                    onFailed?.Invoke();
                    break;
            }

            onStateChanged?.Invoke();
        }
    }
}
