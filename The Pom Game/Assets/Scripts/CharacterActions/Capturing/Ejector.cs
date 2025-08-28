using Pom.Attributes;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.Capturing
{
    public class Ejector : ActionExecutor
    {
        public event Action<Vector2> onEjected;

        public override string GetDisplayName()
        {
            return "Eject";
        }

        void Eject(Vector2 targetPosition)
        {
            onEjected?.Invoke(targetPosition);
        }

        public override bool TryExecute(Vector2 targetPosition, List<ActionExecutionArg> executionArgs, Action finished = null)
        {
            if (IsUsed) return false;

            PathNode targetPositionNode = GridSystem.Instance.NavDict[targetPosition];

            if (targetPositionNode == null) return false;
            if (!targetPositionNode.IsWalkable()) return false;
            if (targetPositionNode.IsSemipermeable()) return false;

            Execute(targetPosition, finished);
            return true;
        }

        protected override void Execute(object args, Action finished)
        {
            base.Execute(args, finished);

            Vector2 targetPosition = (Vector2)args;

            Eject(targetPosition);
            finished?.Invoke();
        }
    }
}
