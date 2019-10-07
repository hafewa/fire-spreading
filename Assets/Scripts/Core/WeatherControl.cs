using UnityEngine;

namespace FireSimulation.Core
{

    public class WeatherControl : MonoBehaviour
    {

        [SerializeField] [Range(0, 30f)] private float windSpeed = 20f;
        [SerializeField] private Vector3 windDirection;
        [SerializeField] [Range(0.1f, 5f)] private float weatherIntervalTick = 1f;

        private LineRenderer lineRenderer;
        private Material windMaterial;
        private float windMaterialOffsetX = 0f;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
            InitWindLines();
        }

        private void Start()
        {
            UpdateWindRotation();
        }

        private void InitWindLines()
        {
            windMaterial = lineRenderer.material;
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
            lineRenderer.startWidth = 1.45f;
            lineRenderer.endWidth = 1.45f;
        }

        private void DrawWindLines()
        {

            Vector3 origin = new Vector3(transform.position.x, 50f, transform.position.z);
            Vector3 dest = GetWindDirection().normalized * 50f;
            dest.y = 50f;
            lineRenderer.SetPosition(0, origin);
            lineRenderer.SetPosition(1, dest);
        }

        private void UpdateWindRotation()
        {
            windDirection = new Vector3(0f, transform.rotation.eulerAngles.y, 0f);
            DrawWindLines();
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

        private void Update()
        {
            // windMaterialOffsetX -= Time.deltaTime;
            // windMaterial.SetTextureOffset(, new Vector2(windMaterialOffsetX, 0));        
        }

    }
}


