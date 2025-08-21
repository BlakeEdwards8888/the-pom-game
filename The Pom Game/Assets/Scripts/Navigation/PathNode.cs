using UnityEditor;
using UnityEngine;

namespace Pom.Navigation
{
    public class PathNode
    {
        const float CIRCLE_CAST_RADIUS = 0.2f;
        const float CIRCLE_CAST_DISTANCE = 0.1f;

        public Vector2 Position { get; private set; }

        LayerMask obstacleLayerMask;
        PathNode previousNode;
        GameObject selectableIndicator;
 

        public PathNode(Vector2 position, LayerMask obstacleLayerMask, GameObject selectableIndicator)
        {
            Position = position;
            this.obstacleLayerMask = obstacleLayerMask;
            this.selectableIndicator = selectableIndicator;
        }

        public void Reset()
        {
            previousNode = null;
        }

        public float GetScore(Vector2 startingPosition, Vector2 endingPosition)
        {
            return GridSystem.GetDistance(startingPosition, Position) + GridSystem.GetDistance(endingPosition, Position);
        }

        public bool IsWalkable()
        {
            return !Physics2D.CircleCast(Position, CIRCLE_CAST_RADIUS, Vector2.up, CIRCLE_CAST_DISTANCE, obstacleLayerMask);
        }

        public void SetPreviousNode(PathNode previousNode)
        {
            this.previousNode = previousNode;
        }

        public PathNode GetPreviousNode()
        {
            return previousNode;
        }

        public bool TryGetOccupyingEntity<T>(out T entity)
        {
            entity = default;
            RaycastHit2D[] hits = Physics2D.CircleCastAll(Position, CIRCLE_CAST_RADIUS, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if(hit.collider.TryGetComponent(out T component))
                {
                    entity = component;
                    return true;
                }
            }

            return false;
        }

        public void ToggleSelectableIndicator(bool toggle)
        {
            selectableIndicator.SetActive(toggle);
        }
    }
}
