using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class BehaviourTree : MonoBehaviour
    {
        private Node _root = null;

        protected virtual void Start()
        {
            _root = SetupTree();
        }

        protected virtual void Update()
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
    }
}
