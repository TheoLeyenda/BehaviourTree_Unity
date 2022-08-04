using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESSE,
        FAILURE,
    }
    public enum EAbortType
    {
        None,
        AbortSelf,
        AbortBoth,
        AbortLowerPriorirty,
    }

    public class Node
    {
        protected NodeState state;
        protected string TypeNode = "Node";
        public Node parent;
        protected List<Node> childrens = new List<Node>();
        protected List<Service> services = new List<Service>();
        protected List<Decorator> decorators = new List<Decorator>();
        protected EAbortType _abortType = EAbortType.None;
        public Node()
        {
            parent = null;
            TypeNode = "Node";
        }

        public Node(List<Node> _childrens)
        {
            foreach (Node child in _childrens)
                _Attach(child);
            TypeNode = "Node";
        }

        public void ShowChildrens()
        {
            if (childrens.Count > 0)
            {
                Debug.Log(TypeNode + " contiene: ");

                foreach (Node child in childrens)
                {
                    Debug.Log(child.TypeNode);
                }

                foreach (Node child in childrens)
                {
                    child.ShowChildrens();
                }
            }
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            childrens.Add(node);
        }

        public bool CheckDecorators() 
        {
            for(int i = 0; i < decorators.Count; i++) 
            {
                if (!decorators[i].CheckDecorator())
                    return false;
            }
            return true;
        }

        protected virtual NodeState ExecuteNode() 
        {
            for (int i = 0; i < services.Count; i++)
            {
                if (services[i] != null)
                {
                    services[i].OnBecomeRelevant();
                }
            }
            if (GetTypeNode() == "Selector")
            {
                return NodeState.FAILURE;
            }
            else 
            {
                return NodeState.SUCCESSE;
            }
        }

        public virtual NodeState Evaluate()
        {
            if (CheckDecorators())
            {
                return ExecuteNode();
            }
            return NodeState.FAILURE;
        }

        public string GetTypeNode() { return TypeNode; }

        public void AddDecorator(Decorator decorator) 
        {
            decorators.Add(decorator);
        }

        public void RemoveDecorator(Decorator decorator) 
        {
            decorators.Remove(decorator);
            UnityEngine.GameObject.Destroy(decorator);
        }

        public void ClearAllDecorators() 
        {
            List<Decorator> auxList = new List<Decorator>();
            for (int i = 0; i < decorators.Count; i++)
            {
                auxList.Add(decorators[i]);
            }
            for (int i = 0; i < auxList.Count; i++)
            {
                RemoveDecorator(auxList[i]);
            }
            auxList.Clear();
            decorators.Clear();
        }

        public void AddService(Service NewService)
        {
            services.Add(NewService);
        }

        public void RemoveService(Service ServiceToRemove)
        {
            services.Remove(ServiceToRemove);
            UnityEngine.GameObject.Destroy(ServiceToRemove);
        }

        public void ClearAllServices()
        {
            List<Service> auxList = new List<Service>();
            for (int i = 0; i < services.Count; i++)
            {
                auxList.Add(services[i]);
            }
            for (int i = 0; i < auxList.Count; i++)
            {
                RemoveService(auxList[i]);
            }
            auxList.Clear();
            services.Clear();
        }

        public void ShowCountServices()
        {
            Debug.Log("Services Count: " + services.Count);
        }

        public void SetState(NodeState nodeState) 
        {
            state = nodeState;
        }

        public void SetAbortType(EAbortType abortType)
        {
            _abortType = abortType;
        }

        public EAbortType GetAbortType() 
        {
            return _abortType;
        }
    }
}
