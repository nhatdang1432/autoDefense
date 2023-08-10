using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This decision returns true if the Character got hit this frame, or after the specified number of hits has been reached.
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDecisionSkillsReady")]
    //[RequireComponent(typeof(Health))]
    public class AIDecisionSkillsReady : AIDecision
    {
        // Start is called before the first frame update
        public override bool Decide()
        {
            if (_brain.Target != null)
                return _brain.Skillready;
            return false;
        }
    }
}
