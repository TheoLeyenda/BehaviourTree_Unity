using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class ServiceNotifiy : Service 
{
    public override void OnBecomeRelevant()
    {
        base.OnBecomeRelevant();

        Debug.Log("OnBecomeRelevant: Notify!");
    }
    protected override void UpdateService()
    {
        Debug.Log("UpdateService: Notify!");
    }
}
