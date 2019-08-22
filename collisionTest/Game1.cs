using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
namespace collisionTest
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        Texture2D personTexture;
        Texture2D blockTexture;
        SpriteBatch spriteBatch;
        bool personHit = false;

        Vector2 personPosition;
        const int PersonMoveSpeed = 5;
        List<Vector2> blockPositions = new List<Vector2>();
        float BlockSpawnProbability = 0.01f;
        const int BlockFallSpeed = 2;
        //const int BlockSpawnOdds = 5;
        Random random = new Random();
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

            base.Initialize();
            personPosition.X = (Window.ClientBounds.Width - personTexture.Width) / 2;
            personPosition.Y = Window.ClientBounds.Height - personTexture.Height;
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            blockTexture = Content.Load<Texture2D>("block_1_32x32");
            personTexture = Content.Load<Texture2D>("char32x32");
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

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
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.Left))
            {
                personPosition.X -= PersonMoveSpeed;
            }
            if (keyboard.IsKeyDown(Keys.Right))
            {
                personPosition.X += PersonMoveSpeed;
            }
            if (keyboard.IsKeyDown(Keys.Up))
            {
                personPosition.Y -= PersonMoveSpeed;
            }
            if (keyboard.IsKeyDown(Keys.Down))
            {
                personPosition.Y += PersonMoveSpeed;
            }

            personPosition.X = MathHelper.Clamp(personPosition.X, 0, Window.ClientBounds.Width - personTexture.Width);
            if (random.NextDouble() < BlockSpawnProbability)
            {
                float x = (float)random.NextDouble() * (Window.ClientBounds.Width - blockTexture.Width);
                blockPositions.Add(new Vector2(x, -blockTexture.Height));
            }
            Rectangle personRectangle = new Rectangle((int)personPosition.X, (int)personPosition.Y, personTexture.Width, personTexture.Height);

            personHit = false;
            for (int i = 0; i < blockPositions.Count; i++)
            {
                blockPositions[i] = new Vector2(blockPositions[i].X, blockPositions[i].Y + BlockFallSpeed);
                Rectangle blockRectangle = new Rectangle((int)blockPositions[i].X, (int)blockPositions[i].Y, blockTexture.Width, blockTexture.Height);

                if (personRectangle.Intersects(blockRectangle))
                {
                    personHit = true;
                }
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
            
            GraphicsDevice device = graphics.GraphicsDevice;
            if (personHit)
            {
                device.Clear(Color.Red);
            }
            else
            {
                device.Clear(Color.CornflowerBlue);
            }
            spriteBatch.Begin();

            spriteBatch.Draw(personTexture, personPosition, Color.White);
            foreach (Vector2 blockPosition in blockPositions)
                spriteBatch.Draw(blockTexture, blockPosition, Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
