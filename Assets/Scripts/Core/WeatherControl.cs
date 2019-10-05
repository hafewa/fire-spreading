using System;
using UnityEngine;

namespace FireSimulation.Core
{

    public class WeatherControl : MonoBehaviour
    {

        [Tooltip("Speed of wind - careful")]
        [SerializeField]
        [Range(20f, 200f)]
        private float windSpeed = 20f;

        [SerializeField]
        private Vector3 windDirection;

        [SerializeField]
        [Range(0.1f, 5f)]
        private float weatherIntervalTick = 1f;

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

        public Vector3 GetWindDirection()
        {
            return transform.TransformDirection(Vector3.forward);
        }

        public float GetWeatherIntervalTick()
        {
            return weatherIntervalTick;
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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawRay(transform.position, transform.TransformDirection(Vector3.forward * 1000f));
        }

    }
}


