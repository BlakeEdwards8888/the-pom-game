using UnityEngine;

namespace Pom.Navigation
{
    public class PathFinder : MonoBehaviour
    {
        public void SetDestination(Vector2 destination)
        {
            Vector2 currentPosition = GridSystem.Instance.GetGridPosition(transform.position);
            float cellSize = GridSystem.Instance.CellSize;
        }
    }
}
