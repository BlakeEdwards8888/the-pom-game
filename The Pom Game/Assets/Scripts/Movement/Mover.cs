using NUnit.Framework;
using Pom.Navigation;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pom.Movement
{
    public class Mover : MonoBehaviour
    {
        const float MINIMUM_STOPPING_DISTANCE = 0.1f;

        [SerializeField] float moveSpeed;

        public IEnumerator MoveAlongPath(List<PathNode> path)
        {
            foreach (PathNode pathNode in path)
            {
                yield return MoveToNewDestination(pathNode.Position);
            }
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
    }
}
