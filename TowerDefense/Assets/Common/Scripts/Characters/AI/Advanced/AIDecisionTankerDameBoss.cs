using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// This decision will return true after the specified duration, picked randomly between the min and max values (in seconds) has passed since the Brain has been in the state this decision is in. Use it to do something X seconds after having done something else.
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDecisionTankerDameBoss")]
    public class AIDecisionTankerDameBoss : AIDecision, MMEventListener<MMGameEvent>
    {
        void OnEnable()
        {
            this.MMEventStartListening<MMGameEvent>();
        }
        void OnDisable()
        {
            this.MMEventStopListening<MMGameEvent>();
        }

        public virtual void OnMMEvent(MMGameEvent gameE)
        {
            _brain.Target = _brain.TargetBoss;
            _brain.OnBoss = true;
            
        }
        public override bool Decide()
        {
            if (_brain.OnBoss == true)
            {
                return true;
            }
            return false;
        }
    }
}
