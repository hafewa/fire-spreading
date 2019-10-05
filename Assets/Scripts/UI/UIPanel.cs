
using System;
using FireSimulation.Controls;
using FireSimulation.Core;
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{

    public class UIPanel : MonoBehaviour
    {
        private WeatherControl weatherControl;

        public Slider windSpeedSlider;
        public Slider windDirectionSlider;

        public Action<float> onWindSpeedChanged;
        public Action<float> onWindDirectionChanged;

        private void Awake()
        {
            weatherControl = FindObjectOfType<WeatherControl>();
        }

        public void Start()
        {
            windDirectionSlider.value = weatherControl.transform.eulerAngles.y;
            windSpeedSlider.value = weatherControl.GetWindSpeed();
        }

        public void ChangeWindSpeed()
        {
            weatherControl.SetWindSpeed(windSpeedSlider.value);
            onWindSpeedChanged(windSpeedSlider.value);
        }

        public void ChangeWindDirection()
        {
            weatherControl.SetWindDirection(windDirectionSlider.value);
            onWindDirectionChanged(windDirectionSlider.value);
        }
    }
}

