using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonMinerExecutable
{
    class Spike: Obstacles
    {
        public Spike(int mod):base(mod)
        {
            Speed = 7;
            Pos = new Rectangle(2000, 300, 10, 100);
        }
    }
}
