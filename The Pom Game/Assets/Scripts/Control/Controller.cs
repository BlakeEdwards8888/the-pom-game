using Pom.Alliances;
using Pom.CharacterActions;
using Pom.Units;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Control
{
    public abstract class Controller : MonoBehaviour
    {
        [SerializeField] protected List<ActionExecutionArg> actionArgs = new List<ActionExecutionArg>();

        protected Alliance alliance => GetComponent<Alliance>();

        protected List<Unit> controllableUnits = new List<Unit>();

        public event Action onTurnStarted;

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
        }

        public abstract void SetActiveUnit(Unit unit);


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
