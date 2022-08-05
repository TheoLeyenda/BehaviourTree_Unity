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

            if (_abortType == EAbortType.AbortSelf)
            {
                return CheckReturnNodeState();
            }

            for (int i = 0; i < childrens.Count; i++) 
            {
                /*if (_abortType == EAbortType.AbortBoth)
                {
                    i = 0;
                    _abortType = EAbortType.None;
                }*/

                /*else if (_abortType == EAbortType.AbortLowerPriorirty)
                {
                    state = childrens[i].Evaluate();
                    i = childrens.Count;
                    _abortType = EAbortType.None;
                    return state;
                }*/
                if (!childrens[i].CheckDecorators() || childrens[i].GetAbortType() == EAbortType.AbortSelf)
                {
                    childrens[i].SetAbortType(EAbortType.None);
                    childrens[i].SetState(NodeState.SUCCESSE);
                    continue;
                }
                else
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
            }

            state = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESSE;
            return state;

        }
    }
}