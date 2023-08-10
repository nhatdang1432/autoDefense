using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
public class Idle : AIState
{
    [SerializeField] private AIAction currentAction;
    public override void OnEnterState()
    {
        Debug.Log("dung yen");
    }
    public override void UpdateState()
    {
        //SwitchState<AIActionDoNothing>();
    }
    public override void OnExitState()
    {

    }

    public void SwitchState<T>() where T : AIAction
    {
        if (currentAction != null)
        {
            currentAction.OnExitState();
        }

        currentAction = GetComponent<T>();
        currentAction.OnEnterState();
    }
}
*/