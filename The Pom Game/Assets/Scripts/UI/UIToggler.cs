using UnityEngine;

namespace Pom.UI
{
    public class UIToggler : MonoBehaviour
    {
        [SerializeField] GameObject uiContainer;

        public void ToggleUI(bool toggle)
        {
            uiContainer.SetActive(toggle);
        }

        public void ToggleUI()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
        }
    }
}
