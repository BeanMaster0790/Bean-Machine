using BeanMachine.Graphics;
using BeanMachine.PhysicsSystem;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.PhysicsSystem
{
    public static class Raycasts
    {
        public static Collider OverlapBox(Vector2 position, int size)
        {
            Sprite tempSprite = new Sprite(rectWidth: size, rectHeight: size);
            tempSprite.AddAddon(new Collider(tempSprite, true) { Height = size, Width = size });
            tempSprite.Position = position;

            foreach (Collider collider in Physics.GetGameColliders())
            {
                if (collider.CheckCollision(tempSprite.GetAddon<Collider>()))
                {
                    return collider;
                }
            }

            return null;
        }
    }
}
