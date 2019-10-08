
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{

    public class DisplayCombustionRate : MonoBehaviour
    {

        private UIPanel panel;

        private void Awake()
        {
            panel = FindObjectOfType<UIPanel>();
            panel.onCombustionRateChanged += PrintCombustionRate;
        }

        private void PrintCombustionRate(float value)
        {
            GetComponent<Text>().text = value.ToString();
        }
    }
}

