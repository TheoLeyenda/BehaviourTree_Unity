using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class DebugLogTask : Task
{
    private string _debugLogMessage;

    Blackboard _blackboard;
    private string _resetTreeKey;
    public DebugLogTask(string debugLogMessage,string resetTreeKey, Blackboard blackboard) 
    {
        _debugLogMessage = debugLogMessage;
        _resetTreeKey = resetTreeKey;
        _blackboard = blackboard;
        TypeNode = "DebugLogTask";
    }

    protected override NodeState ExecuteNode()
    {
        base.ExecuteNode();
        if ((bool)_blackboard.GetValue(_resetTreeKey))
        {
            Debug.Log(_debugLogMessage);
            _blackboard.SetValue(_resetTreeKey, false);
            return NodeState.SUCCESSE;
        }
        else 
        {
            return NodeState.FAILURE;
        }

    }
}
