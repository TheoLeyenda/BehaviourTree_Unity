using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class TaskPatrol : Task
{
    private Transform _transform;
    private Transform[] _waypoints;
    private Animator _animator;

    private StructAnimationAI _structAnimationAI;
    private string _nameWalkingAnimation;
    private string _nameIdleAnimation;

    private int _currentWaypointIndex = 0;
    private float _speed = 0.0f;

    private float _distanceToWaypoint;
    private float _waitTime = 1.0f; //in seconds
    private float _waitCounter = 0;
    private bool _waiting = false;

    public TaskPatrol(Transform transform, Transform[] waypoints,float speed, float waitTime, float distanceToWaypoint)
    {
        _distanceToWaypoint = distanceToWaypoint;
        _transform = transform;
        _waypoints = waypoints;
        _waitTime = waitTime;
        _speed = speed;
        if (_transform)
        {
            _animator = _transform.GetComponent<Animator>();
        }
        TypeNode = "TaskPatrol";
    }

    public TaskPatrol(Transform transform, Transform[] waypoints)
    {
        _transform = transform;
        _waypoints = waypoints;
        if (_transform)
        {
            _animator = _transform.GetComponent<Animator>();
        }
        TypeNode = "TaskPatrol";
    }

    public void SetStructAnimationAI(StructAnimationAI newStructAnimationAI) { _structAnimationAI = newStructAnimationAI; }

    public void SetSpeed(float value) { _speed = value; }

    public void SetNameWalkingAnimation(string newNameWalkingAnimation) { _nameWalkingAnimation = newNameWalkingAnimation; }

    public void SetNameIdleAnimation(string newNameIdleAnimation) { _nameIdleAnimation = newNameIdleAnimation; }

    public void SetAnimator(Animator animator) { _animator = animator; }

    public void SetDistanceToWaypoint(float distanceToWaypoint) => _distanceToWaypoint = distanceToWaypoint;

    public override NodeState Evaluate()
    {
        base.Evaluate();
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter > _waitTime)
            {
                _waiting = false;
                if (_animator && _structAnimationAI)
                {
                    //Camina
                    _structAnimationAI.ClearValuesAnimationSlots();
                    _structAnimationAI.SetDataAnimationSlot(_nameWalkingAnimation);
                }
            }
        }
        else
        {
            Transform wp = _waypoints[_currentWaypointIndex];
            if (Vector3.Distance(_transform.position, wp.position) < _distanceToWaypoint)
            {
                _transform.position = wp.position;
                _waitCounter = 0.0f;
                _waiting = true;
               
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                if (_animator && _structAnimationAI)
                {
                    //Idle
                    _structAnimationAI.ClearValuesAnimationSlots();
                    _structAnimationAI.SetDataAnimationSlot(_nameIdleAnimation);
                }
            }
            else
            {
                _transform.position = Vector3.MoveTowards(_transform.position, wp.position, _speed * Time.deltaTime);
                _transform.LookAt(wp.position);
            }
        }

        state = NodeState.RUNNING;
        return state;
    }
}