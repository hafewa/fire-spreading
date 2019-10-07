
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{

    public class DisplayWindInterval : MonoBehaviour
    {

        private UIPanel panel;

        private void Awake()
        {
            panel = FindObjectOfType<UIPanel>();
            panel.onWindIntervalTickChanged += PrintWindInterval;
        }

        private void PrintWindInterval(float value)
        {
            GetComponent<Text>().text = value + "s";
        }
    }
}

