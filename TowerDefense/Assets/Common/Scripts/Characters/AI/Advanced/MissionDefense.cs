using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This decision will return true if an object on its TargetLayer layermask is within its specified radius, false otherwise. It will also set the Brain's Target to that object.
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/MissionDefense")]
    public class MissionDefense : AIDecision
    {
        public Transform Destination;
        protected Plane _playerPlane;
        protected bool _destinationSet = false;
        protected Camera _mainCamera;
        private Vector3 housePosition;
        public override void Initialization()
        {
            _mainCamera = Camera.main;
            _playerPlane = new Plane(Vector3.up, Vector3.zero);
            housePosition = GameObject.Find("Main").transform.position;
        }

        public override bool Decide()
        {
            return DetectTarget();
        }

        protected virtual bool DetectTarget()
        {
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
                Debug.DrawRay(ray.origin, ray.direction * 100, Color.yellow);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Tower")))
                {
                    Vector3 direction = new Vector3(hit.point.x, 1f, hit.point.z) - housePosition ;
                    direction.Normalize();
                    float distance = Vector3.Distance(new Vector3(hit.point.x, 1f, hit.point.z), housePosition);
                    direction *= distance - 3f;
                    Vector3 targetPosition = housePosition + direction;
                    Destination.position = targetPosition;
                    _brain.TargetPosition = Destination;
                    _brain.KillBoss = false;
                    return true;
                }
            }
            return false;
        }
    }
}
