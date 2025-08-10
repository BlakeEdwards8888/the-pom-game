using NUnit.Framework;
using Pom.Movement;
using Pom.Navigation;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Control
{
    public class PlayerController : MonoBehaviour
    {

        Mover mover => GetComponent<Mover>();
        PathFinder pathFinder => GetComponent<PathFinder>();

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2 mouseGridPosition = GridSystem.Instance.GetGridPosition(worldMousePosition);
                Vector2 currentGridPosition = GridSystem.Instance.GetGridPosition(transform.position);

                List<PathNode> path = pathFinder.GetPath(currentGridPosition, mouseGridPosition);

                StartCoroutine(mover.MoveAlongPath(path));
            }
        }
    }
}
