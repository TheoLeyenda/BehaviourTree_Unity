using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        private Node _root = null;
        private bool isRunning = true;
        protected virtual void Start()
        {
            StartLogic();
            _root = SetupTree();
        }

        public virtual void Update()
        {
            if (isRunning)
            {
                if (_root != null)
                    _root.Evaluate();
                else
                {
                    Debug.Log("RootNull");
                }
            }
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
    }
}
