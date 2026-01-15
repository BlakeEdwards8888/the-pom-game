using Pom.CharacterActions;
using Pom.Navigation;
using Pom.Navigation.Presentation;
using Pom.UndoSystem;
using Pom.Units;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Pom.Control
{
    public class PlayerController : Controller, ICacheable
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

        public BoardStateCache BoardStateCache 
        { 
            get
            {
                return GetComponent<BoardStateCache>();
            }
            private set { }
        }

        RangePresenter rangePresenter => GetComponent<RangePresenter>();
        Unit activeUnit;
        ActionExecutor activeAction;
        bool hasCurrentTurn;

        public event Action<Unit> onUnitSelected;
        public event Action onActiveUnitCleared;


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

            HandleAction();
        }

        private bool HandleUnitSelection()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mouseGridPosition = GetMouseGridPosition();

                if (GridSystem.Instance.NavDict.ContainsKey(mouseGridPosition) && GridSystem.Instance.NavDict[mouseGridPosition].TryGetOccupyingEntity(out Unit unit))
                {
                    if (unit.Alliance.AlliedFaction != alliance.AlliedFaction) return false;
                    SetActiveUnit(unit);
                    return true;
                }
            }

            return false;
        }

        private void HandleAction()
        {
            if (activeAction == null) return;
            if (activeUnit == null) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (!hasCurrentTurn) return;

            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                Vector2 mouseGridPosition = GetMouseGridPosition();
                ActionExecutor usingAction = activeAction;
                SetActiveAction(null);
                BoardStateCache.CaptureState();
                usingAction.TryExecute(mouseGridPosition, actionArgs, RaiseActionCompleted);
            }
        }

        public void SetActiveAction(ActionExecutor action)
        {
            activeAction = action;

            if(activeAction == null || activeAction.IsUsed)
            {
                rangePresenter.ClearSelectableNodes();
            }
            else
            {
                rangePresenter.ShowSelectableNodes(activeAction.GetNodesInRange(activeUnit.Position, (node) => { return node.IsWalkable(); }));
            }
        }

        public void ClearActiveUnit()
        {
            activeUnit = null;
            SetActiveAction(null);
            onActiveUnitCleared?.Invoke();
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

            hasCurrentTurn = true;
        }

        public override void ExitTurn()
        {
            base.ExitTurn();

            SetActiveAction(null);
            hasCurrentTurn = false;
        }

        public override void SetActiveUnit(Unit unit)
        {
            base.SetActiveUnit(unit);

            activeUnit = unit;
            SetActiveAction(unit.Actions[0]);
            onUnitSelected?.Invoke(activeUnit);
        }

        public object CaptureState()
        {
            Dictionary<string, object> state = new Dictionary<string, object>();

            state["active_unit"] = activeUnit;
            state["active_action"] = activeAction;

            return state;
        }

        public void RestoreState(object state)
        {
            Dictionary<string, object> localState = state as Dictionary<string, object>;

            SetActiveUnit((Unit)localState["active_unit"]);
            SetActiveAction((ActionExecutor)localState["active_action"]);
        }
    }
}
