using System;
using UnityEngine;

namespace FireSimulation.Core
{
    // Main class for Weather Control
    // 
    public class WeatherControl : MonoBehaviour
    {
        [Header("Simulation state")]
        [SerializeField] private bool isSimulationActive = true;

        [Header("Weather conditions")]
        [SerializeField] [Range(0, 30f)] private float windSpeed = 20f; // Wind speed variable - moves with fire object
        [SerializeField] private Vector3 windDirection; // Wind rotation - gives direction to fire object
        [SerializeField] [Range(0.1f, 2f)] private float combustionTick = 1f; // Time variable to tick in Combustible Burn coroutine
        [SerializeField] [Range(0, 360)] private float backFireAngle = 0; // Angle tolerance to ignite plants behind itself 

        [Header("GameObjects")]
        [SerializeField] private Transform fireContainer;
        [SerializeField] private GameObject firePrefab;

        public Vector3 regionSize = Vector3.zero;

        public event Action<bool> onSimulationStateChanged;

        private void Start()
        {
            UpdateWindRotation();
            onSimulationStateChanged(isSimulationActive);
        }

        private void UpdateWindRotation()
        {
            windDirection = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
        }

        public float GetWindSpeed()
        {
            return windSpeed;
        }

        // Get windDirection vector for combustible raycasting
        public Vector3 GetWindDirection()
        {
            return transform.TransformDirection(Vector3.forward);
        }

        public float GetWindIntervalTick()
        {
            return combustionTick;
        }

        public float GetFireAngle()
        {
            return backFireAngle;
        }

        public bool GetSimulationState()
        {
            return isSimulationActive;
        }

        public void SetWindSpeed(float value)
        {
            windSpeed = value;
        }

        public void SetWindDirection(float value)
        {
            transform.eulerAngles = new Vector3(0f, value, 0f);
            UpdateWindRotation();
        }

        public void SetWindIntervalTick(float value)
        {
            combustionTick = value;
        }

        public void SetFireAngle(float value)
        {
            backFireAngle = value;
        }

        public void ToggleSimulationState()
        {
            isSimulationActive = !isSimulationActive;
            onSimulationStateChanged(isSimulationActive);
        }

        public void IgniteFire(Vector3 position)
        {
            Instantiate(firePrefab, position, Quaternion.identity, fireContainer);
        }

        public void RemoveAllFire()
        {
            foreach (Transform child in fireContainer)
            {
                if (child.GetComponent<Fire>() != null)
                {
                    Destroy(child.gameObject);
                }
            }
        }

    }
}


