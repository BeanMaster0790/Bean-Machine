using BeanMachine.Debug;
using Microsoft.Xna.Framework;

namespace BeanMachine
{
    public class Time
    {
        public static Time Instance = new Time();

        public int Fps = 0;

        public float DeltaTime { get; private set; }

        public float SecondsSinceStart { get; private set; }

        private long _milsSinceStart = 0;
        private long _lastMilsSinceStart = 0;

        public void Update(GameTime gameTime)
        {
            this.DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            this.SecondsSinceStart = (float)gameTime.TotalGameTime.TotalSeconds;

            this._lastMilsSinceStart = this._milsSinceStart;
            this._milsSinceStart = (long)gameTime.TotalGameTime.TotalMilliseconds;

            long difference = this._milsSinceStart - _lastMilsSinceStart;

            DebugManager.Instance.Monitor(difference, "Diff");

            if(difference > 0)
            {
                float fps = 1000 / difference;
                this.Fps = (int)fps;
            }

        }
    }
}
