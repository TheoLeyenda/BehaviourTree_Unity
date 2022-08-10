using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class ClearIsWaitingTask : Task
{
    private Blackboard _blackboard;
    private string _isWaitingKey;

    public ClearIsWaitingTask(Blackboard blackboard, string isWaitingKey) 
    {
        _blackboard = blackboard;
        _isWaitingKey = isWaitingKey;
        TypeNode = "ClearIsWaitingTask";
    }
    protected override NodeState ExecuteNode()
    {
        base.ExecuteNode();

        if (!_blackboard) 
            return NodeState.FAILURE;

        _blackboard.ClearValue(_isWaitingKey);
        return NodeState.SUCCESSE;
    }
}
