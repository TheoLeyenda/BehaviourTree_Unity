using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
public class BlackboardDecorator : Decorator
{
    protected ETypeNotifyObserver _typeNotifyObserver;
    protected string _blackboardkey;
    protected EKeyQuery _keyQuery;
    protected Blackboard _blackboard;
    protected object _lastValue;

    public enum EKeyQuery
    {
        IsSet,
        IsNotSet,
    }

    public enum ETypeNotifyObserver
    {
        OnValueChange, // cuando se le asigna un valor (no importa si es el mismo)
        OnChangeResult, //cuando el valor cambia.
    }

    protected override void Start()
    {
        base.Start();

        Blackboard.OnChangeValue += CheckOnChangeKeyBlackboard;
    }

    protected virtual void OnDestroy()
    {
        Blackboard.OnChangeValue -= CheckOnChangeKeyBlackboard;
    }

    public void SetTypeNotifyObserver(ETypeNotifyObserver typeNotifyObserver) => _typeNotifyObserver = typeNotifyObserver;

    public void SetTypeObserverAbort(ETypeObserverAbort typeObserverAbort) => _typeObserverAbort = typeObserverAbort;

    public void SetBlackboardKey(string blackboardKey) => _blackboardkey = blackboardKey;

    public void SetKeyQuery(EKeyQuery keyQuery) => _keyQuery = keyQuery;

    public void SetBlackboard(Blackboard blackboard) => _blackboard = blackboard;

    protected void CheckOnChangeKeyBlackboard(Blackboard blackboard)
    {
        if (!_blackboard || _blackboard != blackboard)
            return;
        if (_typeNotifyObserver == ETypeNotifyObserver.OnChangeResult && _blackboard.GetValue(_blackboardkey) != _lastValue
            || _typeNotifyObserver == ETypeNotifyObserver.OnValueChange)
        {
            CheckDecorator();
        }
    }

    public override bool CheckDecorator()
    {
        //FALTA VER LA PARTE EN EL QUE EL DECORATOR EVITA QUE SE EJECUTE UN NODO (YA HACE QUE SE ABORTE)
        switch (_keyQuery)
        {
            case EKeyQuery.IsSet:
                if (_blackboard.GetBlackboardKeysData()[_blackboard.GetIndexBlackboardKeyData(_blackboardkey)].GetIsSet())
                {
                    return true;
                }
                break;
            case EKeyQuery.IsNotSet:
                if (!_blackboard.GetBlackboardKeysData()[_blackboard.GetIndexBlackboardKeyData(_blackboardkey)].GetIsSet())
                {
                    return true;
                }
                break;
        }
        CheckTypeAbort();
        return false;
    }

    public string GetBlackboardKey() { return _blackboardkey; }
}
