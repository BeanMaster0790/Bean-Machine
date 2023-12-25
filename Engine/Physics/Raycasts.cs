using BeanMachine.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace BeanMachine.PhysicsSystem
{
    public static class Raycasts
    {
        public static Collider OverlapBox(Vector2 position, int size)
        {
            Sprite tempSprite = new Sprite(rectWidth: size, rectHeight: size) {Name = "TempCollider" };
            tempSprite.AddAddon(new Collider(tempSprite, isRaycast: true) { Height = size, Width = size });
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

        public static Collider[] OverlapBoxAll(Vector2 position, int size)
        {
            Sprite tempSprite = new Sprite(rectWidth: size, rectHeight: size);
            tempSprite.AddAddon(new Collider(tempSprite, true) { Height = size, Width = size });
            tempSprite.Position = position;

            List<Collider> result = new List<Collider>();

            foreach (Collider collider in Physics.GetGameColliders())
            {
                if (collider.CheckCollision(tempSprite.GetAddon<Collider>()))
                {
                    result.Add(collider);
                }
            }

            return result.ToArray();
        }

        public static Collider ShootRay(Vector2 position, Vector2 direction, int range)
        {
            
            Vector2 currentPosition = position;

            for(int i = 0; i < range; i++)
            {
                Collider collider =  Raycasts.OverlapBox(currentPosition, 1);

                if(collider != null)
                {
                    return collider;
                }

                currentPosition += direction; 
            }

            return null;
        }

        public static Collider[] ShootRayAll(Vector2 position, Vector2 direction, int range)
        {
            Vector2 currentPosition = position;

            List<Collider> result = new List<Collider>();

            for (int i = 0; i < range; i++)
            {
                Collider collider = Raycasts.OverlapBox(currentPosition, 1);

                if (collider != null && !result.Contains(collider))
                {
                    result.Add(collider);
                }

                currentPosition += direction;
            }

            return result.ToArray();
        }
    }
}
