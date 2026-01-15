using Pom.UndoSystem;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Pom.Attributes
{
    public class Health : MonoBehaviour, ICacheable
    {
        [field: SerializeField] public int StartingHealth { get; private set; }

        public int CurrentHealth { get; private set; }

        public UnityEvent<GameObject> onTakeDamage;
        public UnityEvent onDeath;

        private void Start()
        {
            CurrentHealth = StartingHealth;
        }

        public void TakeDamage(int damage, GameObject aggressor)
        {
            if(CurrentHealth > 0)
                CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

            onTakeDamage?.Invoke(aggressor);

            if(CurrentHealth == 0)
            {
                onDeath?.Invoke();
                gameObject.SetActive(false);
            }
        }

        public object CaptureState()
        {
            return CurrentHealth;
        }

        public void RestoreState(object state)
        {
            CurrentHealth = (int)state;
        }
    }
}
