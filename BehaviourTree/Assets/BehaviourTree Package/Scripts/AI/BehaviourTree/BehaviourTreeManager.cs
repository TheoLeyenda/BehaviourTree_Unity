using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace BehaviorTree {
    public class BehaviourTreeManager : MonoBehaviour
    {
        public static BehaviourTreeManager InstanceBTM;

        private List<BehaviourTree> BehaviourTreesToRun;

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
        }

        public void AddBehaviourTreesToRun(BehaviourTree item) 
        {
            if (!BehaviourTreesToRun.Contains(item))
            {
                BehaviourTreesToRun.Add(item);
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
    }
}
