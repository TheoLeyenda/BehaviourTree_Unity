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
    protected string NameDataTarget = "Target";
    [SerializeField]
    protected string NameConditionIdleAnimation;
    [SerializeField]
    protected string NameConditionWalkingAnimation;
    [SerializeField]
    protected string NameConditionChargeAnimation;
    [SerializeField]
    protected string NameConditionAttackAnimation;
    [SerializeField]
    protected string IsMovementKey = "IsWalking";
    [SerializeField]
    protected string IsIdleKey = "IsIdle";
    [SerializeField]
    protected string EnableCheckWalkOrIdleAnimationKey = "EnableCheckWalkOrIdleAnimation";
    [SerializeField]
    protected string IsWaitingKey = "IsWaiting";
    private Animator animator;

    [Header("Task Patrol Settings")]
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
    protected CheckEnemyInAttackRange taskCheckEnemyInAttackRange;

    [Header("Task Attack")]
    [SerializeField]
    protected float attackTime;
    [SerializeField]
    protected float damageAttack;
    protected TaskAttack taskAttack;

    [Header("Service Counter")]
    [SerializeField]
    protected float intervalServiceCounter;
    protected ServiceCounter serviceCounter;

    [Header("Service Notify")]
    [SerializeField]
    protected float intervalServiceNotify;
    protected ServiceNotifiy serviceNotifiy;

    protected CheckWalkOrIdleAnimationTask checkWalkOrIdleAnimationTask;
    protected ClearTargetTask clearTargetTask;
    protected ClearIsWaitingTask clearIsWaitingTask;

    //Keys to Blackboard.
    private object NameValue;
    private object LifeValue = 100;
    private object WeightValue = 80.3f;
    private object IsAliveValue = true;
    private object InitialBotValue = 'T';
    private object TransformBotValue;
    private object PositionBotValue;
    private object IsMovement = false;
    private object IsIdle = false;
    private object EnableCheckWalkOrIdleAnimation = false;

    [Header("EnableAttackDecorator Settings")]
    [SerializeField]
    protected BlackboardDecorator.ETypeNotifyObserver EnableAttacktypeNotifyObserver;
    [SerializeField]
    protected ETypeObserverAbort EnableAttacktypeObserverAbort;
    [SerializeField]
    protected string EnableAttackKey;
    [SerializeField]
    protected BlackboardDecorator.EKeyQuery EnableAttackkeyQuery;

    protected override Root SetupTree()
    {
        animator = GetComponent<Animator>();

        taskPatrol = new TaskPatrol(transform, waypoints, SpeedPatrol, WaitingInPatrol, DistanceToWaypoint, _blackboardComponent,IsMovementKey, IsIdleKey, IsWaitingKey);
        taskPatrol.SetNameIdleAnimation(NameConditionIdleAnimation);
        taskPatrol.SetNameWalkingAnimation(NameConditionWalkingAnimation);
        taskPatrol.SetStructAnimationAI(structAnimationAI);

        checkWalkOrIdleAnimationTask = new CheckWalkOrIdleAnimationTask(NameConditionWalkingAnimation, NameConditionIdleAnimation, structAnimationAI, _blackboardComponent, IsMovementKey, IsIdleKey, EnableCheckWalkOrIdleAnimationKey);

        BlackboardDecorator ClearTargetDecorator = new BlackboardDecorator(
            BlackboardDecorator.ETypeNotifyObserver.OnChangeResult
            , ETypeObserverAbort.None
            , EnableAttackKey
            , BlackboardDecorator.EKeyQuery.IsNotSet
            , _blackboardComponent);

        clearTargetTask = new ClearTargetTask(_blackboardComponent, NameDataTarget);

        clearTargetTask.AddDecorator(ClearTargetDecorator);

        clearIsWaitingTask = new ClearIsWaitingTask(_blackboardComponent, IsWaitingKey);

        taskCheckEnemyInFOVRange = new CheckEnemyInFOVRange(transform, EnemyLayerMask.value, _fovRange, null, NameDataTarget, _blackboardComponent);
        taskCheckEnemyInFOVRange.SetNameWalkingAnimation(NameConditionChargeAnimation);
        taskCheckEnemyInFOVRange.SetStructAnimationAI(structAnimationAI);

        taskGoToTarget = new TaskGoToTarget(transform, DistanceToTarget, speedGoToTarget, NameDataTarget, _blackboardComponent);

        taskCheckEnemyInAttackRange = new CheckEnemyInAttackRange(transform, NameDataTarget, attackRange, _blackboardComponent);
        taskCheckEnemyInAttackRange.SetNameAnimationAttack(NameConditionAttackAnimation);
        taskCheckEnemyInAttackRange.SetStructAnimationAI(structAnimationAI);

        taskAttack = new TaskAttack(transform, NameDataTarget, attackTime, damageAttack, _blackboardComponent);
        taskAttack.SetNameAnimationWalking(NameConditionWalkingAnimation);
        taskAttack.SetStructAnimationAI(structAnimationAI);

        serviceCounter = gameObject.AddComponent<ServiceCounter>();
        //serviceCounter.SetInterval(intervalServiceCounter);
        //serviceCounter.ActivateUpdateService();
        //taskAttack.AddService(serviceCounter);

        serviceNotifiy = gameObject.AddComponent<ServiceNotifiy>();
        //serviceNotifiy.SetInterval(intervalServiceNotify);
        //serviceNotifiy.ActivateUpdateService();
        //taskAttack.AddService(serviceNotifiy);

        SettingStructureAnimationAI();

        BlackboardDecorator CheckEnemyInAttackRangeDecorator = new BlackboardDecorator(
            EnableAttacktypeNotifyObserver
            , EnableAttacktypeObserverAbort
            , EnableAttackKey
            , EnableAttackkeyQuery
            , _blackboardComponent);

        Sequence sequenceCheckEnemyInAttackRange = new Sequence(new List<Node>
        {
            taskCheckEnemyInAttackRange,
            taskAttack,
            clearIsWaitingTask,
        });

        sequenceCheckEnemyInAttackRange.AddDecorator(CheckEnemyInAttackRangeDecorator);

        BlackboardDecorator sequenceCheckEnemyInFOVRangeDecorator = new BlackboardDecorator(
            EnableAttacktypeNotifyObserver
            , EnableAttacktypeObserverAbort
            , EnableAttackKey
            , EnableAttackkeyQuery
            , _blackboardComponent);

        Sequence sequenceCheckEnemyInFOVRange = new Sequence(new List<Node>
        {
            taskCheckEnemyInFOVRange,
            taskGoToTarget,
            clearIsWaitingTask,
        });

        sequenceCheckEnemyInFOVRange.AddDecorator(sequenceCheckEnemyInFOVRangeDecorator);

        Sequence sequencePatrol = new Sequence(new List<Node>
        {
            taskPatrol,
            checkWalkOrIdleAnimationTask,
        });

        Selector compositeRoot = new Selector(new List<Node>
        {
            clearTargetTask,
            sequenceCheckEnemyInAttackRange,
            sequenceCheckEnemyInFOVRange,
            sequencePatrol,
        });

        Root root = new Root(compositeRoot);

        taskCheckEnemyInFOVRange.SetRootNode(root);

        //root.ShowNodesRoot();

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
        GetBlackboardComponent().AddValue(NameDataTarget, null);

        GetBlackboardComponent().AddValue(IsMovementKey, IsMovement);
        GetBlackboardComponent().AddValue(IsIdleKey, IsIdle);
        GetBlackboardComponent().AddValue(EnableCheckWalkOrIdleAnimationKey, EnableCheckWalkOrIdleAnimation);
        GetBlackboardComponent().AddValue(EnableAttackKey, true);
        GetBlackboardComponent().AddValue(IsWaitingKey, false);
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

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            GetBlackboardComponent().SetValue(EnableAttackKey, true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) 
        {
            _blackboardComponent.SetValue(EnableCheckWalkOrIdleAnimationKey, true);
            GetBlackboardComponent().SetValue(EnableAttackKey, false);
        }
        
    }
}
