﻿using UnityEngine;
using System.Collections;
using MoreMountains.TopDownEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// A class to add on a CharacterPathfinder3D equipped character.
    /// It will allow you to click anywhere on screen, which will determine a new target and the character will pathfind its way to it
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Automation/MouseDrivenPathfinderAI3D")]
    public class MouseDrivenPathfinderAI3D : MonoBehaviour 
	{
        [Header("Testing")]
        /// the camera we'll use to determine the destination from
		[Tooltip("the camera we'll use to determine the destination from")]
        public Camera Cam;
        /// a gameobject used to show the destination
		[Tooltip("a gameobject used to show the destination")]
        public GameObject Destination;

        protected CharacterPathfinder3D _characterPathfinder3D;
		protected Plane _playerPlane;
        protected bool _destinationSet = false;
        protected Camera _mainCamera;
        public GameObject house;
        private Vector3 housePosition;
        /// <summary>
        /// On awake we create a plane to catch our ray
        /// </summary>
        protected virtual void Awake()
        {
            _mainCamera = Camera.main;
            _characterPathfinder3D = this.gameObject.GetComponent<CharacterPathfinder3D>();
            _playerPlane = new Plane(Vector3.up, Vector3.zero);
            housePosition = house.transform.position;
            
        }

        /// <summary>
        /// On Update we look for a mouse click
        /// </summary>
        protected virtual void Update()
        {
            DetectMouse();
        }

        /// <summary>
        /// If the mouse is clicked, we cast a ray and if that ray hits the plane we make it the pathfinding target
        /// </summary>
        protected virtual void DetectMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                {
                    Vector3 target = hit.point;
                    Destination.transform.position = target;
                    _destinationSet = true;
                    _characterPathfinder3D.SetNewDestination(Destination.transform);

                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tower")))
                {
                    Vector3 target = (hit.point + housePosition)/3f;
                    Destination.transform.position = target;
                    _destinationSet = true;
                    _characterPathfinder3D.SetNewDestination(Destination.transform);

                }
            }
        }
    }
}