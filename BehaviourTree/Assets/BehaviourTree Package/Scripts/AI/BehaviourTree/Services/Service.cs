using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class Service : MonoBehaviour
    {
        //Cuando pasa por el nodo se ejecuta el "OnBecomeRelevant"

        //En el FixedUpdate se ejecuta el "TickNode"
        [SerializeField]
        protected float _interval = 0.5f;
        protected float _auxInterval = 0.5f;
        private bool _useUpdate = false;
        private bool _enableExecute = true;
        private bool _enableLastExecution = false;
        protected virtual void Start()
        {
            hideFlags = HideFlags.HideInInspector;
        }
        protected void Update()
        {
            if ((_useUpdate && _enableExecute) || _enableLastExecution)
            {
                if (_interval > 0)
                {
                    _interval = _interval - Time.deltaTime;
                }
                else
                {
                    _enableLastExecution = false;
                    UpdateService();
                    _interval = _auxInterval;
                }
            }
        }
        public void SetEnableExecute(bool value)
        {
            bool lastExecute = _enableExecute;
            _enableExecute = value;

            if (!lastExecute && _enableExecute && _useUpdate)
            {
                ActivateUpdateService();
            }
        }

        public bool GetEnableExecute() 
        {
            return _enableExecute;
        }

        public void SetEnableLastExecution(bool value) => _enableLastExecution = value;

        public void SetInterval(float interval)
        {
            _interval = interval;
            _auxInterval = interval;
        }

        public void ActivateUpdateService()
        {
            _useUpdate = true;
        }

        public void DisableUpdateService()
        {
            _useUpdate = false;
        }

        public virtual void OnBecomeRelevant() { }

        protected virtual void UpdateService(){}

    }
}
