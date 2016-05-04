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
    class Obstacles
    {
        //attributes
        private Texture2D image;
        private Rectangle pos;
        private bool active;
        private int speed;
        private int spMod = 0;
        private SoundEffect sfx;

        //animation attributes
        private int frame;
        private double timePerFrame = 50;
        private int numFrames = 3;
        private int framesElapsed;
        private int yVal = 0;
        private int height = 100;
        private int width = 100;        
             
        //properties
        public bool Active
        {
            get { return active; }
            set { active = value; }
        }

        public SoundEffect SFX
        {
            get { return sfx; }
            set { sfx = value; }
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

        //animation properties
        public int NumFrames
        {
            get { return numFrames; }
            set { numFrames = value; }
        }
        public int Width
        {
            get { return width; }
            set { width = value; }
        }
        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        //constructor
        public Obstacles(int mod, SoundEffect sound)
        {            
            active = true;
            pos = new Rectangle(2000, 370, 30, 30);
            speed = 5;
            spMod = mod;
            sfx = sound;
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
                sfx.Play();
            }
            
        }

        //animation method
        public void Animate(GameTime gameTime)
        {
            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frame = framesElapsed % numFrames;
        }

        //draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            if(active == true)
            {
                spriteBatch.Draw(image, Pos, new Rectangle(frame * width, yVal, width, height), Color.White);
            }
        }
    }
}
