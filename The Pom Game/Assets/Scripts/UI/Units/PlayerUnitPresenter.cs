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

        private void OnEnable()
        {
            PlayerController.Instance.onUnitSelected += HandleUnitSelected;
        }

        private void HandleUnitSelected(Unit unit)
        {
            namePresenter.text = unit.DisplayName;
            healthPresenter.text = $"health: {unit.Health.CurrentHealth}/{unit.Health.StartingHealth}";

            GetComponent<UIToggler>().ToggleUI(true);
        }

        private void OnDisable()
        {
            PlayerController.Instance.onUnitSelected -= HandleUnitSelected;
        }
    }
}
