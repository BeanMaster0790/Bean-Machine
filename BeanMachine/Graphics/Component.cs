﻿using Microsoft.Xna.Framework.Graphics;
using System;
using BeanMachine.Scenes;
using BeanMachine.Debug;

namespace BeanMachine.Graphics
{
    public class Component
    {
        public string Name { get; set; }

		public bool IsVisable = true;
		public bool IsActive = true;

		protected bool _started;

        public Scene Scene { get; set; }

        public string Tag { get; set; }

        public bool ToRemove;

        public Component()
        {
            Name = "None";
            Tag = "None";
        }

        public virtual void Draw(SpriteBatch spriteBatch) { }

        public virtual void Destroy()
        {
            this.ToRemove = true;
        }

        public virtual void LateUpdate() { }

        public virtual void Start()
        {
            if (this.Scene == null)
                throw new InvalidOperationException("Cannot add component without using 'Scene.AddComponent'");

            this._started = true;
        }

        public virtual void Update()
        {
            if (!this._started)
                this.Start();
        }

    }
}
