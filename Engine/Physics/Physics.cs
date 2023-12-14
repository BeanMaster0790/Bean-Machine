using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.PhysicsSystem
{
    public static class Physics
    {
        private static List<Collider> _gameColliders = new List<Collider>();
        
        public static void AddGameCollider(Collider collider)
        {
            _gameColliders.Add(collider);
        }

        public static void RemoveGameCollider(Collider collider)
        {
            _gameColliders.Remove(collider);
        }

        public static Collider[] GetGameColliders()
        {
            return _gameColliders.ToArray();
        }

        public static void DrawColliders(SpriteBatch spriteBatch)
        {
            foreach (Collider collider in GetGameColliders())
            {
                collider.DrawCollider(spriteBatch);
            }
        }

        /// <summary>
        /// Should be called AFTER main update loop
        /// </summary>
        public static void Update()
        {
            foreach (Collider currentCollider in GetGameColliders())
            {
                foreach (Collider collider in GetGameColliders())
                {
                    currentCollider.CheckCollision(collider);
                }
            }
        }
    }
}
