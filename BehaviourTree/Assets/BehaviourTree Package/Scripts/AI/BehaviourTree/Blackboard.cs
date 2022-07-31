using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTree
{
    public class Blackboard : MonoBehaviour
    {
        public enum EKeyType
        {
            Null,
            String,
            Int,
            Float,
            Bool,
            Char,
            Object,
        }

        [System.Serializable]
        public class KeyData
        {
            public string Name;
            public EKeyType KeyType;
            [SerializeField]
            private bool isSet;
            [SerializeField]
            private string ShowValue;

            private object Value;

            public KeyData(string name, EKeyType eKeyType)
            {
                Name = name;
                KeyType = eKeyType;
            }

            public void CheckIsSet(object value)
            {
                Value = value;
                UpdateShowValue();
                if (value != null)
                {
                    isSet = true;
                }
                else
                {
                    isSet = false;
                }
            }

            public void UpdateShowValue()
            {
                if (Value != null)
                {
                    ShowValue = Value.ToString();
                }
            }
        }

        [SerializeField]
        private List<KeyData> BlackboardKeysData = new List<KeyData>();

        private Dictionary<string, object> BlackboardKeys = new Dictionary<string, object>();

        private void Awake()
        {
            if (BlackboardKeysData != null)
            {
                BlackboardKeysData.Clear();
            }
        }

        public void UpdateKeysData()
        {
            for (int i = 0; i < BlackboardKeysData.Count; i++)
            {
                BlackboardKeysData[i].UpdateShowValue();
            }
        }

        public void SetValue(string KeyName, object Value)
        {
            if (BlackboardKeys.ContainsKey(KeyName))
            {
                BlackboardKeys[KeyName] = Value;
            }
            SetBlackboardKeyType(KeyName, Value);
        }

        public object GetValue(string KeyName)
        {
            object value = null;

            BlackboardKeys.TryGetValue(KeyName, out value);

            return value;
        }

        public void AddValue(string KeyName, object Value)
        {
            if (!BlackboardKeys.ContainsKey(KeyName))
            {
                BlackboardKeys.Add(KeyName, Value);
                
                BlackboardKeysData.Add(new KeyData(KeyName, GetKeyType(Value)));
                if (BlackboardKeysData[BlackboardKeysData.Count - 1] != null)
                {
                    BlackboardKeysData[BlackboardKeysData.Count - 1].CheckIsSet(Value);
                }
            }
        }

        public void RemoveValue(string KeyName)
        {
            if (BlackboardKeys.ContainsKey(KeyName))
            {
                BlackboardKeys.Remove(KeyName);
                RemoveKeyData(KeyName);
            }
        }

        public void ClearValues()
        {
            BlackboardKeys.Clear();
            BlackboardKeysData.Clear();
        }

        public Dictionary<string, object> GetBlackboardKeys()
        {
            return BlackboardKeys;
        }

        private void SetBlackboardKeyType(string Name,object Value)
        {
            if (BlackboardKeysData == null) return;

            for (int i = 0; i < BlackboardKeysData.Count; i++)
            {
                if (BlackboardKeysData[i].Name == Name)
                {
                    UpdateBlackboardKeyType(Name, GetKeyType(Value));
                    BlackboardKeysData[i].CheckIsSet(Value);
                    break;
                }
            }
        }

        private void UpdateBlackboardKeyType(string Name, EKeyType keyType)
        {
            int index = -1;
            for (int i = 0; i < BlackboardKeysData.Count; i++)
            {
                if (BlackboardKeysData[i].Name == Name)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                BlackboardKeysData.RemoveAt(index);
                BlackboardKeysData.Insert(index, new KeyData(Name, keyType));
            }
        }

        private void RemoveKeyData(string Name)
        {
            int index = -1;
            for (int i = 0; i < BlackboardKeysData.Count; i++)
            {
                if (BlackboardKeysData[i].Name == Name)
                {
                    index = i;
                    break;
                }
            }
            if (index != -1)
            {
                BlackboardKeysData.RemoveAt(index);
            }
        }

        private EKeyType GetKeyType(object Value)
        {
            if (Value == null)
                return EKeyType.Null;

            switch (Value.GetType().ToString())
            {
                case "System.String":
                    return EKeyType.String;
                case "System.Int32":
                    return EKeyType.Int;
                case "System.Single":
                    return EKeyType.Float;
                case "System.Boolean":
                    return EKeyType.Bool;
                case "System.Char":
                    return EKeyType.Char;
                default:
                    return EKeyType.Object;
            }
        }
    }
}
