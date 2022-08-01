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

    private Blackboard _blackboardComponent;

    public TaskAttack(Transform transform, string nameData, float attackTime, float damageAttack, Blackboard blackboardComponent)
    {
        _transform = transform;
        _nameData = nameData;
        _attackTime = attackTime;
        _damageAttack = damageAttack;
        TypeNode = "TaskAttack";
        _blackboardComponent = blackboardComponent;
    }

    public void SetStructAnimationAI(StructAnimationAI structAnimationAI) { _structAnimationAI = structAnimationAI; }

    public void SetNameAnimationWalking(string nameAnimationWalking) { _nameAnimationWalking = nameAnimationWalking; }

    public override NodeState Evaluate()
    {
        base.Evaluate();
        Transform target = (Transform)_blackboardComponent.GetValue(_nameData);

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
                _blackboardComponent.SetValue(_nameData, null);
                _structAnimationAI.ClearValuesAnimationSlots();
                _structAnimationAI.SetDataAnimationSlot(_nameAnimationWalking);
            }
            else
            {
                _attackCounter = 0;
            }
        }

        if (target == null || !_enemyHealthComponent) 
        {
            _blackboardComponent.SetValue(_nameData, null);
            _structAnimationAI.ClearValuesAnimationSlots();
            _structAnimationAI.SetDataAnimationSlot(_nameAnimationWalking);
        }

        state = NodeState.RUNNING;
        return state;
    }
}
