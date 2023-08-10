using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;

namespace MoreMountains.TopDownEngine
{
    /// <summary>
    /// An Action that shoots using the currently equipped weapon. If your weapon is in auto mode, will shoot until you exit the state, and will only shoot once in SemiAuto mode. You can optionnally have the character face (left/right) the target, and aim at it (if the weapon has a WeaponAim component).
    /// </summary>
    [AddComponentMenu("TopDown Engine/Character/AI/Actions/AIActionRotate")]
    public class AIActionRotate : AIAction
    {
        [SerializeField] private GameObject[] Enemy;
        public override void Initialization()
        {

        }
        public override void PerformAction()
        {
            transform.LookAt(_brain.Target.position);
        }
    }
}