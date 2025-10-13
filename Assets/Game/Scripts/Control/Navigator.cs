using UnityEngine;
using UnityEngine.AI;

namespace Game.Scripts.Control
{
    public class Navigator
    {
        public NavMeshPath Path { get; private set; } = new NavMeshPath();
        public int CornerIndex { get; private set; }
        
        public bool CalculatePath(Vector2 origin, Vector2 destination)
        {
            var filter = new NavMeshQueryFilter()
            {
                agentTypeID = 0,
                areaMask = 1,
            };

            CornerIndex = 1;
            
            return NavMesh.CalculatePath(origin, destination, filter, Path);
        }
        
        public bool GetNextPoint(Vector2 origin, out Vector2 nextPoint)
        {
            if (CornerIndex >= Path.corners.Length)
            {
                nextPoint = origin;
                return false;
            }
            
            nextPoint = Path.corners[CornerIndex];

            if (Vector2.Distance(origin, nextPoint) < 0.1f)
            {
                CornerIndex++;

                return CornerIndex < Path.corners.Length && GetNextPoint(origin, out nextPoint);
            }
            
            return true;
        }
    }
}