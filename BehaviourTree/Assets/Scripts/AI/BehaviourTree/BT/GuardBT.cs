using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GuardBT : BehaviourTree
{
    [Header("General Settings")]
    public StructAnimationAI structAnimationAI;
    private Animator animator;

    [Header("Patrol Settings")]
    [SerializeField]
    protected string NameConditionIdleAnimation_TaskPatrol;
    [SerializeField]
    protected string NameConditionWalkingAnimation_TaskPatrol;
    [SerializeField]
    protected float SpeedPatrol = 2.0f;
    [SerializeField]
    protected float WaitingInPatrol = 2.5f;
    [SerializeField]
    protected float DistanceToWaypoint = 0.02f;
    public Transform[] waypoints;
    protected TaskPatrol taskPatrol; 

    [Header("Check Enemy In Fov Range Settings")]
    [SerializeField]
    protected string NameConditionWalkingAnimation_TaskCheckEnemyInFOVRange;
    [SerializeField]
    protected LayerMask EnemyLayerMask;
    [SerializeField]
    protected float _fovRange = 6.0f;
    protected CheckEnemyInFOVRange taskCheckEnemyInFOVRange;
    [SerializeField]
    protected string NameDataTarget = "target";

    [Header("Go To Target Settings")]
    [SerializeField]
    protected float DistanceToTarget = 0.02f;
    [SerializeField]
    protected float speedGoToTarget = 4.0f;
    protected TaskGoToTarget taskGoToTarget;

    protected override Node SetupTree()
    {
        animator = GetComponent<Animator>();

        taskPatrol = new TaskPatrol(transform, waypoints, SpeedPatrol, WaitingInPatrol, DistanceToWaypoint);
        taskPatrol.SetStructAnimationAI(structAnimationAI);
        taskPatrol.SetNameIdleAnimation(NameConditionIdleAnimation_TaskPatrol);
        taskPatrol.SetNameWalkingAnimation(NameConditionWalkingAnimation_TaskPatrol);

        Debug.Log(EnemyLayerMask.value);
        taskCheckEnemyInFOVRange = new CheckEnemyInFOVRange(transform, EnemyLayerMask.value, _fovRange, null, NameDataTarget);
        taskCheckEnemyInFOVRange.SetStructAnimationAI(structAnimationAI);
        taskCheckEnemyInFOVRange.SetNameWalkingAnimation(NameConditionWalkingAnimation_TaskCheckEnemyInFOVRange);

        taskGoToTarget = new TaskGoToTarget(transform, DistanceToTarget, speedGoToTarget, NameDataTarget);

        SettingStructureAnimationAI();

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                taskCheckEnemyInFOVRange,
                taskGoToTarget
            }),
            taskPatrol
        });

        taskCheckEnemyInFOVRange.SetRootNode(root);

        return root;
    }

    public void SettingStructureAnimationAI()
    {
        if (!structAnimationAI) return;

        structAnimationAI.SetAnimator(animator);
        structAnimationAI.SaveDefaultValues();
    }
}
