
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{

    public class DisplayFireAngle : MonoBehaviour
    {

        private UIPanel panel;

        private void Awake()
        {
            panel = FindObjectOfType<UIPanel>();
            panel.onFireAngleChanged += PrintFireAngle;
        }

        private void PrintFireAngle(float value)
        {
            GetComponent<Text>().text = value + "°";
        }
    }
}

