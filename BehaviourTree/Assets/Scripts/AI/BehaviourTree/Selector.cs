using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> childrens) : base(childrens) { }

        public override NodeState Evaluate()
        {
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
