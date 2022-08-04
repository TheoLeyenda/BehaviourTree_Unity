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
    private Blackboard _blackboardComponent;

    public TaskGoToTarget(Transform transform, float distanceToTarget, float speed, string nameDataTarget, Blackboard blackboardComponent)
    {
        _transform = transform;
        _distanceToTarget = distanceToTarget;
        _speed = speed;
        _nameDataTarget = nameDataTarget;
        TypeNode = "TaskGoToTarget";
        _blackboardComponent = blackboardComponent;
    }

    public void SetDistanceToTarget(float distanceToTarget) => _distanceToTarget = distanceToTarget;

    protected override NodeState ExecuteNode()
    {
        base.ExecuteNode();
        Transform target = (Transform)_blackboardComponent.GetValue(_nameDataTarget);
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
