using FireSimulation.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{

    public class DisplaySimulationState : MonoBehaviour
    {
        private WeatherControl weatherControl;

        private void Awake()
        {
            weatherControl = FindObjectOfType<WeatherControl>();
            weatherControl.onSimulationStateChanged += ChangeState;
        }

        private void ChangeState(bool state)
        {
            if (state) GetComponent<Text>().text = "Stop simulation";
            else GetComponent<Text>().text = "Resume simulation";
        }
    }

}
