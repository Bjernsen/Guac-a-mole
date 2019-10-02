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

        Random rnd;

        Point chosenMole;

        Texture2D moleTex, moleKoTex, malletTex, backgroundTex, holeTex, holeForegroundTex;
        int moleYVelocity;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int pixelBuffer;

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

            rnd = new Random();

            moleTex = Content.Load<Texture2D>("mole");
            moleKoTex = Content.Load<Texture2D>("mole_KO");
            holeTex = Content.Load<Texture2D>("hole");
            holeForegroundTex = Content.Load<Texture2D>("hole_foreground");
            malletTex = Content.Load<Texture2D>("mallet");
            backgroundTex = Content.Load<Texture2D>("background");

            currentGameState = GameState.Start;
            moleYVelocity = 0;
            
            pixelBuffer = 15;

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
                    foregroundArray[i, j] = new StaticArt(holeForegroundTex, new Vector2((i * Window.ClientBounds.Width / 3) + pixelBuffer, (j * Window.ClientBounds.Height / 3.5f)  + holeForegroundTex.Height / 1.5f));
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
            IsMouseVisible = true;

            switch (currentGameState)
            {
                case GameState.Start:
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentGameState = GameState.Playing;
                    }
                    break;
                case GameState.Playing:
                    break;




            }


            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                        Exit();

                    // TODO: Add your update logic here

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
            spriteBatch.Draw(backgroundTex, new Vector2(0,0), Color.White);
            for (int i = 0; i < 3; i++)
            {
                DrawAssetRow(spriteBatch, i);
            }

            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void DrawAssetRow(SpriteBatch sb, int row)
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
            chosenMole = new Point(rnd.Next(0, 2), rnd.Next(0, 2));
            while (moleArray[chosenMole.X, chosenMole.Y].IsActive())
            {
                chosenMole = new Point(rnd.Next(0, 2), rnd.Next(0, 2));
            }
            moleArray[chosenMole.X, chosenMole.Y].Activate();
           
        }

              
        enum GameState
        {
            Start,
            Playing,
            End,
        }
    }
}
