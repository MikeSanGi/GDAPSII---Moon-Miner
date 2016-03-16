using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonMiner
{
    class Obstacles
    {
        //attributes
        private Texture2D image;
        private Rectangle pos;
        private Boolean active;

        //properties
        public Boolean Active
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

        //constructor
        public Obstacles(Texture2D img)
        {
            image = img;
            active = true;
        }

        //Movement method
        public void Move()
        {

        }

        //Collision Detection Method
        public Boolean CheckCollision(Player plr)
        {
            Boolean collide = false;
            return collide;
        }

        //draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            if(active == true)
            {
                spriteBatch.Draw(Image, pos, Color.White);
            }
        }
    }
}
