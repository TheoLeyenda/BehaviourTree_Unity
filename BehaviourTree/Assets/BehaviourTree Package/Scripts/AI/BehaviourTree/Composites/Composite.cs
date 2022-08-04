using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Composite : Node
    {
        public Composite() : base()
        {
            TypeNode = "Composite";
        }
        public Composite(List<Node> childrens) : base(childrens)
        {
            TypeNode = "Composite";
        }
    }
}
