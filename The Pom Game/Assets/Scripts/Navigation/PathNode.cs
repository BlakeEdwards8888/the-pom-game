using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

namespace Pom.Navigation
{
    public class PathNode
    {
        const float CIRCLE_CAST_RADIUS = 0.2f;
        const float CIRCLE_CAST_DISTANCE = 0.1f;

        public Vector2 Position { get; private set; }

        LayerMask obstacleLayerMask;
        LayerMask semipermeableLayerMask;
        PathNode previousNode;
        GameObject selectableIndicator;
 

        public PathNode(Vector2 position, LayerMask obstacleLayerMask, LayerMask semipermeableLayerMask, GameObject selectableIndicator)
        {
            Position = position;
            this.obstacleLayerMask = obstacleLayerMask;
            this.semipermeableLayerMask = semipermeableLayerMask;
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

        public bool IsSemipermeable()
        {
            return Physics2D.CircleCast(Position, CIRCLE_CAST_RADIUS, Vector2.up, CIRCLE_CAST_DISTANCE, semipermeableLayerMask);
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

        public bool TryGetAllOccupyingEntities<T>(out List<T> entities)
        {
            entities = new List<T>();
            RaycastHit2D[] hits = Physics2D.CircleCastAll(Position, CIRCLE_CAST_RADIUS, Vector2.zero);

            foreach (RaycastHit2D hit in hits)
            {
                if (hit.collider.TryGetComponent(out T component))
                {
                    entities.Add(component);
                }
            }

            return entities.Count > 0;
        }

        public void ToggleSelectableIndicator(bool toggle)
        {
            selectableIndicator.SetActive(toggle);
        }
    }
}
