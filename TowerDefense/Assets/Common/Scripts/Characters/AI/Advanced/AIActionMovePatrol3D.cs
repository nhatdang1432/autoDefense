using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    [AddComponentMenu("TopDown Engine/Character/AI/Actions/AIActionMovePatrol3D")]
    public class AIActionMovePatrol3D : AIAction
    {
        [Header("Obstacle Detection")]
        public bool ChangeDirectionOnObstacle = true;
        public float ObstacleDetectionDistance = 1f;
        public float ObstaclesCheckFrequency = 1f;
        public LayerMask ObstacleLayerMask = LayerManager.ObstaclesLayerMask;
        public Vector3 LastReachedPatrolPoint { get; set; }

        [Header("Debug")]
        [MMReadOnly]
        public int _currentPathIndex = 0;

        private NavMeshAgent _navMeshAgent;
        private NavMeshObstacle _navMeshObstacle;
        private float _lastObstacleDetectionTimestamp = 0f;
        private MMPath _mmPath;
        private Health _health;
        private float _waitingDelay = 0f;
        protected int _indexLastFrame = -1;
        private Character _character;

        protected override void Awake()
        {
            base.Awake();
        }


        public override void Initialization()
        {
            _character = GetComponentInParent<Character>();
            _mmPath = GetComponentInParent<MMPath>();
            _navMeshAgent = GetComponentInParent<NavMeshAgent>();
            _navMeshObstacle = GetComponentInParent<NavMeshObstacle>();
            _health = _character.CharacterHealth;
            _navMeshAgent.enabled = true;
            _waitingDelay = 0f;
            _currentPathIndex = 0;
            LastReachedPatrolPoint = this.transform.position;
        }

        public override void PerformAction()
        {
            if (_navMeshAgent == null || _mmPath == null)
            {
                return;
            }

            if (!_navMeshAgent.isActiveAndEnabled || !_navMeshAgent.isOnNavMesh)
            {
                return;
            }
            Patrol();
        }

        private void Patrol()
        {

            _waitingDelay -= Time.deltaTime;

            if (_waitingDelay > 0)
            {
                _navMeshAgent.isStopped = true;
                return;
            }

            if (!_navMeshAgent.isActiveAndEnabled)
            {
                return;
            }
            CheckForObstacles();
            /*if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
            {
                _currentPathIndex = _mmPath.CurrentIndex();
                //_currentPathIndex = (_currentPathIndex + 1) % _mmPath.PathElements.Count;
                LastReachedPatrolPoint = _mmPath.CurrentPoint();
                _waitingDelay = _mmPath.PathElements[_currentPathIndex].Delay;
                Debug.Log(LastReachedPatrolPoint);

                if (_waitingDelay > 0)
                {
                    _navMeshAgent.isStopped = true;
                    return;
                }
            }
           

            if (_navMeshAgent.destination != _mmPath.CurrentPoint())
            {
                _navMeshAgent.SetDestination(_mmPath.CurrentPoint());
            }
            _navMeshAgent.isStopped = false;*/
            _currentPathIndex = _mmPath.CurrentIndex();
            if (_currentPathIndex != _indexLastFrame)
            {
                LastReachedPatrolPoint = _mmPath.CurrentPoint();
                _waitingDelay = _mmPath.PathElements[_currentPathIndex].Delay;

                if (_waitingDelay > 0)
                {
                    _navMeshAgent.isStopped = true;
                    _indexLastFrame = _currentPathIndex;
                    return;
                }
            }

            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_mmPath.CurrentPoint());

            _indexLastFrame = _currentPathIndex;

            CheckForObstacles();
        }


        private void CheckForObstacles()
        {
            if (!ChangeDirectionOnObstacle)
            {
                return;
            }

            if (Time.time - _lastObstacleDetectionTimestamp < ObstaclesCheckFrequency)
            {
                return;
            }

            bool hit = Physics.BoxCast(
                _navMeshAgent.transform.position,
                _navMeshAgent.transform.localScale / 2f,
                _navMeshAgent.transform.forward,
                out RaycastHit hitInfo,
                _navMeshAgent.transform.rotation,
                ObstacleDetectionDistance,
                ObstacleLayerMask
            );

          if (hit)
                {
                    ChangeDirection();
                }

            _lastObstacleDetectionTimestamp = Time.time;
        }

        public void ChangeDirection()
        {
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
            _navMeshObstacle.enabled = true;

            StartCoroutine(ResumeAgent());
        }

        private IEnumerator ResumeAgent()
        {
            yield return new WaitForSeconds(1f);

            _navMeshObstacle.enabled = false;
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_mmPath.CurrentPoint());
        }

        public override void OnExitState()
        {
            base.OnExitState();

            if (_navMeshAgent != null && _navMeshAgent.isActiveAndEnabled && _navMeshAgent.isOnNavMesh)
            {
                _navMeshAgent.isStopped = true;
            }
        }

        protected virtual void OnRevive()
        {
            Initialization();
        }

        protected virtual void OnEnable()
        {
            if (_health == null)
            {
                _health = GetComponent<Health>();
            }

            if (_health != null)
            {
                _health.OnRevive += OnRevive;
            }
        }

        protected virtual void OnDisable()
        {
            if (_health != null)
            {
                _health.OnRevive -= OnRevive;
            }
        }

    }
}

