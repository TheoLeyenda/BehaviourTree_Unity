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

    public CheckEnemyInAttackRange(Transform transform, string nameData, float attackRange)
    {
        _transform = transform;
        _nameData = nameData;
        _attackRange = attackRange;
    }

    public void SetStructAnimationAI(StructAnimationAI structAnimationAI) { _structAnimationAI = structAnimationAI; }

    public void SetNameAnimationAttack(string nameAnimationAttack) { _nameAnimationAttack = nameAnimationAttack; }

    public override NodeState Evaluate()
    {
        object t = GetData(_nameData);
        if (t == null)
        {
            state = NodeState.FAILURE;
            return state;
        }

        Transform target = (Transform)t;
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

        state = NodeState.FAILURE;
        return state;
    }


}