using Shells;
using UnityEngine;

namespace Units
{
    public class Tank : Enemy
    {
        // TODO : If the tank class will have own animations and features, the following methods should be implemented.
        
        protected override void OnTriggerEnter2D(Collider2D other)
        {
            throw new System.NotImplementedException();
        }

        protected override void AddWaypointToMovementPath(Waypoint waypoint)
        {
            throw new System.NotImplementedException();
        }

        protected override void GetDamaged(ShellBase shell)
        {
            throw new System.NotImplementedException();
        }

        protected override void Die(bool isKilled = false, float delay = 0)
        {
            throw new System.NotImplementedException();
        }

        protected override void Move()
        {
            throw new System.NotImplementedException();
        }
    }
}