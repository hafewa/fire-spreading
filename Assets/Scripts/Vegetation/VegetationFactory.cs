using FireSimulation.Core;
using UnityEngine;

namespace FireSimulation.Vegetation
{

    // VegetationFactory class is responsible for instantiating of gameObjects, in our case we use vegetationPrefab gameObject to get access to Flower(Plant) gameObject.
    public class VegetationFactory : MonoBehaviour
    {

        [Header("Vegetation settings")]
        [SerializeField] GameObject vegetationPrefab; // Vegetation prefabs to spawn

        [Header("Grid settings")]
        [SerializeField] private bool gridSpawn = false; // Just a boolean for grid spawning
        [SerializeField] [Range(1, 10)] private float spawnRadius = 2; // Grid spawn radius to put gap between plants

        [Header("Base settings")]
        [SerializeField] [Range(0, 20000)] private int maxPlants = 0; // Maximum of plants allowed to spawn on the map
        [SerializeField] [Range(1, 10)] private int randomIgnition = 4; // Number of random ignition points on the map

        [Header("Terrain parameters")]
        [SerializeField] private LayerMask terrainLayerMask; // LayerMask for raycast to spawn vegetationPrefabs only on terrain layer mask
        [SerializeField] private Terrain terrainObject; // terrain object assigned in the inspector to determine size of terrain
        [SerializeField] private float maxHeightTolerance = 10f; // Just some additional value for setting origin of raycast to spawn on the terrain
        [SerializeField] private int terrainEdgePadding = 4; // Value for padding, so the VegetationFactory don't spawn plants on the edge of the map

        [Header("Terrain size")]
        public float terrainWidth = 0f;
        public float terrainLength = 0f;
        public float terrainHeight = 0f;

        [Header("Results")]
        public int spawnedPlants = 0;
        private WeatherControl weatherControl;

        private void Awake()
        {
            weatherControl = FindObjectOfType<WeatherControl>();
            weatherControl.onSimulationStateChanged += ToggleAll;
        }

        // In start method we get propertis from terrain gameobject to determine the size and height of our terrain.
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
            weatherControl.regionSize = new Vector3(terrainWidth, terrainHeight, terrainLength);
        }

        public void GenerateVegetation()
        {
            Generate();
        }

        // Generate method is spawning prefabs on terrain and checks for terrainLayerMask so it spawns only on the terrain 
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
                        // In gridSpawn, SphereCast is used to 
                        if (Physics.SphereCast(rayCastOrigin, spawnRadius, Vector3.down, out hit, terrainHeight + maxHeightTolerance, terrainLayerMask))
                        {
                            Instantiate(vegetationPrefab, hit.point, Quaternion.identity, transform);
                            spawnedPlants++;
                            x += spawnRadius;
                        }
                        if (x > terrainWidth - terrainEdgePadding) // Padding avoidance - used for spawning plants away from edge
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

        // Removes all children gameObjects from VegetationFactory transform
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

        // Instantiates Flower(Plant) prefab on certain point
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
            if (transform.childCount == 0) return;
            int ignited = 0;
            for (int i = 0; i < randomIgnition; i++)
            {
                Combustible combustible;
                foreach (Transform child in transform)
                {
                    if (ignited >= randomIgnition) return;
                    if ((combustible = child.GetComponent<Combustible>()) == null) continue;
                    if (!combustible.IsHealthy()) continue;
                    weatherControl.IgniteFire(combustible.transform.position);
                    ignited++;
                }
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

        private void ToggleAll(bool state)
        {
            Combustible combustible;
            foreach (Transform child in transform)
            {
                if ((combustible = child.GetComponent<Combustible>()) == null) continue;
                if (!combustible.IsBurning()) continue;
                combustible.ToggleBurning(state);
            }
        }

    }
}


