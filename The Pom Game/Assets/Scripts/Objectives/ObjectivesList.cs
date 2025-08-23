using Pom.TurnSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Objectives
{
    public class ObjectivesList : MonoBehaviour
    {
        [field: SerializeField] public List<Objective> VictoryObjectives { get; private set; } = new List<Objective>();
        [field: SerializeField] public List<Objective> DefeatObjectives { get; private set; } = new List<Objective>();

        Dictionary<Objective, bool> victoryObjectiveState = new Dictionary<Objective, bool>();
        Dictionary<Objective, bool> defeatObjectiveState = new Dictionary<Objective, bool>();

        public event Action<Objective> onObjectiveCompleted;

        private void OnEnable()
        {
            foreach (Objective objective in VictoryObjectives)
            {
                victoryObjectiveState[objective] = false;
            }

            foreach (Objective objective in DefeatObjectives)
            {
                defeatObjectiveState[objective] = false;
            }

            TurnShifter.Instance.onFinalRoundComplete += HandleFinalRoundComplete;
        }

        public bool AreAllVictoryObjectivesComplete()
        {
            foreach (KeyValuePair<Objective, bool> objectiveState in victoryObjectiveState)
            {
                if (!objectiveState.Value) return false;
            }

            return true;
        }

        public void CompleteObjective(Objective objective)
        {
            if (victoryObjectiveState.ContainsKey(objective))
            {
                victoryObjectiveState[objective] = true;

                if(AreAllVictoryObjectivesComplete()) Debug.Log("Completed all objectives!");
            }
            else if (defeatObjectiveState.ContainsKey(objective))
            {
                defeatObjectiveState[objective] = true;

                Debug.Log("You just lost the game");
            }
            else
            {
                Debug.Log($"No objective in list with description {objective.Description}");
                return;
            }

            onObjectiveCompleted?.Invoke(objective);
        }

        private void HandleFinalRoundComplete()
        {
            if (!AreAllVictoryObjectivesComplete())
            {
                Debug.Log("You just lost the game");
            }
            else
            {
                Debug.Log("Completed all objectives!");
            }
        }


        private void OnDisable()
        {
            TurnShifter.Instance.onFinalRoundComplete -= HandleFinalRoundComplete;
        }

    }
}
