using Pom.Objectives;
using UnityEngine;

namespace Pom.UI.Objectives
{
    public class ObjectivesPanelPopulator : MonoBehaviour
    {
        [SerializeField] ObjectivesList objectivesList;
        [SerializeField] Transform objectivesPanel;
        [SerializeField] ObjectivePresenter objectivePresenterPrefab;
        [SerializeField] Color victoryObjectiveCompletionColor;
        [SerializeField] Color defeatObjectiveCompletionColor;

        private void Start()
        {
            foreach(Objective objective in objectivesList.VictoryObjectives)
            {
                ObjectivePresenter objectivePresenter = Instantiate(objectivePresenterPrefab, objectivesPanel);
                objectivePresenter.Setup(objective, victoryObjectiveCompletionColor);
            }

            foreach (Objective objective in objectivesList.DefeatObjectives)
            {
                ObjectivePresenter objectivePresenter = Instantiate(objectivePresenterPrefab, objectivesPanel);
                objectivePresenter.Setup(objective, defeatObjectiveCompletionColor);
            }
        }
    }
}
