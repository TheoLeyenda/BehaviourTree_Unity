using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Root : Node
    {
        public class TreeNode
        {
            public TreeNode(Node node, int indexNode)
            {
                _node = node;
                _indexNode = indexNode;
            }
            public Node _node;
            public int _indexNode;
        }
        protected Composite _compositeRoot;

        private List<TreeNode> _allNodes = new List<TreeNode>();
        private int _currentIndexExecute = 0;

        public Root(Composite compositeRoot)
        {
            _compositeRoot = compositeRoot;

            InitNodesToRoot(_compositeRoot, _currentIndexExecute);

            _currentIndexExecute = 0;

            TypeNode = "Root";

            for(int i = 0; i < _allNodes.Count; i++) 
            {
                _allNodes[i]._node.SetRoot(this);
            }
        }

        public void InitNodesToRoot(Node node, int index)
        {
            AddNodeToRoot(node, index);
            if (node.GetChildrens().Count > 0)
            {
                foreach (Node child in node.GetChildrens())
                {
                    _currentIndexExecute++;
                    InitNodesToRoot(child, _currentIndexExecute);
                }
            }
        }

        public void ShowNodesRoot()
        {
            for (int i = 0; i < _allNodes.Count; i++)
            {
                Debug.Log("Type Node: " + _allNodes[i]._node.GetTypeNode() + ", Index: " + _allNodes[i]._indexNode);
            }
        }

        protected override NodeState ExecuteNode()
        {
            return _compositeRoot.Evaluate();
        }

        public Composite GetComposite() { return _compositeRoot; }

        private void AddNodeToRoot(Node node, int index)
        {
            if (node == null)
                return;
            _allNodes.Add(new TreeNode(node, index));
        }

        private void DisableExecuteServicesNode(Node FromNode) 
        {
            FromNode.SetEnableExecutionServices(false);
            for (int i = 0; i < FromNode.GetChildrens().Count; i++)
            {
                FromNode.GetChildrens()[i].SetEnableExecutionServices(false);
            }
        }

        public void AbortNoneNode(Node FromNode)
        {
            int index = GetIndexNode(FromNode);
            if (index != -1)
            {
                DisableExecuteServicesNode(FromNode);
            }
        }

        public void AbortSelfNode(Node FromNode) 
        {
            int index = GetIndexNode(FromNode);
            if (index != -1)
            {
                FromNode.SetExecuteEnable(false);
                DisableExecuteServicesNode(FromNode);
            }
        }

        public void AbortLowPriorityNode(Node FromNode) 
        {
            int index = GetIndexNode(FromNode);
            if (index != -1)
            {
                DisableExecuteNodes(index + 1, true);
            }
        }

        public void AbortBothNode(Node FromNode) 
        {
            int index = GetIndexNode(FromNode);
            if (index != -1)
            {
                DisableExecuteNodes(index, false);
            }
        }

        private void DisableExecuteNodes(int fromNodeIndex, bool lowPriorityAbort) 
        {
            if (fromNodeIndex < _allNodes.Count)
            {
                for (int i = fromNodeIndex; i < _allNodes.Count; i++)
                {
                    bool lastEnableExecutionServices = _allNodes[i]._node.GetEnableExecutionServices();

                    if (lastEnableExecutionServices && lowPriorityAbort) 
                    {
                        _allNodes[i]._node.SetEnableLastExecutionServices(true);
                    }
                    _allNodes[i]._node.SetExecuteEnable(false);
                    _allNodes[i]._node.SetEnableExecutionServices(false);
                }
            }
        }

        private int GetIndexNode(Node node) 
        {
            for(int i = 0; i < _allNodes.Count; i++) 
            {
                if(_allNodes[i]._node == node) 
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
