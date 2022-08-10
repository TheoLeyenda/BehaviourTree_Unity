using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class BreakSequenceTask : Task
{
    public BreakSequenceTask() 
    {
        TypeNode = "BreakSequenceTask";
    }
    protected override NodeState ExecuteNode()
    {
        base.ExecuteNode();
        return NodeState.FAILURE;
    }
}
