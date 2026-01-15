using Pom.CharacterActions;
using Pom.Control;
using Pom.TurnSystem;
using Pom.UndoSystem;
using Pom.Units;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Pom.UI.Control
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] TMP_Text buttonText;

        PlayerController playerController;
        TurnShifter turnShifter;
        ActionExecutor action;

        public void Setup(ActionExecutor action)
        {
            playerController = PlayerController.Instance;
            turnShifter = TurnShifter.Instance;

            playerController.onUnitSelected += HandleUnitSelected;
            turnShifter.onTurnShifted += HandleTurnShifted;

            this.action = action;
            buttonText.text = action.GetDisplayName();
            SetButtonInteractableState();
            action.onActionStarted += SetButtonInteractableState;
        }

        private void SetButtonInteractableState()
        {
            GetComponent<Button>().interactable = CalculateInteractableState();
        }

        private bool CalculateInteractableState()
        {
            if (action.IsUsed) return false;
            if (turnShifter.GetActiveController() != playerController) return false;

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

        public void SetPlayerAction()
        {
            playerController.SetActiveAction(action);
        }

        private void OnDisable()
        {
            playerController.onUnitSelected -= HandleUnitSelected;
            turnShifter.onTurnShifted -= HandleTurnShifted;
            action.onActionStarted -= SetButtonInteractableState;
        }
    }
}
