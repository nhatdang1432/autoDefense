using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDecision2Ally")]
    //[RequireComponent(typeof(Character))]
    public class AIDecision2Ally : AIDecision
    {
        public LayerMask Player;
        public float radius;
        public override bool Decide()
        {
            return WaitBoss();
        }
        protected virtual bool WaitBoss()
        {
            Collider[] hitPlayer = Physics.OverlapSphere(transform.position, radius, Player);
            if (hitPlayer.Length > 2)
            {
                _brain.Target = _brain.TargetBoss;
                _brain.OnBoss = true;
                return true;
            }
            return false;
        }
    }
}
