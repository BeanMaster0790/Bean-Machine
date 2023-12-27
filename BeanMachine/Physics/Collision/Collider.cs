using BeanMachine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework.Graphics;

namespace BeanMachine.PhysicsSystem
{
    public class Collider : Addon
    {
        public Vector2 PositionOffset { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public bool IsSolid { get; set; }
        public bool IsRaycast {  get; set; }

        public EventHandler<CollisionEventArgs> OnCollideEvent;

        private bool _drawCollider;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)(Parent.Rectangle.X + PositionOffset.X), (int)(Parent.Rectangle.Y + PositionOffset.Y), Width, Height);
            }
        }

        public Collider(bool isRaycast = false, bool drawCollider = false)
        {
            this._drawCollider= drawCollider;

            this.IsRaycast = isRaycast;

            if(!isRaycast) 
                Physics.AddGameCollider(this);
        }

        public bool CheckCollision(Collider collider)
        {
            if (collider.Parent == this.Parent || collider.IsRaycast)
            {
                return false;
            }

            CollisionDirection[] directions = { CollisionDirection.None, CollisionDirection.None };


            if (this.Rectangle.Right + this.Parent.Velocity.X > collider.Rectangle.Left &&
                this.Rectangle.Left < collider.Rectangle.Left &&
                this.Rectangle.Bottom > collider.Rectangle.Top &&
                this.Rectangle.Top < collider.Rectangle.Bottom)
            {
                directions[0] = CollisionDirection.Right;
            }
            else if (this.Rectangle.Left + this.Parent.Velocity.X < collider.Rectangle.Right &&
                       this.Rectangle.Right > collider.Rectangle.Right &&
                       this.Rectangle.Bottom > collider.Rectangle.Top &&
                       this.Rectangle.Top < collider.Rectangle.Bottom)
            {
                directions[0] = CollisionDirection.Left;
            }


            if (this.Rectangle.Bottom + this.Parent.Velocity.Y > collider.Rectangle.Top &&
                 this.Rectangle.Top < collider.Rectangle.Top &&
                 this.Rectangle.Right > collider.Rectangle.Left &&
                 this.Rectangle.Left < collider.Rectangle.Right)
            {
                directions[1] = CollisionDirection.Bottom;
            }
            if (this.Rectangle.Top + this.Parent.Velocity.Y < collider.Rectangle.Bottom &&
                 this.Rectangle.Bottom > collider.Rectangle.Bottom &&
                 this.Rectangle.Right > collider.Rectangle.Left &&
                 this.Rectangle.Left < collider.Rectangle.Right)
            {
                directions[1] = CollisionDirection.Top;
            }


            if (directions[0] != CollisionDirection.None || directions[1] != CollisionDirection.None)
            {
                OnCollide(new Collision(this, collider, directions));
                return true;
            }

            return false;
        }

        protected virtual void OnCollide(Collision collision)
        {
            OnCollideEvent?.Invoke(this, new CollisionEventArgs(collision));
        }

        public void DrawCollider(SpriteBatch spriteBatch)
        {
            if (!this._drawCollider)
                return;

            List<Color> colours = new List<Color>();

            for( int x = 0; x < Width; x++ )
            {
                for(int y = 0; y < Height; y++ )
                {
                    if (x == 0 || x == Width -1
                        || y == 0 || y == Height - 1)
                    {
                        colours.Add(Color.Green);
                    }
                    else
                        colours.Add(Color.Transparent);
                }
            }

            Texture2D texture = new Texture2D(Globals.GraphicsDevice, Width, Height);
            texture.SetData(colours.ToArray());

            spriteBatch.Draw(texture, new Vector2(this.Rectangle.X, this.Rectangle.Y), null, Color.White, 0, Vector2.Zero , 1, SpriteEffects.None, 0);
        }

        public override void Destroy()
        {
            Physics.RemoveGameCollider(this);
        }

    }

    public class CollisionEventArgs : EventArgs
    {
        public Collision Collision { get; private set; }

        public CollisionEventArgs(Collision collision) : base()
        {
            this.Collision = collision;
        }
            

    }
}
