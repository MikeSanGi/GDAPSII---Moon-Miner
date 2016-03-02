﻿using Microsoft.Xna.Framework;
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

        // create attributes to hold textures
        Texture2D background;
        Vector2 cavePos;
        Texture2D player;
        Vector2 playerPos;

        // create an attribute to hold direction
        string direction = "down";

        // create jump variables
        int vSpeed = 0;
        int gravity = 0;
        int maxHeight = 0;

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
            player = Content.Load<Texture2D>("boxChar");

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

            // have the cave background loop to create parallax effect
            cavePos.X -= 1;
            if(cavePos.X == -960)
            {
                // reset background position
                cavePos.X = 0;
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

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // create a method named process input
        public void ProcessInput()
        {
            // create a local keyboard state variable
            KeyboardState kState;
            KeyboardState kStatePrev;
            // store the state of the keyboard in the variable
            kState = Keyboard.GetState();

            // if the key is "up arrow"
            if (kState.IsKeyDown(Keys.Up))
            {
                while(playerPos.Y >= 100)
                {
                    // have the player jump
                    playerPos.Y -= 100;
                }
            }
            // if the key is "down arrow"
            else if (kState.IsKeyDown(Keys.Down))
            {
                // have the player duck
                player = Content.Load<Texture2D>("boxChar2");
                while (playerPos.Y <= 300)
                {
                    playerPos.Y += 50;
                }
            }
            // if no key is being pressed reset the box's position
            else if (kState.IsKeyUp(Keys.Down) || kState.IsKeyUp(Keys.Up))
            {
                playerPos.Y = 300;
                player = Content.Load<Texture2D>("boxChar");
            }

            kStatePrev = kState;

        }
    }
}
