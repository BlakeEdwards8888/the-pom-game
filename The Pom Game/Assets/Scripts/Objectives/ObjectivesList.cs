using Pom.TurnSystem;
using Pom.UI;
using Pom.UndoSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Objectives
{
    public class ObjectivesList : MonoBehaviour, ICacheable
    {
        public static ObjectivesList Instance
        {
            get
            {
                if(_instance == null)
                    _instance = FindFirstObjectByType<ObjectivesList>();

                return _instance;
            }
            private set { }
        }

        static ObjectivesList _instance;

        [SerializeField] UIToggler victoryScreen;
        [SerializeField] UIToggler failureScreen;

        [field: SerializeField] public List<Objective> Objectives { get; private set; } = new List<Objective>();

        TurnShifter turnShifter;

        Dictionary<string, Objective> objectiveDict;


        private void OnEnable()
        {
            turnShifter = TurnShifter.Instance;

            turnShifter.onFinalRoundComplete += HandleFinalRoundComplete;
        }

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            BuildObjectiveDict();
        }

        private void BuildObjectiveDict()
        {
            objectiveDict = new Dictionary<string, Objective>();

            foreach (Objective objective in Objectives)
            {
                objectiveDict[objective.Tag] = objective;
            }
        }

        public bool AreAllMandatoryObjectivesComplete()
        {
            foreach (Objective objective in Objectives)
            {
                if (!objective.MustBeCompleted) continue;
                if (objective.State != ObjectiveState.Complete) return false;
            }

            return true;
        }

        public void SetObjectiveState(string tag, ObjectiveState objectiveState)
        {
            if (!objectiveDict.ContainsKey(tag))
            {
                Debug.LogError($"No objective with tag {tag} contained in dictionary");
                return;
            }

            objectiveDict[tag].SetState(objectiveState);
        }

        private void HandleFinalRoundComplete()
        {
            if (!AreAllMandatoryObjectivesComplete())
            {
                TriggerFailureState();
            }
            else
            {
                TriggerVictoryState();
            }
        }

        public void TriggerVictoryState()
        {
            victoryScreen.ToggleUI(true);
        }

        public void TriggerFailureState()
        {
            foreach(Objective objective in Objectives)
            {
                if(objective.State == ObjectiveState.InProgress)
                {
                    objective.SetState(ObjectiveState.Failed);
                }
            }

            failureScreen.ToggleUI(true);
        }

        private void OnDisable()
        {
            turnShifter.onFinalRoundComplete -= HandleFinalRoundComplete;
        }

        public object CaptureState()
        {
            Dictionary<string, ObjectiveState> state = new Dictionary<string, ObjectiveState>();

            foreach(Objective objective in Objectives)
            {
                state[objective.Tag] = objective.State;
            }

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, ObjectiveState> localState = state as Dictionary<string, ObjectiveState>;

            foreach(KeyValuePair<string, ObjectiveState> kvp in localState)
            {
                objectiveDict[kvp.Key].SetState(kvp.Value);
            }
        }
    }
}
