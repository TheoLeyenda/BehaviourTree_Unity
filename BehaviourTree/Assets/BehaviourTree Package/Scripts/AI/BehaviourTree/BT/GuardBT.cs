using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GuardBT : BehaviourTree
{
    [Header("General Settings")]
    public StructAnimationAI structAnimationAI;
    [SerializeField]
    protected string NameDataTarget = "target";
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

    [Header("Go To Target Settings")]
    [SerializeField]
    protected float DistanceToTarget = 0.02f;
    [SerializeField]
    protected float speedGoToTarget = 4.0f;
    protected TaskGoToTarget taskGoToTarget;

    [Header("Check Enemy In Attack Range")]
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected string NameConditionAttackAnimation_TaskCheckEnemyInAttackRange;
    protected CheckEnemyInAttackRange taskCheckEnemyInAttackRange;

    [Header("TaskAttack")]
    [SerializeField]
    protected float attackTime;
    [SerializeField]
    protected float damageAttack;
    [SerializeField]
    protected string NameConditionWalkingAnimation_TaskAttack;
    protected TaskAttack taskAttack;

    protected override Node SetupTree()
    {
        animator = GetComponent<Animator>();

        taskPatrol = new TaskPatrol(transform, waypoints, SpeedPatrol, WaitingInPatrol, DistanceToWaypoint);
        taskPatrol.SetNameIdleAnimation(NameConditionIdleAnimation_TaskPatrol);
        taskPatrol.SetNameWalkingAnimation(NameConditionWalkingAnimation_TaskPatrol);
        taskPatrol.SetStructAnimationAI(structAnimationAI);

        taskCheckEnemyInFOVRange = new CheckEnemyInFOVRange(transform, EnemyLayerMask.value, _fovRange, null, NameDataTarget);
        taskCheckEnemyInFOVRange.SetNameWalkingAnimation(NameConditionWalkingAnimation_TaskCheckEnemyInFOVRange);
        taskCheckEnemyInFOVRange.SetStructAnimationAI(structAnimationAI);

        taskGoToTarget = new TaskGoToTarget(transform, DistanceToTarget, speedGoToTarget, NameDataTarget);

        taskCheckEnemyInAttackRange = new CheckEnemyInAttackRange(transform, NameDataTarget, attackRange);
        taskCheckEnemyInAttackRange.SetNameAnimationAttack(NameConditionAttackAnimation_TaskCheckEnemyInAttackRange);
        taskCheckEnemyInAttackRange.SetStructAnimationAI(structAnimationAI);

        taskAttack = new TaskAttack(transform, NameDataTarget, attackTime, damageAttack);
        taskAttack.SetNameAnimationWalking(NameConditionWalkingAnimation_TaskAttack);
        taskAttack.SetStructAnimationAI(structAnimationAI);

        SettingStructureAnimationAI();

        Sequence sequenceCheckEnemyInAttackRange = new Sequence(new List<Node>
        {
            taskCheckEnemyInAttackRange,
            taskAttack,
        });

        Sequence sequenceCheckEnemyInFOVRange = new Sequence(new List<Node>
        {
            taskCheckEnemyInFOVRange,
            taskGoToTarget,
        });

        Node root = new Selector(new List<Node>
        {
            sequenceCheckEnemyInAttackRange,
            sequenceCheckEnemyInFOVRange,
            taskPatrol,
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
