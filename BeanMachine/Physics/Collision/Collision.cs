using Microsoft.Xna.Framework;

namespace BeanMachine.PhysicsSystem
{
    public class Collision
    {
        public Collider BaseCollider { get; set; }

        public Collider Collider { get; set; }

        public Vector2 Direction { get; set; }

        public Collision(Collider baseCollider, Collider collider, Vector2 direction)
        {
            this.BaseCollider = baseCollider;
            this.Collider = collider;

            this.Direction = direction;
        }
    }
}
