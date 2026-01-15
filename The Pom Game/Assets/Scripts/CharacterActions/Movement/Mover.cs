using Pom.Navigation;
using Pom.UndoSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Pom.Navigation.PathFinder;

namespace Pom.CharacterActions.Movement
{
    public class Mover : ActionExecutor, ICacheable
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

        public override object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();
            
            state["position"] = (Vector2)transform.position;
            state["is_used"] = IsUsed;
            
            return state;
        }

        public override void RestoreState(object state)
        {
            Dictionary<string,object> localState = state as Dictionary<string,object>;

            transform.position = GridSystem.Instance.GetGridPosition((Vector2)localState["position"]);
            IsUsed = (bool)localState["is_used"];
        }
    }
}
