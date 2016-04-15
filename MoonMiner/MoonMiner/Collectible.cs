using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonMinerExecutable
{
    class Collectible:Obstacles
    {
        //constructor
        public Collectible(int mod):base(mod)
        {
            Speed = 6;
            Pos = new Rectangle(2000, 220, 20, 20);
        }

        public override void CheckCollision(Player plr, Game1 gm)
        {
            // check to see if one rectangle intersects the other
            if (plr.Pos.Intersects(Pos) && Active == true)
            {
                plr.NumCol += 1;
                Active = false;
                gm.ScoreNum += 50;
                //add a life
                if(plr.NumCol >= 10)
                {
                    plr.NumLives += 1;
                    plr.NumCol = 0;
                }
            }
        } 
    }
}
