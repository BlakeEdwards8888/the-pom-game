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

        PlayerController playerController;
        TurnShifter turnShifter;

        private void OnEnable()
        {
            playerController = PlayerController.Instance;
            turnShifter = TurnShifter.Instance;

            playerController.onUnitSelected += HandleUnitSelected;
            playerController.onActiveUnitAction += SetButtonInteractableState;
            turnShifter.onTurnShifted += HandleTurnShifted;
        }

        private void SetButtonInteractableState()
        {
            GetComponent<Button>().interactable = CalculateInteractableState();
        }

        private bool CalculateInteractableState()
        {
            if (!playerController.CanUseState(state)) return false;
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
            playerController.onUnitSelected -= HandleUnitSelected;
            playerController.onActiveUnitAction -= SetButtonInteractableState;
            turnShifter.onTurnShifted -= HandleTurnShifted;
        }
    }
}
