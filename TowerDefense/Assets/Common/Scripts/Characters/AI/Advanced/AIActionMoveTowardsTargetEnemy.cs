using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    [AddComponentMenu("TopDown Engine/Character/AI/Actions/AIActionMoveTowardsTargetEnemy")]
    public class AIActionMoveTowardsTargetEnemy : AIAction
    {
        public float MinimumDistance = 1f;

        private NavMeshAgent _navMeshAgent;
        private NavMeshObstacle _navMeshObstacle;
        private Vector3 _targetPosition;

        public override void Initialization()
        {
            _navMeshAgent = GetComponentInParent<NavMeshAgent>();
            _navMeshObstacle = GetComponentInParent<NavMeshObstacle>();
            _navMeshObstacle.enabled = false;
            _navMeshAgent.enabled = true;
        }

        public override void PerformAction()
        {
            Move();
        }

        protected virtual void Move()
        {
            if (_brain.Target == null)
            {
                return;
            }

            _targetPosition = _brain.Target.position;

            if (Vector3.Distance(transform.position, _targetPosition) > MinimumDistance)
            {
                _navMeshObstacle.enabled = false;
                _navMeshAgent.enabled = true;
                _navMeshAgent.SetDestination(_targetPosition);
            }
            else
            {
                _navMeshAgent.enabled = false;
                _navMeshObstacle.enabled = true;
                _navMeshObstacle.carving = true;
            }
        }



        public override void OnExitState()
        {
            base.OnExitState();

        }
    }
}
