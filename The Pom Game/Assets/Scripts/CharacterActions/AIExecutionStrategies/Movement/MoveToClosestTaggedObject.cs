using Pom.CharacterActions.RangeHandling;
using Pom.Navigation;
using Pom.Units;
using UnityEngine;

namespace Pom.CharacterActions.AIExecutionStrategies.Movement
{
    [CreateAssetMenu(fileName = "New Move To Closest Tagged Object", menuName = "AI Execution Strategies/Move To Closest Tagged Object Strategy")]
    public class MoveToClosestTaggedObject : ExecutionStrategy
    {
        [SerializeField] string tag;

        public override bool TryGetTargetPosition(Unit currentUnit, out Vector2 targetPosition, RangeStrategy rangeStrategy)
        {
            targetPosition = Vector2.zero;

            GameObject[] targetableObjects = GameObject.FindGameObjectsWithTag(tag);

            if (targetableObjects.Length == 0) return false;

            targetPosition = GetClosestObjectPosition(currentUnit.Position, targetableObjects);

            return true;
        }

        private Vector2 GetClosestObjectPosition(Vector2 currentPosition, GameObject[] targetableObjects)
        {
            float closestDistance = Mathf.Infinity;
            GameObject closestObject = null;

            foreach (GameObject obj in targetableObjects)
            {
                float distance = GridSystem.GetDistance(currentPosition, obj.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestObject = obj;
                }
            }

            return closestObject.transform.position;
        }
    }
}
