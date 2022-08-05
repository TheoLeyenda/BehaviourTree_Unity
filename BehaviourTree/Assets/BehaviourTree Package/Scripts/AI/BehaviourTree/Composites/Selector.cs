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

            if (_abortType == EAbortType.AbortSelf)
            {
                Debug.Log("Entre al AbortSelf");
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
                    childrens[i].SetState(NodeState.FAILURE);
                    continue;
                }
                else
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
                
            }
            state = NodeState.FAILURE;
            return state;
        }
    }
}
