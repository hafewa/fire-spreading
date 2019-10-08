using System;
using System.Collections;
using System.Collections.Generic;
using FireSimulation.Vegetation;
using UnityEngine;

namespace FireSimulation.Core
{

    [System.Serializable]
    public struct ColorMapping
    {
        public Combustion combustion;
        public Color color;
    }

    /* Main Combustible core class for combustible game objects - in our case we have only Flowers (Plants) */
    public class Combustible : MonoBehaviour
    {
        // Basic combustible parameters
        [SerializeField] private float combustibility = 5f; // Time until the plant got burned (seconds)
        [SerializeField] private float maxRadius = 5f; // Max radius which fire can spread (m)
        [SerializeField] [Range(0.1f, 1f)] private float radiusGrowSpeed = 0.25f;
        [SerializeField] [Range(0.1f, 1f)] private float radiusWindMultiplier = 0.5f;
        [SerializeField] private float backwardAngleTolerance = 90f;
        [SerializeField] private float currentRadius = 0f; // Current spreadRadius
        [SerializeField] private ColorMapping[] colorMappings;

        // Initial state
        private Combustion combustion = Combustion.None;

        //Adjacent gameObjects to ignite
        private Collider[] adjacentColliders = new Collider[0];
        private List<Combustible> combustibleInWind = new List<Combustible>();
        private List<Combustible> combustibleInSide = new List<Combustible>();

        private int igniteInWind = 0;
        private int igniteInSide = 0;

        private float radiusIncr = 0;

        private WeatherControl weatherControl;
        private IEnumerator coroutine;

        private void Start()
        {
            weatherControl = transform.parent.GetComponent<VegetationFactory>().GetWeatherControl();
            ChangeColor();
            coroutine = Burn();
        }

        // Ignition function
        public void Ignite()
        {
            combustion = Combustion.Burning;
            ChangeColor();
            StartCoroutine(coroutine);
        }

        private void ChangeColor()
        {
            foreach (ColorMapping mapping in colorMappings)
            {
                if (mapping.combustion != combustion) continue;
                GetComponent<Flower>().ChangeColor(mapping.color);
            }
        }

        // Extinguish function stops coroutine for Spreading and set back Combustion to None and color to green, but keeps the combustibility value so it's already "damaged". 
        public void Extinguish()
        {
            StopCoroutine(coroutine);
            combustion = Combustion.None;
            ChangeColor();
        }

        // Spread coroutine which is responsible for fire activity on certain Combustible component.
        // This coroutine is running when Combustion is equals to Burning state and while loop within this function is looping each tick based on WeatherControl windIntervalTick variable.
        private IEnumerator Burn()
        {
            while (combustion == Combustion.Burning)
            {
                if (combustibility <= 0) combustion = Combustion.Burned;
                combustibility -= 1;
                yield return new WaitForSeconds(weatherControl.GetWindIntervalTick());
            }
            ChangeColor();
        }

        public bool IsBurning()
        {
            return combustion == Combustion.Burning;
        }

        public bool IsHealthy()
        {
            return combustion == Combustion.None;
        }

        public void ToggleBurning(bool state)
        {
            if (state)
                StartCoroutine(coroutine);
            else
                StopCoroutine(coroutine);
        }
    }
}

