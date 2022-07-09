using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class GuardBT : BehaviourTree
    {
        [Header("Patrol Settings")]
        [SerializeField]
        protected string NameConditionIdleAnimation;
        [SerializeField]
        protected string NameConditionWalkingAnimation;
        public Transform[] waypoints;
        public float _speed = 2.0f;
        public StructAnimationAI structAnimationAI;
        protected TaskPatrol taskPatrol;

        protected override Node SetupTree()
        {
            Node root = new TaskPatrol(transform, waypoints);
            taskPatrol = (TaskPatrol)root;
            taskPatrol.SetSpeed(_speed);
            taskPatrol.SetStructAnimationAI(structAnimationAI);
            taskPatrol.SetNameIdleAnimation(NameConditionIdleAnimation);
            taskPatrol.SetNameWalkingAnimation(NameConditionWalkingAnimation);
            taskPatrol.SettingStructureAnimationAI();

            return root;
        }
    }
}
