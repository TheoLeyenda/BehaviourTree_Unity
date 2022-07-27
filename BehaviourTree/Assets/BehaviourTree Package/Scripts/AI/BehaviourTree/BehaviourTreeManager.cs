using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
namespace BehaviorTree {
    public class BehaviourTreeManager : MonoBehaviour
    {
        [Min(0), SerializeField]
        private float Interval = 0.5f; //InSeconds

        private bool isRunning = true;

        public static BehaviourTreeManager InstanceBTM;

        private List<BehaviourTree> BehaviourTreesToRun = new List<BehaviourTree>();

        private Thread thread;

        private void Awake()
        {
            if (InstanceBTM == null)
            {
                InstanceBTM = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            //ACA VOY A CORRER EN UN THREAD TODOS LOS UPDATES DE LOS BEHAVIUR TREE
            StartManager();
            SetInterval(Interval);
            StartThreadQueuer(() => { UpdateBehaviurTrees(); } );
        }

        private void UpdateBehaviurTrees()
        {
            Debug.Log("Ejecucion en Thread");

            Thread.Sleep((int)Interval);

            for(int i = 0; i < BehaviourTreesToRun.Count; i++)
            {
                BehaviourTreesToRun[i].UpdateBehaviourTree();
            }

            Debug.Log("Solo se ejecuto una vez");

            //Descomentar para que funcione el loop
            if (BehaviourTreesToRun.Count > 0 && isRunning)
            {
                thread.Start();
            }
        }

        public void AddBehaviourTreesToRun(BehaviourTree item) 
        {
            if (!BehaviourTreesToRun.Contains(item))
            {
                BehaviourTreesToRun.Add(item);

                if(thread != null && BehaviourTreesToRun.Count == 1)
                {
                    thread.Start();
                }
            }
        }

        public void RemoveBehaviourTreeToRun(BehaviourTree item) 
        {
            if (BehaviourTreesToRun.Contains(item))
            {
                BehaviourTreesToRun.Remove(item);
            }
        }

        public void ClearBehaviourTreesToRun() 
        {
            BehaviourTreesToRun.Clear();
        }

        public void StartThreadQueuer(Action SomeFunctions)
        {
            thread = new Thread(new ThreadStart(SomeFunctions));

            thread.Start();
        }

        public void StartManager() 
        {
            isRunning = true;
            if (thread != null) 
            {
                thread.Start();
            }
        }

        public void StopManager() 
        {
            isRunning = false;
        }

        //Interval in seconds
        public void SetInterval(float NewInterval)
        {
            Interval = NewInterval;
            Interval = Interval * 1000;
        }
    }
}
