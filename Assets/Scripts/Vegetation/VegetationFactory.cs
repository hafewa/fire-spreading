using FireSimulation.Core;
using UnityEngine;

namespace FireSimulation.Vegetation
{
    public class VegetationFactory : MonoBehaviour
    {

        [Header("Vegetation settings")]
        [SerializeField] GameObject vegetationPrefab; // Vegetation prefabs to spawn
        [Header("Grid settings")]
        [SerializeField] private bool gridSpawn = false;
        [SerializeField] [Range(1, 10)] private float spawnRadius = 2;
        [Header("Base settings")]
        [SerializeField] [Range(0, 20000)] int maxPlants = 0;
        [SerializeField] [Range(1, 10)] private int randomIgnition = 4;

        [Header("Terrain parameters")]
        [SerializeField] private LayerMask terrainLayerMask;
        [SerializeField] private Terrain terrainObject;

        private float maxHeightTolerance = 10f;
        private int terrainEdgePadding = 4;

        private float terrainWidth = 0f;
        private float terrainLength = 0f;
        private float terrainHeight = 0f;

        private WeatherControl weatherControl;

        [Header("Results")]
        public int spawnedPlants = 0;

        private void Awake()
        {
            weatherControl = FindObjectOfType<WeatherControl>();
        }

        private void Start()
        {
            if (terrainObject == null)
            {
                Debug.LogError("Terrain object is not set!");
                return;
            }
            terrainWidth = terrainObject.terrainData.size.x;
            terrainLength = terrainObject.terrainData.size.z;
            terrainHeight = terrainObject.terrainData.size.y;
        }

        public void GenerateVegetation()
        {
            Generate();
        }

        private void Generate()
        {
            ClearVegetation();
            float coordX, coordY;
            Vector3 rayCastOrigin;
            RaycastHit hit;

            if (gridSpawn)
            {
                float x, z = 0;
                for (x = terrainEdgePadding; x < terrainWidth;)
                {
                    for (z = terrainEdgePadding; z < terrainLength;)
                    {
                        rayCastOrigin = new Vector3(x, terrainHeight + maxHeightTolerance, z);
                        if (Physics.SphereCast(rayCastOrigin, spawnRadius, Vector3.down, out hit, terrainHeight + maxHeightTolerance, terrainLayerMask))
                        {
                            Instantiate(vegetationPrefab, hit.point, Quaternion.identity, transform);
                            spawnedPlants++;
                            x += spawnRadius;
                        }
                        if (x > terrainWidth - terrainEdgePadding)
                        {
                            x = terrainEdgePadding;
                            z += spawnRadius;
                        }
                    }
                    if (z > terrainLength - terrainEdgePadding - spawnRadius)
                        return;
                }
            }
            else
            {
                for (int i = 0; i < maxPlants; i++)
                {
                    coordX = Mathf.Clamp(UnityEngine.Random.Range(0, terrainWidth), 0 + terrainEdgePadding, terrainWidth - terrainEdgePadding);
                    coordY = Mathf.Clamp(UnityEngine.Random.Range(0, terrainLength), 0 + terrainEdgePadding, terrainLength - terrainEdgePadding);

                    rayCastOrigin = new Vector3(coordX, terrainHeight + maxHeightTolerance, coordY);
                    if (Physics.Raycast(rayCastOrigin, Vector3.down, out hit, terrainHeight + maxHeightTolerance, terrainLayerMask))
                    {
                        if (hit.transform.GetComponent<Flower>() != null) continue;
                        Instantiate(vegetationPrefab, hit.point, Quaternion.identity, transform);
                        spawnedPlants++;
                    }
                }
            }
        }

        public void ClearVegetation()
        {
            if (transform.childCount > 0)
            {
                foreach (Transform child in transform)
                {
                    Destroy(child.gameObject);
                }
            }
            spawnedPlants = 0;
            return;
        }

        public void SpawnFlower(Vector3 point)
        {
            spawnedPlants++;
            Instantiate(vegetationPrefab, point, Quaternion.identity, transform);
        }

        public void RemoveFlower(Flower flower)
        {
            spawnedPlants--;
            Destroy(flower.transform.gameObject);
        }

        public float GetSpawnRadius()
        {
            return spawnRadius;
        }

        public void FireRandom()
        {
            for (int i = 0; i < randomIgnition; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, spawnedPlants);
                Transform child = transform.GetChild(randomIndex);
                if (child == null) return;

                Combustible combustible = child.GetComponent<Combustible>();
                if (combustible == null) return;
                combustible.Ignite();
            }
        }

        public WeatherControl GetWeatherControl()
        {
            return weatherControl;
        }

        public int GetMaxPlants()
        {
            return maxPlants;
        }

        public void SetMaxPlants(int value)
        {
            maxPlants = value;
        }

        public float GetMaxRadius()
        {
            return spawnRadius;
        }

        public void SetMaxRadius(float value)
        {
            this.spawnRadius = value;
        }

        public void SetGrid(bool state)
        {
            gridSpawn = state;
        }

        public bool GetGrid()
        {
            return gridSpawn;
        }
    }
}


