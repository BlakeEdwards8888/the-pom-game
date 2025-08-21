using Pom.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Control
{
    public class AIController : Controller
    {
        const float DELAY_BETWEEN_ACTIONS = 0.5f;

        IEnumerator TurnCoroutine()
        {
            foreach (EnemyUnit unit in controllableUnits)
            {
                yield return unit.TakeTurn();
                yield return new WaitForSeconds(DELAY_BETWEEN_ACTIONS);
            }
        }

        public override void InitiateTurn()
        {
            base.InitiateTurn();

            StartCoroutine(TurnCoroutine());
        }
    }
}
