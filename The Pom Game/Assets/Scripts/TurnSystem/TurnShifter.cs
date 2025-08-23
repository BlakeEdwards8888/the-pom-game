using Pom.Control;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.TurnSystem
{
    public class TurnShifter : MonoBehaviour
    {
        public static TurnShifter Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = FindFirstObjectByType<TurnShifter>();
                }

                return _instance;
            }
            private set { }
        }

        static TurnShifter _instance;

        [SerializeField] List<Controller> controllers = new List<Controller>();

        int turnIndex;

        public event Action<Controller> onTurnShifted;

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }else if(_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            controllers[0].InitiateTurn();
        }

        public void ShiftTurns()
        {
            controllers[turnIndex].ExitTurn();

            turnIndex++;

            if (turnIndex >= controllers.Count)
            {
                turnIndex = 0;
            }

            controllers[turnIndex].InitiateTurn();

            onTurnShifted?.Invoke(controllers[turnIndex]);
        }

        public Controller GetActiveController()
        {
            return controllers[turnIndex];
        }
    }
}
