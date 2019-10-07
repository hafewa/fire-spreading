using UnityEngine;

namespace FireSimulation.Core
{
    // Main class for Weather Control
    // 
    public class WeatherControl : MonoBehaviour
    {

        [SerializeField] [Range(0, 30f)] private float windSpeed = 20f;
        [SerializeField] private Vector3 windDirection;
        [SerializeField] [Range(0.1f, 2f)] private float windIntervalTick = 1f;

        private void Start()
        {
            UpdateWindRotation();
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
            return windIntervalTick;
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
            windIntervalTick = value;
        }

    }
}


