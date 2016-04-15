using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonMinerExecutable
{
    class Rocks:Obstacles
    {       
        //constructor
        public Rocks(int mod):base(mod)
        {
            Speed = 4;
            Pos = new Rectangle(2000, 370, 30, 30);
        }
    }
}
