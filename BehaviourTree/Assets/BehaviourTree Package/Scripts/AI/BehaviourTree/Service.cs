using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class Service : MonoBehaviour
    {
        //Cuando pasa por el nodo se ejecuta el "OnBecomeRelevant"

        //En el FixedUpdate se ejecuta el "TickNode"

        [Header("Settings Service")]
        [SerializeField]
        private float _interval = 0.5f;

        protected virtual void Start()
        {
            SetInterval(_interval);
        }

        public void SetInterval(float interval)
        {
            _interval = interval;
            Time.fixedDeltaTime = _interval;
        }

        public virtual void OnBecomeRelevant() { }

        protected virtual void FixedUpdate() { }
    }
}
