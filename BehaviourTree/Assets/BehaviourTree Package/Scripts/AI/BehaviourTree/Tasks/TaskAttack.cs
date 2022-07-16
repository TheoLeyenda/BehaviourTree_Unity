using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskAttack : Task
{
    private string _nameData;
    private Transform _transform;
    private Transform _lastTarget;
    private HealthComponent _enemyHealthComponent;

    private float _attackTime;
    private float _attackCounter;
    private float _damageAttack;

    private string _nameAnimationWalking;

    private StructAnimationAI _structAnimationAI;

    public TaskAttack(Transform transform, string nameData, float attackTime, float damageAttack)
    {
        _transform = transform;
        _nameData = nameData;
        _attackTime = attackTime;
        _damageAttack = damageAttack;
    }

    public void SetStructAnimationAI(StructAnimationAI structAnimationAI) { _structAnimationAI = structAnimationAI; }

    public void SetNameAnimationWalking(string nameAnimationWalking) { _nameAnimationWalking = nameAnimationWalking; }

    public override NodeState Evaluate()
    {
        Transform target = (Transform)GetData(_nameData);

        if (target != _lastTarget)
        {
            _enemyHealthComponent = target.GetComponent<HealthComponent>();
            _lastTarget = target;
        }

        _attackCounter += Time.deltaTime;
        if (_attackCounter >= _attackTime && _enemyHealthComponent)
        {
            bool enemyIsDead = _enemyHealthComponent.OnTakeDamage(_damageAttack, _transform);

            if (enemyIsDead)
            {
                ClearData(_nameData);
                _structAnimationAI.ClearValuesAnimationSlots();
                _structAnimationAI.SetDataAnimationSlot(_nameAnimationWalking);
            }
            else
            {
                _attackCounter = 0;
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}
