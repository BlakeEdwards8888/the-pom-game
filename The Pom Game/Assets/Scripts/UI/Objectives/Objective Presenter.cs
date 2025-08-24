using Pom.Objectives;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace Pom.UI.Objectives
{
    public class ObjectivePresenter : MonoBehaviour
    {
        [SerializeField] TMP_Text descriptionText;
        [SerializeField] Color objectiveCompleteColor;
        [SerializeField] Color objectiveFailedColor;

        Objective objective;
        Color defaultColor;

        public UnityEvent HandleObjectiveFailed { get; private set; }

        private void Start()
        {
            defaultColor = descriptionText.color;
        }

        public void Setup(Objective objective)
        {
            this.objective = objective;
            descriptionText.text = objective.Description;

            objective.onStateChanged += HandleObjectiveStateChanged;
        }

        private void HandleObjectiveStateChanged()
        {
            switch (objective.State)
            {
                case ObjectiveState.InProgress:
                    descriptionText.color = defaultColor;
                    break;
                case ObjectiveState.Complete:
                    descriptionText.color = objectiveCompleteColor;
                    break;
                case ObjectiveState.Failed:
                    descriptionText.color = objectiveFailedColor;
                    break;
            }
        }
    }
}
