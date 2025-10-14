using System;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class AudioListenerFollower : MonoBehaviour
    {
        [SerializeField] private Transform follow;

        private void Update()
        {
            transform.position = follow.position;
        }
    }
}
