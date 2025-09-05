using UnityEngine;
using UnityEngine.Events;

namespace Pom.TurnSystem
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] int delay, duration;
        [SerializeField] bool loop;

        TurnShifter turnShifter;
        int elapsedDurationRounds;
        int elapsedDelayRounds;

        public UnityEvent onTimerFinished;

        private void OnEnable()
        {
            turnShifter = TurnShifter.Instance;
            turnShifter.onRoundIncremented += TurnShifter_OnRoundIncremented;
        }

        private void TurnShifter_OnRoundIncremented()
        {
            elapsedDelayRounds++;

            if (elapsedDelayRounds > delay)
            {
                elapsedDurationRounds++;

                if (elapsedDurationRounds == duration)
                {
                    onTimerFinished?.Invoke();
                    if(loop) elapsedDurationRounds = 0;
                }
            }
        }

        private void OnDisable()
        {
            turnShifter.onRoundIncremented -= TurnShifter_OnRoundIncremented;
        }
    }
}
