using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;


namespace Whack_a_mole
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GameState currentGameState;

        Mole[,] moleArray;
        StaticArt[,] holeArray;
        StaticArt[,] foregroundArray;

        MouseState mouseState, oldMouseState;
        Point mousePos;

        List<StaticArt> lifeList;
        
        Texture2D livesTex;
        SpriteFont uiFont;
        StaticArt life;
        Vector2 scorePos;
        Vector2 timeLeftPos;
        int lives;
        int score;
        int lifePosOffset;
        double timeLeft;

        Random rnd = new Random();

        Point chosenMoleInd;

        Texture2D moleTex, moleKoTex, malletTex, backgroundTex, holeTex, holeForegroundTex;
        int moleYVelocity;
        const int pixelBuffer = 15;
        double timeSinceLastMole;
        double timeBetweenMoles = 1000;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 650;
            graphics.PreferredBackBufferWidth = 650;
            graphics.ApplyChanges();
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

            livesTex = Content.Load<Texture2D>("life");
            moleTex = Content.Load<Texture2D>("mole");
            moleKoTex = Content.Load<Texture2D>("mole_KO");
            holeTex = Content.Load<Texture2D>("hole");
            holeForegroundTex = Content.Load<Texture2D>("hole_foreground");
            malletTex = Content.Load<Texture2D>("mallet");
            backgroundTex = Content.Load<Texture2D>("background");
            uiFont = Content.Load<SpriteFont>("uiFont");

            currentGameState = GameState.Start;
            moleYVelocity = 0;

            score = 0;
            lifePosOffset = 10;
            lives = 3;
            timeLeft = 60;

            // life = new StaticArt(livesTex, new Vector2(Window.ClientBounds.Width - (livesTex.Width * 6), 20));
            scorePos = new Vector2(20, 20);
            timeLeftPos = new Vector2(Window.ClientBounds.Width / 2 - 20, 20);

            lifeList = new List<StaticArt>();

            for (int i= 0; i < lives; i++)
            {
                life = new StaticArt(livesTex, new Vector2(Window.ClientBounds.Width - (i * livesTex.Width) - lifePosOffset, 20));
                
                lifeList.Add(life);
            }

            moleArray = new Mole[3, 3];

            for (int i = 0; i < moleArray.GetLength(1); i++)
            {
                for (int j = 0; j < moleArray.GetLength(0); j++)
                {
                    moleArray[i, j] = new Mole(moleTex, new Vector2((i * Window.ClientBounds.Width / 3) + pixelBuffer, (j * Window.ClientBounds.Height / 3.5f) + moleTex.Height / 1.5f), moleYVelocity, moleKoTex);
                }
            }

            holeArray = new StaticArt[3, 3];

            for (int i = 0; i < holeArray.GetLength(1); i++)
            {
                for (int j = 0; j < holeArray.GetLength(0); j++)
                {
                    holeArray[i, j] = new StaticArt(holeTex, new Vector2((i * Window.ClientBounds.Width / 3) + pixelBuffer, (j * Window.ClientBounds.Height / 3.5f) + holeTex.Height / 1.5f));
                }

            }

            foregroundArray = new StaticArt[3, 3];

            for (int i = 0; i < foregroundArray.GetLength(1); i++)
            {
                for (int j = 0; j < foregroundArray.GetLength(0); j++)
                {
                    foregroundArray[i, j] = new StaticArt(holeForegroundTex, new Vector2((i * Window.ClientBounds.Width / 3) + pixelBuffer, (j * Window.ClientBounds.Height / 3.5f) + holeForegroundTex.Height / 1.5f));
                }
            }



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
            // IsMouseVisible = true;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (currentGameState)
            {
                case GameState.Start:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        resetGame();
                        currentGameState = GameState.Playing;
                    }
                    break;
                case GameState.Playing:

                    timeLeft = timeLeft - gameTime.ElapsedGameTime.TotalSeconds;
                    
                    oldMouseState = mouseState;
                    mouseState = Mouse.GetState();
                    mousePos = new Point(mouseState.X, mouseState.Y);

                    if (lives <= 0)
                    {
                        currentGameState = GameState.Lose;
                    }

                    if (timeLeft <= 0 && lives > 0)
                    {
                        currentGameState = GameState.Win;
                    }
                    

                    foreach (Mole mole in moleArray)
                    {
                        // if (mole.GetPos().Y <= )  MAYBE MOVE UPDATE LOGIC FROM MOLE TO HERE TO ALLOW FOR -= LIVES
                        
                        if (mole.hitBox.Contains(mousePos) && mouseState.LeftButton == ButtonState.Pressed && mole.IsActive())
                        {
                            mole.SetTexture(moleKoTex);
                            mole.SetVelocity(-10);
                            mole.Deactivate();                            
                            score += 10;
                        }
                        

                        mole.Update(gameTime);
                    }

                    // timeBetweenMoles = rnd.Next(500, 2000);
                    timeSinceLastMole += gameTime.ElapsedGameTime.TotalMilliseconds;
                    if (timeSinceLastMole >= timeBetweenMoles)
                    {
                        ActivateRndMole();
                        timeSinceLastMole = 0;
                        timeBetweenMoles = rnd.Next(500, 1500);
                    }
                    break;

                case GameState.Lose:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        resetGame();
                        currentGameState = GameState.Playing;
                    }
                    break;
                case GameState.Win:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        resetGame();
                        currentGameState = GameState.Playing;
                    }
                    break;
                    
            }

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            // spelet ska ritas uppifrån och ner i rätt ordning. Bilderna ska överlappa varandra för att skapa önskad effekt.

            GraphicsDevice.Clear(new Color(111, 209, 72)); // önskad bakgrundsfärg


            spriteBatch.Begin();
            spriteBatch.Draw(backgroundTex, new Vector2(0, 0), Color.White);

            switch (currentGameState)
            {
                case GameState.Start:
                    spriteBatch.DrawString(uiFont, "Welcome to Guac-a-mole.\nPress Enter to start playing.\nPress Esc at any time to quit", new Vector2(Window.ClientBounds.Width / 12, Window.ClientBounds.Height / 3), Color.CornflowerBlue);
                    // draw start manu and animation here
                    break;
                case GameState.Playing:
                    
                    for (int i = 0; i < 3; i++)
                    {
                        DrawRow(spriteBatch, i);
                    }

                    spriteBatch.Draw(malletTex, new Vector2(mousePos.X - malletTex.Width / 6, mousePos.Y - 2 * malletTex.Height / 3), Color.White);

                  /*  foreach (StaticArt life in lifeList)
                    {
                        life.Draw(spriteBatch);
                    } */
                    
                    spriteBatch.DrawString(uiFont, "Score: " + score, scorePos, Color.Black);
                    spriteBatch.DrawString(uiFont, "" + (int)timeLeft, timeLeftPos, Color.Black);
                    
                    break;
                case GameState.Win:
                    spriteBatch.DrawString(uiFont, "You won! Great job!\nYour score was: " + score + " points.\nPress Enter to play again.", new Vector2(Window.ClientBounds.Width / 12, Window.ClientBounds.Height / 3), Color.CornflowerBlue);
                    // draw win menu here
                    break;
                case GameState.Lose:
                    spriteBatch.DrawString(uiFont, "You lost! No sauce for you!\nYour score was: " + score + " points.\nPress Enter to play again.", new Vector2(Window.ClientBounds.Width / 12, Window.ClientBounds.Height / 3), Color.CornflowerBlue);
                    // draw lose menu here
                    break;
            }

            

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void DrawRow(SpriteBatch sb, int row)
        {
            for (int i = 0; i < 3; i++)
            {
                holeArray[i, row].Draw(spriteBatch);
                moleArray[i, row].Draw(spriteBatch);
                foregroundArray[i, row].Draw(spriteBatch);
            }


        }

        public void ActivateRndMole()
        {
            Random rand = new Random();
            int rndMoleSpeed = rand.Next(2, 4);
            chosenMoleInd = new Point(rand.Next(0, 3), rand.Next(0, 3));

            while (moleArray[chosenMoleInd.X, chosenMoleInd.Y].IsActive())
            {
                chosenMoleInd = new Point(rnd.Next(0, 2), rnd.Next(0, 2));
            }
            moleArray[chosenMoleInd.X, chosenMoleInd.Y].Activate(rndMoleSpeed);

        }

        public void resetGame()
        {
            score = 0;
            timeLeft = 60;
            lives = 3;
            timeSinceLastMole = 0;
            foreach (Mole mole in moleArray)
            {
                mole.ResetPosition();
            }
        }


        enum GameState
        {
            Start,
            Playing,
            Win,
            Lose,
        }
    }
}
