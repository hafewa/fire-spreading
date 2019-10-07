
using System;
using FireSimulation.Core;
using FireSimulation.Vegetation;
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{
    // UIPanel class is just a simple manager for UI behavior. This class handles all user input through UI and communicate with main 2 classes - WeatherControl and VegetationFactory
    public class UIPanel : MonoBehaviour
    {
        private WeatherControl weatherControl;
        private VegetationFactory vegetationFactory;

        public Slider windSpeedSlider;
        public Slider windDirectionSlider;
        public Slider windIntervalTickSlider;
        public Slider spawnRadius;
        public Slider maxPlants;

        public Toggle toggleGrid;

        // Public delegates for change states and updating values so we can avoid of udpating values through update function        
        public event Action<float> onWindSpeedChanged;
        public event Action<float> onWindDirectionChanged;
        public event Action<float> onWindIntervalTickChanged;
        public event Action<float> onSpawnRadiusChanged;
        public event Action<float> onMaxPlantsChanged;

        private void Awake()
        {
            weatherControl = FindObjectOfType<WeatherControl>();
            vegetationFactory = FindObjectOfType<VegetationFactory>();
        }

        public void Start()
        {
            windDirectionSlider.value = weatherControl.transform.eulerAngles.y;
            windSpeedSlider.value = weatherControl.GetWindSpeed();
            spawnRadius.value = vegetationFactory.GetMaxRadius();
            maxPlants.value = vegetationFactory.GetMaxPlants();
            toggleGrid.isOn = vegetationFactory.GetGrid();
        }

        public void ChangeWindSpeed()
        {
            windSpeedSlider.value = Mathf.Floor(windSpeedSlider.value * 100) / 100;
            weatherControl.SetWindSpeed(windSpeedSlider.value);
            onWindSpeedChanged(windSpeedSlider.value);
        }

        public void ChangeWindDirection()
        {
            windDirectionSlider.value = Mathf.Floor(windDirectionSlider.value * 100) / 100;
            weatherControl.SetWindDirection(windDirectionSlider.value);
            onWindDirectionChanged(windDirectionSlider.value);
        }

        public void ChangeWindIntervalTick()
        {
            windIntervalTickSlider.value = Mathf.Floor(windIntervalTickSlider.value * 100) / 100;
            weatherControl.SetWindIntervalTick(windIntervalTickSlider.value);
            onWindIntervalTickChanged(windIntervalTickSlider.value);
        }

        public void ChangeSpawnRadius()
        {
            spawnRadius.value = Mathf.Floor(spawnRadius.value * 100) / 100;
            vegetationFactory.SetMaxRadius(spawnRadius.value);
            onSpawnRadiusChanged(spawnRadius.value);
        }

        public void ChangeMaxPlants()
        {
            vegetationFactory.SetMaxPlants((int)maxPlants.value);
            onMaxPlantsChanged(maxPlants.value);
        }

        public void ToggleGrid()
        {
            vegetationFactory.SetGrid(toggleGrid.isOn);
        }
    }
}

