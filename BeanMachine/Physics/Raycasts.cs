using BeanMachine.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace BeanMachine.PhysicsSystem
{
    public class Raycasts
    {
        public static Raycasts Instance = new Raycasts();

        public Collider OverlapBox(Vector2 position, int size)
        {
            Sprite tempSprite = new Sprite(rectWidth: size, rectHeight: size) { Name = "TempCollider" };

            Collider tempCollider = new Collider(size, size, isRaycast: true);

			tempSprite.AddAddon(tempCollider);

            tempSprite.Position = position;

            foreach (Collider collider in Physics.Instance.GetGameColliders())
            {
                if (collider.CheckCollision(tempCollider))
                {
                    return collider;
                }
            }

            return null;
        }

        public Collider[] OverlapBoxAll(Vector2 position, int size)
        {
            Sprite tempSprite = new Sprite(rectWidth: size, rectHeight: size);

            Collider tempCollider = new Collider(size, size, isRaycast: true) { Height = size, Width = size };

			tempSprite.AddAddon(tempCollider);


            tempSprite.Position = position;

            List<Collider> result = new List<Collider>();

            foreach (Collider collider in Physics.Instance.GetGameColliders())
            {
                if (collider.CheckCollision(tempCollider))
                {
                    result.Add(collider);
                }
            }

            return result.ToArray();
        }

        public RayHit ShootRay(Vector2 position, Vector2 direction, int range, Component ignore = null)
        {
            Line ray = new Line(position, position + direction *  range);

            Collider[] colliders = Physics.Instance.GetGameColliders();

            RayHit ClosestHit = new RayHit(new Vector2(range * 4, range * 4), null);

            foreach (Collider collider in colliders)
            {
                if(ignore == collider.Parent || !collider.IsActive)
                    continue;

                foreach (Line edge in collider.Edges.Edges)
                {
                    Vector2? intercept = GetIntercept(ray, edge);


                    if(intercept != null)
                    {
                        float distanceFromOrigin = Vector2.Distance(position, (Vector2)intercept);

                        if(distanceFromOrigin > range)
                            continue;

                        if(distanceFromOrigin < Vector2.Distance(position, ClosestHit.HitPoint))
                            ClosestHit = new RayHit((Vector2)intercept, collider);
                    }
                }
            }

            if(ClosestHit.Collider == null)
                return null;

            return ClosestHit;

        }

		public RayHit[] ShootRayAll(Vector2 position, Vector2 direction, int range, Component ignore = null)
		{
			Line ray = new Line(position, position + direction * range);

			Collider[] colliders = Physics.Instance.GetGameColliders();

            List<RayHit> results = new List<RayHit> ();

			foreach (Collider collider in colliders)
			{
				if (ignore == collider.Parent || !collider.IsActive)
					continue;

				foreach (Line edge in collider.Edges.Edges)
				{
					Vector2? intercept = GetIntercept(ray, edge);


					if (intercept != null)
					{
						float distanceFromOrigin = Vector2.Distance(position, (Vector2)intercept);

						if (distanceFromOrigin > range)
							continue;

						results.Add(new RayHit((Vector2)intercept, collider));

                        break;
					}
				}
			}

			return results.ToArray();

		}

		public Vector2? GetIntercept(Line ray, Line edge)
        {
			float denominator = (ray.StartPoint.X - ray.EndPoint.X) * (edge.StartPoint.Y - edge.EndPoint.Y) - (ray.StartPoint.Y - ray.EndPoint.Y) * (edge.StartPoint.X - edge.EndPoint.X);

			if (denominator == 0)
				return null;

			float t = ((ray.StartPoint.X - edge.StartPoint.X) * (edge.StartPoint.Y - edge.EndPoint.Y) - (ray.StartPoint.Y - edge.StartPoint.Y) * (edge.StartPoint.X - edge.EndPoint.X)) / denominator;
			float u = -((ray.StartPoint.X - ray.EndPoint.X) * (ray.StartPoint.Y - edge.StartPoint.Y) - (ray.StartPoint.Y - ray.EndPoint.Y) * (ray.StartPoint.X - edge.StartPoint.X)) / denominator;

			if (t >= 0 && u >= 0 && u <= 1)
			{
				float intersectionX = ray.StartPoint.X + t * (ray.EndPoint.X - ray.StartPoint.X);
				float intersectionY = ray.StartPoint.Y + t * (ray.EndPoint.Y - ray.StartPoint.Y);
				return new Vector2(intersectionX, intersectionY);
			}

			return null;
		}
    }

    public class RayHit
    {
        public Vector2 HitPoint;

        public Collider Collider;

        public RayHit(Vector2 hitPoint, Collider collider)
        {
            this.HitPoint = hitPoint;
            this.Collider = collider;
        }
    }

    public class Line
    {
        public Vector2 StartPoint;
		public Vector2 EndPoint;

        public Vector2 Direction;

        public float Angle;

        public int Length;

        public Color Colour;

        public int Thickness;

        public Line(Vector2 startPoint, Vector2 endPoint)
		{
			this.StartPoint = startPoint;
			this.EndPoint = endPoint;

            this.Direction = this.EndPoint - this.StartPoint;

            this.Direction.Normalize();

            this.Angle = MathF.Atan2(this.Direction.Y, this.Direction.X);

            this.Angle = MathHelper.ToDegrees(this.Angle);

            this.Length = (int)Vector2.Distance(this.StartPoint, this.EndPoint);

            this.Colour = Color.White;
		}

        public Line(Vector2 startPoint, Vector2 endPoint, Color colour, int thickness) : this(startPoint, endPoint)
        {
            this.Colour = colour;
            this.Thickness = thickness;
        }

        public Line(Vector2 startPoint, float angle, int length)
        {
            this.StartPoint = startPoint;

            this.Angle = angle;

            angle = MathHelper.ToRadians(angle);

            this.Direction = new Vector2(MathF.Cos(angle), MathF.Sin(angle));

            this.Direction.Normalize();

            this.Length = length;

            this.EndPoint = startPoint + this.Direction * this.Length;
		}

		public Line(Vector2 startPoint, float angle,int length , Color colour, int thickness) : this(startPoint, angle, length)
		{
			this.Colour = colour;
			this.Thickness = thickness;
		}

	}

    public class ColliderEdges
    {
        public Line Top;
        public Line Bottom;
        public Line Left;
        public Line Right;

        public Line[] Edges = new Line[4];

        public ColliderEdges(Collider collider)
        {
			Top = new Line(new Vector2(collider.Rectangle.Left, collider.Rectangle.Top), new Vector2(collider.Rectangle.Right, collider.Rectangle.Top));
			Bottom = new Line(new Vector2(collider.Rectangle.Left, collider.Rectangle.Bottom), new Vector2(collider.Rectangle.Right, collider.Rectangle.Bottom));
			Left = new Line(new Vector2(collider.Rectangle.Left, collider.Rectangle.Top), new Vector2(collider.Rectangle.Left, collider.Rectangle.Bottom));
			Right = new Line(new Vector2(collider.Rectangle.Right, collider.Rectangle.Top), new Vector2(collider.Rectangle.Right, collider.Rectangle.Bottom));

			this.Edges[0] = this.Top;
            this.Edges[1] = this.Bottom;
            this.Edges[2] = this.Left;
            this.Edges[3] = this.Right;
        }
    }
}
