using Pom.Movement;
using Pom.Navigation;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Control
{
    public class PlayerController : MonoBehaviour
    {

        Mover mover => GetComponent<Mover>();

        private void Start()
        {
                Debug.Log($"Grid system: {GridSystem.Instance.name}");
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Vector2 gridPosition = GridSystem.Instance.GetGridPosition(worldMousePosition);

                Debug.Log("Attempting to move to " + gridPosition);

                mover.SetDestination(gridPosition);
            }
        }
    }
}
