using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{

    public class Service : MonoBehaviour
    {
        //Cuando pasa por el nodo se ejecuta el "OnBecomeRelevant"

        //En el FixedUpdate se ejecuta el "TickNode"

        protected float _interval = 0.5f;
        private bool _useUpdate = true;
        private bool _enableExecute = true;
        private bool _enableLastExecution = false;
        private bool _corrutineStarted = false;
        protected virtual void Start()
        {
            hideFlags = HideFlags.HideInInspector;
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
            Time.fixedDeltaTime = _interval;
        }

        public void ActivateUpdateService()
        {
            if (!_corrutineStarted)
            {
                StartCoroutine(InternalUpdateService());
                _corrutineStarted = true;
            }
            _useUpdate = true;
        }

        public void DisableUpdateService()
        {
            _useUpdate = false;
        }

        public virtual void OnBecomeRelevant() { }

        private IEnumerator InternalUpdateService()
        {
            yield return new WaitForSeconds(_interval);

            if ((_useUpdate && _enableExecute) || _enableLastExecution)
            {
                _enableLastExecution = false;
                UpdateService();
            }
            StartCoroutine(InternalUpdateService());
        }

        protected virtual void UpdateService(){}

    }
}
