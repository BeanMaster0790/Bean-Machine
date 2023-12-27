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
        private bool _started;

        public virtual void Start()
        {
            if (Scene == null)
                throw new InvalidOperationException("Cannot add component without using 'Scene.AddComponent'");
            
            this._started = true;
        }

        public virtual void Update(GameTime gameTime) 
        {
            if(!this._started)
                Start();        
        }

        public virtual void LateUpdate() { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        public virtual void Destroy()
        {
            this.ToRemove= true;
        }

        public Component()
        {
            Tag = "None";
            Name = "None";
        }
    }
}
