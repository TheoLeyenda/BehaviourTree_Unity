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

    private Blackboard _blackboard;
    private string _walkingKey;
    private string _idleKey;
    private string _isWaitingKey;
    public TaskPatrol(Transform transform, Transform[] waypoints,float speed, float waitTime, float distanceToWaypoint, Blackboard blackboard, string walkingKey, string idleKey, string isWaiting)
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
        _blackboard = blackboard;
        _walkingKey = walkingKey;
        _idleKey = idleKey;
        _isWaitingKey = isWaiting;
    }

    public void SetBlackboard(Blackboard blackboard) => _blackboard = blackboard;

    public void SetWalkingKey(string walkingKey) => _walkingKey = walkingKey;

    public void SetIdleKey(string idleKey) => _idleKey = idleKey;

    public void SetIsWaitingKey(string isWaitingKey) => _isWaitingKey = isWaitingKey;

    public void SetStructAnimationAI(StructAnimationAI newStructAnimationAI) { _structAnimationAI = newStructAnimationAI; }

    public void SetSpeed(float value) { _speed = value; }

    public void SetNameWalkingAnimation(string newNameWalkingAnimation) { _nameWalkingAnimation = newNameWalkingAnimation; }

    public void SetNameIdleAnimation(string newNameIdleAnimation) { _nameIdleAnimation = newNameIdleAnimation; }

    public void SetAnimator(Animator animator) { _animator = animator; }

    public void SetDistanceToWaypoint(float distanceToWaypoint) => _distanceToWaypoint = distanceToWaypoint;

    protected override NodeState ExecuteNode()
    {
        base.ExecuteNode();
        _waiting = (bool)_blackboard.GetValue(_isWaitingKey);
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if (_waitCounter > _waitTime)
            {
                _waiting = false;
                _blackboard.SetValue(_isWaitingKey, false);
                _blackboard.SetValue(_idleKey, false);
                _blackboard.SetValue(_walkingKey, true);
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
                _blackboard.SetValue(_idleKey, true);
                _blackboard.SetValue(_walkingKey, false);
                _blackboard.SetValue(_isWaitingKey, true);
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