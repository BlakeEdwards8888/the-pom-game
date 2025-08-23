using Pom.Alliances;
using Pom.Units;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Control
{
    public abstract class Controller : MonoBehaviour
    {
        protected Alliance alliance => GetComponent<Alliance>();

        protected List<Unit> controllableUnits = new List<Unit>();

        public virtual void InitiateTurn()
        {
            FindControllableUnits();

            foreach(Unit unit in controllableUnits)
            {
                unit.ResetActionStates();
            }
        }

        public virtual void ExitTurn(){ }

        protected void FindControllableUnits()
        {
            controllableUnits.Clear();

            Unit[] allUnits = FindObjectsByType<Unit>(FindObjectsSortMode.None);

            foreach (Unit unit in allUnits)
            {
                if (unit.Alliance.AlliedFaction == GetComponent<Alliance>().AlliedFaction)
                {
                    controllableUnits.Add(unit);
                }
            }
        }
    }
}
