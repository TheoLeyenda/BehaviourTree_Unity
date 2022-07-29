using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackboard : MonoBehaviour
{
    [System.Serializable]
    public struct BlackboardKey
    {
        string KeyName;
        object Value;
    }

    [SerializeField]
    private Blackboard Keys;

    private Dictionary<string, BlackboardKey> BlackboardKeys = new Dictionary<string, BlackboardKey>();

    public Dictionary<string, BlackboardKey> GetBlackboardKeys() 
    {
        return BlackboardKeys;
    }

}
