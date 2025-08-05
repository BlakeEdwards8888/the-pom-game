using UnityEngine;

namespace Pom.Movement
{
    public class Mover : MonoBehaviour
    {
        const float MINIMUM_STOPPING_DISTANCE = 0.1f;

        [SerializeField] float moveSpeed;

        Vector2 destination;

        private void Awake()
        {
            destination = transform.position;
        }

        private void Update()
        {
            if (Vector2.Distance(transform.position, destination) <= MINIMUM_STOPPING_DISTANCE)
            { 
                transform.position = destination;
                return; 
            }

            Vector2 direction = (destination - (Vector2)transform.position).normalized;
            Vector2 movement = direction * moveSpeed * Time.deltaTime;

            transform.Translate(movement);
        }

        public void SetDestination(Vector2 destination)
        {
            this.destination = destination;
        }
    }
}
