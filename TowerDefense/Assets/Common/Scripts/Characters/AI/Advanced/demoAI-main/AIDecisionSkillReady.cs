using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDecisionSkillReady")]
    public class AIDecisionSkillReady : AIDecision
    {
        public override bool Decide()
        {
            return _brain.Skillready;
        }
    }
}