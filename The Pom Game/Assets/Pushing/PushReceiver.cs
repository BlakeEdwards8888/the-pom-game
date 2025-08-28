using Pom.CharacterActions.Movement;
using Pom.Navigation;
using UnityEngine;

namespace Pom.Pushing
{
    public class PushReceiver : MonoBehaviour
    {
        Mover mover => GetComponent<Mover>();

        public void TakePush(GameObject pusher)
        {
            Vector2 direction = (transform.position - pusher.transform.position).normalized;
            Vector2 endingPosition = (Vector2)transform.position + direction;

            if (GridSystem.Instance.TryGetGridPosition(endingPosition, out Vector2 finalGridPosition))
            {
                if (!GridSystem.Instance.NavDict[finalGridPosition].IsWalkable()) return;
                if (GridSystem.Instance.NavDict[finalGridPosition].IsSemipermeable()) return;

                StartCoroutine(mover.MoveToNewDestination(finalGridPosition));
            }
        }
    }
}
