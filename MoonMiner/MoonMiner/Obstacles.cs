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
        public Obstacles(Rectangle position)
        {
            Pos = position;
            active = true;
        }

        //Movement method
        public void Move()
        {
            pos.X -= 5;
            if (pos.X == -100)
            {
                pos.X = 500;
            }
        }

        //Collision Detection Method
        public bool CheckCollision(Player plr)
        {
            // check to see if one rectangle intersects the other
            if(plr.Pos.Intersects(Pos))
            {
                return true;
            }
            else
            {
                return false;
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
