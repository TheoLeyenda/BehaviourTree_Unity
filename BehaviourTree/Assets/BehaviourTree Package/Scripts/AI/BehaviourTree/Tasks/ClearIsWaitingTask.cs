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
    }
    protected override NodeState ExecuteNode()
    {
        if (!_blackboard || !(bool)_blackboard.GetValue(_isWaitingKey)) 
            return NodeState.FAILURE;

        _blackboard.ClearValue(_isWaitingKey);
        return NodeState.SUCCESSE;
    }
}
