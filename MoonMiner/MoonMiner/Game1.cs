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
    enum GameState { MainMenu, HowToPlay, Game, Pause, GameOver, Highscore };
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background;        
        Texture2D player;        
        Texture2D floorImg;
        Texture2D rockImg;
        Texture2D battery;
        SpriteFont font;
        Texture2D lives;
        Texture2D menu;
        Vector2 menuPos;
        Texture2D menuSelector;
        Vector2 selectorPos;
        Vector2 selectorPosInstruct;
        Texture2D instruct;
        Texture2D pause;
        Texture2D gameover;
        Vector2 selectorPosOver;
        Texture2D highscore;
        Vector2 selectorPosHigh;

        // GameState variable
        GameState currState;

        // create keyboardState variables for the gameState switches
        KeyboardState gkState;
        KeyboardState gkStatePrev;

         //Creating attributes for difficulty (It's set on 'Easy' by default)
        double scoreModifier = .5;
        int scoreNum = 0;
        int initialSpeed;
        int speed = 4;
        int speedMod = 0;
        double initialMod;
        double score = 0;
        string highscoreName;
        double obstacleFrequency = 1;
        bool difficultyUp = false;
        int secondCounter;
        int spawnCounter = 0;
        int tenSecondCounter;
        StreamReader reader;

        //Highscore
        List<int> highscoreList;
        List<string> highscoreNameList;
        bool highscorePrint = false;

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
            try 
            {
                reader = new StreamReader("../../../../../MoonMiner - External Tool/MoonMiner - External Tool/bin/Debug/difficulty.txt");
                string difficultyBase = reader.ReadLine();
                reader.Close();
                string[] difficultyExtraction = difficultyBase.Split(' ');
                int[] difficultyConverted = System.Array.ConvertAll<string, int>(difficultyExtraction, int.Parse);
                obstacleFrequency = difficultyConverted[0];
                speed = difficultyConverted[1];                
                scoreModifier = difficultyConverted[2];
            }
            catch (Exception)
            {
                obstacleFrequency = 1;
                speed = 4;
                scoreModifier = 0.5; 
            }
            initialSpeed = speed;
            initialMod = scoreModifier;

            //create character objects
            playChar = new Player(new Rectangle(1000, 300,100,100));

            //create floor objects
            wall = new FloorObjects(new Vector2(0, 0),speed);
            floor = new FloorObjects(new Vector2(0, 400), speed);

            // create obstacles
            //rocks = new Obstacles(new Rectangle(500,500,30,30));
            obstacles = new List<Obstacles>();

            // set the number of obstacles
            numObstacles = 10;

            // set the initial game state
            currState = GameState.MainMenu;
            gkState = Keyboard.GetState();

            //set the obstacles to be active
           //rocks.Active = true;
           
           //Menu position
            menuPos.X = 0;
            menuPos.Y = 0;
            selectorPos.X = 150;
            selectorPos.Y = 275;
            selectorPosInstruct.X = 157;
            selectorPosInstruct.Y = 390;
            selectorPosOver.X = 135;
            selectorPosOver.Y = 382;
            selectorPosHigh.X = 135;
            selectorPosHigh.Y = 382;

            //Highscore list
            highscoreList = new List<int>();
            highscoreList.Add(0);
            highscoreNameList = new List<string>();

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
            menu = Content.Load<Texture2D>("Main Menu");
            instruct = Content.Load<Texture2D>("Instructions");
            menuSelector = Content.Load<Texture2D>("SelectorTool");
            pause = Content.Load<Texture2D>("Pause");
            gameover = Content.Load<Texture2D>("GameOver");
            battery = Content.Load<Texture2D>("battery");
            highscore = Content.Load<Texture2D>("Highscore");

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

           

            // create a gameState switch to detect the game State
            switch (currState)
            {
                case GameState.MainMenu: 
                if (SingleKeyPress(Keys.Right))
                    {
                        selectorPos.X = 360;
                        selectorPos.Y = 275;
                    }
                    if (SingleKeyPress(Keys.Left))
                    {
                        selectorPos.X = 150;
                        selectorPos.Y = 275;
                    }
                    if (SingleKeyPress(Keys.Up))
                    {
                        selectorPos.X = 150;
                        selectorPos.Y = 275;
                    }
                    if (SingleKeyPress(Keys.Down))
                    {
                        selectorPos.X = 310;
                        selectorPos.Y = 367;
                    }
                    if (selectorPos.X == 150)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.Game;
                        }
                    }
                    if (selectorPos.X == 310)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.Highscore;
                        }
                    }
                    if (selectorPos.X == 360)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.HowToPlay;
                        }
                    }
                        break;
                case GameState.HowToPlay:
                    if (SingleKeyPress(Keys.Left))
                    {
                        selectorPosInstruct.X = 157;
                        selectorPosInstruct.Y = 390;
                    }
                    if (SingleKeyPress(Keys.Right))
                    {
                        selectorPosInstruct.X = 455;
                        selectorPosInstruct.Y = 390;
                    }
                    if (selectorPosInstruct.X == 157)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.MainMenu;
                        }
                    }
                    if (selectorPosInstruct.X == 455)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.Game;
                        }
                    }
                    break;
                case GameState.Game:
                highscorePrint = false;
                    //call floorobject movement
                    wall.MoveFloor();
                    floor.MoveFloor();

                    spawnCounter++;
                    if(spawnCounter > 50)
                    {
                        spawnCounter = 0;
                    }

                    if (spawnCounter == 50)
                    {
                        ObstacleSpawn();
                    }
                    

                    foreach(Obstacles stuff in obstacles)
                    {
                        stuff.Move();
                    }

                   
                    //check for a collison
                    foreach (Obstacles rock in obstacles)
                    {
                        rock.CheckCollision(playChar);                  
                    }

                    //gameover state
                    if (playChar.NumLives <= 0)
                    {
                        // switch to the gameOver state
                        currState = GameState.GameOver;
                    }

                    //Update score based on speed and score modifier, and find new difficulty
                    secondCounter++;
                    tenSecondCounter++;
                    if (secondCounter >= 60)
                    {
                        secondCounter = 0;
                        scoreNum = Convert.ToInt32(scoreNum + speed * scoreModifier);
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
                        speedMod++;
                        scoreModifier = scoreModifier + .1;
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
                if (highscorePrint == false)
                    {
                        for (int i = 0; i < highscoreList.Count; i++)
                        {
                            if (scoreNum > highscoreList.IndexOf(i)) 
                            {
                                highscoreList.Insert(i, scoreNum);
                                break;
                            }
                        }
                        highscoreList.Sort();
                        highscorePrint = true;

                        StreamWriter output = new StreamWriter("highscore.txt");
                        foreach (int element in highscoreList)
                        {
                            output.WriteLine(element);
                        }
                        output.Close();

                        /*List<string> highscoreListStrings = new List<string>();
                        for (int i = 0; i < highscoreList.Count; i++)
                        {
                            highscoreListStrings[i] = Convert.ToString(highscoreList[i]);
                        }
                        System.IO.File.WriteAllLines("highscore.txt", highscoreListStrings);
                        */

                    }
                    if (SingleKeyPress(Keys.Left))
                    {
                        selectorPosOver.X = 135;
                        selectorPosOver.Y = 382;
                    }
                    if (SingleKeyPress(Keys.Right))
                    {
                        selectorPosOver.X = 410;
                        selectorPosOver.Y = 382;
                    }
                    if (selectorPosOver.X == 135)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.MainMenu;
                            // reset the obstacles
                            Reset();
                        }
                    }
                    if (selectorPosOver.X == 410)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.Game;
                            // reset the obstacles
                            Reset();
                        }
                    }
                    break;
                    case GameState.Highscore:
                    if (selectorPosHigh.X == 135)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.MainMenu;
                        }
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
                    spriteBatch.Draw(menu, menuPos, Color.White);
                    spriteBatch.Draw(menuSelector, selectorPos, Color.White);
                    break;
                case GameState.HowToPlay:
                    spriteBatch.Draw(instruct, menuPos, Color.White);
                    spriteBatch.Draw(menuSelector, selectorPosInstruct, Color.White);
                    break;
                case GameState.Game:
                    wall.Draw(spriteBatch);
                    playChar.Draw(spriteBatch);
                    floor.Draw(spriteBatch);
                    //rocks.Draw(spriteBatch);
                    //spriteBatch.Draw(rocks.Image,rocks.Pos,Color.White);

                    // loop to draw the obstacles
                    foreach(Obstacles rock in obstacles)
                    {
                        if (rock.Active)
                        {
                            spriteBatch.Draw(rock.Image, rock.Pos, Color.White);
                        }
                    }

                    //Draws Score to the screen
                    spriteBatch.DrawString(font, "Score: " + scoreNum, new Vector2(12, 12), Color.Black);
                    spriteBatch.DrawString(font, "Score: " + scoreNum, new Vector2(10, 10), Color.White);
                    
                    //draws Battery to house lives
                    spriteBatch.Draw(battery, new Vector2(12, 417), Color.Black);
                    spriteBatch.Draw(battery, new Vector2(10, 415), Color.White);
                    
                    //draw lives
                    for (int i = 0; i < playChar.NumLives; i++)
                    {
                        spriteBatch.Draw(lives, new Vector2(19 + ((i * 35)), 425), Color.White);
                    }
                    break;
                case GameState.Pause:
                    spriteBatch.Draw(pause, menuPos, Color.White);
                    break;
                case GameState.GameOver:
                    spriteBatch.Draw(gameover, menuPos, Color.White);
                    spriteBatch.Draw(menuSelector, selectorPosOver, Color.White);
                    break;
                case GameState.Highscore:
                    spriteBatch.Draw(highscore, menuPos, Color.White);
                    spriteBatch.Draw(menuSelector, selectorPosHigh, Color.White);
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
                while (playChar.PosY <= 300 && playChar.PlayerJump == false && playChar.Duck == true)
                {
                    playChar.PosY += 50;
                    playChar.Pos = new Rectangle(playChar.PosX, playChar.PosY,100,100);                    
                }
                playChar.Duck = false;        
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

            int enemy = rgen.Next(1, 3);

            switch(enemy)
            {
                case 1:
                    {
                        //create a bat
                        Bat rock = new Bat(speedMod);
                        // set the image for the game object
                        rock.Image = rockImg;
                        //add to list
                        obstacles.Add(rock);
                        break;
                    }
                case 2:
                    {
                        Rocks rock = new Rocks(speedMod);
                        rock.Image = rockImg;
                        obstacles.Add(rock);
                        break;
                    }
            }

                // add the collectible to the list
           
            
        }

        // create a method to reset the game objects
        public void Reset()
        {
            obstacles.Clear();
            playChar.NumLives = 3;
            //reset score
            scoreNum = 0;
            //reset speed
            speed = initialSpeed;
            //reset modifier
            scoreModifier = initialMod;
            speedMod = 0;
           
        }


    }
}
