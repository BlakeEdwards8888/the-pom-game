using Pom.Control;
using UnityEngine;

namespace Pom.Core
{
    public class PlayerDisabler : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerController.Instance.enabled = false;
        }

        private void OnDisable()
        {
            PlayerController.Instance.enabled = true;
        }
    }
}
