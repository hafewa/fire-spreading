using System.Collections;
using FireSimulation.Vegetation;
using UnityEngine;

namespace FireSimulation.Core
{
    public class Combustible : MonoBehaviour
    {
        [SerializeField] private LayerMask combustibleLayerMask; // Combustible layer mask
        [SerializeField] private float combustibility = 5f; // Time until the plant got burned (seconds)
        [SerializeField] private float maxRadius = 5f; // Max radius which fire can spread (m)
        [SerializeField] [Range(0f, 1f)] private float sideSpreadMult = 0.65f; // 65% of windDirectional spread effect
        [SerializeField] private float backwardAngleTolerance = 0f;
        private float currentRadius = 0f; // Current spreadRadius

        private bool isBurning = false;
        private Combustion combustion = Combustion.None;

        private Renderer _renderer;
        private MaterialPropertyBlock _propertyBlock;

        private float intervalTick = 1f;
        private WeatherControl weatherControl;

        [SerializeField] private float windRadius = 0f;
        [SerializeField] private float sideRadius = 0f;

        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock();
            _renderer = GetComponent<Renderer>();
        }

        private void Start()
        {
            weatherControl = transform.parent.GetComponent<VegetationFactory>().GetWeatherControl();
        }

        public void Ignite()
        {
            combustion = Combustion.Burning;
            ChangeColor(Color.red);
            StartCoroutine(Burn());
        }

        public void Extinguish()
        {
            StopCoroutine(Burn());
            combustion = Combustion.None;
            ChangeColor(Color.green);
        }

        private IEnumerator Burn()
        {
            combustion = Combustion.Burning;
            ChangeColor(Color.red);
            yield return Spread(weatherControl.GetWeatherIntervalTick());
            ChangeColor(Color.black);
        }

        // private void Update()
        // {
        //     if (combustion == Combustion.None || combustion == Combustion.Burned) return;
        // }

        private IEnumerator Spread(float intervalTick)
        {
            SphereCollider sphereCollider = GetComponent<SphereCollider>();

            while (combustion == Combustion.Burning)
            {
                if (combustibility <= 0) combustion = Combustion.Burned;
                combustibility -= 1;
                if (currentRadius < maxRadius)
                    currentRadius += Time.deltaTime;
                IgniteInWind();
                yield return new WaitForSeconds(intervalTick);
                IgniteAdjacent();
            }
        }

        private void IgniteInWind()
        {
            Combustible combustible;
            RaycastHit hit;
            windRadius = currentRadius * weatherControl.GetWindSpeed();
            if (Physics.SphereCast(transform.position, windRadius, weatherControl.GetWindDirection(), out hit, windRadius, combustibleLayerMask))
            {
                Debug.DrawRay(transform.position, weatherControl.GetWindDirection(), Color.red, 1f);
                if ((combustible = hit.transform.GetComponent<Combustible>()) != null)
                {
                    if (combustible.combustion == Combustion.None)
                    {
                        combustible.Ignite();
                    }
                }
            }
        }

        private void IgniteAdjacent()
        {
            Combustible combustible;
            sideRadius = currentRadius * weatherControl.GetWindSpeed();

            Collider[] hitColliders = Physics.OverlapSphere(transform.position, sideRadius, combustibleLayerMask);
            if (hitColliders.Length > 0)
            {
                for (int i = 0; i < hitColliders.Length; i++)
                {
                    if ((combustible = hitColliders[i].transform.GetComponent<Combustible>()) != null)
                    {
                        if (combustible.combustion == Combustion.None)
                        {
                            Vector3 adjacentDirection = combustible.transform.position - transform.position;
                            float angleAgainstWind = Vector3.Angle(adjacentDirection, weatherControl.GetWindDirection());
                            if (angleAgainstWind > 90 + backwardAngleTolerance) continue;
                            combustible.Ignite();
                        }
                    }
                }
            }
        }

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

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(transform.position, currentRadius * sideSpreadMult);
        }

    }
}

