using UnityEngine;

namespace FireSimulation.Core
{
    // Just a simple class for WindArrowIndicator to get the rotation as WeatherControl for wind
    public class WindIndicator : MonoBehaviour
    {

        private WeatherControl weatherControl;

        private void Awake()
        {
            weatherControl = FindObjectOfType<WeatherControl>();
        }

        private void Update()
        {
            transform.rotation = weatherControl.transform.rotation;
        }

    }
}