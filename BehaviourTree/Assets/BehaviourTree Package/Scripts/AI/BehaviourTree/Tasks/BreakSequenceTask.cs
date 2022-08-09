using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class BreakSequenceTask : Task
{
    protected override NodeState ExecuteNode()
    {
        return NodeState.FAILURE;
    }
}
