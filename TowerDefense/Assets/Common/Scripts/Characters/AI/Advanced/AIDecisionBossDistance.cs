using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This Decision will return true if the current Brain's Target is within the specified range, false otherwise.
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDecisionBossDistance")]
    public class AIDecisionBossDistance : AIDecision
    {
        /// The possible comparison modes
        public enum ComparisonModes { StrictlyLowerThan, LowerThan, Equals, GreatherThan, StrictlyGreaterThan }
        /// the comparison mode
        [Tooltip("the comparison mode")]
        public ComparisonModes ComparisonModeb = ComparisonModes.GreatherThan;
        /// the distance to compare with
        [Tooltip("the distance to compare with")]
        public float Distanceb = 4f;

        /// <summary>
        /// On Decide we check our distance to the Target
        /// </summary>
        /// <returns></returns>
        public override bool Decide()
        {
            return EvaluateDistanceb();
        }

        /// <summary>
        /// Returns true if the distance conditions are met
        /// </summary>
        /// <returns></returns>
        protected virtual bool EvaluateDistanceb()
        {
            if (_brain.Target == null)
            {
                return false;
            }
            if (_brain.Target.name == this.gameObject.name + "BackToPatrolBeacon")
            {
                return false;
            }

            float distance = Vector3.Distance(this.transform.position, _brain.Target.position);

            if (ComparisonModeb == ComparisonModes.StrictlyLowerThan)
            {
                return (distance < Distanceb);
            }
            if (ComparisonModeb == ComparisonModes.LowerThan)
            {
                return (distance <= Distanceb);
            }
            if (ComparisonModeb == ComparisonModes.Equals)
            {
                return (distance == Distanceb);
            }
            if (ComparisonModeb == ComparisonModes.GreatherThan)
            {
                return (distance >= Distanceb);
            }
            if (ComparisonModeb == ComparisonModes.StrictlyGreaterThan)
            {
                return (distance > Distanceb);
            }
            return false;
        }
    }
}

