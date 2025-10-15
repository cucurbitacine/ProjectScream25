using UnityEngine;

namespace Game.Scripts.Core
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Object Type", fileName = "ObjectType", order = 0)]
    public class ObjectType : ScriptableObject
    {
        [SerializeField] private string description;
    }
}