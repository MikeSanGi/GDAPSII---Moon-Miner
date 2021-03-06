﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Collections.Generic; // needed for Lists
using System; // needed for RNG
using Microsoft.Xna.Framework.Audio; //needed for sound

namespace MoonMinerExecutable
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    enum GameState { MainMenu, HowToPlay, NameEntry, Game, Pause, GameOver, Highscore };
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
        Texture2D gems;
        Vector2 selectorPosName;
        Texture2D arrow;
        Texture2D darrow;
        Texture2D NameEntry;

        Texture2D spikes;
        Texture2D pits;
        Texture2D batSS;
        Texture2D rox;

        // create an enemy variable
        int enemy;

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
        int speedMod;
        double initialMod;
        double score = 0;
        string highscoreName;
        double obstacleFrequency = 1;
        bool difficultyUp = false;
        bool saveOnce = false;
        int secondCounter;
        int spawnCounter = 0;
        int tenSecondCounter;

        // high score attributes
        StreamReader reader;

        //Highscore
        bool highscorePrint = false;
        bool entry = true;
        String newScore;
        string name;
        char[] alphabet = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        int let1 = 0; int let2 = 0; int let3 = 0;
        ScoreLinkedList sll = new ScoreLinkedList();

        // create a list of collectibles
        List<Obstacles> obstacles;

        // int for number of obstacles
        int numObstacles;
        int gracePeriod;

        //variables for sound
        SoundEffect jump;
        SoundEffect collect;
        SoundEffect hit;
        SoundEffect menuSound;
        SoundEffect select;
        SoundEffect lose;
        SoundEffect life;

        //create objects
        FloorObjects wall;
        FloorObjects floor;
        Player playChar;
        Obstacles rocks;

        public int ScoreNum
        {
            get { return scoreNum; }
            set { scoreNum = value; }
        }

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


                StreamReader sr = new StreamReader("highscore.txt");
                StreamReader nr = new StreamReader("highscoreNames.txt");
                string nameIn;
                int scoreIn;
                while((nameIn = nr.ReadLine()) != null)
                {
                    string scoreStr = sr.ReadLine();
                    int.TryParse(scoreStr, out scoreIn);
                    sll.Add(nameIn, scoreIn);
                }
            nr.Close();
            sr.Close();

            speedMod = speed;
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

            // set the gracePeriod
            gracePeriod = 0;

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
            selectorPosName.X = 225;
            selectorPosName.Y = 200;

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
            rockImg = Content.Load<Texture2D>("rockSS");
            lives = Content.Load<Texture2D>("TempLife");
            menu = Content.Load<Texture2D>("Main Menu");
            instruct = Content.Load<Texture2D>("Instructions");
            menuSelector = Content.Load<Texture2D>("SelectorTool");
            pause = Content.Load<Texture2D>("Pause");
            gameover = Content.Load<Texture2D>("GameOver");
            battery = Content.Load<Texture2D>("battery");
            highscore = Content.Load<Texture2D>("Highscore");
            gems = Content.Load<Texture2D>("gemSpriteSheet");
            arrow = Content.Load<Texture2D>("Arrow");
            darrow = Content.Load<Texture2D>("downarrow");
            NameEntry = Content.Load<Texture2D>("NameEntry");

            pits = Content.Load<Texture2D>("Pit");
            spikes = Content.Load<Texture2D>("Spike");
            batSS = Content.Load<Texture2D>("batSpriteSheets");

            //Sounds 
            jump = Content.Load<SoundEffect>("jump1");
            menuSound = Content.Load<SoundEffect>("menu");
            select = Content.Load<SoundEffect>("select");
            collect = Content.Load<SoundEffect>("collect2");
            lose = Content.Load<SoundEffect>("lose");
            hit = Content.Load<SoundEffect>("jump2");
            life = Content.Load<SoundEffect>("collect");

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
                        menuSound.Play();
                    }
                    if (SingleKeyPress(Keys.Left))
                    {
                        selectorPos.X = 150;
                        selectorPos.Y = 275;
                        menuSound.Play();
                    }
                    if (SingleKeyPress(Keys.Up))
                    {
                        selectorPos.X = 150;
                        selectorPos.Y = 275;
                        menuSound.Play();
                    }
                    if (SingleKeyPress(Keys.Down))
                    {
                        selectorPos.X = 310;
                        selectorPos.Y = 367;
                        menuSound.Play();
                    }
                    if (selectorPos.X == 150)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.NameEntry;
                            select.Play();
                        }
                    }
                    if (selectorPos.X == 310)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.Highscore;
                            select.Play();
                        }
                    }
                    if (selectorPos.X == 360)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.HowToPlay;
                            select.Play();
                        }
                    }
                        break;
                case GameState.HowToPlay:
                    if (SingleKeyPress(Keys.Left))
                    {
                        selectorPosInstruct.X = 157;
                        selectorPosInstruct.Y = 390;
                        menuSound.Play();
                    }
                    if (SingleKeyPress(Keys.Right))
                    {
                        selectorPosInstruct.X = 455;
                        selectorPosInstruct.Y = 390;
                        menuSound.Play();
                    }
                    if (selectorPosInstruct.X == 157)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.MainMenu;
                            select.Play();
                        }
                    }
                    if (selectorPosInstruct.X == 455)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.NameEntry;
                            select.Play();
                        }
                    }
                    break;
                case GameState.NameEntry:

                    //Selector for first letter, upper arrow
                    if (selectorPosName.X == 225 && selectorPosName.Y == 200)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            if (let1 == 25)
                            {
                                let1 = 0;
                            }
                            else
                            {
                                let1++;
                            }
                        }
                        if (SingleKeyPress(Keys.Right))
                        {
                            selectorPosName.X = 325;
                        }
                        if (SingleKeyPress(Keys.Down))
                        {
                            selectorPosName.Y = 300;
                        }
                    }

                    //selector for second letter, upper portion
                    if (selectorPosName.X == 325 && selectorPosName.Y == 200)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            if (let2 == 25)
                            {
                                let2 = 0;
                            }
                            else
                            {
                                let2++;
                            }
                        }
                        if (SingleKeyPress(Keys.Right))
                        {
                            selectorPosName.X = 425;
                        }
                        if (SingleKeyPress(Keys.Down))
                        {
                            selectorPosName.Y = 300;
                        }
                        if (SingleKeyPress(Keys.Left))
                        {
                            selectorPosName.X = 225;
                        }
                    }

                    //selector for third letter, upper portion
                    if (selectorPosName.X == 425 && selectorPosName.Y == 200)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            if (let3 == 25)
                            {
                                let3 = 0;
                            }
                            else
                            {
                                let3++;
                            }
                        }
                        if (SingleKeyPress(Keys.Right))
                        {
                            selectorPosName.X = 425;
                        }
                        if (SingleKeyPress(Keys.Down))
                        {
                            selectorPosName.Y = 300;
                        }
                        if (SingleKeyPress(Keys.Left))
                        {
                            selectorPosName.X = 325;
                        }
                    }

                    //selector for third letter, lower portion
                    if (selectorPosName.X == 425 && selectorPosName.Y == 300)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            if (let3 == 0)
                            {
                                let3 = 25;
                            }
                            else
                            {
                                let3--;
                            }
                        }
                        if (SingleKeyPress(Keys.Left))
                        {
                            selectorPosName.X = 325;
                        }
                        if (SingleKeyPress(Keys.Up))
                        {
                            selectorPosName.Y = 200;
                        }
                        if (SingleKeyPress(Keys.Down))
                        {
                            selectorPosName.Y = 375;
                            selectorPosName.X = 325;
                        }
                    }

                    //selector for second letter, lower potion
                    if (selectorPosName.X == 325 && selectorPosName.Y == 300)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            if (let2 == 0)
                            {
                                let2 = 25;
                            }
                            else
                            {
                                let2--;
                            }
                        }
                        if (SingleKeyPress(Keys.Left))
                        {
                            selectorPosName.X = 225;
                        }
                        if (SingleKeyPress(Keys.Up))
                        {
                            selectorPosName.Y = 200;
                        }
                        if (SingleKeyPress(Keys.Down))
                        {
                             selectorPosName.Y = 375;
                            selectorPosName.X = 325;
                        }
                        if (SingleKeyPress(Keys.Right))
                        {
                            selectorPosName.X = 425;
                        }
                    }

                    //selector for first letter, lower portion
                    if (selectorPosName.X == 225 && selectorPosName.Y == 300)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            if (let1 == 0)
                            {
                                let1 = 25;
                            }
                            else
                            {
                                let1--;
                            }
                        }
                        if (SingleKeyPress(Keys.Up))
                        {
                            selectorPosName.Y = 200;
                        }
                        if (SingleKeyPress(Keys.Down))
                        {
                             selectorPosName.Y = 375;
                            selectorPosName.X = 325;
                        }
                        if (SingleKeyPress(Keys.Right))
                        {
                            selectorPosName.X = 325;
                        }
                    }

                    //proceed selection, saves name
                    if (selectorPosName.Y == 375)
                    {
                        if(SingleKeyPress(Keys.Up))
                        {
                            selectorPosName.Y = 300;
                            selectorPosName.X = 425;
                        }
                        if (SingleKeyPress(Keys.Enter))
                        {
                            name = (alphabet[let1].ToString()) + (alphabet[let2].ToString()) + (alphabet[let3].ToString());
                            currState = GameState.Game;
                        }

                    }
                    break;
                case GameState.Game:
                    //call floorobject movement
                    wall.MoveFloor();
                    floor.MoveFloor();

                    // create the random object here
                    Random rgen = new Random();
                    enemy = rgen.Next(1, 5);

                    spawnCounter++;
                    if(spawnCounter > 50-speedMod)
                    {
                        spawnCounter = 0;
                    }
                    if (spawnCounter == 50-speedMod)
                    {
                        gracePeriod++;
                        ObstacleSpawn();
                        GemSpawn();
                    }

                    if (gracePeriod >= 10)
                    {
                        gracePeriod = 0;
                        difficultyUp = true;
                        spawnCounter = -50;
                    }

                        foreach (Obstacles stuff in obstacles)
                    {
                        stuff.Move();
                        stuff.Animate(gameTime);
                    }

                   
                    //check for a collison
                    foreach (Obstacles rock in obstacles)
                    {
                        rock.CheckCollision(playChar, this);                  
                    }

                    //gameover state
                    if (playChar.NumLives <= 0)
                    {
                        // switch to the gameOver state
                        currState = GameState.GameOver;
                        lose.Play();
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
                        select.Play();
                    }
                    break;
                case GameState.Pause:

                    if (SingleKeyPress(Keys.Enter))
                    {
                        // switch the state back to the game
                        currState = GameState.Game;
                        select.Play();
                    }
                    break;
                case GameState.GameOver:
                 
                    if (SingleKeyPress(Keys.Left))
                    {
                        selectorPosOver.X = 135;
                        selectorPosOver.Y = 382;
                        menuSound.Play();
                    }
                    if (SingleKeyPress(Keys.Right))
                    {
                        selectorPosOver.X = 410;
                        selectorPosOver.Y = 382;
                        menuSound.Play();
                    }
                    if (selectorPosOver.X == 135)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.MainMenu;
                            select.Play();
                            // reset the obstacles
                            Reset();
                        }
                    }
                    if (selectorPosOver.X == 410)
                    {
                        if (SingleKeyPress(Keys.Enter))
                        {
                            currState = GameState.Game;
                            select.Play();
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
                            select.Play();
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
                case GameState.NameEntry:
                    spriteBatch.Draw(NameEntry, menuPos, Color.White);

                    //Draws selection for first initial in Highscore entry
                    spriteBatch.Draw(arrow, new Rectangle(275, 200, 30, 30), Color.White);
                    spriteBatch.DrawString(font, alphabet[let1].ToString(), new Vector2(280, 250), Color.White);
                    spriteBatch.Draw(darrow, new Rectangle(275, 300, 30, 30), Color.White);

                    //Draws selection for second initial in Highscore entry
                    spriteBatch.Draw(arrow, new Rectangle(375, 200, 30, 30), Color.White);
                    spriteBatch.DrawString(font, alphabet[let2].ToString(), new Vector2(380, 250), Color.White);
                    spriteBatch.Draw(darrow, new Rectangle(375, 300, 30, 30), Color.White);

                    //Draws selection for third initial in Highscore entry
                    spriteBatch.Draw(arrow, new Rectangle(475, 200, 30, 30), Color.White);
                    spriteBatch.DrawString(font, alphabet[let3].ToString(), new Vector2(480, 250), Color.White);
                    spriteBatch.Draw(darrow, new Rectangle(475, 300, 30, 30), Color.White);

                    spriteBatch.Draw(menuSelector, selectorPosName, Color.White);

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
                            rock.Draw(spriteBatch);
                        }
                    }

                    //Draws Score to the screen
                    spriteBatch.DrawString(font, "Score: " + scoreNum, new Vector2(12, 12), Color.Black);
                    spriteBatch.DrawString(font, "Score: " + scoreNum, new Vector2(10, 10), Color.White);
                    
                    //draws Battery to house lives
                    spriteBatch.Draw(battery, new Vector2(12, 417), Color.Black);
                    spriteBatch.Draw(battery, new Vector2(10, 415), Color.White);

                    //draws current players name
                    spriteBatch.DrawString(font, name, new Vector2(702, 12), Color.Black);
                    spriteBatch.DrawString(font, name, new Vector2(700, 10), Color.White);
                      
                    //draw lives
                   for (int i = 0; i < playChar.NumLives; i++)
                    {
                        if(playChar.NumLives == 3)
                        spriteBatch.Draw(lives, new Vector2(19 + ((i * 35)), 425), Color.LimeGreen);

                        if (playChar.NumLives == 2)
                            spriteBatch.Draw(lives, new Vector2(19 + ((i * 35)), 425), Color.Yellow);

                        if (playChar.NumLives == 1)
                            spriteBatch.Draw(lives, new Vector2(19 + ((i * 35)), 425), Color.Red);
                    }
                    break;
                case GameState.Pause:
                    spriteBatch.Draw(pause, menuPos, Color.White);
                    break;
                case GameState.GameOver:
                    if (!saveOnce)
                    {
                        sll.Add(name, scoreNum);
                        sll.SaveScores();
                        sll.SaveNames();
                        saveOnce = true;
                    }

                    spriteBatch.Draw(gameover, menuPos, Color.White);
                    spriteBatch.DrawString(font, "Final Score: " + scoreNum, new Vector2(302, 252), Color.Black);
                    spriteBatch.DrawString(font, "Final Score: " + scoreNum, new Vector2(300, 250), Color.White);
                    spriteBatch.Draw(menuSelector, selectorPosOver, Color.White);
                    break;
                case GameState.Highscore:
                    spriteBatch.Draw(highscore, menuPos, Color.White);
                    spriteBatch.DrawString(font, sll.PrintList(), new Vector2(204, 214), Color.Black);
                    spriteBatch.DrawString(font, sll.PrintList(), new Vector2(202, 212), Color.White);

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
                jump.Play();
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
                gkStatePrev = gkState;
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
            //Random rgen = new Random();

            //int enemy = rgen.Next(1, 3);

            switch (enemy)
            {
                case 1:
                    {
                        //create a bat
                        Bat bat = new Bat(speedMod, hit);
                        // set the image for the game object
                        bat.Image = batSS;
                        bat.Pos = new Rectangle(2000, 300, 40, 30);
                        bat.ObjColor = Color.HotPink;
                        //add to list
                        obstacles.Add(bat);

                        // create a second bat
                        Bat bat2 = new Bat(speedMod, hit);
                        bat2.Image = batSS;
                        bat2.ObjColor = Color.HotPink;
                        obstacles.Add(bat2);

                        break;
                    }
                case 2:
                    {
                        Rocks rock = new Rocks(speedMod, hit);
                        rock.Image = rockImg;
                        obstacles.Add(rock);
                        break;
                    }
                case 3:
                    {
                        Spike rock = new Spike(speedMod, hit);
                        rock.Image = spikes;
                        rock.ObjColor = Color.Red;
                        obstacles.Add(rock);
                        break;
                    }
                case 4:
                    {
                        Pit rock = new Pit(speedMod, hit);
                        rock.Image = pits;
                        obstacles.Add(rock);
                        break;
                    }
                                    
            }
        }

        public void GemSpawn()
        {
            Random rgen = new Random();
            int chance = rgen.Next(0, 1001);
            if (chance > 900)
            {
                Collectible gem = new Collectible(speedMod, collect, life);
                gem.Image = gems;
                obstacles.Add(gem);
            }
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
            wall.Speed = speed;
            floor.Speed = speed;
            //reset modifier
            scoreModifier = initialMod;
            speedMod = speed;
            playChar.NumCol = 0;
            saveOnce = false;
        }
    }
}
