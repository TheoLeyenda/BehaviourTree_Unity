using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class ServiceCounter : Service
{
    private float _counterOnBecomeRelevat = 0;

    private float _counterUpdate = 0;

    public override void OnBecomeRelevant()
    {
        base.OnBecomeRelevant();

        _counterOnBecomeRelevat++;

        Debug.Log("OnBecomeRelevant: " + _counterOnBecomeRelevat);
    }
    protected override IEnumerator UpdateService()
    {
        _counterUpdate++;
        
        Debug.Log("UpdateService: " + _counterUpdate);
        return base.UpdateService();
    }
}
