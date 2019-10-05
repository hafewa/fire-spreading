
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{

    public class DisplayWindDirection : MonoBehaviour
    {

        private UIPanel panel;

        private void Start()
        {
            panel = FindObjectOfType<UIPanel>();
            panel.onWindDirectionChanged += PrintWindDirection;
        }

        private void PrintWindDirection(float windDirection)
        {
            GetComponent<Text>().text = windDirection + "°";
        }
    }
}

