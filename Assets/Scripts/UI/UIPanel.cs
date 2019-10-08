
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
        public Slider combustionRateSlider;
        public Slider fireAngleSlider;
        public Slider spawnRadius;
        public Slider maxPlants;

        public Toggle toggleGrid;

        // Public delegates for change states and updating values so we can avoid of udpating values through update function        
        public event Action<float> onWindSpeedChanged;
        public event Action<float> onWindDirectionChanged;
        public event Action<float> onCombustionRateChanged;
        public event Action<float> onFireAngleChanged;
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
            fireAngleSlider.value = weatherControl.GetFireAngle();
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

        public void ChangeWindIntervalTick()
        {
            combustionRateSlider.value = Mathf.Floor(combustionRateSlider.value * 100) / 100;
            weatherControl.SetWindIntervalTick(combustionRateSlider.value);
            onCombustionRateChanged(combustionRateSlider.value);
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

        public void ChangeFireAngle()
        {
            weatherControl.SetFireAngle(fireAngleSlider.value);
            onFireAngleChanged(fireAngleSlider.value);
        }

        public void ChangeSimulationState()
        {
            weatherControl.ToggleSimulationState();
        }

        public void ToggleGrid()
        {
            vegetationFactory.SetGrid(toggleGrid.isOn);
        }
    }
}

