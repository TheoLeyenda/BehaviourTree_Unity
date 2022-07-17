using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class ServiceCounter : Service
{

    private float _counterOnBecomeRelevat = 0;

    private float _counterFixedUpdate = 0;

    public override void OnBecomeRelevant()
    {
        base.OnBecomeRelevant();

        _counterOnBecomeRelevat++;

        Debug.Log("OnBecomeRelevant: " + _counterOnBecomeRelevat);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        _counterFixedUpdate++;

        Debug.Log("FixedUpdate: " + _counterFixedUpdate);
    }
}
