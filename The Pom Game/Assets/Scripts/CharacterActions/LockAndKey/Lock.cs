using Pom.UndoSystem;
using UnityEngine;
using UnityEngine.Events;

namespace Pom.CharacterActions.LockAndKey
{

    public class Lock : MonoBehaviour, ICacheable
    {
        public UnityEvent onUnlocked;

        public object CaptureState()
        {
            return gameObject.activeSelf;
        }

        public void RestoreState(object state)
        {
            gameObject.SetActive((bool)state);
        }
    }
}
