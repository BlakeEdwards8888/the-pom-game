using Pom.Control;
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

        Unit unit;

        private void OnEnable()
        {
            PlayerController.Instance.onUnitSelected += HandleUnitSelected;
        }

        private void HandleUnitSelected(Unit unit)
        {
            if(unit != null) unit.Health.onTakeDamage -= UpdateHealthText;

            this.unit = unit;
            namePresenter.text = unit.DisplayName;
            unit.Health.onTakeDamage += UpdateHealthText;
            UpdateHealthText();

            GetComponent<UIToggler>().ToggleUI(true);
        }

        private void UpdateHealthText()
        {
            healthPresenter.text = $"health: {unit.Health.CurrentHealth}/{unit.Health.StartingHealth}";
        }

        private void OnDisable()
        {
            PlayerController.Instance.onUnitSelected -= HandleUnitSelected;
            unit.Health.onTakeDamage -= UpdateHealthText;
        }
    }
}
