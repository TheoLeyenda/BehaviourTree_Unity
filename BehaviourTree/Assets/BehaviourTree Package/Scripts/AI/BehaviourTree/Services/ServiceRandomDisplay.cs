using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class ServiceRandomDisplay : Service
{
    public List<string> RandomDisplayInBecomeRelevant = new List<string>();
    public List<string> RandomDisplayInUpdate = new List<string>();

    public override void OnBecomeRelevant()
    {
        base.OnBecomeRelevant();
        int index = Random.Range(0, RandomDisplayInBecomeRelevant.Count - 1);
        Debug.Log("OnBecomeRelevant: " + RandomDisplayInBecomeRelevant[index]);
    }
    protected override void UpdateService()
    {
        base.UpdateService();
        int index = Random.Range(0, RandomDisplayInUpdate.Count - 1);
        Debug.Log("UpdateService: " + RandomDisplayInUpdate[index]);
    }
}
