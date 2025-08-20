using UnityEngine;

namespace Pom.Attributes
{
    public class Health : MonoBehaviour
    {
        [SerializeField] int startingHealth;

        int currentHealth;

        private void Start()
        {
            currentHealth = startingHealth;
        }

        public void TakeDamage(int damage)
        {
            currentHealth = Mathf.Max(currentHealth - damage, 0);

            Debug.Log($"{gameObject.name} has {currentHealth} health remaining");

            if(currentHealth == 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
