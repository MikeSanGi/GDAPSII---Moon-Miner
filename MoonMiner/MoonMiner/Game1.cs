using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MoonMiner
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;        
        Vector2 cavePos;
        Texture2D player;
        Vector2 playerPos;
        Texture2D floor;
        Vector2 floorPos;

        // jump/gravity attempt
        int baseY = 300;
        float vsp = -20;        
        float grav = 1F;       
        Boolean playerJump = false;
        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            cavePos = new Vector2(0, 0);
            playerPos = new Vector2(100, 300);
            floorPos = new Vector2(0, 400);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            background = Content.Load<Texture2D>("Background2");
            player = Content.Load<Texture2D>("boxChar1");
            floor = Content.Load<Texture2D>("Floor");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
            cavePos.X -= 3;
            if(cavePos.X == -960)
            {
                cavePos.X = 0;
            }
            floorPos.X -= 3;
            if (floorPos.X == -960)
            {
                floorPos.X = 0;
            }

            // call the process input method
            ProcessInput();
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            spriteBatch.Draw(background,cavePos,Color.White);
            spriteBatch.Draw(player, playerPos, Color.White);
            spriteBatch.Draw(floor, floorPos, Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // create a method named process input
        public void ProcessInput()
        {
            // create a local keyboard state variable
            KeyboardState kState;
            KeyboardState prevKState;
            Boolean ducking = false;
            
            // store the state of the keyboard in the variable
            kState = Keyboard.GetState();
        

            // if the key is "up arrow"
            if (kState.IsKeyDown(Keys.Up))
            {
                // have the player jump
               // playerPos.Y = 300;
                playerJump = true;
                
            }
            if (playerJump == true)
            {
                Jump();
            }
            /*if(kState.IsKeyUp(Keys.Up))
            {
                playerPos.Y = 300;
                player = Content.Load<Texture2D>("boxChar");
            }*/

            // if the key is "down arrow"
            if (kState.IsKeyDown(Keys.Down))
            {
                // have the player duck
                player = Content.Load<Texture2D>("boxChar2");              
                while(playerPos.Y <= 300)
                {
                    playerPos.Y += 50;
                }
            }
            /*if (kState.IsKeyUp(Keys.Down))
            {
                playerPos.Y = 300;
                player = Content.Load<Texture2D>("boxChar");
            }*/
            prevKState = kState;

        }

        public void Jump()
        {

            playerJump = true;            
            vsp += grav;
            playerPos.Y += vsp;           
            if(playerPos.Y == baseY)
            {
                playerJump = false;
                vsp = -20;                
            }                       
        }
    }
}
