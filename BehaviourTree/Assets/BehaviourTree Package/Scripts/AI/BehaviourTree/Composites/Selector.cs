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

        public override NodeState Evaluate()
        {
            base.Evaluate();
            foreach (Node node in childrens)
            {
                switch (node.Evaluate())
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
