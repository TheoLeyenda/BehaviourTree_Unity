using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Root : Node
    {
        protected Composite _compositeRoot;

        public Root(Composite compositeRoot)
        {
            _compositeRoot = compositeRoot;
        }

        public override NodeState Evaluate()
        {
            return _compositeRoot.Evaluate();
        }

        public Composite GetComposite() { return _compositeRoot; }
    }
}
