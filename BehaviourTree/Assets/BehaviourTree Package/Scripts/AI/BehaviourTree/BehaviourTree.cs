using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    [RequireComponent(typeof(Blackboard))]
    public abstract class BehaviourTree : MonoBehaviour
    {
        private Node _root = null;
        private bool _isRunning = true;

        protected Blackboard _blackboardComponent;

        protected virtual void Start()
        {
            _blackboardComponent = GetComponent<Blackboard>();

            InitBlackboardKeys();

            StartLogic();
            _root = SetupTree();
        }

        protected virtual void Update()
        {
            if (_isRunning)
            {
                if (_root != null)
                    _root.Evaluate();
                else
                {
                    Debug.LogError("RootNull");
                }

                UpdateBlackboardKeys();
                _blackboardComponent.UpdateKeysData();
            }
        }

        protected virtual void OnDestroy()
        {
            DeinitBlackboardKeys();
        }

        public void StartLogic()
        {
            _isRunning = true;
        }

        public void StopLogic()
        {
            _isRunning = false;
        }

        protected abstract Node SetupTree();

        public void ShowTree()
        {
            Debug.Log("Root contiene: ");
            if (_root != null)
            {
                _root.ShowChildrens();
            }
        }

        protected virtual void InitBlackboardKeys() { }

        protected virtual void UpdateBlackboardKeys() { }

        protected virtual void DeinitBlackboardKeys()
        {
            if (_blackboardComponent != null)
            {
                _blackboardComponent.ClearValues();
            }
        }

        public Blackboard GetBlackboardComponent()
        {
            return _blackboardComponent;
        }
    }
}
