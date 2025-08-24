using Pom.Objectives;
using UnityEngine;

namespace Pom.UI.Objectives
{
    public class ObjectivesPanelPopulator : MonoBehaviour
    {
        [SerializeField] ObjectivesList objectivesList;
        [SerializeField] Transform objectivesPanel;
        [SerializeField] ObjectivePresenter objectivePresenterPrefab;

        private void Start()
        {
            foreach (Objective objective in objectivesList.Objectives)
            {
                ObjectivePresenter objectivePresenter = Instantiate(objectivePresenterPrefab, objectivesPanel.transform);
                objectivePresenter.Setup(objective);
            }
        }
    }
}
