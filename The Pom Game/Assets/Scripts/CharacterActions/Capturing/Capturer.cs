using Pom.CaptureSystem;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CharacterActions.Capturing
{
    public class Capturer : ActionExecutor
    {
        public override string GetDisplayName()
        {
            return "Capture";
        }

        void Capture (CapturableEntity capturableEntity)
        {
            capturableEntity.RaiseCaptureEvent(gameObject);
        }

        public override bool TryExecute(Vector2 targetPosition, List<ActionExecutionArg> executionArgs, Action finished)
        {
            if (!GridSystem.Instance.NavDict[targetPosition].TryGetOccupyingEntity(out CapturableEntity capturableEntity)) return false;
            if(!IsTargetInRange(transform.position, targetPosition)) return false;

            Execute(capturableEntity, finished);

            return true;
        }

        protected override void Execute(object args, Action finished)
        {
            base.Execute(args, finished);

            CapturableEntity capturableEntity = args as CapturableEntity;

            Capture(capturableEntity);
            finished?.Invoke();
        }
    }
}
