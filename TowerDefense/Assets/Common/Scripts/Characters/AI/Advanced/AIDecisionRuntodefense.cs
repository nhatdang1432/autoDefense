using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This decision will return true after the specified duration, picked randomly between the min and max values (in seconds) has passed since the Brain has been in the state this decision is in. Use it to do something X seconds after having done something else.
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDecisionRuntodefense")]
    public class AIDecisionRuntodefense : AIDecision
    {
        public Transform Destination;
        protected Plane _playerPlane;
        protected bool _destinationSet = false;
        protected Camera _mainCamera;
        private Vector3 housePosition;
        private GameObject[] Tower;
        private List<GameObject> towers;
        public override void Initialization()
        {
            Tower = GameObject.FindObjectsOfType<GameObject>();
            towers = new List<GameObject>();
            foreach (GameObject a in Tower)
            {
                if(a.layer == LayerMask.NameToLayer("Tower"))
                {
                    towers.Add(a);
                }
            }
            _mainCamera = Camera.main;
            _playerPlane = new Plane(Vector3.up, Vector3.zero);
            housePosition = GameObject.Find("Main").transform.position;

        }
        public override void OnEnterState()
        {
            base.OnEnterState();
            _brain.OnDame = false;
        }
        public void GetDame()
        {
            _brain.OnDame = true;
        }
        public override bool Decide()
        {
            if (_brain.OnDame == true)
                return DetectTarget();
            return false;

        }
        public override void OnExitState()
        {
            base.OnExitState();
        }
        protected virtual bool DetectTarget()
        {
            towers.Sort(delegate (GameObject a, GameObject b)
            {
                return Vector3.Distance(this.transform.position, a.transform.position)
                   .CompareTo(
                       Vector3.Distance(this.transform.position, b.transform.position));
            });
            foreach (GameObject t in towers)
            {
                Vector3 direction = t.transform.position - housePosition;
                direction.Normalize();
                float distance = Vector3.Distance(t.transform.position, housePosition);
                direction *= distance - 3f;
                Vector3 targetPosition = housePosition + direction;
                Destination.position = targetPosition;
                _brain.TargetPosition = Destination;
                _brain.KillBoss = false;
                return true;
            }
            return false;
        }
    }
}
