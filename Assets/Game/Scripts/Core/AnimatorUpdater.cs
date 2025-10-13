using System;
using Game.Scripts.Control;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class AnimatorUpdater : MonoBehaviour
    {
        private static readonly int Run = Animator.StringToHash("Run");
        
        private Animator animator;
        private KinematicBody kinematic;

        private void UpdateAnimation()
        {
            animator.SetBool(Run, kinematic.VelocityDesire.sqrMagnitude > 0f);
        }
        
        private void Awake()
        {
            animator = GetComponentInChildren<Animator>();
            kinematic = GetComponent<KinematicBody>();
        }

        private void Update()
        {
            UpdateAnimation();
        }
    }
}