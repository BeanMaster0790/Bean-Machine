using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine.Graphics.Lighting
{
	public class Light : Addon
	{
		public float Intencity;

		public int Distance;

		public int FalloffDistance;

		public Color Colour;

		public Texture2D LightTexture;

		public bool CastShadows;

		public Light(float intencity, int distance,int falloffDistance, Color colour, bool castShadows = false)
		{
			this.Intencity = intencity;
			this.Distance = distance;
			this.FalloffDistance = falloffDistance;
			this.Colour = colour;

			this.CastShadows = castShadows;
		}

		public override void Start()
		{
			base.Start();

			base.Parent.Scene.LightingManager.AddLight(this);
		}

		public override void Destroy()
		{
			base.Destroy();

			base.Parent.Scene.LightingManager.RemoveLight(this);
		}
	}
}
