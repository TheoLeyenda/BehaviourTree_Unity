using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class CheckWalkOrIdleAnimationTask : Task
{
    private string _nameConditionAnimationWalk;
    private string _nameConditionAnimationIdle;
    private StructAnimationAI _structAnimationAI;
    private Blackboard _blackboard;
    private string _isWalkingKey;
    private string _isIdleKey;
    private string _enableCheckWalkOrIdleAnimationKey;

    public CheckWalkOrIdleAnimationTask(string nameConditionAnimationWalk, string nameConditionAnimationIdle, StructAnimationAI structAnimationAI, Blackboard blackboard, string isWalkingKey, string isIdleKey, string enableCheckWalkOrIdleAnimationKey) 
    {
        _nameConditionAnimationIdle = nameConditionAnimationIdle;
        _nameConditionAnimationWalk = nameConditionAnimationWalk;
        _structAnimationAI = structAnimationAI;
        _blackboard = blackboard;
        _isWalkingKey = isWalkingKey;
        _isIdleKey = isIdleKey;
        _enableCheckWalkOrIdleAnimationKey = enableCheckWalkOrIdleAnimationKey;
        TypeNode = "CheckWalkOrIdleAnimationTask";
    }

    protected override NodeState ExecuteNode()
    {
        base.ExecuteNode();

        if (!_structAnimationAI || !_blackboard || !(bool)_blackboard.GetValue(_enableCheckWalkOrIdleAnimationKey)) 
            return NodeState.FAILURE;

        if ((bool)_blackboard.GetValue(_isIdleKey)) 
        {
            _structAnimationAI.ClearValuesAnimationSlots();
            _structAnimationAI.SetDataAnimationSlot(_nameConditionAnimationIdle);
            state = NodeState.SUCCESSE;
        }
        else if((bool)_blackboard.GetValue(_isWalkingKey)) 
        {
            _structAnimationAI.ClearValuesAnimationSlots();
            _structAnimationAI.SetDataAnimationSlot(_nameConditionAnimationWalk);
            state = NodeState.SUCCESSE;
        }

        _blackboard.SetValue(_enableCheckWalkOrIdleAnimationKey, false);

        return state;
    }

}
