using Pom.Control;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Pom.Pausing
{
    public class PauseScreen : MonoBehaviour
    {
        private void OnEnable()
        {
            PlayerController.Instance.enabled = false;
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            PlayerController.Instance.enabled = true;
            Time.timeScale = 1;
        }
    }
}
