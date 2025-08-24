using UnityEngine;

namespace Pom.Objectives
{
    public class ObjectiveSignaler : MonoBehaviour
    {
        [SerializeField] string objectiveTag;
        [SerializeField] ObjectiveState objectiveState;

        public void SetObjectiveState()
        {
            ObjectivesList.Instance.SetObjectiveState(objectiveTag, objectiveState);
        }
    }
}
