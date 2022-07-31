using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class GuardBT : BehaviourTree
{
    [Header("Guard Settings")]
    [SerializeField]
    protected int Life = 100;
    [SerializeField]
    protected float Weight = 80.3f;
    [SerializeField]
    protected bool IsAlive = true;
    [SerializeField]
    protected char InitialBot = 'T';

    [Header("General Settings")]
    public StructAnimationAI structAnimationAI;
    [SerializeField]
    protected string NameDataTarget = "target";
    private Animator animator;

    [Header("Task Patrol Settings")]
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

    [Header("Task Check Enemy In Fov Range Settings")]
    [SerializeField]
    protected string NameConditionWalkingAnimation_TaskCheckEnemyInFOVRange;
    [SerializeField]
    protected LayerMask EnemyLayerMask;
    [SerializeField]
    protected float _fovRange = 6.0f;
    protected CheckEnemyInFOVRange taskCheckEnemyInFOVRange;

    [Header("Task Go To Target Settings")]
    [SerializeField]
    protected float DistanceToTarget = 0.02f;
    [SerializeField]
    protected float speedGoToTarget = 4.0f;
    protected TaskGoToTarget taskGoToTarget;

    [Header("Task Check Enemy In Attack Range")]
    [SerializeField]
    protected float attackRange;
    [SerializeField]
    protected string NameConditionAttackAnimation_TaskCheckEnemyInAttackRange;
    protected CheckEnemyInAttackRange taskCheckEnemyInAttackRange;

    [Header("Task Attack")]
    [SerializeField]
    protected float attackTime;
    [SerializeField]
    protected float damageAttack;
    [SerializeField]
    protected string NameConditionWalkingAnimation_TaskAttack;
    protected TaskAttack taskAttack;

    [Header("Service Counter")]
    [SerializeField]
    protected float intervalServiceCounter;
    protected ServiceCounter serviceCounter;

    [Header("Service Notify")]
    [SerializeField]
    protected float intervalServiceNotify;
    protected ServiceNotifiy serviceNotifiy;


    //Keys to Blackboard.
    private object NameValue;
    private object LifeValue = 100;
    private object WeightValue = 80.3f;
    private object IsAliveValue = true;
    private object InitialBotValue = 'T';
    private object TransformBotValue;
    private object PositionBotValue;

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

        serviceCounter = gameObject.AddComponent<ServiceCounter>();
        serviceCounter.SetInterval(intervalServiceCounter);
        serviceCounter.ActivateUpdateService();
        taskAttack.AddService(serviceCounter);

        serviceNotifiy = gameObject.AddComponent<ServiceNotifiy>();
        serviceNotifiy.SetInterval(intervalServiceNotify);
        serviceNotifiy.ActivateUpdateService();
        taskAttack.AddService(serviceNotifiy);

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

    protected override void InitBlackboardKeys()
    {
        base.InitBlackboardKeys();

        NameValue = gameObject.name;
        LifeValue = Life;
        WeightValue = Weight;
        IsAliveValue = IsAlive;
        InitialBotValue = InitialBot;
        TransformBotValue = transform;
        PositionBotValue = transform.position;

        GetBlackboardComponent().AddValue("NameBot", NameValue);
        GetBlackboardComponent().AddValue("Life", LifeValue);
        GetBlackboardComponent().AddValue("Weight", WeightValue);
        GetBlackboardComponent().AddValue("IsAlive", IsAliveValue);
        GetBlackboardComponent().AddValue("InicialBot", InitialBotValue);
        GetBlackboardComponent().AddValue("Transform Bot", TransformBotValue);
        GetBlackboardComponent().AddValue("Position Bot", PositionBotValue);
    }

    protected override void UpdateBlackboardKeys()
    {
        base.UpdateBlackboardKeys();

        NameValue = gameObject.name;
        LifeValue = Life;
        WeightValue = Weight;
        IsAliveValue = IsAlive;
        InitialBotValue = InitialBot;
        TransformBotValue = transform;
        PositionBotValue = transform.position;

        GetBlackboardComponent().SetValue("NameBot", NameValue);
        GetBlackboardComponent().SetValue("Life", LifeValue);
        GetBlackboardComponent().SetValue("Weight", WeightValue);
        GetBlackboardComponent().SetValue("IsAlive", IsAliveValue);
        GetBlackboardComponent().SetValue("InicialBot", InitialBotValue);
        GetBlackboardComponent().SetValue("Transform Bot", TransformBotValue);
        GetBlackboardComponent().SetValue("Position Bot", PositionBotValue);
    }

    protected override void DeinitBlackboardKeys()
    {
        base.DeinitBlackboardKeys();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            taskAttack.RemoveService(serviceNotifiy);
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            taskAttack.RemoveService(serviceCounter);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            taskAttack.ClearAllServices();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            serviceCounter.DisableUpdateService();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            serviceCounter.ActivateUpdateService();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            serviceNotifiy.DisableUpdateService();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            serviceNotifiy.ActivateUpdateService();
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            GetBlackboardComponent().RemoveValue("Transform Bot");
        }
    }
}
