using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class CheckEnemyInFOVRange : Task
{
    private int _enemyLayerMask;
    private Transform _transform;
    private float _fovRange;
    private Node _rootNode;
    private Animator _animator;
    private StructAnimationAI _structAnimationAI;
    private string _nameDataTarget;
    private string _nameWalkingAnimation;

    public void SetStructAnimationAI(StructAnimationAI newStructAnimationAI) { _structAnimationAI = newStructAnimationAI; }

    public void SetNameWalkingAnimation(string newNameWalkingAnimation) { _nameWalkingAnimation = newNameWalkingAnimation; }

    public void SetAnimator(Animator animator) { _animator = animator; }

    public CheckEnemyInFOVRange(Transform transform, int enemyLayerMask, float fovRange, Node rootNode, string nameDataTarget)
    {
        _transform = transform;
        _enemyLayerMask = enemyLayerMask;
        _fovRange = fovRange;
        _rootNode = rootNode;
        _nameDataTarget = nameDataTarget;
        if (_transform)
        {
            _animator = _transform.GetComponent<Animator>();
        }
        TypeNode = "CheckEnemyInFOVRange";
    }

    public void SetRootNode(Node node) => _rootNode = node;

    public override NodeState Evaluate()
    {
        //Debug.Log("Evaluate");
        object t = GetData(_nameDataTarget);
        if (t == null)
        {
            Collider[] colliders = Physics.OverlapSphere(_transform.position, _fovRange, _enemyLayerMask);

            if (colliders.Length > 0)
            {
                if (_rootNode != null)
                {
                    _rootNode.SetData(_nameDataTarget, colliders[0].transform);
                    Debug.Log(colliders[0].transform.name);
                    if (_animator && _structAnimationAI)
                    {
                        _structAnimationAI.ClearValuesAnimationSlots();
                        _structAnimationAI.SetDataAnimationSlot(_nameWalkingAnimation);
                    }
                    state = NodeState.SUCCESSE;
                    return state;
                }
                else
                {
                    Debug.LogError("_rootNode Nulo");
                }

                state = NodeState.FAILURE;
                return state;
            }
        }
        state = NodeState.SUCCESSE;
        return state;
    }

    
}
