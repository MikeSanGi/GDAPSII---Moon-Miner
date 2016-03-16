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
        private bool duck = false;


        //animation attributes
        int frame;
        double timePerFrame = 50;
        int numFrames = 3;
        int framesElapsed;
        const int CART_Y = 0;
        const int CART_HEIGHT = 100;
        const int CART_WIDTH = 100;
        const int CART_X_OFFSET = 0;

        //animation method
        public void Animate(GameTime gameTime)
        {
            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frame = framesElapsed % numFrames;
        }

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
        public bool Duck
        {
            get { return duck; }
            set { duck = value; }
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
            if(Duck == true)
            {
                spriteBatch.Draw(image, Pos, new Rectangle(CART_X_OFFSET + frame * CART_WIDTH, CART_Y, CART_WIDTH, 50), Color.White);
            }
            spriteBatch.Draw(image, Pos, new Rectangle(CART_X_OFFSET + frame * CART_WIDTH, CART_Y, CART_WIDTH, CART_HEIGHT), Color.White);
        }
    }
}
