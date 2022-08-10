using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class ClearTargetTask : Task
{
    private Blackboard _blackboard;
    private string _targetKey;

    public ClearTargetTask(Blackboard blackboard, string targetKey) 
    {
        _blackboard = blackboard;
        _targetKey = targetKey;
        TypeNode = "ClearTargetTask";
    }

    protected override NodeState ExecuteNode()
    {
        base.ExecuteNode();

        if (!_blackboard || _blackboard.GetValue(_targetKey) == null)
            return NodeState.FAILURE;

        _blackboard.ClearValue(_targetKey);

        return NodeState.SUCCESSE;
    }
}
