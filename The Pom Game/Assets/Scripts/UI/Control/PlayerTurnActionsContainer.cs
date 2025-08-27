using Pom.Control;
using Pom.TurnSystem;
using UnityEngine;

namespace Pom.UI
{
    public class PlayerTurnActionsContainer : MonoBehaviour
    {
        UIToggler uiToggler => GetComponent<UIToggler>();

        TurnShifter turnShifter;

        private void OnEnable()
        {
            turnShifter = TurnShifter.Instance;
            turnShifter.onTurnShifted += HandleTurnShifted;
        }

        private void Start()
        {
            uiToggler.ToggleUI(TurnShifter.Instance.GetActiveController() == PlayerController.Instance);
        }

        private void HandleTurnShifted(Controller controller)
        {
            uiToggler.ToggleUI(controller == PlayerController.Instance);
        }

        private void OnDisable()
        {
            turnShifter.onTurnShifted -= HandleTurnShifted;
        }
    }
}
