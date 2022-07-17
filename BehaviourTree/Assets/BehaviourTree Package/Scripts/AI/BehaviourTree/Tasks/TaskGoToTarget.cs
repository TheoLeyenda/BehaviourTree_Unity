using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class TaskGoToTarget : Task
{
    private Transform _transform;
    private float _distanceToTarget;
    private float _speed;
    private string _nameDataTarget;
    public TaskGoToTarget(Transform transform, float distanceToTarget, float speed, string nameDataTarget)
    {
        _transform = transform;
        _distanceToTarget = distanceToTarget;
        _speed = speed;
        _nameDataTarget = nameDataTarget;
        TypeNode = "TaskGoToTarget";
    }

    public TaskGoToTarget(Transform transform)
    {
        _transform = transform;
        TypeNode = "TaskGoToTarget";
    }

    public void SetDistanceToTarget(float distanceToTarget) => _distanceToTarget = distanceToTarget;

    public override NodeState Evaluate()
    {
        base.Evaluate();
        Transform target = (Transform)GetData(_nameDataTarget);
        if (target)
        {
            if (Vector3.Distance(_transform.position, target.position) > _distanceToTarget)
            {

                _transform.position = Vector3.MoveTowards(_transform.position, target.position, _speed * Time.deltaTime);
                _transform.LookAt(target.position);
            }
            
            state = NodeState.RUNNING;
            return state;
        }
        else
        {
            state = NodeState.FAILURE;
            return state;
        }

    }

}
