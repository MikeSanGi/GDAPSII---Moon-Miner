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
    class Rocks:Obstacles
    {       
        //constructor
        public Rocks(int mod, SoundEffect sfx) :base(mod, sfx)
        {
            Speed = 5;
            Pos = new Rectangle(2000, 370, 30, 30);
            Width = 29;
            Height = 29;
            NumFrames = 12;
        }
    }
}
