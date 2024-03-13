using BeanMachine.Debug;
using Microsoft.Xna.Framework;

namespace BeanMachine
{
    public class Time
    {
        public float DeltaTime { get; private set; }

        public int Fps = 0;

        public static Time Instance = new Time();

        private long _lastMilsSinceStart = 0;

        private long _milsSinceStart = 0;

        public float SecondsSinceStart { get; private set; }


        public void Update(GameTime gameTime)
        {
            this.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.SecondsSinceStart = (float)gameTime.TotalGameTime.TotalSeconds;

            this._lastMilsSinceStart = this._milsSinceStart;
            this._milsSinceStart = (long)gameTime.TotalGameTime.TotalMilliseconds;

            long difference = this._milsSinceStart - _lastMilsSinceStart;


            if(difference > 0)
            {
                float fps = 1000 / difference;
                this.Fps = (int)fps;
            }

            DebugManager.Instance.Monitor(difference, "Frame Time");
            DebugManager.Instance.Monitor(Fps, "FPS");

        }
    }
}
