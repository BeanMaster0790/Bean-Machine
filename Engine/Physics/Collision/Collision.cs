using BeanMachine.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.PhysicsSystem
{
    public class Collision
    {
        public Collider BaseCollider { get; set; }
        public Collider Collider { get; set; }

        public CollisionDirection[] Directions { get; set; }

        public Collision(Collider baseCollider, Collider collider, CollisionDirection[] directions)
        {
            this.BaseCollider = baseCollider;
            this.Collider = collider;
            this.Directions = directions;
        }
    }

    public enum CollisionDirection
    {
        Top,
        Bottom,
        Left,
        Right,
        None
    }
}
