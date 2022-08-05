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

        protected void CheckTypeAbort()
        {
            switch (_typeObserverAbort) 
            {
                case ETypeObserverAbort.None:
                    _nodeDecorator.SetAbortType(EAbortType.None);
                    break;
                case ETypeObserverAbort.Both:
                    _nodeDecorator.SetAbortType(EAbortType.AbortBoth);
                    break;
                case ETypeObserverAbort.Self:
                    _nodeDecorator.SetAbortType(EAbortType.AbortSelf);
                    break;
                case ETypeObserverAbort.LowerPriority:
                    _nodeDecorator.SetAbortType(EAbortType.AbortLowerPriorirty);
                    break;
            }
        }
    }
}
