using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This Decision will return true if the current Brain's Target is within the specified range, false otherwise.
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDistanceToTargetMouse")]
    public class AIDistanceToTargetMouse : AIDecision
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
        public override void Initialization()
        {
            _mainCamera = Camera.main;
            _characterPathfinder3D = this.gameObject.GetComponent<CharacterPathfinder3D>();
            _playerPlane = new Plane(Vector3.up, Vector3.zero);
        }
        public override bool Decide()
        {
            return DetectMouse();
        }

        /// <summary>
        /// Returns true if the distance conditions are met
        /// </summary>
        /// <returns></returns>
        protected virtual bool DetectMouse()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ground")))
                {
                    Vector3 target = new Vector3(hit.point.x,1,hit.point.z);
                    Destination.transform.position = target;
                    _brain.Target.position = target;
                    return true;
                }
            }
            return false;
        }
    }
}