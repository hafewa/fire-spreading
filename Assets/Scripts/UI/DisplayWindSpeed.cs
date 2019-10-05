
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{
    public class DisplayWindSpeed : MonoBehaviour
    {

        private UIPanel panel;

        private void Start()
        {
            panel = FindObjectOfType<UIPanel>();
            panel.onWindSpeedChanged += PrintWindSpeed;
        }

        private void PrintWindSpeed(float windSpeed)
        {
            GetComponent<Text>().text = windSpeed.ToString();
        }
    }
}
