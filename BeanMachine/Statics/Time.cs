using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine
{
    public static class Time
    {
        public static float DeltaTime { get; private set; }

        public static float SecondsSinceStart { get; private set; }

        public static void Update(GameTime gameTime)
        {
            DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            SecondsSinceStart = (float)gameTime.TotalGameTime.TotalSeconds;
        }
    }
}
