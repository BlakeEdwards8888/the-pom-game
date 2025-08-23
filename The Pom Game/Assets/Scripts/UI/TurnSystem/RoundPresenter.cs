using Pom.TurnSystem;
using System;
using TMPro;
using UnityEngine;

namespace Pom.UI.TurnSystem
{
    public class RoundPresenter : MonoBehaviour
    {
        [SerializeField] TMP_Text roundText;

        private void OnEnable()
        {
            TurnShifter.Instance.onRoundIncremented += UpdateText;
        }

        private void Start()
        {
            UpdateText();
        }

        private void UpdateText()
        {
            roundText.text = $"Round {TurnShifter.Instance.CurrentRound}";

            if (TurnShifter.Instance.MaxRounds > 0) roundText.text += $"/{TurnShifter.Instance.MaxRounds}";
        }

        private void OnDisable()
        {
            TurnShifter.Instance.onRoundIncremented -= UpdateText;
        }
    }
}
