
using FireSimulation.Controls;
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{
    public class DisplayMode : MonoBehaviour
    {
        private PlayerControls playerControls;

        private void Awake()
        {
            playerControls = FindObjectOfType<PlayerControls>();
            playerControls.onModeChanged += PrintModeText;
        }

        private void PrintModeText(InteractionMode interactionMode)
        {
            GetComponent<Text>().text = interactionMode.ToString();
        }
    }

}