using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;

namespace BeanMachine.PhysicsSystem
{
    public class Physics
    {
        private List<Collider> _gameColliders = new List<Collider>();

        public static Physics Instance = new Physics();

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

        public void Update()
        {
            while (true)
            {
                Collider[] colliders = GetGameColliders();

                foreach (Collider currentCollider in colliders)
                {
                    foreach (Collider collider in colliders)
                    {
                        currentCollider.CheckCollision(collider);
                    }
                }

                Thread.Sleep(17);
            }

        }
    }
}
