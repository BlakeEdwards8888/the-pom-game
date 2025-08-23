using Pom.Control;
using System;
using UnityEngine;

namespace Pom.UI.Control
{
    public class PlayerActionSignaler : MonoBehaviour
    {
        public void SwitchPlayerState(ActionButton actionButton)
        {
            PlayerController.Instance.SwitchState(actionButton.state);
        }

        public void ClearActiveUnit()
        {
            PlayerController.Instance.ClearActiveUnit();
        }
    }
}
