using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckEnemyInAttackRange : Task
{
    private Transform _transform;
    private float _attackRange;
    private string _nameData;
    private string _nameAnimationAttack;
    private StructAnimationAI _structAnimationAI;
    private Blackboard _blackboardComponent;

    public CheckEnemyInAttackRange(Transform transform, string nameData, float attackRange, Blackboard blackboardComponent)
    {
        _transform = transform;
        _nameData = nameData;
        _attackRange = attackRange;
        TypeNode = "CheckEnemyInAttackRange";
        _blackboardComponent = blackboardComponent;
    }

    public void SetStructAnimationAI(StructAnimationAI structAnimationAI) { _structAnimationAI = structAnimationAI; }

    public void SetNameAnimationAttack(string nameAnimationAttack) { _nameAnimationAttack = nameAnimationAttack; }
    public override NodeState Evaluate()
    {
        base.Evaluate();

        if (!_blackboardComponent) return NodeState.FAILURE;

        object t = _blackboardComponent.GetValue(_nameData);
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
        if (target)
        {
            if (Vector3.Distance(_transform.position, target.position) <= _attackRange)
            {
                if (_structAnimationAI)
                {
                    _structAnimationAI.ClearValuesAnimationSlots();
                    _structAnimationAI.SetDataAnimationSlot(_nameAnimationAttack);
                }
                state = NodeState.SUCCESSE;
                return state;
            }
        }
        else 
        {
            state = NodeState.SUCCESSE;
            return state;
        }
        state = NodeState.FAILURE;
        return state;
    }


}
