using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonMinerExecutable
{
    class Obstacles
    {
        //attributes
        private Texture2D image;
        private Rectangle pos;
        private bool active;
        private int speed;
        private int spMod = 0;

        //properties
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }
        public Rectangle Pos
        {
            get { return pos; }
            set { pos = value; }

        }
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }
        // create X and Y properties for easier access to the rectangle position
        public int X
        {
            get { return pos.X; }
            set { pos.X = value; }
        }
        public int Y
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        //constructor
        public Obstacles(int mod)
        {            
            active = true;
            pos = new Rectangle(2000, 370, 30, 30);
            speed = 5;
            spMod = mod;
        }

        //Movement method
        public void Move()
        {
            pos.X -= (Speed + spMod);
            if (pos.X == -100)
            {
                active = false;                
            }
        }

        //Collision Detection Method
        public virtual void CheckCollision(Player plr, Game1 gm)
        {
            // check to see if one rectangle intersects the other
            if(plr.Pos.Intersects(Pos) && active == true)
            {
                plr.NumLives -=1;
                active = false;
            }
            
        }

        //draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            if(active == true)
            {
                spriteBatch.Draw(Image, new Rectangle(pos.X,pos.Y,50,50), Color.White);
                spriteBatch.Draw(Image, new Vector2(500, 500), Color.White);
            }
        }
    }
}
