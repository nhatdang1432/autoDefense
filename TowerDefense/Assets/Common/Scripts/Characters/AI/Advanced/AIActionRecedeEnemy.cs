using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// Requires a CharacterMovement ability. Makes the character move up to the specified MinimumDistance in the direction of the target. 
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Actions/AIActionRecedeEnemy")]
    public class AIActionRecedeEnemy : AIAction
    {
        protected CharacterMovement _characterMovement;
        protected CharacterPathfinder3D _characterPathfinder3D;
        protected Health _health;
        private Vector3 Position;
        public float radius;
        private Vector3 safePosition;
        public Transform Decision;
        [SerializeField] private float _searchRadius = 30f;

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
            Check();
            Move();
        }

        /// <summary>
        /// Moves the character towards the target if needed
        /// </summary>
        protected virtual void Check()
        {
            float healthper = (float)(_health.CurrentHealth) / (float)(_health.MaximumHealth) * 100;
            if (healthper > 70)
            {
                _brain.Distance = 3;
            }
            else if (healthper < 70 && healthper > 20)
            {
                _brain.Distance = 4;
            }
            else
            {
                _brain.Distance = 5;
            }
        }

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
                float distanceToSafePosition = distance - _brain.Distance;
                safePosition = transform.position + direction * distanceToSafePosition;
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

