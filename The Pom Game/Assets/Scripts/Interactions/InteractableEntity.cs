using Pom.Alliances;
using Pom.Control;
using UnityEngine;
using UnityEngine.Events;
using Pom.Units;
using Pom.Navigation;
using System.Collections.Generic;

namespace Pom.Interactions
{
    public class InteractableEntity : MonoBehaviour
    {
        [SerializeField] Faction interactableFilter;

        public UnityEvent onInteracted;

        private void OnEnable()
        {
            foreach (Controller controller in FindObjectsByType<Controller>(FindObjectsSortMode.None))
            {
                controller.onActionCompleted += CheckForInteractions;
            }
        }

        void CheckForInteractions()
        {
            if (!GridSystem.Instance.NavDict[transform.position].TryGetAllOccupyingEntities(out List<Unit> overlappingUnits)) return;

            foreach (Unit unit in overlappingUnits)
            {
                if (unit.gameObject == gameObject) continue;

                if (unit.Alliance.AlliedFaction == interactableFilter)
                {
                    onInteracted?.Invoke();
                    return;
                }
            }
        }

        private void OnDisable()
        {
            foreach (Controller controller in FindObjectsByType<Controller>(FindObjectsSortMode.None))
            {
                controller.onActionCompleted -= CheckForInteractions;
            }
        }
    }
}
