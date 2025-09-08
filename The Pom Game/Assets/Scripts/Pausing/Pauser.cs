using Pom.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Pom.Pausing
{
    public class Pauser : MonoBehaviour
    {
        [SerializeField] UIToggler pauseScreen;


        void Update()
        {
            if (Keyboard.current.pKey.wasPressedThisFrame)
            {
                pauseScreen.ToggleUI();
            }
        }
    }
}
