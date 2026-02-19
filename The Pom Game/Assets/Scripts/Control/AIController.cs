using Pom.CharacterActions;
using Pom.TurnSystem;
using Pom.Units;
using UnityEngine;

namespace Pom.Control
{
    public class AIController : Controller
    {
        const float DELAY_BETWEEN_ACTIONS = 0.5f;

        int unitIndex;
        int actionIndex;

        private void StartTurn()
        {
            unitIndex = 0;
            actionIndex = 0;

            ExecuteNextAction();
        }

        void ExecuteNextAction()
        {
            if(unitIndex >= controllableUnits.Count)
            {
                TurnShifter.Instance.ShiftTurns();
                return;
            }

            Unit currentUnit = controllableUnits[unitIndex];

            if (actionIndex >= currentUnit.Actions.Count)
            {
                IterateUnitIndex();
                actionIndex = 0;
                ExecuteNextAction();
                return;
            }

            ActionExecutor activeAction = currentUnit.Actions[actionIndex];

            if(activeAction.AIExecutionStrategy.TryGetTargetPosition(currentUnit, out Vector2 targetPosition, activeAction.RangeStrategy))
            {
                if(currentUnit.Actions[actionIndex].TryExecute(targetPosition, actionArgs,
                    () => {
                        RaiseActionCompleted();
                        IterateActionIndex();
                        ExecuteNextAction(); 
                    }))
                {
                        return;
                }
            }

            IterateActionIndex();
            ExecuteNextAction();
        }

        void IterateUnitIndex()
        {
            unitIndex++;
        }

        void IterateActionIndex()
        {
            actionIndex++;
        }

        public override void InitiateTurn()
        {
            base.InitiateTurn();

            StartTurn();
        }

        public override void SetActiveUnit(Unit unit)
        {
            base.SetActiveUnit(unit);

            controllableUnits[unitIndex] = unit;

            //Setting actionIndex to -1 here to make sure that when the next action gets executed,
            //We're starting over with this new unit from zero
            actionIndex = -1;
        }
    }
}
