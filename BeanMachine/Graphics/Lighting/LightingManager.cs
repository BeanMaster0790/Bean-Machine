using BeanMachine.PhysicsSystem;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Graphics.Lighting
{
	public class LightingManager
	{
		private Camera _camera;

		private List<Light> _sceneLights = new List<Light>();
		private List<Shadow> _sceneShadows = new List<Shadow>();

		private RenderTarget2D _lightMap;

		public Color DarknessColour = new Color(8,8,8);

		public LightingManager(Camera camera)
		{
			this._camera = camera;

			SetMapSize(this, null);

			this._camera.SizeChange += SetMapSize;
		}

		public void SetMapSize(object sender, EventArgs args)
		{
			this._lightMap?.Dispose();

			Vector2 screenSize = new Vector2(this._camera.GetWidth(), this._camera.GetHeight());
			this._lightMap = new RenderTarget2D(GraphicsManager.Instance.GraphicsDevice, (int)screenSize.X, (int)screenSize.Y);
		}

		public void AddLight(Light light)
		{
			this._sceneLights.Add(light);
		}

		public void RemoveLight(Light light) 
		{
			this._sceneLights.Remove(light);
		}		
		
		public void AddShadow(Shadow shadow)
		{
			this._sceneShadows.Add(shadow);
		}

		public void RemoveShadow(Shadow shadow) 
		{
			this._sceneShadows.Remove(shadow);
		}


		public RenderTarget2D DrawLightingMap(SpriteBatch spriteBatch, BasicEffect effect)
		{
			GraphicsManager.Instance.GraphicsDevice.SetRenderTarget(this._lightMap);

			GraphicsManager.Instance.GraphicsDevice.Clear(this.DarknessColour);

			spriteBatch.Begin(samplerState: SamplerState.PointClamp, rasterizerState: RasterizerState.CullNone, effect: effect, sortMode: SpriteSortMode.FrontToBack);

			foreach (Light light in this._sceneLights)
			{
				DrawLight(light, spriteBatch);
			}

			foreach (Light light in this._sceneLights)
			{
				if(light.CastShadows)
					DrawShadows(light, spriteBatch);
			}

			spriteBatch.End();

			GraphicsManager.Instance.GraphicsDevice.SetRenderTarget(null);

			return this._lightMap;
		}

		private void DrawShadows(Light light, SpriteBatch spriteBatch)
		{
			foreach (Shadow shadow in this._sceneShadows) 
			{
				if (Vector2.Distance(shadow.Parent.Position, light.Parent.Position) > light.Distance)
					continue;

				Vector2 directionToShadow = shadow.Parent.Position - light.Parent.Position;

				directionToShadow.Normalize();

				Vector2[] orderedPoints = new Vector2[4];
				float[] distances = new float[4];

				for (int i = 0; i < distances.Length; i++)
				{
					Vector2 worldPoint = shadow.points[i] + shadow.Parent.Position;

					distances[i] = Vector2.DistanceSquared(worldPoint, light.Parent.Position);
					orderedPoints[i] = shadow.points[i];
				}

				float tempF = 0;

				Vector2 tempV = Vector2.Zero;

				bool swapped = false;

				for (int i = 0; i < distances.Length - 1; i++)
				{
					swapped = false;

					for (int y = 0; y < distances.Length - 1; y++)
					{
						if (distances[y] >  distances[y + 1]) 
						{
							tempF = distances[y];
							distances[y] = distances[y + 1];
							distances[y + 1] = tempF;

							tempV = orderedPoints[y];
							orderedPoints[y] = orderedPoints[y + 1];
							orderedPoints[y + 1] = tempV;

							swapped = true;
						}
					}

					if(!swapped)
						break;
				}

				Vector2[] shadowPoints = new Vector2[5];

				if(directionToShadow == new Vector2(0,1) ||  directionToShadow == new Vector2(1,0) || directionToShadow == new Vector2(-1, 0) || directionToShadow == new Vector2(0, -1))
				{
					shadowPoints[0] = orderedPoints[0] / 5;
					shadowPoints[1] = orderedPoints[1] / 5;
					shadowPoints[2] = orderedPoints[1] / 5;
				}
				else
				{
					shadowPoints[0] = orderedPoints[1] / 5;
					shadowPoints[1] = orderedPoints[0] / 5;
					shadowPoints[2] = orderedPoints[2] / 5;
				}

				Vector2 directionToPoint = shadow.Parent.Position + shadowPoints[2] - light.Parent.Position;
				directionToPoint.Normalize();

				shadowPoints[3] = shadowPoints[2] + directionToPoint * (light.Distance) / 5;

				directionToPoint = shadow.Parent.Position + shadowPoints[0] - light.Parent.Position;
				directionToPoint.Normalize();

				shadowPoints[4] = shadowPoints[0] + directionToPoint * (light.Distance) / 5;

				CustomShape shadowShape = new CustomShape((light.Distance * 4) / 5, (light.Distance * 4) / 5, true, this.DarknessColour);
				shadowShape.SetPoints(shadowPoints.ToList());

				spriteBatch.Draw(shadowShape.Texture, shadow.Parent.Position - shadow.Parent.Scene.Camera.Position, null, Color.White, 0, new Vector2(shadowShape.Texture.Width / 2, shadowShape.Texture.Height / 2), 5, SpriteEffects.None, 0f);
			}
		}

		public void DrawLight(Light light, SpriteBatch spriteBatch)
		{
			if(light.LightTexture == null)
				GenerateLightTexture(light);

			spriteBatch.Draw(light.LightTexture, light.Parent.Position - light.Parent.Scene.Camera.Position, null, light.Colour, 0, new Vector2(light.LightTexture.Width / 2, light.LightTexture.Height / 2), 1, SpriteEffects.None, 0f);
		}

		private static void GenerateLightTexture(Light light)
		{
			LightShape customShape = new LightShape(Color.White, light.Intencity, light.FalloffDistance);

			Vector2[] points = new Vector2[360];

			for (int i = 0; i < 360; i++)
			{
				float angle = MathHelper.ToRadians(i);

				Vector2 direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));

				direction.Normalize();

				RayHit hit = Raycasts.Instance.ShootRay(light.Parent.Position, direction, light.Distance);

				points[i] = direction * light.Distance;
			}

			customShape.SetPoints(points.ToList());

			light.LightTexture = customShape.Texture;
		}

		public int VectorToIndex(Vector2 position, Texture2D texture)
		{
			return ((int)position.Y * texture.Width) + (int)position.X;
		}

	}
}
