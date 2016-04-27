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
    class Collectible:Obstacles
    {
        private SoundEffect life;
        //constructor
        public Collectible(int mod, SoundEffect sfx, SoundEffect lf):base(mod, sfx)
        {
            Speed = 6;
            Pos = new Rectangle(2000, 220, 20, 20);
            life = lf;
        }

        public override void CheckCollision(Player plr, Game1 gm)
        {
            // check to see if one rectangle intersects the other
            if (plr.Pos.Intersects(Pos) && Active == true)
            {
                plr.NumCol += 1;
                Active = false;
                gm.ScoreNum += 50;
                SFX.Play();
                //add a life
                if(plr.NumCol >= 10)
                {
                    plr.NumLives += 1;
                    plr.NumCol = 0;
                    life.Play();
                }
            }
        } 
    }
}
