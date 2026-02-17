using Pom.Alliances;
using Pom.CharacterActions;
using Pom.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Pom.Control
{
    public abstract class Controller : MonoBehaviour
    {
        [SerializeField] protected List<ActionExecutionArg> actionArgs = new List<ActionExecutionArg>();

        protected Alliance alliance => GetComponent<Alliance>();

        protected List<Unit> controllableUnits = new List<Unit>();

        public event Action onTurnStarted;
        public event Action onActionCompleted;

        public UnityEvent onUnitsCleared;


        void Start()
        {
            FindControllableUnits();
        }

        public virtual void InitiateTurn()
        {
            FindControllableUnits();

            foreach(Unit unit in controllableUnits)
            {
                unit.ResetActionStates();
            }

            onTurnStarted?.Invoke();
        }

        public virtual void ExitTurn(){ }

        protected virtual void FindControllableUnits()
        {
            List<Unit> controllableUnitsCache = new List<Unit>(controllableUnits);

            controllableUnits.Clear();

            Unit[] allUnits = FindObjectsByType<Unit>(FindObjectsSortMode.None);

            foreach (Unit unit in allUnits)
            {
                if (unit.Health.CurrentHealth == 0) continue;
                if (unit.Alliance.AlliedFaction == GetComponent<Alliance>().AlliedFaction)
                {
                    controllableUnits.Add(unit);
                }
            }

            foreach (Unit unit in controllableUnits)
            {
                if (controllableUnitsCache.Contains(unit)) continue;
                unit.Health.onDeath.AddListener(HandleUnitDeath);
            }
        }

        protected void RaiseActionCompleted()
        {
            onActionCompleted?.Invoke();
        }

        protected void HandleUnitDeath()
        {
            FindControllableUnits();

            if (controllableUnits.Count == 0)
            {
                onUnitsCleared?.Invoke();
            }
        }

        public virtual void SetActiveUnit(Unit unit)
        {
            if (!controllableUnits.Contains(unit))
            {
                controllableUnits.Add(unit);
                unit.Health.onDeath.AddListener(HandleUnitDeath);
            }
        }

        public static Controller GetControllerByFaction(Faction faction)
        {
            foreach(Controller controller in FindObjectsByType<Controller>(FindObjectsSortMode.None))
            {
                if(controller.alliance.AlliedFaction == faction) return controller;
            }

            Debug.LogWarning($"No controllers found for faction {faction.ToString()}");
            return null;
        }
    }
}
