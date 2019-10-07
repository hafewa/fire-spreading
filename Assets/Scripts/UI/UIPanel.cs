
using System;
using FireSimulation.Core;
using FireSimulation.Vegetation;
using UnityEngine;
using UnityEngine.UI;

namespace FireSimulation.UI
{

    public class UIPanel : MonoBehaviour
    {
        private WeatherControl weatherControl;
        private VegetationFactory vegetationFactory;

        public Slider windSpeedSlider;
        public Slider windDirectionSlider;
        public Slider spawnRadius;
        public Slider maxPlants;

        public Toggle toggleGrid;

        public Action<float> onWindSpeedChanged;
        public Action<float> onWindDirectionChanged;
        public Action<float> onSpawnRadiusChanged;
        public Action<float> onMaxPlantsChanged;

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
            weatherControl.SetWindSpeed(windSpeedSlider.value);
            onWindSpeedChanged(windSpeedSlider.value);
        }

        public void ChangeWindDirection()
        {
            weatherControl.SetWindDirection(windDirectionSlider.value);
            onWindDirectionChanged(windDirectionSlider.value);
        }

        public void ChangeSpawnRadius()
        {
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

