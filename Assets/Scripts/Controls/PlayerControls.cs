﻿using FireSimulation.Vegetation;
using FireSimulation.Core;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

namespace FireSimulation.Controls
{
    public class PlayerControls : MonoBehaviour
    {
        [Header("Interaction mode")]
        [SerializeField] private InteractionMode interactionMode;

        private ControlsConfig controlsConfig;
        private VegetationFactory vegetationFactory;
        private WeatherControl weatherControl;

        [Header("Camera settings")]
        public Transform mainCamera;
        [SerializeField] [Range(1, 10)] private int cameraSpeed = 2;
        [SerializeField] [Range(1, 10)] private int rotationSpeed = 2;
        [SerializeField] [Range(1, 10)] private int speedMultiplier = 2;
        [SerializeField] private bool freeLook = false;

        private float horizontalRotation = 0f;
        private float verticalRotation = 0f;
        private float speed = 0f;

        public Action<InteractionMode> onModeChanged;

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

        public void GenerateVegetation()
        {
            vegetationFactory.GenerateVegetation();
        }

        public void ClearVegetation()
        {
            vegetationFactory.ClearVegetation();
        }

        private void Update()
        {
            if (ProcessUI()) return;
            ProcessInteraction();
        }

        private void LateUpdate()
        {
            if (FreeLook())
                CameraMovement();
            else return;
        }

        private bool ProcessUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
            return false;
        }


        private void ProcessInteraction()
        {
            if (Input.GetKeyDown(controlsConfig.lbm))
            {
                RaycastHit hit;
                if (Physics.Raycast(GetMouseRay(), out hit))
                {
                    switch (interactionMode)
                    {
                        case InteractionMode.Add:
                            if (hit.transform.GetComponent<Combustible>() == null)
                            {
                                vegetationFactory.SpawnFlower(hit.point);
                            }
                            break;
                        case InteractionMode.Remove:
                            Flower flower = hit.transform.GetComponent<Flower>();
                            if (flower != null)
                            {
                                vegetationFactory.RemoveFlower(flower);
                            }
                            break;
                        case InteractionMode.Toggle:
                            Combustible combustible = hit.transform.GetComponent<Combustible>();
                            if (combustible == null) return;
                            if (combustible.IsBurning())
                                combustible.Extinguish();
                            else
                                combustible.Ignite();
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

