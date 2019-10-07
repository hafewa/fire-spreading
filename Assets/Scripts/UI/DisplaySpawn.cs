using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{
    public class DisplaySpawn : MonoBehaviour
    {
        private UIPanel panel;

        private void Start()
        {
            panel = FindObjectOfType<UIPanel>();
            panel.onSpawnRadiusChanged += PrintSpawnValue;
        }

        private void PrintSpawnValue(float value)
        {
            GetComponent<Text>().text = value + "m";
        }
    }
}

