using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Task : Node
    {
        public Task() : base()
        {
            TypeNode = "Task";
        }
    }
}
