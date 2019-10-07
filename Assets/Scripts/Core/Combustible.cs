using System;
using System.Collections;
using FireSimulation.Vegetation;
using UnityEngine;

namespace FireSimulation.Core
{
    /* Main Combustible core class for combustible game objects - in our case we have only Flowers (Plants) */
    public class Combustible : MonoBehaviour
    {
        // Basic combustible parameters
        [SerializeField] private LayerMask combustibleLayerMask; // Combustible layer mask
        [SerializeField] private float combustibility = 5f; // Time until the plant got burned (seconds)
        [SerializeField] private float maxRadius = 5f; // Max radius which fire can spread (m)
        [SerializeField] [Range(0f, 1f)] private float sideSpreadMult = 0.65f; // 65% of windDirectional spread effect
        [SerializeField] private float backwardAngleTolerance = 0f;
        [SerializeField] private float currentRadius = 0f; // Current spreadRadius
        [SerializeField] private float growRadiusSpeed = 5f;

        // Initial state
        private Combustion combustion = Combustion.None;

        // Collisions with adjacent and forward objects
        private bool spreadInWind = false;
        private Combustible windCombustible;
        private bool spreadInSide = false;
        private Collider[] sideCombustibles = new Collider[0];

        /* 
        Rend. + Property block for material 
        - Usage of custom shader with GPU Instancing enabled. Unfortunately I don't really understand GPU optimization and 

        */ 

        private Renderer _renderer;
        private MaterialPropertyBlock _propertyBlock;
        private WeatherControl weatherControl;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            weatherControl = transform.parent.GetComponent<VegetationFactory>().GetWeatherControl();
        }

        // Growing radius function
        private void GrowRadius()
        {
            if (currentRadius < maxRadius)
                currentRadius += 1f;
        }

        // Ignition function
        public void Ignite()
        {
            combustion = Combustion.Burning;
            ChangeColor(Color.red);
            StartCoroutine(Spread());
        }

        // Extinguish function stops coroutine for Spreading and set back Combustion to None and color to green, but keeps the combustibility value so it's already "damaged". 
        public void Extinguish()
        {
            StopCoroutine(Spread());
            combustion = Combustion.None;
            ChangeColor(Color.green);
        }

        // Spread coroutine which is responsible for fire activity on certain Combustible component.
        // This coroutine is running when Combustion is equals to Burning state and while loop within this function is looping each tick based on WeatherControl windIntervalTick variable.
        private IEnumerator Spread()
        {
            combustion = Combustion.Burning;
            ChangeColor(Color.red);
            while (combustion == Combustion.Burning)
            {
                if (combustibility <= 0) combustion = Combustion.Burned;
                combustibility -= 1;
                IgniteInWind();
                IgniteAdjacent();
                GrowRadius();
                yield return new WaitForSeconds(weatherControl.GetWindIntervalTick());
            }
            ChangeColor(Color.black);
        }


        /* 
            IgniteInWind is a main function for directional fire spread.
        
            This function is heavy on performance so the raycast is cast only once when the object with this component is being initialized. 
            It raycasts in direction same as wind and if the result is not null, it stores its Combustible component into windCombustible variable. 
            Next time this function is called, it checks whether distance to this object is equals to currentRadius of this object. 
            If so, it can ignite the windCombustible object.
        */
        private void IgniteInWind()
        {
            if (spreadInWind && windCombustible == null) return; // If no raycast result - just skip this function
            if (spreadInWind)
            {
                if (windCombustible.combustion != Combustion.None) return;
                float radius = currentRadius * weatherControl.GetWindSpeed();
                float distanceToCombustible = Vector3.Distance(transform.position, windCombustible.transform.position);
                if (radius < distanceToCombustible) return;
                windCombustible.Ignite();
            }
            else
            {
                RaycastHit hit;
                //SphereCast is most suitable for this solution because it checks more objects within radius in front of this object.
                if (Physics.SphereCast(transform.position, maxRadius / 2f, weatherControl.GetWindDirection(), out hit, maxRadius, combustibleLayerMask))
                {
                    windCombustible = hit.transform.GetComponent<Combustible>();
                }
                spreadInWind = true;
            }
        }

        /* 
            IgniteAdjacent()
            - is a main function for side fire spread.
            - Logic is very similar to IgniteInWind apart from RayCasting. It uses OverlapSphere to determine adjecant gameobjects with Combustible component.
            If there is at least one object within half of the main radius, it stores this object into sideCombustibles array.
            - It also check whether the objects are within the angle of spread. By default is set to 0 degrees so it takes all the gameobjects within 180 degrees in front of it. 
            In the editor inspector, by default is set to 60 so it can be easily visible how the fire spreads in certain direction based on wind direction.
            - Radius of sideSpreading is half o main radius for directional ignition. This value can be tweaked, changed.
        */
        private void IgniteAdjacent()
        {
            if (spreadInSide && sideCombustibles.Length == 0) return;
            if (spreadInSide)
            {
                Combustible _combustible;
                float radius = currentRadius * weatherControl.GetWindSpeed() * sideSpreadMult;
                for (int i = 0; i < sideCombustibles.Length; i++)
                {
                    _combustible = sideCombustibles[i].transform.GetComponent<Combustible>();
                    if (_combustible.combustion != Combustion.None) continue;
                    float distanceToCombustible = Vector3.Distance(transform.position, _combustible.transform.position);
                    if (radius < distanceToCombustible) continue;
                    Vector3 adjacentDirection = _combustible.transform.position - transform.position;
                    float angleAgainstWind = Vector3.Angle(adjacentDirection, weatherControl.GetWindDirection());
                    if (angleAgainstWind > 90 + backwardAngleTolerance) continue;
                    _combustible.Ignite();
                }
            }
            else
            {
                sideCombustibles = Physics.OverlapSphere(transform.position, maxRadius / 2f, combustibleLayerMask);
                spreadInSide = true;
            }
        }

        /*
            ChangeColor()
            - is used for changing texture color of Plant prefabs using property blocks to gain some performance. 
            If this method wouldn't be used, it might cause massive frame drops because of texture copying and loosing memory 
            so it pre-renders its color. 
         */
        private void ChangeColor(Color _color)
        {
            _renderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetColor("_Color", Color.Lerp(_propertyBlock.GetColor("_Color"), _color, 1f));
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        public bool IsBurning()
        {
            return combustion == Combustion.Burning;
        }


        /*
            CheckAdjecentOnFire()
            - is called only during manual plant spawn. It is important to determine whether any other ignited plants are nearby so it can be ignited immedietally.
        */
        public void CheckAdjacentOnFire()
        {
            Combustible adjacentOnFire;
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, maxRadius, combustibleLayerMask);
            foreach (Collider collider in hitColliders)
            {
                if ((adjacentOnFire = collider.transform.GetComponent<Combustible>()) != null) continue;
                if (adjacentOnFire.combustion != Combustion.Burning) continue;
                this.Ignite();
            }
        }

    }
}

