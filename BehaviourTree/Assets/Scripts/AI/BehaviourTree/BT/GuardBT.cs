using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class GuardBT : BehaviourTree
    {
        public Transform[] waypoints;
        public float _speed = 2.0f;
        //Setearle esta speed a la task "TaskPatrol"
        protected override Node SetupTree()
        {
            Node root = new TaskPatrol(transform, waypoints);
            TaskPatrol taskPatrol = (TaskPatrol)root;
            taskPatrol.SetSpeed(_speed);
            return root;
        }
    }
}
