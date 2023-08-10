using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Requires a CharacterMovement ability. Makes the character move up to the specified MinimumDistance in the direction of the target. 
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Actions/AIActionRecedeBoss")]
    public class AIActionRecedeBoss : AIAction
    {
        protected CharacterMovement _characterMovement;
        protected CharacterPathfinder3D _characterPathfinder3D;
        protected Health _health;
        private Vector3 Position;
        private Vector3 safePosition;
        public Transform Decision;
        private float distanceboss = 4f;
        [SerializeField] private float _searchRadius = 30f;
        private Vector3 initialPosition;
        private Vector3 initialDirection;

        /// <summary>
        /// On init we grab our CharacterMovement ability
        /// </summary>
        public override void Initialization()
        {
            Position = GameObject.Find("Main").transform.position;
            _characterMovement = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterMovement>();
            _characterPathfinder3D = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterPathfinder3D>();
            _health = this.gameObject.GetComponent<Health>();
            if (_characterPathfinder3D == null)
            {
                Debug.LogWarning(this.name + " : the AIActionPathfinderToTarget3D AI Action requires the CharacterPathfinder3D ability");
            }
        }

        /// <summary>
        /// On PerformAction we move
        /// </summary>
        public override void PerformAction()
        {
            Move();
        }

        /// <summary>
        /// Moves the character towards the target if needed
        /// </summary>

        protected void CheckAndSortWithNearbyTanks()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _searchRadius);
            GameObject closestTank = null;
            // Tìm tướng tank gần nhất
            float closestDistance = float.MaxValue;

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Tank"))
                {
                    float distance = Vector3.Distance(hitCollider.transform.position, transform.position);
                    if (distance < closestDistance)
                    {
                        closestTank = hitCollider.gameObject;
                        closestDistance = distance;
                    }
                }
            }
            if (closestTank != null)
            {
                Position = closestTank.transform.position;
            }
        }

        /*protected virtual void Move()
        {
            if (_brain.Target == null)
            {
                _characterPathfinder3D.SetNewDestination(null);
                return;
            }
            else
            {
                CheckAndSortWithNearbyTanks();
                float distance = Vector3.Distance(transform.position, _brain.Target.transform.position);
                Vector3 direction = _brain.Target.transform.position - Position;
                direction.Normalize();
                float distanceToSafePosition = distance - distanceboss - 2;
                RaycastHit hit;
                if (Physics.Raycast(transform.position, direction, out hit, distance))
                {
                    if (hit.collider.CompareTag("wall"))
                    {
                        float distanceToWall = hit.distance;
                        if (distanceToWall <= 2f)
                        {
                            Vector3 wallNormal = hit.normal;
                            Vector3 rightDirection = Vector3.Cross(wallNormal, Vector3.up).normalized;
                            Vector3 avoidPosition = hit.point + rightDirection * 2f;
                            direction = (avoidPosition - transform.position).normalized;
                        }
                    }
                }
                safePosition = transform.position + direction * distanceToSafePosition;
                Decision.position = safePosition;
                _characterPathfinder3D.SetNewDestination(Decision);
            }
        }*/
        protected virtual void Move()
        {
            if (_brain.Target == null)
            {
                _characterPathfinder3D.SetNewDestination(null);
                return;
            }
            else
            {
                CheckAndSortWithNearbyTanks();
                float distance = Vector3.Distance(transform.position, _brain.Target.transform.position);
                Vector3 direction = _brain.Target.transform.position - Position;
                direction.Normalize();
                float distanceToSafePosition = distance - distanceboss - 2;
                RaycastHit hit;
                bool hitWall = Physics.Raycast(transform.position, direction, out hit, distance) && hit.collider.CompareTag("wall");

                if (hitWall)
                {
                    float distanceToWall = hit.distance;
                    if (distanceToWall <= 2f)
                    {
                        Vector3 wallNormal = hit.normal;
                        Vector3 rightDirection = Vector3.Cross(wallNormal, Vector3.up).normalized;
                        Vector3 avoidPosition = hit.point + rightDirection * 2f;

                        // Kiểm tra va chạm khi đi vòng qua vật thể
                        if (!Physics.Raycast(avoidPosition, direction, distance))
                        {
                            // Nếu không gặp va chạm, đi vòng qua vật thể
                            Vector3 newDirection = (avoidPosition - transform.position).normalized;
                            direction = Vector3.Lerp(direction, newDirection, 0.5f).normalized;
                        }
                    }
                }

                Vector3 safePosition = transform.position + direction * distanceToSafePosition;
                Decision.position = safePosition;
                _characterPathfinder3D.SetNewDestination(Decision);
            }
        }



        /// <summary>
        /// On exit state we stop our movement
        /// </summary>
        public override void OnExitState()
        {
            base.OnExitState();
            _characterPathfinder3D?.SetNewDestination(null);
            _characterMovement?.SetHorizontalMovement(0f);
            _characterMovement?.SetVerticalMovement(0f);
        }
    }
}

