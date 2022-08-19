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

    public class Node
    {
        protected NodeState state;
        protected string TypeNode = "Node";
        public Node parent;
        protected Root root;
        protected List<Node> childrens = new List<Node>();
        protected List<Service> services = new List<Service>();
        protected List<Decorator> decorators = new List<Decorator>();
        protected bool executeEnable = true;
        protected ETypeObserverAbort lastAbortType;
        public Node()
        {
            parent = null;
            TypeNode = "Node";

            InitActionsEvents();
        }

        public Node(List<Node> _childrens)
        {
            foreach (Node child in _childrens)
                _Attach(child);
            TypeNode = "Node";

            InitActionsEvents();
        }

        ~Node() 
        {
            DeinitActionsEvents();
        }

        private void InitActionsEvents() 
        {
            Decorator.OnAbortBoth += OnAbortBoth;
            Decorator.OnAbortLowerPriority += OnAbortLowPriority;
            Decorator.OnAbortSelf += OnAbortSelf;
            Decorator.OnAbortNone += OnAbortNone;
        }

        private void DeinitActionsEvents() 
        {
            Decorator.OnAbortBoth -= OnAbortBoth;
            Decorator.OnAbortLowerPriority -= OnAbortLowPriority;
            Decorator.OnAbortSelf -= OnAbortSelf;
            Decorator.OnAbortNone -= OnAbortNone;
        }
        public void SetRoot(Root newRoot) => root = newRoot;

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
            if (node != null)
            {
                node.parent = this;
                childrens.Add(node);
            }
        }

        public bool CheckDecorators()
        {
            for (int i = 0; i < decorators.Count; i++)
            {
                if (!decorators[i].CheckDecorator())
                    return false;
            }
            lastAbortType = ETypeObserverAbort.None;
            executeEnable = true;

            if(decorators.Count > 0) 
            {
                SetEnableExecutionServicesMeAndChilds(this, true);
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
            return CheckReturnNodeState();
        }

        public NodeState Evaluate()
        {
            if (CheckDecorators() || (executeEnable && lastAbortType == ETypeObserverAbort.LowerPriority))
            {
                NodeState nodeState = ExecuteNode();
                if (!CheckDecorators()) 
                {
                    executeEnable = false;
                }
                return nodeState;
            }
            return CheckReturnNodeState();
        }
        protected NodeState CheckReturnNodeState()
        {
            if (parent != null && parent.GetTypeNode() == "Selector")
            {
                return NodeState.FAILURE;
            }
            else
            {
                return NodeState.SUCCESSE;
            }
        }
        public string GetTypeNode() { return TypeNode; }

        public void AddDecorator(Decorator decorator)
        {
            decorator.SetNodeDecorator(this);
            decorators.Add(decorator);
        }

        public void RemoveDecorator(Decorator decorator)
        {
            decorator.SetNodeDecorator(null);
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

        public void SetEnableExecutionServicesMeAndChilds(Node node, bool value) 
        {
            node.SetEnableExecutionServices(value);
            for (int i = 0; i < node.childrens.Count; i++)
            {
                SetEnableExecutionServicesMeAndChilds(node.childrens[i], value);
            }
        }

        public void SetEnableExecutionServices(bool value) 
        {
            for(int i = 0; i < services.Count; i++) 
            {
                services[i].SetEnableExecute(value);
            }
        }

        public bool GetEnableExecutionServices() 
        {
            for (int i = 0; i < services.Count; i++)
            {
                if (!services[i].GetEnableExecute()) 
                {
                    return false;
                }
            }
            return true;
        }

        public void SetEnableLastExecutionServices(bool value) 
        {
            for(int i = 0; i < services.Count; i++) 
            {
                services[i].SetEnableLastExecution(value);
            }
        }

        public void ShowCountServices()
        {
            Debug.Log("Services Count: " + services.Count);
        }

        public void SetState(NodeState nodeState)
        {
            state = nodeState;
        }

        public void SetEnableExecutionNodeMeAndChilds(Node FromNode, bool value)
        {
            FromNode.SetExecuteEnable(value);
            for (int i = 0; i < FromNode.GetChildrens().Count; i++)
            {
                SetEnableExecutionNodeMeAndChilds(FromNode.GetChildrens()[i], value);
            }
        }

        public void SetExecuteEnable(bool enable) => executeEnable = enable;

        public bool GetExecuteEnable() { return executeEnable; }

        public List<Node> GetChildrens() { return childrens; }

        private void OnAbortBoth(Decorator decorator)
        {
            if (CheckContainsDecorator(decorator) && root != null)
            {
                lastAbortType = ETypeObserverAbort.Both;
                root.AbortBothNode(this);
            }
        }
        private void OnAbortLowPriority(Decorator decorator) 
        {
            if (CheckContainsDecorator(decorator) && root != null)
            {
                lastAbortType = ETypeObserverAbort.LowerPriority;
                root.AbortLowPriorityNode(this);
            }
        }

        private void OnAbortSelf(Decorator decorator) 
        {
            if (CheckContainsDecorator(decorator) && root != null)
            {
                lastAbortType = ETypeObserverAbort.Self;
                root.AbortSelfNode(this);
            }
        }

        private void OnAbortNone(Decorator decorator) 
        {
            if (CheckContainsDecorator(decorator) && root != null) 
            {
                root.AbortNoneNode(this);
            }
        }

        private bool CheckContainsDecorator(Decorator decorator) 
        {
            for(int i = 0; i < decorators.Count; i++) 
            {
                if(decorators[i] == decorator) 
                {
                    return true;
                }
            }

            return false;
        }
    }
}
