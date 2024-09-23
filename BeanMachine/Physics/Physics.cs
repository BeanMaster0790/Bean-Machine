using BeanMachine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Threading;

namespace BeanMachine.PhysicsSystem
{
    public class Physics
    {
        private List<Collider> _gameColliders = new List<Collider>();

        public static Physics Instance = new Physics();

        private Texture2D _texture;

        public void AddGameCollider(Collider collider)
        {
            _gameColliders.Add(collider);
        }

        public void DrawColliders(SpriteBatch spriteBatch)
        {
            if(this._texture == null)
            {
                this._texture = new Texture2D(GraphicsManager.Instance.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

			    this._texture.SetData(new [] { Color.White });
            }


			foreach (Collider collider in GetGameColliders())
            {
                collider.DrawCollider(spriteBatch, this._texture);
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
            Collider[] colliders = GetGameColliders();

            foreach (Collider currentCollider in colliders)
            {
                foreach (Collider collider in colliders)
                {
                    if(collider.IsActive && currentCollider.IsActive)
                        currentCollider.CheckCollision(collider);
                }
            }

        }
    }
}
