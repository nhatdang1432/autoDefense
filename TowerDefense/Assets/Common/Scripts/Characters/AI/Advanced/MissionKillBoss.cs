using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/MissionKillBoss")]
    public class MissionKillBoss : AIDecision, MMEventListener<MMBossMission>
    {
        private Vector3 housePosition;
        public Transform Destination;
        public List<Transform> bosses;
        public float radius = 4f;
        public override void Initialization()
        {
            housePosition = GameObject.Find("Main").transform.position;
        }

        public override bool Decide()
        {
            return DetectTarget();
        }

        protected virtual bool DetectTarget()
        {

            if (_brain.TargetBoss != null && _brain.OnBoss == true)
            {
                if (_brain.TargetBoss.GetComponent<Health>().CurrentHealth == 0)
                {
                    _brain.Target = null;
                    _brain.OnBoss = false;
                    _brain.KillBoss = false;
                }
                return false;
            }
            bosses.Sort(delegate (Transform a, Transform b)
            {
                int healthA = a.GetComponent<Health>().CurrentHealth;
                int healthB = b.GetComponent<Health>().CurrentHealth;

                // Kiểm tra nếu healthA hoặc healthB bằng 0 thì loại bỏ khỏi danh sách
                if (healthA == 0)
                {
                    return 1;
                }
                else if (healthB == 0)
                {
                    return -1;
                }
                else
                {
                    return Vector3.Distance(this.transform.position, a.transform.position)
                       .CompareTo(
                           Vector3.Distance(this.transform.position, b.transform.position));
                }
            });

            if (bosses.Count > 0 && _brain.Defense == false && _brain.OnBoss == false)
            {
                Vector3 direction = bosses[0].position - housePosition;
                direction.Normalize();
                Vector3 targetPosition = housePosition + direction * (Vector3.Distance(bosses[0].position, housePosition) - radius);
                Destination.position = targetPosition;
                _brain.TargetPosition = Destination;
                _brain.TargetBoss = bosses[0].transform;
                return true;
            }
            return false;
        }

        protected void OnEnable()
        {
            this.MMEventStartListening<MMBossMission>();
        }
        protected void OnDisable()
        {
            this.MMEventStopListening<MMBossMission>();
        }

        public virtual void OnMMEvent(MMBossMission gameE)
        {
            bosses.Add(gameE.EventName);
        }
    }
}
