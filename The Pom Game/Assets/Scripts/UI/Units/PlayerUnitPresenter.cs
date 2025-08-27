using Pom.CharacterActions;
using Pom.Control;
using Pom.UI.Control;
using Pom.Units;
using System;
using TMPro;
using UnityEngine;

namespace Pom.UI.Units
{
    public class PlayerUnitPresenter : MonoBehaviour
    {
        [SerializeField] TMP_Text namePresenter;
        [SerializeField] TMP_Text healthPresenter;
        [SerializeField] Transform actionButtonContainer;
        [SerializeField] ActionButton actionButtonPrefab;

        Unit unit;
        PlayerController playerController;

        private void OnEnable()
        {
            playerController = PlayerController.Instance;

            playerController.onUnitSelected += HandleUnitSelected;
        }

        private void HandleUnitSelected(Unit unit)
        {
            if(this.unit != null) this.unit.Health.onTakeDamage -= UpdateHealthText;

            this.unit = unit;
            namePresenter.text = unit.DisplayName;
            unit.Health.onTakeDamage += UpdateHealthText;
            UpdateHealthText();

            for (int i = actionButtonContainer.childCount - 1; i >= 0; i--)
            {
                Transform child = actionButtonContainer.GetChild(i);

                Destroy(child.gameObject);
            }

            foreach (ActionExecutor action in unit.Actions)
            {
                ActionButton actionButton = Instantiate(actionButtonPrefab, actionButtonContainer);
                actionButton.Setup(action);
            }

            GetComponent<UIToggler>().ToggleUI(true);
            Canvas.ForceUpdateCanvases();
        }

        private void UpdateHealthText()
        {
            healthPresenter.text = $"health: {unit.Health.CurrentHealth}/{unit.Health.StartingHealth}";
        }

        public void ClearActiveUnit()
        {
            playerController.ClearActiveUnit();
        }

        private void OnDisable()
        {
            playerController.onUnitSelected -= HandleUnitSelected;
            unit.Health.onTakeDamage -= UpdateHealthText;
        }
    }
}
