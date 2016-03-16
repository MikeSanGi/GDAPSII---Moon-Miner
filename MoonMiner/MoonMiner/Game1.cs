using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;

namespace MoonMiner
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    enum GameState { MainMenu, HowToPlay, Game, Pause, GameOver };
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;        
        Texture2D player;        
        Texture2D floorImg;
        SpriteFont font;

        // GameState variable
        GameState currState;

        // create keyboardState variables for the gameState switches
        KeyboardState gkState;
        KeyboardState gkStatePrev;

        //Creating attributes for difficulty (It's set on 'Easy' by default)
        double scoreModifier = .5;
        double speed = 1;
        double score = 0;
        double obstacleFrequency = 1;
        bool difficultyUp = false;
        int secondCounter;
        //PUT CODE IN HERE FOR STREAM READER WHEN I FIGURE IT OUT TO REPLACE DEFAULT VALUES FOR THOSE VARIABLES

        
        //create objects
        FloorObjects wall;
        FloorObjects floor;
        Player playChar;


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
        
            
            //create character objects
            playChar = new Player(new Vector2(1000, 300));

            //create floor objects
            wall = new FloorObjects(new Vector2(0, 0));
            floor = new FloorObjects(new Vector2(0, 400));

            // set the initial game state
            currState = GameState.MainMenu;
            gkState = Keyboard.GetState();


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


            // TODO: use this.Content to load your game content here
            background = Content.Load<Texture2D>("Background2");
            player = Content.Load<Texture2D>("boxChar1");
            floorImg = Content.Load<Texture2D>("Floor");
            font = Content.Load<SpriteFont>("Arial");

            //load images into floor objects
            wall.Image = background;
            floor.Image = floorImg;

            //load image to player object
            playChar.Image = player;
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
            /*
            //call floorobject movement
            wall.MoveFloor();
            floor.MoveFloor();
            
            //Update score based on speed and score modifier
            secondCounter++;
            if (secondCounter >= 60)
            {
                secondCounter = 0;
                score = score + speed * scoreModifier;
            }
            */

            // create a gameState switch to detect the game State
            switch(currState)
            {
                case GameState.MainMenu: if (SingleKeyPress(Keys.Enter))
                    {
                        // switch the state to HowToPlay
                        currState = GameState.HowToPlay;
                    }
                        break;
                case GameState.HowToPlay:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        // switch the state to the Game
                        currState = GameState.Game;
                    }
                    break;
                case GameState.Game:
                    //call floorobject movement
                    wall.MoveFloor();
                    floor.MoveFloor();
                    //Update score based on speed and score modifier
                    secondCounter++;
                    if (secondCounter >= 60)
                    {
                        secondCounter = 0;
                        score = score + speed * scoreModifier;
                    }
                    // call the process input method
                    ProcessInput();
                    if (SingleKeyPress(Keys.Enter))
                    {
                        // switch the state to Pause
                        currState = GameState.Pause;
                    }
                    break;
                case GameState.Pause:

                    if (SingleKeyPress(Keys.Enter))
                    {
                        // switch the state back to the game
                        currState = GameState.Game;
                    }
                    break;
                case GameState.GameOver:
                    if (SingleKeyPress(Keys.Enter))
                    {
                        // return back to the menu
                        currState = GameState.MainMenu;
                    }
                    break;
            }

            // set the previous gamestate variable to the current one
            gkStatePrev = gkState;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();



            // create a gameState switch to change what appears on screen at a specific time
            switch(currState)
            {
                case GameState.MainMenu:
                    spriteBatch.DrawString(font, "MOON MINER TEST TITLE SCREEN", new Vector2(300,300), Color.White);
                    break;
                case GameState.HowToPlay:
                    spriteBatch.DrawString(font, "HOW TO PLAY", new Vector2(300, 300), Color.White);
                    spriteBatch.DrawString(font, "Press up arrow to jump and down to duck. That is all. Press enter to begin.", new Vector2(100, 400), Color.White);
                    break;
                case GameState.Game:
                    wall.Draw(spriteBatch);
                    floor.Draw(spriteBatch);
                    playChar.Draw(spriteBatch);
                    spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);
                    break;
                case GameState.Pause:
                    spriteBatch.DrawString(font, "GAME IS PAUSED", new Vector2(300, 300), Color.White);
                    break;
                case GameState.GameOver:
                    spriteBatch.DrawString(font, "GAME OVER", new Vector2(300, 300), Color.White);
                    break;
            }

            // set the previous mouse state to the current mouse state
            gkStatePrev = Keyboard.GetState();

            spriteBatch.End();

            base.Draw(gameTime);
        }

        // create a method named process input
        public void ProcessInput()
        {
            // create a local keyboard state variable
            KeyboardState kState;
            KeyboardState prevKState;
         

            // store the state of the keyboard in the variable
            kState = Keyboard.GetState();


            // if the key is "up arrow"
           if (kState.IsKeyDown(Keys.Up) && playChar.PlayerJump == false)
            {
                // have the player jump
                playChar.PosY = 300;
                playChar.Pos = new Vector2(playChar.PosX, playChar.PosY);
                playChar.PlayerJump = true;
            }

            if (playChar.PlayerJump == true)
            {
                player = Content.Load<Texture2D>("BoxChar1");
                playChar.Image = player;
                playChar.Jump();
            }

            // if the key is "down arrow"
            if (kState.IsKeyDown(Keys.Down) && playChar.PlayerJump == false)
            {
                // have the player duck
                player = Content.Load<Texture2D>("boxChar2");
                playChar.Image = player;
                while (playChar.PosY <= 300 && playChar.PlayerJump == false)
                {
                    playChar.PosY += 50;
                    playChar.Pos = new Vector2(playChar.PosX, playChar.PosY);
                }         
            }

            //If no keys are pressed, makes sure all states are reverted to default
            if (kState.IsKeyUp(Keys.Down) && kState.IsKeyUp(Keys.Up) && playChar.PlayerJump == false)
            {
                playChar.PosY = 300;
                playChar.Pos = new Vector2(playChar.PosX, playChar.PosY);
                player = Content.Load<Texture2D>("BoxChar1");
                playChar.Image = player;
            }

            prevKState = kState;

        }

        // create a method called SingleKeyPress
        public bool SingleKeyPress(Keys key)
        {
            // get the keyboard state variable
            gkState = Keyboard.GetState();
            if (gkState.IsKeyDown(key) && gkStatePrev.IsKeyUp(key))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


    }
}
