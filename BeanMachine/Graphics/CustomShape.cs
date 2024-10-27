using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Graphics
{
	public class CustomShape
	{
		public Texture2D Texture;
		private Vector2[] _points;

		private Vector2[] outlineVectors;

		private Color[] _texData; 

		public CustomShape(List<Vector2> points)
		{
			this._points = points.ToArray();

			this.InitializeTexture();
			this.SetTextureOutline();
			this.SetTextureFill();

			this.Texture.SetData(this._texData);
		}

		public void InitializeTexture()
		{
			int maxX = 0;
			int maxY = 0;

			foreach (Vector2 point in this._points) 
			{ 
				if(point.X > maxX) maxX = (int)point.X;

				if (point.Y > maxY) maxY = (int)point.Y;
			}

			this.Texture = new Texture2D(GraphicsManager.Instance.GraphicsDevice, maxX + 1, maxY + 1);
		}

		public void SetTextureOutline()
		{
			this._texData = new Color[Texture.Width * Texture.Height];

			List<Vector2> filledPixels = new List<Vector2>();

			for (int i = 0; i < this._points.Length; i++)
			{
				Vector2 startPoint = this._points[i];

				Vector2 endPoint = new Vector2();

				Vector2 currentPoint = startPoint;
				
				if(i == this._points.Length - 1)
					endPoint = this._points[0];
				else
					endPoint = this._points[i+1];

				Vector2 distanceBetweenPoints = endPoint - startPoint;

				distanceBetweenPoints = new Vector2(Math.Abs(distanceBetweenPoints.X), Math.Abs(distanceBetweenPoints.Y));

				Vector2 steps = new Vector2((startPoint.X < endPoint.X) ? 1 : -1, (startPoint.Y < endPoint.Y) ? 1 : -1);

				if (startPoint.X < endPoint.X)
					steps.X = 1;
				else if (startPoint.X == endPoint.X)
					steps.X = 0;
				else
					steps.X = -1;

				if (startPoint.Y < endPoint.Y)
					steps.Y = 1;
				else if (startPoint.Y == endPoint.Y)
					steps.Y = 0;
				else
					steps.Y = -1;

				int error = (int)(Math.Abs(distanceBetweenPoints.X) - Math.Abs(distanceBetweenPoints.Y));

				while(currentPoint != endPoint)
				{
					filledPixels.Add(currentPoint);

					int error2 = error * 2;

                    if (error2 > -distanceBetweenPoints.Y)
					{
						error -= (int)distanceBetweenPoints.Y;
						currentPoint.X += steps.X;
					}

					if (error2 < distanceBetweenPoints.X)
					{
						error += (int)distanceBetweenPoints.X;
						currentPoint.Y += steps.Y;
					}
				}

				this.outlineVectors = filledPixels.ToArray();
			}

			foreach(Vector2 point in filledPixels)
			{
				this._texData[VectorToIndex(point)] = Color.White;
			}

		}

		public void SetTextureFill()
		{
			bool filling = true;

			for (int i = 0; i < this._texData.Length; i++)
			{
				Color color = this._texData[i];

				if (color != new Color(0, 0, 0, 0))
				{
					filling = !filling;
				}

				if (filling)
				{
					this._texData[i] = Color.White;
				}
			}
		}

		public int VectorToIndex(Vector2 point)
		{
			return ((int)point.Y * this.Texture.Width) + (int)point.X;
		}
	}
}
