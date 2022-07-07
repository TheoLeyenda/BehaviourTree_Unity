using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class TaskPatrol : Task
    {
        private Transform _transform;
        private Transform[] _waypoints;

        private int _currentWaypointIndex = 0;
        private float _speed = 0.0f;


        private float _waitTime = 1.0f; //in seconds
        private float _waitCounter = 0;
        private bool _waiting = false;


        public TaskPatrol(Transform transform, Transform[] waypoints, float waitTime)
        {
            _transform = transform;
            _waypoints = waypoints;
            _waitTime = waitTime;
        }

        public TaskPatrol(Transform transform, Transform[] waypoints)
        {
            _transform = transform;
            _waypoints = waypoints;
        }

        public void SetSpeed(float value) { _speed = value; }

        public override NodeState Evaluate()
        {
            if (_waiting)
            {
                _waitCounter += Time.deltaTime;
                if (_waitCounter > _waitTime)
                    _waiting = false;
            }
            else
            {
                Transform wp = _waypoints[_currentWaypointIndex];
                if (Vector3.Distance(_transform.position, wp.position) < 0.02f)
                {
                    _transform.position = wp.position;
                    _waitCounter = 0.0f;
                    _waiting = true;

                    _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
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
}
