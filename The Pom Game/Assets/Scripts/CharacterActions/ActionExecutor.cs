using Pom.CharacterActions.AIExecutionStrategies;
using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using Pom.UndoSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions
{
    public abstract class ActionExecutor : MonoBehaviour, ICacheable
    {
        [field: SerializeField] public RangeStrategy RangeStrategy { get; private set; }
        [field: SerializeField] public ExecutionStrategy AIExecutionStrategy { get; private set; }

        public event Action onActionStarted;

        public bool IsUsed { get; protected set; } = false;

        public bool IsTargetInRange(Vector2 currentPosition, Vector2 targetPosition, Func<PathNode, bool> condition = null)
        {
            return RangeStrategy.IsTargetInRange(currentPosition, targetPosition);
        }

        public List<PathNode> GetNodesInRange(Vector2 startingGridPosition, Func<PathNode, bool> condition = null)
        {
            return RangeStrategy.GetNodesInRange(startingGridPosition);
        }

        public int GetRange()
        {
            return RangeStrategy.Range;
        }

        public abstract bool TryExecute(Vector2 targetPosition, List<ActionExecutionArg> executionArgs, Action finished = null);

        protected virtual void Execute(object args, Action finished)
        {
            IsUsed = true;
            onActionStarted?.Invoke();
        }

        public void ResetUsageState()
        {
            IsUsed = false;
        }

        public abstract string GetDisplayName();

        public virtual object CaptureState()
        {
            return IsUsed;
        }

        public virtual void RestoreState(object state)
        {
            IsUsed = (bool)state;
        }
    }
}
