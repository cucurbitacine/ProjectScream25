using System;
using Game.Scripts.Combat;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Game.Scripts.Core
{
    public class DeadState : MonoBehaviour
    {
        [SerializeField] private HealthController health;
        [Space] [SerializeField] private Animator animator;
        [SerializeField] private Collider2D[] colliders;
        [SerializeField] private ShadowCaster2D[] shadows;
        [SerializeField] private SortingGroup[] sortingGroups;

        public void Dead()
        {
            animator.enabled = false;

            foreach (var cld in colliders)
            {
                cld.enabled = false;
            }
            
            foreach (var shadow in shadows)
            {
                shadow.enabled = false;
            }
            
            foreach (var sortingGroup in sortingGroups)
            {
                sortingGroup.sortingOrder--;
            }
        }

        public void Revive()
        {
            animator.enabled = true;

            foreach (var cld in colliders)
            {
                cld.enabled = true;
            }
            
            foreach (var shadow in shadows)
            {
                shadow.enabled = true;
            }
            
            foreach (var sortingGroup in sortingGroups)
            {
                sortingGroup.sortingOrder++;
            }
        }

        private void OnDied(bool died)
        {
            if (died) Dead();
            else Revive();
        }

        private void Awake()
        {
            health = GetComponent<HealthController>();
            animator = GetComponentInChildren<Animator>();
            colliders = GetComponentsInChildren<Collider2D>();
            shadows = GetComponentsInChildren<ShadowCaster2D>();
            sortingGroups = GetComponentsInChildren<SortingGroup>();
        }

        private void OnEnable()
        {
            health.Died += OnDied;
        }

        private void OnDisable()
        {
            health.Died -= OnDied;
        }
    }
}