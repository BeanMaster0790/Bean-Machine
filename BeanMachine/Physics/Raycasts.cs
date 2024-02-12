using BeanMachine.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace BeanMachine.PhysicsSystem
{
    public class Raycasts
    {
        public static Raycasts Instance = new Raycasts();

        public Collider OverlapBox(Vector2 position, int size)
        {
            Sprite tempSprite = new Sprite(rectWidth: size, rectHeight: size) {Name = "TempCollider" };
            tempSprite.AddAddon(new Collider(isRaycast: true) { Height = size, Width = size });
            tempSprite.Position = position;

            foreach (Collider collider in Physics.Instance.GetGameColliders())
            {
                if (collider.CheckCollision(tempSprite.GetAddon<Collider>()))
                {
                    return collider;
                }
            }

            return null;
        }

        public Collider[] OverlapBoxAll(Vector2 position, int size)
        {
            Sprite tempSprite = new Sprite(rectWidth: size, rectHeight: size);
            tempSprite.AddAddon(new Collider(isRaycast: true) { Height = size, Width = size });
            tempSprite.Position = position;

            List<Collider> result = new List<Collider>();

            foreach (Collider collider in Physics.Instance.GetGameColliders())
            {
                if (collider.CheckCollision(tempSprite.GetAddon<Collider>()))
                {
                    result.Add(collider);
                }
            }

            return result.ToArray();
        }

        public Collider ShootRay(Vector2 position, Vector2 direction, int range)
        {
            
            Vector2 currentPosition = position;

            for(int i = 0; i < range; i++)
            {
                Collider collider =  Raycasts.Instance.OverlapBox(currentPosition, 1);

                if(collider != null)
                {
                    return collider;
                }

                currentPosition += direction; 
            }

            return null;
        }

        public Collider[] ShootRayAll(Vector2 position, Vector2 direction, int range)
        {
            Vector2 currentPosition = position;

            List<Collider> result = new List<Collider>();

            for (int i = 0; i < range; i++)
            {
                Collider collider = Raycasts.Instance.OverlapBox(currentPosition, 1);

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
