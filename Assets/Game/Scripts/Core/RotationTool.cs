using System;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class RotationTool : MonoBehaviour
    {
        [SerializeField] private float angularSpeed = 30f;

        private void Update()
        {
            transform.rotation = Quaternion.Euler(0f, 0f, angularSpeed * Time.deltaTime) * transform.rotation;
        }
    }
}
