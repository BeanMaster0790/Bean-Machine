using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using BeanMachine.Graphics.Animations;
using System.Collections.Generic;
using System.Linq;
using System;
using BeanMachine.Debug;
using System.Diagnostics;
using BeanMachine.PhysicsSystem;

namespace BeanMachine.Graphics
{
    public class Sprite : Component, IDebuggable
    {
        public List<Addon> Addons = new List<Addon>();

        public AnimationManager AnimationManager;

        public Color Colour = Color.White;

        public float layer = 0;

        private Vector2 _origin;

        public Vector2 GetOrigin()
        {
            if (this._userChangedOrigin)
                return this._origin;

            if (this.AnimationManager != null)
                return this.GetAddon<AnimationManager>().CurrentAnimation.GetCenterOrigin();

            if (this._texture == null)
                return Vector2.Zero;

            return new Vector2(this._texture.Width / 2, this._texture.Height / 2);
        }

		public bool ShowDebugInfo { get; set; }

		public List<string> DebugValueNames { get { return this._debugValueNames; } }

		private List<string> _debugValueNames = new List<string>();

		private bool _userChangedOrigin;

        public Vector2 Position;

        public Sprite Parent;

        public float Rotation;

        public float Scale;
        
        public SpriteEffects spriteEffect;

        protected Texture2D _texture;

        private int _rectWidth;

        private int _rectHeight;

        public Rectangle GetSpriteRectangle()
        {
            if (this.AnimationManager != null)
            {
                Vector2 widthAndHeight = this.AnimationManager.CurrentAnimation.GetFrameDimentions();
                return new Rectangle((int)(Position.X - GetOrigin().X * this.Scale), (int)(Position.Y - GetOrigin().Y * this.Scale), (int)(widthAndHeight.X * this.Scale), (int)(widthAndHeight.Y * this.Scale));
            }

            if (_texture != null)
                return new Rectangle((int)(Position.X - GetOrigin().X * this.Scale), (int)(Position.Y - GetOrigin().Y * this.Scale), (int)(this._texture.Width * this.Scale), (int)(this._texture.Height * this.Scale));
            else
                return new Rectangle((int)Position.X, (int)Position.Y, _rectWidth, _rectHeight);
        }

        public Vector2 Velocity;

        public Sprite(int rectWidth = 0, int rectHeight = 0) : base()
        {

            if (rectWidth != 0)
                this._rectWidth = rectWidth;

            if (rectHeight != 0)
                this._rectHeight = rectHeight;
        }

        public Sprite(Texture2D texture) : this()
        {
            this._texture = texture;
        }

        public void AddAddon(Addon addon)
        {
            addon.Parent = this;
            this.Addons.Add(addon);

            this.AnimationManager = this.GetAddon<AnimationManager>();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            if (this.AnimationManager != null)
            {
                this.AnimationManager.Draw(spriteBatch);
            }

            else if (this._texture != null)
                spriteBatch.Draw(this._texture, this.Position - base.Scene.Camera.Position, null, this.Colour, MathHelper.ToRadians(this.Rotation), this.GetOrigin(), this.Scale, this.spriteEffect, this.layer);

			foreach (Addon addon in this.Addons.ToArray())
			{
				addon.Draw(spriteBatch);
			}

		}

        public override void Destroy()
        {
            base.Destroy();

            foreach (Addon addon in this.Addons.ToArray())
            {
                addon.Destroy();
                this.Addons.Remove(addon);
            }
        }

        public T GetAddon<T>()
        {
            foreach (Addon addon in this.Addons)
            {
                if (addon.GetType() == typeof(T))
                    return (T)Convert.ChangeType(addon, typeof(T));
            }
            return default(T);
        }

        public T[] GetAddons<T>()
        {
            List<T> list = new List<T>();

            foreach (Addon addon in this.Addons)
            {
                if (addon.GetType() == typeof(T))
                    list.Add((T)Convert.ChangeType(addon, typeof(T)));
            }

            return list.ToArray();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            foreach (Addon addon in this.Addons)
            {
                addon.LateUpdate();
            }

        }


        public override void Start()
        {
            base.Start();

			DebugManager.Instance.AddDebugabble(this);
        }

        public void SetOrigin(Vector2 origin)
        {
            this._origin = origin;
            this._userChangedOrigin = true;
        }

        public override void Update()
        {
            base.Update();

            foreach (Addon addon in this.Addons)
            {
                addon.Update();
            }
        }

		public void AddDebugValue(string name)
		{
            this._debugValueNames.Add(name);
		}

		public Vector2 GetDebugDrawPosition()
		{
            return new Vector2(this.Position.X + this.GetSpriteRectangle().Width / 2, this.Position.Y - this.GetSpriteRectangle().Height / 2);
		}
	}
}
