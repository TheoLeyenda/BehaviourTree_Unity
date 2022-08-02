using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree 
{
    public enum EResultDecorator 
    {
        CONTINUE,
        LOOP,
        WAIT,
        ABORT,
    }

    public class Decorator : MonoBehaviour
    {
        protected virtual void Start()
        {
            hideFlags = HideFlags.HideInInspector;
        }
        public Decorator() {}

        public virtual EResultDecorator CheckDecorator() 
        {
            return EResultDecorator.CONTINUE;
        }
    }
}
