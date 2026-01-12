using Pom.Control;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace Pom.Pausing
{
    public class PauseScreen : MonoBehaviour
    {
        private void OnEnable()
        {
            Time.timeScale = 0;
        }

        private void OnDisable()
        {
            Time.timeScale = 1;
        }
    }
}
