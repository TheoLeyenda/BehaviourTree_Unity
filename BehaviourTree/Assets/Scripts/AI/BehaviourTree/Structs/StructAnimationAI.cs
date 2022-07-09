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
    private List<string> registerKeys;
    private Animator animator;
    private Dictionary<string, AnimationDataAI> registerAnimationsDataAI = new Dictionary<string, AnimationDataAI>();

    public void Start()
    {
        registerKeys = new List<string>();
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
        if (!animator) return;

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
        if (!animator) return;
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
        if (!animator) return;
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

    public void SetDataAnimationSlot(string animationName)
    {
        if (!registerAnimationsDataAI.ContainsKey(animationName) || !animator) return;

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
}
