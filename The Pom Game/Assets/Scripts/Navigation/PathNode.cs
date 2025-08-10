using UnityEditor;
using UnityEngine;

namespace Pom.Navigation
{
    public class PathNode
    {
        public Vector2 Position { get; private set; }

        LayerMask obstacleLayerMask;
        PathNode previousNode;
        float fScore = Mathf.Infinity;
        PathNodeDebugObject pathNodeDebugObject;
 

        public PathNode(Vector2 position, LayerMask obstacleLayerMask)
        {
            Position = position;
            this.obstacleLayerMask = obstacleLayerMask;
        }

        public void Reset()
        {
            fScore = Mathf.Infinity;
            previousNode = null;
        }

        public float GetDistanceFromPoint(Vector2 point)
        {
            Vector2 gridDistance = point - Position;

            float xScore = Mathf.Abs(gridDistance.x);
            float yScore = Mathf.Abs(gridDistance.y);

            return xScore + yScore;
        }

        public float GetScore(Vector2 startingPosition, Vector2 endingPosition)
        {
            if (fScore != Mathf.Infinity) return fScore;

            fScore = GetDistanceFromPoint(startingPosition) + GetDistanceFromPoint(endingPosition);

            return fScore;
        }

        public bool IsWalkable()
        {
            return !Physics.Raycast(Position, Vector3.back, 1, obstacleLayerMask);
        }

        public void SetPreviousNode(PathNode previousNode)
        {
            this.previousNode = previousNode;
        }

        public void SetPathNodeDebugObject(PathNodeDebugObject pathNodeDebugObject)
        {
            this.pathNodeDebugObject = pathNodeDebugObject;
        }

        public PathNode GetPreviousNode()
        {
            return previousNode;
        }

        public PathNodeDebugObject GetPathNodeDebugObject()
        {
            return pathNodeDebugObject;
        }
    }
}
