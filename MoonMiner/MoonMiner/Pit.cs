using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace MoonMinerExecutable
{
    class Pit: Obstacles
    {
        //constructor
        public Pit(int mod, SoundEffect sfx) :base(mod, sfx)
        {
            Speed = 4;
            Pos = new Rectangle(2000, 399, 50, 100);
        }
    }
}
