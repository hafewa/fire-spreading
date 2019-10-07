using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{
    public class DisplayPlants : MonoBehaviour
    {
        private UIPanel panel;

        private void Awake()
        {
            panel = FindObjectOfType<UIPanel>();
            panel.onMaxPlantsChanged += PrintMaxPlants;
        }

        private void PrintMaxPlants(float value)
        {
            GetComponent<Text>().text = value.ToString();
        }
    }
}


