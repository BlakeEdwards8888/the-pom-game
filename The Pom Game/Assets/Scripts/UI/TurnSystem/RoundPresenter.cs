using Pom.TurnSystem;
using System;
using TMPro;
using UnityEngine;

namespace Pom.UI.TurnSystem
{
    public class RoundPresenter : MonoBehaviour
    {
        [SerializeField] TMP_Text roundText;

        TurnShifter turnShifter;

        private void OnEnable()
        {
            turnShifter = TurnShifter.Instance;
            turnShifter.onRoundIncremented += UpdateText;
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
            turnShifter.onRoundIncremented -= UpdateText;
        }
    }
}
