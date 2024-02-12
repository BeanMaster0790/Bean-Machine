using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace BeanMachine.PhysicsSystem
{
    public class Physics
    {
        public static Physics Instance = new Physics();

        private List<Collider> _gameColliders = new List<Collider>();
        
        public void AddGameCollider(Collider collider)
        {
            _gameColliders.Add(collider);
        }

        public void DrawColliders(SpriteBatch spriteBatch)
        {
            foreach (Collider collider in GetGameColliders())
            {
                collider.DrawCollider(spriteBatch);
            }
        }

        public Collider[] GetGameColliders()
        {
            return _gameColliders.ToArray();
        }

        public void RemoveGameCollider(Collider collider)
        {
            _gameColliders.Remove(collider);
        }

        /// <summary>
        /// Should be called AFTER main update loop
        /// </summary>
        public void Update()
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
