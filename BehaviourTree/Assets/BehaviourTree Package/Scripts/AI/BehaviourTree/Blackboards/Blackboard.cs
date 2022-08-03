using System.Collections.Generic;
using UnityEngine;
using System;
namespace BehaviorTree
{
    public class Blackboard : MonoBehaviour
    {
        public static event Action<Blackboard> OnChangeValue;
        public enum EKeyType
        {
            Null,
            String,
            Int,
            Float,
            Bool,
            Char,
            Vector2,
            Vector2Int,
            Vector3,
            Vector3Int,
            Vector4,
            Quaternion,
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
                if(KeyType == EKeyType.Bool) 
                {
                    isSet = (bool)Value;
                }
                else if (value != null)
                {
                    isSet = true;
                }
                else 
                {
                    isSet = false;
                }
            }

            public bool GetIsSet() { return isSet; }

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

        public List<KeyData> GetBlackboardKeysData() { return BlackboardKeysData; }

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
                SetBlackboardKeyType(KeyName, Value);
                if (OnChangeValue != null)
                {
                    OnChangeValue(this);
                }
            }
        }

        public void ClearValue(string KeyName)
        {
            int index = GetIndexBlackboardKeyData(KeyName);

            if (index != -1 && BlackboardKeys.ContainsKey(KeyName))
            {
                switch (BlackboardKeysData[index].KeyType) 
                {
                    case EKeyType.Bool:
                        SetValue(KeyName, false);
                        break;
                    case EKeyType.Char:
                        SetValue(KeyName, ' ');
                        break;
                    case EKeyType.Float:
                        SetValue(KeyName, 0.0f);
                        break;
                    case EKeyType.Int:
                        SetValue(KeyName, 0);
                        break;
                    case EKeyType.String:
                        SetValue(KeyName, "");
                        break;
                    case EKeyType.Object:
                        SetValue(KeyName, null);
                        break;
                    case EKeyType.Vector2:
                        SetValue(KeyName, Vector2.zero);
                        break;
                    case EKeyType.Vector2Int:
                        SetValue(KeyName, Vector2Int.zero);
                        break;
                    case EKeyType.Vector3:
                        SetValue(KeyName, Vector3.zero);
                        break;
                    case EKeyType.Vector3Int:
                        SetValue(KeyName, Vector3Int.zero);
                        break;
                    case EKeyType.Vector4:
                        SetValue(KeyName, Vector4.zero);
                        break;
                    case EKeyType.Quaternion:
                        SetValue(KeyName, Quaternion.identity);
                        break;
                    case EKeyType.Null:
                        SetValue(KeyName, null);
                        break;
                    default:
                        SetValue(KeyName, null);
                        break;
                }
            }
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

        private void SetBlackboardKeyType(string NameKey,object Value)
        {
            if (BlackboardKeysData == null) return;

            for (int i = 0; i < BlackboardKeysData.Count; i++)
            {
                if (BlackboardKeysData[i].Name == NameKey)
                {
                    UpdateBlackboardKeyType(NameKey, GetKeyType(Value));
                    BlackboardKeysData[i].CheckIsSet(Value);
                    break;
                }
            }
        }
        public int GetIndexBlackboardKeyData(string NameKey)
        {
            int index = -1;
            for (int i = 0; i < BlackboardKeysData.Count; i++)
            {
                if (BlackboardKeysData[i].Name == NameKey)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }

        private void UpdateBlackboardKeyType(string NameKey, EKeyType keyType)
        {
            int index = GetIndexBlackboardKeyData(NameKey);
            if (index != -1)
            {
                BlackboardKeysData.RemoveAt(index);
                BlackboardKeysData.Insert(index, new KeyData(NameKey, keyType));
            }
        }
        
        private void RemoveKeyData(string NameKey)
        {
            int index = GetIndexBlackboardKeyData(NameKey);   
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
                case "UnityEngine.Vector2":
                    return EKeyType.Vector2;
                case "UnityEngine.Vector2Int":
                    return EKeyType.Vector2Int;
                case "UnityEngine.Vector3":
                    return EKeyType.Vector3;
                case "UnityEngine.Vector3Int":
                    return EKeyType.Vector3Int;
                case "UnityEngine.Vector4":
                    return EKeyType.Vector4;
                case "UnityEngine.Quaternion":
                    return EKeyType.Quaternion;
                default:
                    return EKeyType.Object;
            }
        }
    }
}
