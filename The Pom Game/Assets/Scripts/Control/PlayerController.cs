using Pom.Navigation;
using Pom.Navigation.Presentation;
using Pom.Units;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Control
{
    public class PlayerController : Controller
    {
        public enum PlayerState
        {
            Idle,
            Move,
            Attack
        }

        public static PlayerController Instance 
        { 
            get
            {
                if (_instance == null)
                    _instance = FindFirstObjectByType<PlayerController>();

                return _instance;
            }
            private set { } 
        }

        static PlayerController _instance;

        RangePresenter rangePresenter => GetComponent<RangePresenter>();
        PlayerUnit activeUnit;
        PlayerState currentState = PlayerState.Idle;

        public event Action<Unit> onUnitSelected;
        public event Action onActiveUnitAction;

        private void Awake()
        {
            if(_instance == null)
            {
                _instance = this;
            }
            else if(_instance != this)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (HandleUnitSelection()) return;

            switch (currentState) 
            {
                case PlayerState.Move:
                    HandleMovement();
                    break;
                case PlayerState.Attack:
                    HandleAttack();
                    break;
            }
        }

        private bool HandleUnitSelection()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (GridSystem.Instance.NavDict[GetMouseGridPosition()].TryGetOccupyingEntity(out PlayerUnit unit))
                {
                    SwitchState(PlayerState.Idle);
                    activeUnit = unit;
                    onUnitSelected?.Invoke(unit);
                    return true;
                }
            }

            return false;
        }

        private void HandleMovement()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (activeUnit.TryMoveTo(GetMouseGridPosition()))
                {
                    SwitchState(PlayerState.Idle);
                    onActiveUnitAction?.Invoke();
                }
            }
        }

        void HandleAttack()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                if (activeUnit.TryAttack(GetMouseGridPosition()))
                {
                    SwitchState(PlayerState.Idle);
                    onActiveUnitAction?.Invoke();
                }
            }
        }

        public void ClearActiveUnit()
        {
            activeUnit = null;
        }

        public void SwitchState(PlayerState newState)
        {
            HandleExitState();
            HandleEnterState(newState);

            currentState = newState;
        }

        void HandleEnterState(PlayerState newState)
        {
            Vector2 activeUnitGridPosition = Vector2.zero;

            if(activeUnit != null) activeUnitGridPosition = GridSystem.Instance.GetGridPosition(activeUnit.transform.position);

            switch (newState)
            {
                case PlayerState.Idle:
                    rangePresenter.ClearSelectableNodes();
                    break;
                case PlayerState.Move:
                    rangePresenter.ShowSelectableNodes(activeUnit.Mover.GetNodesInRange(activeUnitGridPosition, (node) => { return node.IsWalkable(); }));
                    break;
                case PlayerState.Attack:
                    rangePresenter.ShowSelectableNodes(activeUnit.Attacker.GetNodesInRange(activeUnitGridPosition,
                        (node) =>
                        {
                            if (!node.IsWalkable()) return false;

                            if (Physics2D.Linecast(node.Position, activeUnitGridPosition, GridSystem.Instance.ObstacleLayerMask)) return false;

                            return true;
                        }));
                    break;
            }
        }

        void HandleExitState()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    break;
                case PlayerState.Move:
                    break;
                case PlayerState.Attack:
                    break;
            }
        }

        public bool CanUseState(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Move:
                    return activeUnit.CanMove;
                case PlayerState.Attack:
                    return activeUnit.CanAttack;
            }

            return true;
        }

        private Vector2 GetMouseGridPosition()
        {
            Vector2 worldMousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            GridSystem.Instance.TryGetGridPosition(worldMousePosition, out Vector2 mouseGridPosition);
            return mouseGridPosition;
        }

        public override void InitiateTurn()
        {
            base.InitiateTurn();
        }
    }
}
