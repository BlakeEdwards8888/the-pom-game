using Pom.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pom.Navigation.PathFinder;

namespace Pom.CharacterActions.Movement
{
    public class Mover : ActionExecutor
    {
        const float MINIMUM_STOPPING_DISTANCE = 0.1f;

        [SerializeField] float moveSpeed;

        PathFinder pathFinder => GetComponent<PathFinder>();

        public override string GetDisplayName()
        {
            return "Move";
        }

        public IEnumerator MoveAlongPath(List<PathNode> path, Action finished)
        {
            foreach (PathNode pathNode in path)
            {
                yield return MoveToNewDestination(pathNode.Position);
            }

            finished?.Invoke();
        }

        public IEnumerator MoveToNewDestination(Vector2 destination)
        {
            while (Vector2.Distance(transform.position, destination) > MINIMUM_STOPPING_DISTANCE)
            {
                Vector2 direction = (destination - (Vector2)transform.position).normalized;
                Vector2 movement = direction * moveSpeed * Time.deltaTime;

                transform.Translate(movement);

                yield return null;
            }

            transform.position = destination;
        }

        public override bool TryExecute(Vector2 targetPosition, List<ActionExecutionArg> executionArgs, Action finished)
        {
            if (IsUsed) return false;

            Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

            RangeOverflowMode rangeOverflowMode = RangeOverflowMode.Cancel;

            if (executionArgs != null)
            {
                foreach (ActionExecutionArg executionArg in executionArgs)
                {
                    if (executionArg.predicate == "RangeOverflowMode")
                    {
                        if (Enum.TryParse(executionArg.value, out RangeOverflowMode result))
                        {
                            rangeOverflowMode = result;
                            break;
                        }
                    }
                }
            }

            List<PathNode> path = pathFinder.GetPath(currentGridPosition, targetPosition, GetRange(), rangeOverflowMode);

            if (path == null) return false;

            Execute(path, finished);

            return true;
        }

        protected override void Execute(object args, Action finished)
        {
            base.Execute(args, finished);

            List<PathNode> path = args as List<PathNode>;

            StartCoroutine(MoveAlongPath(path, finished));
        }
    }
}
