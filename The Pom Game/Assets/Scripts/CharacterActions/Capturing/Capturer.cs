using Pom.AnimationHandling;
using Pom.CaptureSystem;
using Pom.Navigation;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Pom.CharacterActions.Capturing
{
    public class Capturer : ActionExecutor
    {
        AnimationStateMachine animationStateMachine => GetComponent<AnimationStateMachine>();
        
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

            Vector2 direction = CalculateDirection(capturableEntity);
            animationStateMachine.SwitchState(AnimationTag.Capture, ("direction", direction));

            animationStateMachine.onCurrentAnimationFinished += () =>
            {
                Capture(capturableEntity);
                finished?.Invoke();
            };
        }

        private Vector2 CalculateDirection(CapturableEntity capturableEntity)
        {
            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);
            Vector2 capturableEntityGridPosition = GridSystem.Instance.GetGridPosition(capturableEntity.transform.position);

            return GridSystem.GetDirection(currentGridPosition, capturableEntityGridPosition);
        }
    }
}
