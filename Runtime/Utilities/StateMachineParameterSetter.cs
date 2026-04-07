using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Utilities
{
    public class StateMachineParameterSetter : StateMachineBehaviour
    {
        [Serializable]
        public enum StateMachineLifecycle
        {
            Enter = 1,
            Exit = 2
        }

        [Serializable]
        public enum ParameterType
        {
            INT = 1,
            BOOL = 2,
            FLOAT = 3,
            TRIGGER = 4
        }

        [Serializable]
        public struct ParameterSetterConfig
        {
            public string parameterName;
            public StateMachineLifecycle whenToCall;
            public ParameterType parameterType;
            public int intValue;
            public float floatValue;
            public bool boolAndTriggerValue;
        }

        [SerializeField]
        ParameterSetterConfig[] _setterConfigs = null;
        

        private static void UpdateParameter(Animator animator, ParameterSetterConfig config)
        {
            switch (config.parameterType)
            {
                case ParameterType.INT:
                    animator.SetInteger(config.parameterName, config.intValue);
                    break;
                case ParameterType.BOOL:
                    animator.SetBool(config.parameterName, config.boolAndTriggerValue);
                    break;
                case ParameterType.FLOAT:
                    animator.SetFloat(config.parameterName, config.floatValue);
                    break;
                case ParameterType.TRIGGER:
                    if (config.boolAndTriggerValue)
                    {
                        animator.SetTrigger(config.parameterName);
                    }
                    else
                    {
                        animator.ResetTrigger(config.parameterName);
                    }
                    break;
            }
        }


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var config in _setterConfigs)
            {
                if (config.whenToCall == StateMachineLifecycle.Enter)
                {
                    UpdateParameter(animator, config);
                }
            }
        }
        
        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            foreach (var config in _setterConfigs)
            {
                if (config.whenToCall == StateMachineLifecycle.Exit)
                {
                    UpdateParameter(animator, config);
                }
            }
        }
        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    
        //}


        // OnStateMove is called right after Animator.OnAnimatorMove()
        //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that processes and affects root motion
        //}

        // OnStateIK is called right after Animator.OnAnimatorIK()
        //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        //{
        //    // Implement code that sets up animation IK (inverse kinematics)
        //}
    }
}