using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        private Node _root = null;

        private BehaviourTreeManager BTM;

        protected virtual void Start()
        {
            InitBTM();
            CheckIsValidBTM();
            _root = SetupTree();
            StartLogic();
        }
        protected void InitBTM()
        {
            BTM = BehaviourTreeManager.InstanceBTM;
        }

        protected bool CheckIsValidBTM() 
        {
            if (BTM) 
            {
                return true;
            }
            Debug.LogError("Error: There is no BehaviourTreeManager in the scene, BTM is NULL");
            return false;
        }

        protected virtual void UpdateBehaviourTree()
        {
            if (_root != null)
                _root.Evaluate();
            else
            {
                Debug.Log("RootNull");
            }
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

        public void StartLogic() 
        {
            if (CheckIsValidBTM()) 
            {
                BTM.AddBehaviourTreesToRun(this);
            }
        }

        public void StopLogic()
        {
            if (CheckIsValidBTM())
            {
                BTM.RemoveBehaviourTreeToRun(this);
            }
        }
    }
}
