using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    [RequireComponent(typeof(Blackboard))]
    public abstract class BehaviourTree : MonoBehaviour
    {
        private Node _root = null;
        private bool isRunning = true;

        protected Blackboard BlackboardComponent;

        protected virtual void Start()
        {
            BlackboardComponent = GetComponent<Blackboard>();

            InitBlackboardKeys();

            StartLogic();
            _root = SetupTree();
        }

        protected virtual void Update()
        {
            if (isRunning)
            {
                if (_root != null)
                    _root.Evaluate();
                else
                {
                    Debug.LogError("RootNull");
                }

                UpdateBlackboardKeys();
                BlackboardComponent.UpdateKeysData();
            }
        }

        protected virtual void OnDestroy()
        {
            DeinitBlackboardKeys();
        }

        public void StartLogic()
        {
            isRunning = true;
        }

        public void StopLogic()
        {
            isRunning = false;
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
            if (BlackboardComponent != null)
            {
                BlackboardComponent.ClearValues();
            }
        }

        public Blackboard GetBlackboardComponent()
        {
            return BlackboardComponent;
        }
    }
}
