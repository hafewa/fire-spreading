using System;
using UnityEngine;

namespace FireSimulation.Core
{
    // Fire component class 
    public class Fire : MonoBehaviour
    {
        private Rigidbody rb;
        private WeatherControl weatherControl;
        private SphereCollider sphereCollider;

        private float maxX, maxY, maxZ = 0;

        [SerializeField] private float maxRadius = 20f;


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            sphereCollider = GetComponent<SphereCollider>();
            weatherControl = FindObjectOfType<WeatherControl>();
            sphereCollider.radius = 0f;
        }

        private void Update()
        {
            ClampPosition();
        }


        // This function limit fire gameObject to fly away from map
        private void ClampPosition()
        {
            Vector3 position = transform.position;
            position.x = Mathf.Clamp(position.x, 0, weatherControl.regionSize.x);
            position.y = Mathf.Clamp(position.y, 0, weatherControl.regionSize.y);
            position.z = Mathf.Clamp(position.z, 0, weatherControl.regionSize.z);
            transform.localPosition = position;
        }

        // Moves the Fire gameobject in direction of wind
        private void FixedUpdate()
        {
            if (!weatherControl.GetSimulationState()) return;
            if (sphereCollider.radius < maxRadius)
                sphereCollider.radius += (weatherControl.GetWindSpeed() / 2) * Time.fixedDeltaTime * 0.5f;
            rb.MovePosition(transform.position + transform.TransformDirection(weatherControl.GetWindDirection()) * Time.fixedDeltaTime * weatherControl.GetWindSpeed());
        }

        // Whenever a healthy plant enters the fire collider, it ignites particular plant
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Combustible")
            {
                Combustible combustible;
                if ((combustible = other.GetComponent<Combustible>()) == null) return;
                if (combustible == this) return;
                if (!combustible.IsHealthy()) return;
                Vector3 directionToIgnition = transform.position - other.transform.position;
                float angleToIgnition = Vector3.Angle(directionToIgnition, weatherControl.GetWindDirection());
                if (angleToIgnition < weatherControl.GetFireAngle()) return; // Checks whether to ignite 
                combustible.Ignite();
            }
        }

        private void IncreaseRadius(float value)
        {
            sphereCollider.radius += value;
        }

    }
}