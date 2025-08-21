using Pom.Control;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.TurnSystem
{
    public class TurnShifter : MonoBehaviour
    {
        [SerializeField] List<Controller> controllers = new List<Controller>();

        int turnIndex;

        public void ShiftTurns()
        {
            turnIndex++;

            if (turnIndex >= controllers.Count)
            {
                turnIndex = 0;
            }

            controllers[turnIndex].InitiateTurn();
        }
    }
}
