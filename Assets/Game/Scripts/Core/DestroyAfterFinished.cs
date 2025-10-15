using System;
using UnityEngine;

namespace Game.Scripts.Core
{
    [RequireComponent(typeof(ParticleSystem))]
    public class DestroyAfterFinished : MonoBehaviour
    {
        private ParticleSystem effect;
        
        private void Awake()
        {
            effect = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (effect.isPlaying) return;
            
            Destroy(gameObject);
        }
    }
}
