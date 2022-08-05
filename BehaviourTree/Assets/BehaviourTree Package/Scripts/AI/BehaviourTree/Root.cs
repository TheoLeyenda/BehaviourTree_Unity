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

        }

        public void InitNodesToRoot(Node node, int index)
        {
            AddNodeToRoot(node, index);
            //node.ShowChildrens();
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
    }
}
