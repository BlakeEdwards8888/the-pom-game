using Pom.Control;
using Pom.Units;
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
            SetButtonInteractableState();
        }

        private void SetButtonInteractableState()
        {
            GetComponent<Button>().interactable = PlayerController.Instance.CanUseState(state);
        }

        void HandleUnitSelected(Unit unit)
        {
            SetButtonInteractableState();
        }

        private void OnDisable()
        {
            PlayerController.Instance.onUnitSelected -= HandleUnitSelected;
            PlayerController.Instance.onActiveUnitAction -= SetButtonInteractableState;
        }
    }
}
