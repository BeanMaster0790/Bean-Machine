using BeanMachine.Debug;
using BeanMachine.PhysicsSystem;
using BeanMachine.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace BeanMachine.Graphics
{
    public class Camera
    {
        public Vector2 Position;
        private float _z;
        private float _baseZ;

        private float _aspectRatio;
        private float _fov;

        private Matrix _viewMatrix;
        private Matrix _projectionMatrix;

        private BasicEffect _effect;

        private GraphicsDevice _graphicsDevice;

        public Camera(GraphicsDevice graphicsDevice)
        {

            this.Position = new Vector2(0, 0);

            this.UpdateAspectRatio(this, EventArgs.Empty);

            this._fov = MathHelper.PiOver2;

            this._baseZ = GetZFromHeight(GraphicsManager.Instance.GetScreenSize().Y);
            this._z = this._baseZ;

            this._graphicsDevice = graphicsDevice;

            this._effect = new BasicEffect(graphicsDevice);
            this._effect.FogEnabled = false;
            this._effect.TextureEnabled = true;
            this._effect.LightingEnabled = false;
            this._effect.PreferPerPixelLighting = false;
            this._effect.VertexColorEnabled = true;
            this._effect.Texture = null;
            this._effect.Projection = Matrix.Identity;
            this._effect.View = Matrix.Identity;
            this._effect.World = Matrix.Identity;

            this.UpdateMatix(this, EventArgs.Empty);

            GraphicsManager.Instance.GraphicsChanged += this.UpdateAspectRatio;
            GraphicsManager.Instance.GraphicsChanged += this.UpdateMatix;
        }

        public float GetZFromHeight(float height)
        {
            return -((height / 2) / MathF.Tan(this._fov / 2));
        }

        public float GetHeight()
        {
            return (this._z * -2) * MathF.Tan(this._fov / 2);
        }

        public float GetWidth()
        {
            return GetHeight() * this._aspectRatio;
        }

        public float GetTop()
        {
            return this.Position.Y - GetHeight() / 2;
        }

        public float GetBottom() 
        {
			return this.Position.Y + GetHeight() / 2;
		}

        public float GetLeft()
        {
            return this.Position.X - GetWidth() / 2;
        }

        public float GetRight()
        {
            return this.Position.X + GetWidth() / 2;
        }

        public void UpdateMatix(object sender, EventArgs e)
        {
            this._viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, this._z), Vector3.Zero, Vector3.Down);
            this._projectionMatrix = Matrix.CreatePerspectiveFieldOfView(this._fov, this._aspectRatio, 1, 8192);
        }

        public void UpdateAspectRatio(object sender, EventArgs e)
        {
            this._aspectRatio = GraphicsManager.Instance.GetScreenAspectRatio();
        }

        public void MoveZ(float amount)
        {
            this._z += amount;
        }

        public void SetZ(float z)
        {
            this._z = z;
            UpdateMatix(this, EventArgs.Empty);
        }


        public void Draw(SpriteBatch spriteBatch, List<Component> components)
        {

            this._effect.View = this._viewMatrix;
            this._effect.Projection = this._projectionMatrix;

            this._graphicsDevice.Clear(Color.Black);

            spriteBatch.Begin(samplerState: SamplerState.PointClamp, rasterizerState: RasterizerState.CullNone, effect: this._effect, sortMode: SpriteSortMode.FrontToBack);

            foreach (Component component in components)
            {
                if(component.IsVisable)
                    component.Draw(spriteBatch);
            }

            //DebugManager.Instance.Draw(spriteBatch);

            spriteBatch.End();
        }

        public Vector2 ScreenToWorld(Vector2 position)
        {
            Vector2 screenSize = GraphicsManager.Instance.GetScreenSize();

            Viewport screenViewport = new Viewport(0, 0, (int)screenSize.X, (int)screenSize.Y);

            Ray ray = CreateMouseRay(position, screenViewport);

            Plane worldPlane = new Plane(new Vector3(0, 0, 1), 0f);

            float? dist = ray.Intersects(worldPlane);
            Vector3 interceptPoint = ray.Position + ray.Direction * dist.Value;

            Vector2 result = new Vector2(interceptPoint.X, interceptPoint.Y) + this.Position;

            return result;
        }

        private Ray CreateMouseRay(Vector2 position, Viewport viewport)
        {
            Vector3 nearPoint = new Vector3(position, 0);
            Vector3 farPoint = new Vector3(position, 1);

            nearPoint = viewport.Unproject(nearPoint, this._projectionMatrix, this._viewMatrix, Matrix.Identity);
            farPoint = viewport.Unproject(farPoint, this._projectionMatrix, this._viewMatrix, Matrix.Identity);

            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            Ray ray = new Ray(nearPoint, direction);

            return ray;
        }
    }
}
