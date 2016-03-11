using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace MoonMiner
{
    class Player
    {
        //attributes
        private Texture2D image;
        private Vector2 pos;
        private int posY;
        private int posX = 50;      

        //jump attributes
        private int baseY = 300;
        private float vsp = -20;
        private float grav = 1F;
        private bool playerJump = false;
        private bool falling = false;


        //constructor
        public Player(Vector2 ps)
        {
            pos = ps;
        }

        //properties
        public bool PlayerJump
        {
            get { return playerJump; }
            set { playerJump = value; }
        }

        public bool Falling
        {
            get { return falling; }
            set { falling = value; }
        }

        public Texture2D Image
        {
            get { return image; }
            set { image = value; }
        }

        public int PosY
        {
            get { return posY; }
            set { posY = value; }
        }

        public int PosX
        {
            get { return posX; }            
        }

        public Vector2 Pos
        {
            get { return pos; }
            set { pos = value; }
        }

        //Jump method
        public void Jump()
        {
            {
                playerJump = true;
                vsp += grav;
                pos.Y += vsp;
                if (pos.Y <= 150)
                {
                    falling = true;
                }
                if (falling == true && pos.Y == baseY)
                {
                    falling = false;
                    playerJump = false;
                    vsp = -20;
                }
                if (pos.Y > 300)
                {
                    pos.Y = 300;
                    playerJump = false;
                }
            }
        }

        //Draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(image, pos, Color.White);
        }
    }
}
