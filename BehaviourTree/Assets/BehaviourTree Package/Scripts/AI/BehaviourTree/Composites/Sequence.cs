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

        protected override NodeState ExecuteNode()
        {
            base.ExecuteNode();

            bool anyChildIsRunning = false;

            for (int i = 0; i < childrens.Count; i++)
            {
                if (!executeEnable)
                {
                    executeEnable = true;
                    for (int j = 0; j < childrens.Count; j++)
                    {
                        childrens[j].SetExecuteEnable(true);
                    }
                    return CheckReturnNodeState();
                }

                if (childrens[i].GetExecuteEnable())
                {
                    switch (childrens[i].Evaluate())
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
                else
                    childrens[i].SetExecuteEnable(true);
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESSE;
            return state;

        }
    }
}