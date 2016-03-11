using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MoonMiner
{
    class FloorObjects
    {
        //attributes
        private Texture2D image;
        private Vector2 pos;

        //properties
        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        //constructor
        public FloorObjects(Vector2 ps)
        {
           pos = ps;
        }

        //Movement
        public void MoveFloor()
        {
            pos.X -= 3;
            if (pos.X == -960)
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
