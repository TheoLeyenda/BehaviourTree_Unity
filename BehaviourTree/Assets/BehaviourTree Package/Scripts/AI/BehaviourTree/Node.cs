using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public enum NodeState
    {
        RUNNING,
        SUCCESSE,
        FAILURE
    }
    public class Node
    {
        protected NodeState state;
        protected string TypeNode = "Node";
        public Node parent;
        protected List<Node> childrens = new List<Node>();
        protected List<Service> services = new List<Service>();

        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();

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

        public virtual NodeState Evaluate()
        {
            for (int i = 0; i < services.Count; i++)
            {
                if (services[i] != null)
                {
                    Debug.Log(TypeNode);
                    services[i].OnBecomeRelevant();
                }
            }
            return NodeState.FAILURE;
        }

        public string GetTypeNode() { return TypeNode; }

        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (_dataContext.TryGetValue(key, out value))
                return value;

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                    return value;

                node = node.parent;
            }
            return null;
        }

        public bool ClearData(string key)
        {
            if (_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }

            Node node = parent;

            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                    return true;
                node = node.parent;
            }
            return false;
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
            Debug.Log("Services Count: "+services.Count);
        }
    }
}
