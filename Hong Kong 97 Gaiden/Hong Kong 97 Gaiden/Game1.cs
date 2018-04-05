using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Hong_Kong_97_Gaiden
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        #region Declare Game Textures
        #region Player texture dictionaries
        // Idle Textures
        Dictionary<string, Texture2D> texturesPI = new Dictionary<string, Texture2D>();
        // Movement Textures
        Dictionary<string, Texture2D> texturesPM = new Dictionary<string, Texture2D>();
        // Shoot Textures
        Dictionary<string, Texture2D> texturesPS = new Dictionary<string, Texture2D>();
        // Hurt Textures
        Dictionary<string, Texture2D> texturesPH = new Dictionary<string, Texture2D>();
        #endregion
        #endregion

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

            // Set mouse to visible.
            IsMouseVisible = true;

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

            #region Load Player Textures
            // Idle.
            texturesPI = Loader.ContentLoad<Texture2D>(Content, "Player Sprites/1_Idle");
            // Move.
            texturesPM = Loader.ContentLoad<Texture2D>(Content, "Player Sprites/2_Move");
            // Shoot.
            texturesPS = Loader.ContentLoad<Texture2D>(Content, "Player Sprites/3_Shoot");
            // Hurt.
            texturesPH = Loader.ContentLoad<Texture2D>(Content, "Player Sprites/4_Hurt");
            #endregion

            // Create player object.


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
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
