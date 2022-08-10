using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
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
        public static Action<Decorator> OnAbortSelf;
        public static Action<Decorator> OnAbortLowerPriority;
        public static Action<Decorator> OnAbortBoth;
        public static Action<Decorator> OnAbortNone;

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
                case ETypeObserverAbort.Both:
                    if (OnAbortBoth != null) 
                    {
                        OnAbortBoth(this);
                    }
                    break;
                case ETypeObserverAbort.LowerPriority:
                    if (OnAbortLowerPriority != null) 
                    {
                        OnAbortLowerPriority(this);
                    }
                    break;
                case ETypeObserverAbort.Self:
                    if (OnAbortSelf != null) 
                    {
                        OnAbortSelf(this);
                    }
                    break;
                case ETypeObserverAbort.None:
                    if(OnAbortNone != null) 
                    {
                        OnAbortNone(this);
                    }
                    break;
            }
        }
    }
}
