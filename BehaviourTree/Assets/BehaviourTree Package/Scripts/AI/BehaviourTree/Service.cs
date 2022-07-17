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
        protected float _interval = 0.5f;
        private bool _useUpdate = true;

        public void SetInterval(float interval)
        {
            _interval = interval;
            Time.fixedDeltaTime = _interval;
        }

        public void ActivateUpdateService()
        {
            StartCoroutine(UpdateService());
            _useUpdate = true;
        }

        public void DisableUpdateService()
        {
            StopCoroutine(UpdateService());
            _useUpdate = false;
        }

        public virtual void OnBecomeRelevant() { }

        protected virtual IEnumerator UpdateService()
        {
            yield return new WaitForSeconds(_interval);

            if (_useUpdate)
            {
                StartCoroutine(UpdateService());
            }
        }


    }
}
