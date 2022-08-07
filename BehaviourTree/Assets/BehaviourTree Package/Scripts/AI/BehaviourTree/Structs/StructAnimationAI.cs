using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructAnimationAI : MonoBehaviour
{
    public enum TypeTransitionAnimationUse
    {
        FloatType,
        IntegerType,
        BooleanType,
        TriggerType,
    }
    [System.Serializable]
    public class AnimationDataAI
    {
        [Header("Animation name:")]
        public string AnimationName;

        [Header("Type of transition variable to use:")]
        public TypeTransitionAnimationUse typeTransitionAnimationUse;

        public float FloatValue;
        public int IntegerValue;
        public bool BooleanValue;

        [HideInInspector]
        public float DefaultFloatValue;
        [HideInInspector]
        public int DefaultIntegerValue;
        [HideInInspector]
        public bool DefaultBooleanValue;
    }

    [SerializeField]
    private AnimationDataAI[] AnimationsDataAI;
    private List<string> registerKeys = new List<string>();
    private Animator animator;
    private Dictionary<string, AnimationDataAI> registerAnimationsDataAI = new Dictionary<string, AnimationDataAI>();

    public void Start()
    {
        InitRegisterAnimationData();
    }

    private void InitRegisterAnimationData()
    {
        registerAnimationsDataAI.Clear();
        registerKeys.Clear();
        foreach (AnimationDataAI currentAnimationData in AnimationsDataAI)
        {
            if (!registerAnimationsDataAI.ContainsKey(currentAnimationData.AnimationName))
            {
                registerAnimationsDataAI.Add(currentAnimationData.AnimationName, currentAnimationData);
                registerKeys.Add(currentAnimationData.AnimationName);
            }
            else
            {
                Debug.LogError("Error: Duplicate data in \"AnimationsDataAI\", cannot have two elements with the same AnimationName.");
            }
        }
    }

    public void SetAnimator(Animator newAnimator) { animator = newAnimator; }

    public void ClearValuesAnimationSlots()
    {
        if (!animator)
        {
            Debug.LogError("Error: animator null in ClearValuesAnimationSlots() function.");
            return;
        }

        for (int i = 0; i < registerKeys.Count; i++)
        {
            if (registerAnimationsDataAI.ContainsKey(registerKeys[i]))
            {

                switch (registerAnimationsDataAI[registerKeys[i]].typeTransitionAnimationUse)
                {
                    case TypeTransitionAnimationUse.BooleanType:
                        animator.SetBool(registerAnimationsDataAI[registerKeys[i]].AnimationName, false);
                        break;
                    case TypeTransitionAnimationUse.FloatType:
                        animator.SetFloat(registerAnimationsDataAI[registerKeys[i]].AnimationName, 0);
                        break;
                    case TypeTransitionAnimationUse.IntegerType:
                        animator.SetInteger(registerAnimationsDataAI[registerKeys[i]].AnimationName, 0);
                        break;
                    case TypeTransitionAnimationUse.TriggerType:
                        break;
                }
            }
        }
    }

    public void SaveDefaultValues()
    {
        if (!animator)
        {
            Debug.LogError("Error: animator null in SaveDefaultValues() function.");
            return;
        }

        for (int i = 0; i < registerKeys.Count; i++)
        {
            if (registerAnimationsDataAI.ContainsKey(registerKeys[i]))
            {

                switch (registerAnimationsDataAI[registerKeys[i]].typeTransitionAnimationUse)
                {
                    case TypeTransitionAnimationUse.BooleanType:
                        registerAnimationsDataAI[registerKeys[i]].DefaultBooleanValue = animator.GetBool(registerAnimationsDataAI[registerKeys[i]].AnimationName);
                        break;
                    case TypeTransitionAnimationUse.FloatType:
                        registerAnimationsDataAI[registerKeys[i]].DefaultFloatValue = animator.GetFloat(registerAnimationsDataAI[registerKeys[i]].AnimationName);
                        break;
                    case TypeTransitionAnimationUse.IntegerType:
                        registerAnimationsDataAI[registerKeys[i]].DefaultIntegerValue = animator.GetInteger(registerAnimationsDataAI[registerKeys[i]].AnimationName);
                        break;
                    case TypeTransitionAnimationUse.TriggerType:
                        break;
                }
            }
        }
    }

    public void ResetValuesAnimationSlots()
    {
        if (!animator)
        {
            Debug.LogError("Error: animator null in ResetValuesAnimationSlots() function.");
            return;
        }

        for (int i = 0; i < registerKeys.Count; i++)
        {
            if (registerAnimationsDataAI.ContainsKey(registerKeys[i]))
            {

                switch (registerAnimationsDataAI[registerKeys[i]].typeTransitionAnimationUse)
                {
                    case TypeTransitionAnimationUse.BooleanType:
                        animator.SetBool(registerAnimationsDataAI[registerKeys[i]].AnimationName,registerAnimationsDataAI[registerKeys[i]].DefaultBooleanValue);
                        break;
                    case TypeTransitionAnimationUse.FloatType:
                        animator.SetFloat(registerAnimationsDataAI[registerKeys[i]].AnimationName, registerAnimationsDataAI[registerKeys[i]].DefaultFloatValue);
                        break;
                    case TypeTransitionAnimationUse.IntegerType:
                        animator.SetInteger(registerAnimationsDataAI[registerKeys[i]].AnimationName, registerAnimationsDataAI[registerKeys[i]].DefaultIntegerValue);
                        break;
                    case TypeTransitionAnimationUse.TriggerType:
                        break;
                }
            }
        }
    }

    public bool GeDataAnimationSlotBoolean(string animationName)
    {
        if (registerAnimationsDataAI.ContainsKey(animationName))
        {
            return animator.GetBool(animationName);
        }
        return false;
    }

    public int GetDataAnimationSlotInteger(string animationName)
    {
        if (registerAnimationsDataAI.ContainsKey(animationName))
        {
            return animator.GetInteger(animationName);
        }
        return -1;
    }

    public float GetDataAnimationSlotFloat(string animationName)
    {
        if (registerAnimationsDataAI.ContainsKey(animationName))
        {
            return animator.GetFloat(animationName);
        }
        return -1.0f;
    }

    public void SetDataAnimationSlot(string animationName)
    {
        if (!registerAnimationsDataAI.ContainsKey(animationName))
        {
            Debug.LogError("Error: Invalid Key in SetDataAnimationSlot() function.");
            return;
        }

        if (!animator)
        {
            Debug.LogError("Error: animator null in SetDataAnimationSlot() function.");
            return;
        }

        switch (registerAnimationsDataAI[animationName].typeTransitionAnimationUse)
        {
            case TypeTransitionAnimationUse.BooleanType:
                animator.SetBool(registerAnimationsDataAI[animationName].AnimationName, registerAnimationsDataAI[animationName].BooleanValue);
                break;
            case TypeTransitionAnimationUse.FloatType:
                animator.SetFloat(registerAnimationsDataAI[animationName].AnimationName, registerAnimationsDataAI[animationName].FloatValue);
                break;
            case TypeTransitionAnimationUse.IntegerType:
                animator.SetInteger(registerAnimationsDataAI[animationName].AnimationName, registerAnimationsDataAI[animationName].IntegerValue);
                break;
            case TypeTransitionAnimationUse.TriggerType:
                animator.SetTrigger(registerAnimationsDataAI[animationName].AnimationName);
                break;
        }
    }

    public void SetIntegerValueAnimationSlot(string animationName, int value)
    {
        if (!registerAnimationsDataAI.ContainsKey(animationName))
        {
            Debug.LogError("Error: Invalid Key in SetIntegerValueAnimationSlot() function.");
            return;
        }

        if (registerAnimationsDataAI[animationName].typeTransitionAnimationUse == TypeTransitionAnimationUse.IntegerType)
        {
            registerAnimationsDataAI[animationName].IntegerValue = value;
        }
        else
        {
            Debug.LogError("Error: Invalid typeTransitionAnimationUse in SetIntegerValueAnimationSlot() function.");
        }
    }

    public void SetFloatValueAnimationSlot(string animationName, float value)
    {
        if (!registerAnimationsDataAI.ContainsKey(animationName))
        {
            Debug.LogError("Error: Invalid Key in SetFloatValueAnimationSlot() function.");
            return;
        }

        if (registerAnimationsDataAI[animationName].typeTransitionAnimationUse == TypeTransitionAnimationUse.FloatType)
        {
            registerAnimationsDataAI[animationName].FloatValue = value;
        }
        else
        {
            Debug.LogError("Error: Invalid typeTransitionAnimationUse in SetFloatValueAnimationSlot() function.");
        }
    }

    public void SetBooleanValueAnimationSlot(string animationName, bool value)
    {
        if (!registerAnimationsDataAI.ContainsKey(animationName))
        {
            Debug.LogError("Error: Invalid Key in SetBooleanValueAnimationSlot() function.");
            return;
        }

        if (registerAnimationsDataAI[animationName].typeTransitionAnimationUse == TypeTransitionAnimationUse.BooleanType)
        {
            registerAnimationsDataAI[animationName].BooleanValue = value;
        }
        else
        {
            Debug.LogError("Error: Invalid typeTransitionAnimationUse in SetBooleanValueAnimationSlot() function.");
        }
    }

    public void PlayAnimation(string NameId)
    {
        if (animator)
        {
            animator.Play(NameId);
        }
    }

    public void SendTriggerValueAnimationSlot(string animationName)
    {
        if (!registerAnimationsDataAI.ContainsKey(animationName))
        {
            Debug.LogError("Error: Invalid Key in SendTriggerValueAnimationSlot() function.");
            return;
        }

        if (!animator)
        {
            Debug.LogError("Error: animator null in SendTriggerValueAnimationSlot() function.");
            return;
        }

        if (registerAnimationsDataAI[animationName].typeTransitionAnimationUse == TypeTransitionAnimationUse.TriggerType)
        {
            animator.SetTrigger(animationName);
        }
    }

    public Animator GetAnimator() 
    {
        return animator;
    }
}
