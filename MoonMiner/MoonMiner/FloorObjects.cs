using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonMinerExecutable
{
    class FloorObjects
    {
        //attributes
        private Texture2D image;
        private Vector2 pos;
        private int speed;

        //properties
        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        //constructor
        public FloorObjects(Vector2 ps, int sp)
        {
           pos = ps;
           speed = sp;
        }

        //property
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        //Movement
        public void MoveFloor()
        {
            pos.X -= speed;
            if (pos.X <= -960)
            {
                pos.X = 0;
            }
        }

        //Draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, pos, Color.White);
        }
    }
}
