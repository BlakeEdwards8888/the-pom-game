using Pom.CaptureSystem;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.AI;

namespace Pom.CharacterActions.LockAndKey
{
    public class Unlocker : ActionExecutor
    {
        public override string GetDisplayName()
        {
            return "Unlock";
        }

        public override bool TryExecute(Vector2 targetPosition, List<ActionExecutionArg> executionArgs, Action finished = null)
        {
            if (!GridSystem.Instance.NavDict[targetPosition].TryGetOccupyingEntity(out Lock occupyingLock)) return false;

            Execute(occupyingLock, finished);
            return true;
        }

        protected override void Execute(object args, Action finished)
        {
            base.Execute(args, finished);

            Lock occupyingLock = args as Lock;
            occupyingLock.onUnlocked?.Invoke();

            occupyingLock.gameObject.SetActive(false);
            finished?.Invoke();
        }
    }
}
