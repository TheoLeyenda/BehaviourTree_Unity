using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Selector : Composite
    {
        public Selector() : base()
        {
            TypeNode = "Selector";
        }
        public Selector(List<Node> childrens) : base(childrens)
        {
            TypeNode = "Selector";
        }

        protected override NodeState ExecuteNode()
        {
            base.ExecuteNode();

            for (int i = 0; i < childrens.Count; i++)
            {
                switch (childrens[i].Evaluate())
                {
                    case NodeState.FAILURE:
                        continue;
                    case NodeState.SUCCESSE:
                        state = NodeState.SUCCESSE;
                        return state;
                    case NodeState.RUNNING:
                        state = NodeState.RUNNING;
                        return state;
                    default:
                        continue;
                }
                
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}
