using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class DebugLogTask : Task
{
    private string _debugLogMessage;

    Blackboard _blackboard;
    private string _resetTreeKey;
    public DebugLogTask(string debugLogMessage) 
    {
        _debugLogMessage = debugLogMessage;
        TypeNode = "DebugLogTask";
    }

    protected override NodeState ExecuteNode()
    {
        base.ExecuteNode();
        Debug.Log(_debugLogMessage);
        return NodeState.SUCCESSE;
    }
}
