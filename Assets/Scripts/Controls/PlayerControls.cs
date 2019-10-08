using FireSimulation.Vegetation;
using FireSimulation.Core;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace FireSimulation.Controls
{

    // Player Controller class for camera movement around the scene
    public class PlayerControls : MonoBehaviour
    {
        [Header("Interaction mode")]
        private InteractionMode interactionMode = InteractionMode.Add;
        [SerializeField] private LayerMask interactionLayerMask;

        [Header("Simulation")]
        private bool simulationState = true;

        [Header("Camera settings")]
        public Transform mainCamera;
        [SerializeField] [Range(1, 10)] private int cameraSpeed = 2; // forward/backward speed of camera
        [SerializeField] [Range(1, 10)] private int rotationSpeed = 2; // vertical/horizontal rotation speed of camera
        [SerializeField] [Range(1, 10)] private int speedMultiplier = 2; // fastCamera mode multiplier
        [SerializeField] private bool freeLook = false; // freeLook boolean for toggling the freeLook

        private float horizontalRotation = 0f;
        private float verticalRotation = 0f;
        private float speed = 0f;

        // Pointers for main application classes
        private ControlsConfig controlsConfig;
        private VegetationFactory vegetationFactory;
        private WeatherControl weatherControl;

        public event Action<InteractionMode> onModeChanged; // delegate invoked from UIPanel for changing the InteractionMode 

        private void Awake()
        {
            controlsConfig = GetComponent<ControlsConfig>();
            vegetationFactory = FindObjectOfType<VegetationFactory>();
            weatherControl = FindObjectOfType<WeatherControl>();
        }

        private void Start()
        {
            horizontalRotation = mainCamera.eulerAngles.x;
            verticalRotation = mainCamera.eulerAngles.y;
            onModeChanged(interactionMode);
        }

        public void ChangeInteractionMode()
        {
            int currentMode = (int)interactionMode;
            if ((currentMode + 1) == Enum.GetNames(typeof(InteractionMode)).Length)
            {
                currentMode = 0;
            }
            else
            {
                currentMode++;
            }
            interactionMode = (InteractionMode)currentMode;
            onModeChanged(interactionMode);
        }

        public void ChangeSimulationState()
        {
            weatherControl.ToggleSimulationState();
        }

        public void GenerateVegetation()
        {
            weatherControl.RemoveAllFire();
            vegetationFactory.GenerateVegetation();
        }

        public void ClearVegetation()
        {
            weatherControl.RemoveAllFire();
            vegetationFactory.ClearVegetation();
        }

        private void Update()
        {
            if (ProcessUI()) return;
            ProcessInteraction();
        }

        private void LateUpdate()
        {
            if (FreeLook() || ToggleFreeLook())
                CameraMovement();
            else return;
        }

        private bool ProcessUI()
        {
            // Prevent raycasting on terrain if UI is above
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            return false;
        }

        // Simple interaction method to determine user input from mouse
        // Based on InteractionMode enum set through the user panel it process the input
        private void ProcessInteraction()
        {
            if (Input.GetKeyDown(controlsConfig.lbm))
            {
                RaycastHit hit;
                if (Physics.Raycast(GetMouseRay(), out hit, interactionLayerMask))
                {
                    switch (interactionMode)
                    {
                        // Adding plants
                        case InteractionMode.Add:
                            if (hit.transform.GetComponent<Combustible>() == null)
                            {
                                vegetationFactory.SpawnFlower(hit.point);
                            }
                            break;
                        // Removing plants
                        case InteractionMode.Remove:
                            Flower flower = hit.transform.GetComponent<Flower>();
                            if (flower != null)
                            {
                                vegetationFactory.RemoveFlower(flower);
                            }
                            break;
                        // Toggle between ignition and extinguishing
                        case InteractionMode.Toggle:
                            Combustible combustible = hit.transform.GetComponent<Combustible>();
                            if (combustible == null) return;
                            if (combustible.IsBurning())
                                combustible.Extinguish();
                            else
                                weatherControl.IgniteFire(hit.point);
                            break;
                    }
                }
            };
        }

        private bool FreeLook()
        {
            return Input.GetKey(controlsConfig.rbm);
        }

        private void CameraMovement()
        {
            if (Input.GetKey(controlsConfig.fastCamera))
            {
                speed = cameraSpeed * speedMultiplier;
            }
            else
            {
                speed = cameraSpeed;
            }

            if (Input.GetKey(controlsConfig.forward))
            {
                mainCamera.Translate(Vector3.forward * speed);
            }

            if (Input.GetKey(controlsConfig.backward))
            {
                mainCamera.Translate(Vector3.back * speed);
            }

            if (Input.GetKey(controlsConfig.left))
            {
                mainCamera.Translate(Vector3.left * speed);
            }

            if (Input.GetKey(controlsConfig.right))
            {
                mainCamera.Translate(Vector3.right * speed);
            }

            horizontalRotation += Input.GetAxis("Mouse X");
            verticalRotation -= Input.GetAxis("Mouse Y");

            mainCamera.eulerAngles = new Vector3(Mathf.Clamp(verticalRotation * rotationSpeed, -90.0f, 90.0f), horizontalRotation * rotationSpeed, 0.0f);
        }

        private bool ToggleFreeLook()
        {
            if (Input.GetKeyDown(controlsConfig.toggleFreeLook))
            {
                freeLook = !freeLook;
            }

            return freeLook;
        }

        public void FireRandom()
        {
            vegetationFactory.FireRandom();
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}

