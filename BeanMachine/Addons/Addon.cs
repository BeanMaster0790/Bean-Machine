using BeanMachine.Graphics;
using Microsoft.Xna.Framework;
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

        public virtual void Update(GameTime gameTime) { }
        public virtual void Destroy() { } 
    }
}
