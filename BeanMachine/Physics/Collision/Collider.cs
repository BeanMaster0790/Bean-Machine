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

namespace BeanMachine.PhysicsSystem
{
    public class Collider : Addon
    {
        public List<Collider> CheckedColliders;

        private bool _drawCollider;

        public int Height { get; set; }

        public bool IsSolid { get; set; }

        public bool IsRaycast { get; set; }


        //Event Is not calling despite collision
        public EventHandler<CollisionEventArgs> OnCollideEvent;

        public Vector2 PositionOffset { get; set; }

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)(Parent.GetSpriteRectangle().X + PositionOffset.X), (int)(Parent.GetSpriteRectangle().Y + PositionOffset.Y), Width, Height);
            }
        }

        public int Width { get; set; }

        public Collider(bool isRaycast = false, bool drawCollider = false)
        {
            this._drawCollider = drawCollider;

            this.IsRaycast = isRaycast;

            this.CheckedColliders = new List<Collider>();

            if (!isRaycast)
                Physics.Instance.AddGameCollider(this);
        }

        public override void Update()
        {
            this.CheckedColliders.Clear();
            base.Update();
        }

        public bool CheckCollision(Collider collider, bool isRay = false)
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

            if (!isRay && collider.IsRaycast)
            {
                return false;
            }

            if (isRay && collider.IsRaycast)
            {
                return false;
            }


            this.CheckedColliders.Add(collider);
            collider.CheckedColliders.Add(this);

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
                this.OnCollide(new Collision(this, collider, direction));
                collider.OnCollide(new Collision(collider, this, -direction));
                return true;
            }

            return false;
        }

        public void DrawCollider(SpriteBatch spriteBatch)
        {
            //if (!this._drawCollider)
            //    return;

            //List<Color> colours = new List<Color>();

            //for (int x = 0; x < Width; x++)
            //{
            //    for (int y = 0; y < Height; y++)
            //    {
            //        if (x == 0 || x == Width - 1
            //            || y == 0 || y == Height - 1)
            //        {
            //            colours.Add(Color.Green);
            //        }
            //        else
            //            colours.Add(Color.Transparent);
            //    }
            //}

            //Texture2D texture = new Texture2D(Globals.GraphicsDevice, Width, Height);
            //texture.SetData(colours.ToArray());

            //spriteBatch.Draw(texture, new Vector2(this.Rectangle.X, this.Rectangle.Y), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0);

            //NEEDS REWORK
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
