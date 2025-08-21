using UnityEngine;

namespace Pom.Attributes
{
    public class Health : MonoBehaviour
    {
        [field: SerializeField] public int StartingHealth { get; private set; }

        public int CurrentHealth { get; private set; }

        private void Start()
        {
            CurrentHealth = StartingHealth;
        }

        public void TakeDamage(int damage)
        {
            CurrentHealth = Mathf.Max(CurrentHealth - damage, 0);

            Debug.Log($"{gameObject.name} has {CurrentHealth} health remaining");

            if(CurrentHealth == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
