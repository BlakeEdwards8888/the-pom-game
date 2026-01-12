using Pom.CharacterActions.Capturing;
using Pom.Control;
using Pom.Units;
using UnityEngine;

namespace Pom.CaptureSystem
{
    public class CapturableEntityContainer : MonoBehaviour
    {
        [SerializeField] CapturableEntity inanimateState;
        [SerializeField] Unit capturedState;

        public Unit OccupyingUnit { get; private set; }
        Controller activeController;

        private void OnEnable()
        {
            inanimateState.onCaptured += HandleCaptured;
        }

        void SwitchState(bool isCaptured)
        {
            inanimateState.gameObject.SetActive(!isCaptured);
            capturedState.gameObject.SetActive(isCaptured);

            OccupyingUnit.gameObject.SetActive(!isCaptured);

            Controller alliedController = Controller.GetControllerByFaction(OccupyingUnit.Alliance.AlliedFaction);

            if(alliedController != activeController)
            {
                if(activeController != null) activeController.onTurnStarted -= HandleTurnStarted;
                activeController = alliedController;
                activeController.onTurnStarted += HandleTurnStarted;
            }

            inanimateState.transform.position = capturedState.transform.position;

            activeController.SetActiveUnit(isCaptured ? capturedState : OccupyingUnit);
        }

        private void HandleCaptured(GameObject capturer)
        {
            Transform rootCapturerTransform = capturer.transform.root;

            if (rootCapturerTransform.TryGetComponent(out CapturableEntityContainer otherCapturableEntity))
            {
                OccupyingUnit = otherCapturableEntity.OccupyingUnit;
                otherCapturableEntity.SwitchState(false);
                OccupyingUnit.gameObject.SetActive(false);
            }
            else
            {
                OccupyingUnit = capturer.GetComponent<Unit>();
            }

            capturedState.Alliance.SetAlliedFaction(OccupyingUnit.Alliance.AlliedFaction);
            capturedState.GetComponent<Ejector>().onEjected += HandleEjected;

            SwitchState(true);
        }

        void HandleEjected(Vector2 targetPosition)
        {
            if (OccupyingUnit == null) return;

            OccupyingUnit.transform.position = targetPosition;

            SwitchState(false);

            OccupyingUnit = null;
        }

        private void HandleTurnStarted()
        {
            if(OccupyingUnit) OccupyingUnit.ResetActionStates();
            capturedState.ResetActionStates();
        }

        private void OnDisable()
        {
            inanimateState.onCaptured -= HandleCaptured;
        }
    }
}
