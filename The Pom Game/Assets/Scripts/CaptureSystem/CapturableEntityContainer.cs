using Pom.CharacterActions.Capturing;
using Pom.Control;
using Pom.Navigation;
using Pom.UndoSystem;
using Pom.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.CaptureSystem
{
    public class CapturableEntityContainer : MonoBehaviour, ICacheable
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

            inanimateState.transform.position = capturedState.transform.position;

            if (OccupyingUnit == null) return;

            OccupyingUnit.gameObject.SetActive(!isCaptured);

            Controller alliedController = Controller.GetControllerByFaction(OccupyingUnit.Alliance.AlliedFaction);

            if(alliedController != activeController)
            {
                if(activeController != null) activeController.onTurnStarted -= HandleTurnStarted;
                activeController = alliedController;
                activeController.onTurnStarted += HandleTurnStarted;
            }

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

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            state["position"] = (Vector2)transform.position;
            state["is_captured"] = capturedState.gameObject.activeSelf;
            state["occupying_unit"] = OccupyingUnit;

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> localState = state as Dictionary<string, object>;

            transform.position = GridSystem.Instance.GetGridPosition((Vector2)localState["position"]);
            OccupyingUnit = (Unit)localState["occupying_unit"];
            SwitchState((bool)localState["is_captured"]);
        }
    }
}
