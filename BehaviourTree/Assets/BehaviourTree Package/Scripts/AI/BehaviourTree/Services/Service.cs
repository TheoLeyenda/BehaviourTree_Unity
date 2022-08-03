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

        protected virtual void Start()
        {
            hideFlags = HideFlags.HideInInspector;
        }

        public void SetInterval(float interval)
        {
            _interval = interval;
            Time.fixedDeltaTime = _interval;
        }

        public void ActivateUpdateService()
        {
            StartCoroutine(InternalUpdateService());
            _useUpdate = true;
        }

        public void DisableUpdateService()
        {
            StopCoroutine(InternalUpdateService());
            _useUpdate = false;
        }

        public virtual void OnBecomeRelevant() { }

        private IEnumerator InternalUpdateService()
        {
            yield return new WaitForSeconds(_interval);

            if (_useUpdate)
            {
                UpdateService();
                StartCoroutine(InternalUpdateService());
            }
        }

        protected virtual void UpdateService(){}

    }
}
