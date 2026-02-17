using Pom.AnimationHandling;
using Pom.UndoSystem;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Pom.Attributes
{
    public class Health : MonoBehaviour, ICacheable
    {
        [field: SerializeField] public int StartingHealth { get; private set; }

        public int CurrentHealth { get; private set; }

        public UnityEvent<GameObject> onTakeDamage;
        public UnityEvent onDeath;

        AnimationStateMachine animationStateMachine => GetComponent<AnimationStateMachine>();

        private void Start()
        {
            CurrentHealth = StartingHealth;
        }

        void Update()
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                TakeDamage(1, null);
            }
        }

        public void TakeDamage(int damage, GameObject aggressor)
        {
            if(CurrentHealth > 0)
                CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

            if(aggressor != null)
            onTakeDamage?.Invoke(aggressor);

            if(CurrentHealth == 0)
            {
                Die();
            }
            else
            {
                animationStateMachine.SwitchState(AnimationTag.Hit);
            }
        }

        private void Die()
        {
            animationStateMachine.SwitchState(AnimationTag.Death);
            animationStateMachine.onCurrentAnimationFinished += () => { gameObject.SetActive(false); };
            onDeath?.Invoke();
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
