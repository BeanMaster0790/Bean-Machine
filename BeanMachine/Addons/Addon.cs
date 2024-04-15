using BeanMachine.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeanMachine
{
    public class Addon
    {
        public Sprite Parent;

        private bool _started;

        public virtual void Destroy() { }

        public virtual void Start()
        {
            this._started = true;
        }

        public virtual void Update()
        {
            if (!this._started)
                this.Start();
        }

        public virtual void LateUpdate()
        {

        }
    }
}
