using Pom.Control;
using Pom.TurnSystem;
using Pom.Units;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Pom.UI.Control
{
    public class ActionButton : MonoBehaviour
    {
        [field: SerializeField] public PlayerController.PlayerState state;

        private void OnEnable()
        {
            PlayerController.Instance.onUnitSelected += HandleUnitSelected;
            PlayerController.Instance.onActiveUnitAction += SetButtonInteractableState;
            TurnShifter.Instance.onTurnShifted += HandleTurnShifted;
        }

        private void SetButtonInteractableState()
        {
            GetComponent<Button>().interactable = CalculateInteractableState();
        }

        private bool CalculateInteractableState()
        {
            if (!PlayerController.Instance.CanUseState(state)) return false;
            if (TurnShifter.Instance.GetActiveController() != PlayerController.Instance) return false;

            return true;
        }

        void HandleUnitSelected(Unit unit)
        {
            SetButtonInteractableState();
        }

        private void HandleTurnShifted(Controller controller)
        {
            SetButtonInteractableState();
        }


        private void OnDisable()
        {
            PlayerController.Instance.onUnitSelected -= HandleUnitSelected;
            PlayerController.Instance.onActiveUnitAction -= SetButtonInteractableState;
            TurnShifter.Instance.onTurnShifted -= HandleTurnShifted;
        }
    }
}
