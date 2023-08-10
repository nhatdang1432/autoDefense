using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using UnityEngine.AI;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This action will make the character pathfind its way back to its last patrol point
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Actions/AIActionPathfinderToPatrol3D")]
    //[RequireComponent(typeof(AIActionMovePatrol3D))]
    //[RequireComponent(typeof(CharacterMovement))]
    public class AIActionPathfinderToPatrol3D : AIAction
    {
        protected NavMeshAgent _navMeshAgent;
        protected NavMeshObstacle _navMeshObstacle;
        protected Transform _backToPatrolTransform;
        protected AIActionMovePatrol3D _aiActionMovePatrol3D;

        /// <summary>
        /// On init we grab our CharacterMovement ability
        /// </summary>
        public override void Initialization()
        {

            _navMeshAgent = GetComponentInParent<NavMeshAgent>();
            _navMeshObstacle = GetComponentInParent<NavMeshObstacle>();
            _aiActionMovePatrol3D = GetComponentInParent<AIActionMovePatrol3D>();
            GameObject backToPatrolBeacon = new GameObject();
            backToPatrolBeacon.name = this.gameObject.name + "BackToPatrolBeacon";
            _backToPatrolTransform = backToPatrolBeacon.transform;
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
        protected virtual void Move()
        {
            if (_aiActionMovePatrol3D == null)
            {
                return;
            }

            
            _backToPatrolTransform.position = _aiActionMovePatrol3D.LastReachedPatrolPoint;
            _navMeshAgent.SetDestination(new Vector3 (_backToPatrolTransform.position.x,0f,_backToPatrolTransform.position.z));
            _brain.Target = _backToPatrolTransform;
            //Debug.Log(_aiActionMovePatrol3D.LastReachedPatrolPoint);
        }


        public override void OnEnterState()
        {
            base.OnEnterState();
            _navMeshObstacle.enabled = false;
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
        }

        /// <summary>
        /// On exit state we stop our movement
        /// </summary>
        public override void OnExitState()
        {
            base.OnExitState();

            _navMeshAgent.isStopped = true;
        }
    }
}
