using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public enum ETypeObserverAbort
    {
        None,
        Self,
        LowerPriority,
        Both,
    }

    public class Decorator : MonoBehaviour
    {
        protected Node _nodeDecorator;
        protected ETypeObserverAbort _typeObserverAbort = ETypeObserverAbort.None;
        protected virtual void Start()
        {
            hideFlags = HideFlags.HideInInspector;
        }
        public Decorator() { }

        public virtual bool CheckDecorator(){ return true; }

        public void SetNodeDecorator(Node node) => _nodeDecorator = node;
    }
}
