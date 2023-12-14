using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using BeanMachine.Scenes;

namespace BeanMachine.Graphics
{
    public class Component
    {
        public string Name { get; set; }
        public string Tag { get; set; }
        public Scene Scene { get; set; }

        public bool ToRemove;

        public virtual void Update(GameTime gameTime) 
        {
            if (Scene == null)
            {
                throw new InvalidOperationException("Cannot add component without using 'Scene.AddComponent'");
            }
        }

        public virtual void LateUpdate() { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Destroy() { }

        public Component()
        {
            Tag = "None";
            Name = "None";
        }
    }
}
