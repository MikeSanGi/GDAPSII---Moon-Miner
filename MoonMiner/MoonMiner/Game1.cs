using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic; // needed for Lists
using System; // needed for RNG

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
        Texture2D rockImg;
        SpriteFont font;
        Texture2D lives;

        // GameState variable
        GameState currState;

        // create keyboardState variables for the gameState switches
        KeyboardState gkState;
        KeyboardState gkStatePrev;

         //Creating attributes for difficulty (It's set on 'Easy' by default)
        double scoreModifier = .5;
        int speed = 1;
        double score = 0;
        double obstacleFrequency = 1;
        bool difficultyUp = false;
        int secondCounter;
        int tenSecondCounter;
        StreamReader reader;

        // create a list of collectibles
        List<Obstacles> obstacles;
        // int for number of obstacles
        int numObstacles;

        //create objects
        FloorObjects wall;
        FloorObjects floor;
        Player playChar;
        Obstacles rocks;


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
        
            //Initialize the difficulty
            reader = new StreamReader("../../../../../MoonMiner - External Tool/MoonMiner - External Tool/bin/Debug/difficulty.txt");
            string difficultyBase = reader.ReadLine();
            reader.Close();
            string[] difficultyExtraction = difficultyBase.Split(' ');
            int[] difficultyConverted = System.Array.ConvertAll<string, int>(difficultyExtraction, int.Parse);
            obstacleFrequency = difficultyConverted[0];
            speed = difficultyConverted[1];
            scoreModifier = difficultyConverted[2];
            
            //create character objects
            playChar = new Player(new Rectangle(1000, 300,100,100));

            //create floor objects
            wall = new FloorObjects(new Vector2(0, 0),speed);
            floor = new FloorObjects(new Vector2(0, 400), speed);

            // create obstacles
            rocks = new Obstacles(new Rectangle(500,500,30,30));
            obstacles = new List<Obstacles>();

            // set the number of obstacles
            numObstacles = 10;

            // set the initial game state
            currState = GameState.MainMenu;
            gkState = Keyboard.GetState();

            //set the obstacles to be active
            rocks.Active = true;

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
            player = Content.Load<Texture2D>("CharSpriteSheet");
            floorImg = Content.Load<Texture2D>("Floor");
            font = Content.Load<SpriteFont>("Arial");
            rockImg = Content.Load<Texture2D>("boxChar");
            lives = Content.Load<Texture2D>("TempLife");
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

            //time for Animation
            playChar.Animate(gameTime);

            //Update score based on speed and score modifier, and find new difficulty
            secondCounter++;
            tenSecondCounter++;
            if (secondCounter >= 60)
            {
                secondCounter = 0;
                score = score + speed * scoreModifier;
                tenSecondCounter++;
            }
            if (tenSecondCounter >= 600)
            {
                tenSecondCounter = 0;
                difficultyUp = true;
            }
            if (difficultyUp == true)
            {
                difficultyUp = false;
                obstacleFrequency++;
                wall.Speed++;
                floor.Speed++;
                scoreModifier = scoreModifier + .1;
            }

            // create a gameState switch to detect the game State
            switch (currState)
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

                    // loop to spawn the obstacles
                    for (int i = 0; i < obstacles.Count; i++)
                    {
                        if (obstacles[i].Active)
                        {
                            ObstacleSpawn();
                            rocks.Move();
                        }
                    }

                    //check for a collison
                    if (rocks.CheckCollision(playChar))
                    {
                        //remove a life
                        playChar.NumLives -= 1;
                        if (playChar.NumLives <= 0)
                        {
                            // switch to the gameOver state
                            currState = GameState.GameOver;
                        }
                    }



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
                        // reset the obstacles
                        Reset();
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
                    //rocks.Draw(spriteBatch);
                    //spriteBatch.Draw(rocks.Image,rocks.Pos,Color.White);

                    // loop to draw the obstacles
                    for (int i = 0; i < obstacles.Count; i++)
                    {
                        if (obstacles[i].Active)
                        {
                            spriteBatch.Draw(obstacles[i].Image, obstacles[i].Pos, Color.White);
                        }
                    }

                    spriteBatch.DrawString(font, "Score: " + score, new Vector2(10, 10), Color.White);
                    //draw lives
                    for (int i = 0; i < playChar.NumLives; i++)
                    {
                        spriteBatch.Draw(lives, new Vector2(10 + (i * 40), 20), Color.White);
                    }
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
                playChar.Pos = new Rectangle(playChar.PosX, playChar.PosY,100,100);
                playChar.PlayerJump = true;
            }

            if (playChar.PlayerJump == true)
            {
                player = Content.Load<Texture2D>("JumpCharSheet");
                playChar.Image = player;
                playChar.Jump();
            }

            // if the key is "down arrow"
            if (kState.IsKeyDown(Keys.Down) && playChar.PlayerJump == false)
            {
                // have the player duck
                player = Content.Load<Texture2D>("Char2SpriteSheet");
                playChar.Duck = true;
                playChar.Image = player;
                while (playChar.PosY <= 300 && playChar.PlayerJump == false)
                {
                    playChar.PosY += 50;
                    playChar.Pos = new Rectangle(playChar.PosX, playChar.PosY,100,50);                    
                }         
            }

            //If no keys are pressed, makes sure all states are reverted to default
            if (kState.IsKeyUp(Keys.Down) && kState.IsKeyUp(Keys.Up) && playChar.PlayerJump == false)
            {
                playChar.PosY = 300;
                playChar.Duck = false;
                playChar.Pos = new Rectangle(playChar.PosX, playChar.PosY,100,100);
                player = Content.Load<Texture2D>("CharSpriteSheet");
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

        // create a method for obstacle generation
        public void ObstacleSpawn()
        {
            // create random number generator
            Random rgen = new Random();

            // loop to create the collectibles
            for (int i = 0; i < numObstacles; i++)
            {

                // set the y values of the obstacles to random spots on the screen
                int posY = rgen.Next(0, 301);

                // create a new collectible object and make them all be the same size
                Obstacles rock = new Obstacles(new Rectangle(500,posY,30,30));

                // set the image for the game object
                rock.Image = rockImg;

                // add the collectible to the list
                obstacles.Add(rock);
            }
        }

        // create a method to reset the game objects
        public void Reset()
        {
            rocks.Active = false;
            rocks.Pos = new Rectangle(1000, 1000, 0, 0);
        }


    }
}
