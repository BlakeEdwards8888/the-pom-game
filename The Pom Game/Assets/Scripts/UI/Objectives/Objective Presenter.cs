using Pom.Objectives;
using System;
using TMPro;
using UnityEngine;

namespace Pom.UI.Objectives
{
    public class ObjectivePresenter : MonoBehaviour
    {
        [SerializeField] TMP_Text descriptionText;

        Objective objective;
        Color completionColor;

        public void Setup(Objective objective, Color completionColor)
        {
            this.objective = objective;
            descriptionText.text = objective.Description;
            this.completionColor = completionColor;

            FindFirstObjectByType<ObjectivesList>().onObjectiveCompleted += HandleObjectiveCompleted;
        }

        private void HandleObjectiveCompleted(Objective objective)
        {
            if (objective != this.objective) return;

            descriptionText.color = completionColor;
        }
    }
}
