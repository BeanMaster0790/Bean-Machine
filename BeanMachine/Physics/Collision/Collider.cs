using BeanMachine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;
using BeanMachine.Debug;
using BeanMachine.Graphics.UI;

namespace BeanMachine.PhysicsSystem
{
    public class Collider : Addon
    {
        public List<Collider> CheckedColliders;

		private Color _drawColour;

        public ColliderEdges Edges;

        public int Height { get; set; }

		public bool IsActive;

		public bool IsRaycast;

        public EventHandler<CollisionEventArgs> OnCollideEvent;

		public Vector2 PositionOffset;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)(Parent.GetSpriteRectangle().X + PositionOffset.X), (int)(Parent.GetSpriteRectangle().Y + PositionOffset.Y), Width, Height);
            }
        }

        public int Width { get; set; }

        public Collider(int width, int height, bool isRaycast = false)
        {
			this.Width = width;

			this.Height = height;

            this.IsRaycast = isRaycast;

            this.CheckedColliders = new List<Collider>();

            if (!isRaycast)
                Physics.Instance.AddGameCollider(this);
        }

		public override void Start()
		{
			base.Start();

			this.Edges = new ColliderEdges(this);

			this.IsActive = true;

			this._drawColour = new Color(Random.RandomInt(1, 255), Random.RandomInt(1, 255), Random.RandomInt(1, 255), 75);
		}

		public override void Update()
        {
            base.Update();

            this.CheckedColliders.Clear();

			this.Edges = new ColliderEdges(this);
		}

		public bool CheckCollision(Collider collider)
		{
			if (collider == null)
			{
				return false;
			}

			if (this.CheckedColliders.Contains(collider) || collider.CheckedColliders.Contains(this))
			{
				return false;
			}

			if (collider.Parent == this.Parent)
			{
				return false;
			}

			if (this.IsRaycast && collider.IsRaycast)
			{
				return false;
			}


			bool collided = false;

			Vector2 direction = Vector2.Zero;

			if (this.Rectangle.Right + this.Parent.Velocity.X > collider.Rectangle.Left &&
				this.Rectangle.Left < collider.Rectangle.Left &&
				this.Rectangle.Bottom - 30 > collider.Rectangle.Top &&
				this.Rectangle.Top + 30 < collider.Rectangle.Bottom)
			{
				collided = true;

				direction.X = -1;
			}
			else if (this.Rectangle.Left + this.Parent.Velocity.X < collider.Rectangle.Right &&
					   this.Rectangle.Right > collider.Rectangle.Right &&
					   this.Rectangle.Bottom - 30 > collider.Rectangle.Top &&
					   this.Rectangle.Top + 30 < collider.Rectangle.Bottom)
			{
				collided = true;

				direction.X = 1;
			}


			if (this.Rectangle.Bottom + this.Parent.Velocity.Y > collider.Rectangle.Top &&
				 this.Rectangle.Top < collider.Rectangle.Top &&
				 this.Rectangle.Right - 30 > collider.Rectangle.Left &&
				 this.Rectangle.Left + 30 < collider.Rectangle.Right)
			{
				collided = true;

				direction.Y = -1;
			}
			if (this.Rectangle.Top + this.Parent.Velocity.Y < collider.Rectangle.Bottom &&
				 this.Rectangle.Bottom > collider.Rectangle.Bottom &&
				 this.Rectangle.Right - 30 > collider.Rectangle.Left &&
				 this.Rectangle.Left + 30 < collider.Rectangle.Right)
			{
				collided = true;

				direction.Y = 1;
			}


			if (collided == true)
			{
				if (!collider.IsRaycast)
				{
					this.OnCollide(new Collision(this, collider, direction));
					collider.OnCollide(new Collision(collider, this, -direction));
				}

				return true;
			}

			return false;
		}


		public void DrawCollider(SpriteBatch spriteBatch, Texture2D texture)
        {
			spriteBatch.Draw(texture, new Vector2(this.Rectangle.X, this.Rectangle.Y) - base.Parent.Scene.Camera.Position, null, this._drawColour, 0, Vector2.Zero, new Vector2(this.Rectangle.Width, this.Rectangle.Height), SpriteEffects.None, 0.9f);
		}

		public override void Destroy()
        {
            Physics.Instance.RemoveGameCollider(this);
        }

        protected virtual void OnCollide(Collision collision)
        {
            OnCollideEvent?.Invoke(this, new CollisionEventArgs(collision));
        }

    }

    public class CollisionEventArgs : EventArgs
    {
        public CollisionEventArgs(Collision collision) : base()
        {
            this.Collision = collision;
        }

        public Collision Collision { get; private set; }
    }
}
