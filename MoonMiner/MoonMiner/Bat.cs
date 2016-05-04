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
    class Bat:Obstacles
    {
        // attributes
        int posY = 0;
        Random seedGen = new Random();
        int seed = 0;
        Random rgen;

        //constructor
        public Bat(int mod, SoundEffect sfx) :base(mod, sfx)
        {
            seed = seedGen.Next();
            rgen = new Random(seed);
            posY = rgen.Next(100, 400);
            Speed = 4;
            Pos = new Rectangle(2000, posY, 40, 30);
            //animation
            Height = 30;
            Width = 40;
            NumFrames = 3;
        }
    }
}
