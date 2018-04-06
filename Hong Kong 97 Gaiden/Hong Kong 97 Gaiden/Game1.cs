using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;

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
        // Player Texture dictionary.
        Dictionary<string, Texture2D> playerTextures = new Dictionary<string, Texture2D>();
        // Enemy Texture dictionary.
        Dictionary<string, Texture2D> enemyTextures = new Dictionary<string, Texture2D>();

        // Background Image
        Texture2D background1;
        #endregion

        // Declare player.
        Player p1;
        Texture2D playerProjectile, enemyProjectile;

        // Declare font for player score.
        SpriteFont scoreFont;

        // Declare enemy list.
        List<Enemy> enemies;

        // Enemy Spawn timer.
        int counter = 3;
        int limit = 0;
        float countDuration = 1f; //every  1s.
        float currentTime = 0f;

        // Random generator.
        Random randomG = new Random();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;

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

            #region Load Game Textures
            // Player
            playerTextures = Loader.ContentLoad<Texture2D>(Content, "Player");
            // Enemies
            enemyTextures = Loader.ContentLoad<Texture2D>(Content, "Enemy");
            // Backgrounds
            background1 = Content.Load<Texture2D>("Backgrounds/BG 1");

            // Load projectile images.
            playerProjectile = Content.Load<Texture2D>("Projectiles/Player Projectile");
            enemyProjectile = Content.Load<Texture2D>("Projectiles/Enemy Projectile");
            #endregion

            // Load game font.
            scoreFont = Content.Load<SpriteFont>("Score Font");

            // Create player object.
            p1 = new Player(this, playerTextures["Stand Down"], new Vector2(640, 550), Color.White, 2, playerTextures, scoreFont, playerProjectile);

            // Load enemy list.
            enemies = new List<Enemy>();


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

            // Update the player.
            p1.Update(gameTime);

            // Spawn enemies.
            SpawnEnemies(gameTime);

            #region Collision Checking.
            // Check enemy collision with player.
            foreach (Enemy e in enemies)
            {
                e.CheckPlayerCollision(p1);

                // Check player projectile collision with enemy.
                foreach (Projectile p in p1.projectilesFired)
                {
                    p.CheckEnemyCollision(e);
                }

                // Check enemy projectile collision with player.
                foreach (Projectile p in e.projectilesFired)
                {
                    p1.CheckEnemyProjectileCollision(p);
                }
            }          
            
            #endregion

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            // Draw the background.
            spriteBatch.Draw(background1, new Vector2(0, 0), Color.White);          

            // Draw the player.
            p1.Draw(spriteBatch);

            #region Draw enemies.
            foreach(Enemy e in enemies)
            {
                e.Draw(spriteBatch);

                // Draw any projectiles fired by the enemies.
                foreach (Projectile p in e.projectilesFired)
                {
                    p.Draw(spriteBatch);
                }
            }
            #endregion

            // Draw any projectiles fired by the player.
            foreach (Projectile p in p1.projectilesFired)
            {
                p.Draw(spriteBatch);
            }        

            // Draw the player score.
            spriteBatch.DrawString(scoreFont, "Score: " + p1.Score, new Vector2(10, 20), Color.White);
            spriteBatch.DrawString(scoreFont, "Health: " + p1.Health, new Vector2(10, 60), Color.White);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        public void SpawnEnemies(GameTime gameTime)
        {
            #region Timer ensures that enemies can only be spawn at a set pace.
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

            if (currentTime >= countDuration)
            {
                counter--;
                currentTime -= countDuration; // "use up" the time
                                              //any actions to perform
            }
            #endregion

            #region Spawn new enemies if the conditions are right.
            if (enemies.Count <= 15 && counter <= limit)
            {
                enemies.Add(new Enemy(this, enemyTextures["Enemy Type A"], new Vector2(RandomInt(10, GraphicsDevice.Viewport.Width-100), GraphicsDevice.Viewport.Y - 100),
                    Color.White, 1, "A", enemyTextures, enemyProjectile));

                counter = 3;   
            }
            #endregion

            #region Update each visible enemy.
            foreach (Enemy e in enemies)
            {
                e.Update(gameTime);

                // Update any projectiles fired by the enemies.
                foreach (Projectile p in e.projectilesFired)
                {
                    p.Update(gameTime);
                }
            }
            #endregion

            // Remove any dead or off-screen enemies from the list.
            for (int i = 0; i < enemies.Count; i++)
            {
                // If an enemy has been defeated...
                if (!enemies[i].Visible && enemies[i].EnemyHealth <= 0)
                {
                    // Update player score
                    p1.Score += enemies[i].ScoreWorth;

                    // Remove enemy.
                    enemies.RemoveAt(i);
                }

                // If an enemy manages to get past the player.
                else if (!enemies[i].Visible && enemies[i].EnemyHealth > 0)
                {
                    // The player will lose health and score.
                    if (p1.Score > enemies[i].ScoreWorth)
                    {
                        p1.Score -= enemies[i].ScoreWorth;
                    }

                    p1.Health -= 20;

                    // Remove enemy.
                    enemies.RemoveAt(i);
                }
            }
        }

        // Quick method to get a random number within a specified range.
        public int RandomInt(int min, int max)
        {
            return randomG.Next(min, max);
        }
    }
}
