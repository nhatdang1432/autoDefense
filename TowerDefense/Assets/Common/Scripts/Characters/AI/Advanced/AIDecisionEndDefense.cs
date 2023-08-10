using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This decision will return true when entering the state this Decision is on.
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDecisionEndDefense")]
    public class AIDecisionEndDefense : AIDecision
    {
        /// <summary>
        /// We return true on Decide
        /// </summary>
        /// <returns></returns>
        public override bool Decide()
        {
            if (_brain.TimeInStateDefense >= 10f)
            {
                _brain.Defense = false;
                return true;
            }
            return false;
        }


    }
}
