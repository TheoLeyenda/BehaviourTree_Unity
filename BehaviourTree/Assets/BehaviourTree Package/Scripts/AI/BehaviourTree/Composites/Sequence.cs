using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Composite
    {
        public Sequence() : base()
        {
            TypeNode = "Sequence";
        }
        public Sequence(List<Node> childrens) : base(childrens)
        {
            TypeNode = "Sequence";
        }

        public override NodeState Evaluate()
        {
            base.Evaluate();
            bool anyChildIsRunning = false;
            foreach (Node node in childrens)
            {
                switch (node.Evaluate())
                {
                    case NodeState.FAILURE:
                        state = NodeState.FAILURE;
                        return state;
                    case NodeState.SUCCESSE:
                        continue;
                    case NodeState.RUNNING:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        state = NodeState.SUCCESSE;
                        return state;
                }
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESSE;
            return state;

        }
    }
}