using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    [AddComponentMenu("TopDown Engine/Character/AI/Actions/AiActionEmBattleDefense")]
    public class AiActionEmBattleDefense : AIAction
    {
        protected CharacterMovement _characterMovement;
        [SerializeField] private float _searchRadius = 30f;
        protected CharacterPathfinder3D _characterPathfinder3D;
        public Transform Destination;
        [SerializeField] private float _standBehindDistance = 30f;

        //private State currentState;

        public override void Initialization()
        {
            _characterMovement = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterMovement>();
            _characterPathfinder3D = this.gameObject.GetComponent<CharacterPathfinder3D>();
        }
        public override void PerformAction()
        {
            CheckAndSortWithNearbyTanks();
        }
        public override void OnEnterState()
        {
            _brain.Defense = true;
        }

        protected void CheckAndSortWithNearbyTanks()
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, _searchRadius);

            // Tìm tướng tank gần nhất
            GameObject closestTank = null;
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
                // Đặt hero đứng sau tướng tank gần nhất
                Vector3 tankPosition = closestTank.transform.position;
                Vector3 heroPosition = transform.position;
                Vector3 direction = (heroPosition - tankPosition).normalized;
                Vector3 destination = tankPosition + direction * _standBehindDistance;
                Destination.position = destination;
                _characterPathfinder3D.SetNewDestination(Destination);
            }
            else if (_brain.TargetPosition != null)
            {
                _characterPathfinder3D.SetNewDestination(_brain.TargetPosition);
            }
            
        }


        public override void OnExitState()
        {
            base.OnExitState();

            _characterPathfinder3D?.SetNewDestination(null);
            _characterMovement?.SetHorizontalMovement(0f);
            _characterMovement?.SetVerticalMovement(0f);
        }
    }
}
