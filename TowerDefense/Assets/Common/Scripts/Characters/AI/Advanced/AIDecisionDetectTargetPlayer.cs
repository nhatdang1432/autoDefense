using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDecisionDetectTargetPlayer")]
    public class AIDecisionDetectTargetPlayer : AIDecision
    {
        /// the radius to search our target in
        [Tooltip("the radius to search our target in")]
        public float Radius = 3f;
        /// the offset to apply (from the collider's center)
        [Tooltip("the offset to apply (from the collider's center)")]
        public Vector3 DetectionOriginOffset = new Vector3(0, 0, 0);
        /// the layer(s) to search our target on
        [Tooltip("the layer(s) to search our target on")]
        public LayerMask TargetLayerMask;
        /// the layer(s) to block the sight
        [Tooltip("the layer(s) to block the sight")]
        public LayerMask ObstacleMask = LayerManager.ObstaclesLayerMask;
        /// the frequency (in seconds) at which to check for obstacles
        [Tooltip("the frequency (in seconds) at which to check for obstacles")]
        public float TargetCheckFrequency = 1f;
        /// if this is true, this AI will be able to consider itself (or its children) a target
        [Tooltip("if this is true, this AI will be able to consider itself (or its children) a target")]
        public bool CanTargetSelf = false;
        /// the maximum amount of targets the overlap detection can acquire
        [Tooltip("the maximum amount of targets the overlap detection can acquire")]
        public int OverlapMaximum = 10;
        [Tooltip("the distance to compare with")]
        public float Radiusa = 3f;

        protected Collider _collider;
        protected Vector3 _raycastOrigin;
        protected Character _character;
        protected Color _gizmoColor = Color.yellow;
        protected bool _init = false;
        protected Vector3 _raycastDirection;
        protected Collider[] _hits;
        protected float _lastTargetCheckTimestamp = 0f;
        protected bool _lastReturnValue = false;
        protected List<Transform> _potentialTargets;

        /// <summary>
        /// On init we grab our Character component
        /// </summary>
        public override void Initialization()
        {
            _lastTargetCheckTimestamp = 0f;
            _potentialTargets = new List<Transform>();
            _character = this.gameObject.GetComponentInParent<Character>();
            _collider = this.gameObject.GetComponentInParent<Collider>();
            _gizmoColor.a = 0.25f;
            _init = true;
            _lastReturnValue = false;
            _hits = new Collider[OverlapMaximum];
        }

        /// <summary>
        /// On Decide we check for our target
        /// </summary>
        /// <returns></returns>
        public override bool Decide()
        {
            return DetectTarget();
        }

        /// <summary>
        /// Returns true if a target is found within the circle
        /// </summary>
        /// <returns></returns>
        protected virtual bool DetectTarget()
        {
            // we check if there's a need to detect a new target
            if (Time.time - _lastTargetCheckTimestamp < TargetCheckFrequency)
            {
                return _lastReturnValue;
            }

            _lastTargetCheckTimestamp = Time.time;
            _raycastOrigin = _collider.bounds.center + DetectionOriginOffset / 2;
            int numberOfCollidersFound = Physics.OverlapSphereNonAlloc(_raycastOrigin, Radius, _hits, TargetLayerMask);

            // if there are no targets around, we exit
            if (numberOfCollidersFound == 0)
            {
                _lastReturnValue = false;
                return false;
            }
            // we go through each collider found
            for (int i = 0; i < OverlapMaximum; i++)
            {
                if (_hits[i] == null)
                {
                    continue;
                }

                if (!CanTargetSelf)
                {
                    if ((_hits[i].gameObject == this.gameObject) || (_hits[i].transform.IsChildOf(this.transform)))
                    {
                        continue;
                    }
                }

                _potentialTargets.Add(_hits[i].gameObject.transform);
            }


            _potentialTargets.Sort(delegate (Transform a, Transform b)
            {
                string tagA = a.gameObject.tag;
                string tagB = b.gameObject.tag;
                float distanceToA = Vector3.Distance(a.position, this.transform.position);
                float distanceToB = Vector3.Distance(b.position, this.transform.position);
                int healthA = 0;
                int healthB = 0;
                Health healthComponentA = a.GetComponent<Health>();
                if (healthComponentA != null)
                {
                    healthA = healthComponentA.CurrentHealth;
                }

                Health healthComponentB = b.GetComponent<Health>();
                if (healthComponentB != null)
                {
                    healthB = healthComponentB.CurrentHealth;
                }
                // Kiểm tra nếu healthA hoặc healthB bằng 0 thì loại bỏ khỏi danh sách
                if (healthA == 0)
                {

                    return 1;
                }
                else if (healthB == 0)
                {
                    return -1;
                }
                if (tagA == "Player" && tagB != "Player")
                    return -1;
                else if (tagA != "Player" && tagB == "Player")
                    return 1;
                // Nếu enemy A cách hero f nhỏ hơn enemy B thì đưa enemy A lên đầu danh sách
                if (distanceToA <= Radiusa && distanceToB > Radiusa)
                {
                    return -1;
                }
                else if (distanceToA > Radiusa && distanceToB <= Radiusa) // Nếu enemy B cách hero 1f nhỏ hơn enemy A thì đưa enemy B lên đầu danh sách
                {
                    return 1;
                }// Nếu cả hai enemy cách hero 1f bằng nhau hoặc cách xa hơn 1f thì sắp xếp theo thứ tự tăng dần của CurrentHealth
                else
                {
                    return healthA.CompareTo(healthB);
                }
            });/*
            // we sort our targets by distance
            _potentialTargets.Sort(delegate(Transform a, Transform b)
            {return Vector3.Distance(this.transform.position,a.transform.position)
                .CompareTo(
                    Vector3.Distance(this.transform.position,b.transform.position) );
            });*/
            // we return the first unobscured target
            foreach (Transform t in _potentialTargets)
            {
                _raycastDirection = t.position - _raycastOrigin;
                RaycastHit hit = MMDebug.Raycast3D(_raycastOrigin, _raycastDirection, _raycastDirection.magnitude, ObstacleMask.value, Color.yellow, true);
                if (hit.collider == null)
                {
                    _brain.Target = t;
                    _lastReturnValue = true;
                    _potentialTargets.Clear();
                    return true;
                }
            }
            _lastReturnValue = false;
            _potentialTargets.Clear();
            return false;
        }

        /// <summary>
        /// Draws gizmos for the detection circle
        /// </summary>
        protected virtual void OnDrawGizmosSelected()
        {
            _raycastOrigin = transform.position + DetectionOriginOffset / 2;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_raycastOrigin, Radius);
            if (_init)
            {
                Gizmos.color = _gizmoColor;
                Gizmos.DrawSphere(_raycastOrigin, Radius);
            }
        }
    }
}
