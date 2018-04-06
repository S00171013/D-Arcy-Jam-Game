using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Hong_Kong_97_Gaiden
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        // Set up enum for the game's states.
        public enum gameState { TITLE, INTRO, ROUND1, ROUND2, ROUND3, GAMEOVER, WIN }
        public gameState currentState;
        string[] enemyTypes = { "A", "B", "C" };

        #region Declare Game Textures
        // Player Texture dictionary.
        Dictionary<string, Texture2D> playerTextures = new Dictionary<string, Texture2D>();
        // Enemy Texture dictionary.
        Dictionary<string, Texture2D> enemyTextures = new Dictionary<string, Texture2D>();

        // BG Texture dictionary.
        Dictionary<string, Texture2D> bgTextures = new Dictionary<string, Texture2D>();

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

        // BGM 
        Dictionary<string, Song> bgm = new Dictionary<string, Song>();
        Song currentlyPlaying;

        // SFX
        Dictionary<string, SoundEffect> sfx = new Dictionary<string, SoundEffect>();

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
            bgTextures = Loader.ContentLoad<Texture2D>(Content, "Backgrounds");

            //background1 = Content.Load<Texture2D>("Backgrounds/BG 1");


            // Load projectile images.
            playerProjectile = Content.Load<Texture2D>("Projectiles/Player Projectile");
            enemyProjectile = Content.Load<Texture2D>("Projectiles/Enemy Projectile");
            #endregion

            // Load game font.
            scoreFont = Content.Load<SpriteFont>("Score Font");

            // Load BGM and SFX  
            // I do not own any of the BGM used.
            bgm = Loader.ContentLoad<Song>(Content, "BGM");

            // Set volume.
            MediaPlayer.Volume = 0f;
            MediaPlayer.IsRepeating = true;

            // SFX
            sfx = Loader.ContentLoad<SoundEffect>(Content, "SFX");


            // Create player object.
            p1 = new Player(this, playerTextures["Stand Down"], new Vector2(640, 550), Color.White, 2, playerTextures, scoreFont, playerProjectile, sfx);

            // Load enemy list.
            enemies = new List<Enemy>();

            // Set inital game state.
            currentState = gameState.TITLE;

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

            switch (currentState)
            {
                case gameState.TITLE:
                    #region BGM
                    if (!MediaPlayer.Equals(currentlyPlaying, bgm["Smile Hirasaka"]))
                    {
                        currentlyPlaying = bgm["Smile Hirasaka"];
                        MediaPlayer.Volume += 0.5f;
                        MediaPlayer.Play(bgm["Smile Hirasaka"]);
                        MediaPlayer.IsRepeating = true;
                    }
                    #endregion                    

                    if(Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        currentState = gameState.INTRO;
                    }
                    break;

                case gameState.INTRO:
                    if (!MediaPlayer.Equals(currentlyPlaying, bgm["Illusions"]))
                    {
                        currentlyPlaying = bgm["Illusions"];
                        MediaPlayer.Volume += 0.2f;
                        MediaPlayer.Play(bgm["Illusions"]);
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.C))
                    {
                        currentState = gameState.ROUND1;
                    }
                    break;

                case gameState.ROUND1:
                    if (!MediaPlayer.Equals(currentlyPlaying, bgm["Mt Mifune"]))
                    {
                        currentlyPlaying = bgm["Mt Mifune"];
                        MediaPlayer.Volume -= 0.2f;
                        MediaPlayer.Play(bgm["Mt Mifune"]);
                    }             

                    // Update the player.
                    p1.Update(gameTime);

                    // Spawn enemies.
                    SpawnEnemies(gameTime);

                    if (p1.Score >= 1000)
                    {
                        currentState = gameState.ROUND2;
                    }
                    break;

                case gameState.ROUND2:
                    if (!MediaPlayer.Equals(currentlyPlaying, bgm["Abandoned Factory"]))
                    {
                        currentlyPlaying = bgm["Abandoned Factory"];
                        MediaPlayer.Volume += 0.2f;
                        MediaPlayer.Play(bgm["Abandoned Factory"]);
                    }                

                    // Update the player.
                    p1.Update(gameTime);

                    // Spawn enemies.
                    SpawnEnemies(gameTime);

                    if (p1.Score >= 2000)
                    {
                        currentState = gameState.ROUND3;
                    }
                    break;

                case gameState.ROUND3:
                    if (!MediaPlayer.Equals(currentlyPlaying, bgm["Monado Mandala"]))
                    {
                        currentlyPlaying = bgm["Monado Mandala"];
                        MediaPlayer.Play(bgm["Monado Mandala"]);
                    }

                    // Update the player.
                    p1.Update(gameTime);

                    // Spawn enemies.
                    SpawnEnemies(gameTime);

                    if (p1.Score >= 3000)
                    {
                        currentState = gameState.WIN;
                    }
                    break;

                case gameState.GAMEOVER:
                    if (!MediaPlayer.Equals(currentlyPlaying, bgm["Game Over"]))
                    {
                        currentlyPlaying = bgm["Game Over"];
                        MediaPlayer.Play(bgm["Game Over"]);
                    }
                    break;

                case gameState.WIN:
                    if (!MediaPlayer.Equals(currentlyPlaying, bgm["BGM II"]))
                    {
                        currentlyPlaying = bgm["BGM II"];
                        MediaPlayer.Play(bgm["BGM II"]);
                    }
                    break;
            }

            #region Collision Checking.
            // Check enemy collision with player.
            if (currentState != gameState.GAMEOVER)
            {
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
            }
            #endregion
            // TODO: Add your update logic here

            // End the game if the player's health reaches 0.
            if (p1.Health <= 0 && currentState != gameState.GAMEOVER)
            {
                currentState = gameState.GAMEOVER;
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);
                       
            switch (currentState)
            {
                case gameState.TITLE:
                    spriteBatch.Begin();
                    // Draw the background.
                    spriteBatch.Draw(bgTextures["HK 97 G Logo"], new Vector2(0, 0), Color.White);
                    spriteBatch.End();
                    break;

                case gameState.INTRO:
                    spriteBatch.Begin();
                    // Draw the background.
                    spriteBatch.Draw(bgTextures["HK 97 G Story"], new Vector2(0, 0), Color.White);
                    spriteBatch.End();
                    break;

                case gameState.ROUND1:
                    #region Round 1
                    spriteBatch.Begin();
                    // Draw the background.
                    spriteBatch.Draw(bgTextures["BG 1"], new Vector2(0, 0), Color.White);

                    // Draw the player.
                    p1.Draw(spriteBatch);

                    #region Draw enemies.
                    foreach (Enemy e in enemies)
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
                    #endregion
                    spriteBatch.End();
                    break;

                case gameState.ROUND2:
                    #region Round 2
                    spriteBatch.Begin();
                    // Draw the background.
                    spriteBatch.Draw(bgTextures["BG 1"], new Vector2(0, 0), Color.White);

                    // Draw the player.
                    p1.Draw(spriteBatch);

                    #region Draw enemies.
                    foreach (Enemy e in enemies)
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
                    #endregion
                    break;

                case gameState.ROUND3:
                    #region Round 3.
                    spriteBatch.Begin();
                    // Draw the background.
                    spriteBatch.Draw(bgTextures["BG 1"], new Vector2(0, 0), Color.White);

                    // Draw the player.
                    p1.Draw(spriteBatch);

                    #region Draw enemies.
                    foreach (Enemy e in enemies)
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
                    #endregion
                    break;

                case gameState.GAMEOVER:
                    spriteBatch.Begin();
                    // Draw the game over background.
                    spriteBatch.Draw(bgTextures["HK 97 Game Over"], new Vector2(0, 0), Color.White);
                    spriteBatch.End();
                    break;

                case gameState.WIN:
                    spriteBatch.Begin();
                    // Draw the background.
                    spriteBatch.Draw(bgTextures["HK 97 G Win"], new Vector2(0, 0), Color.White);
                    spriteBatch.End();
                    break;
            }            
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
                switch (currentState)
                {
                    case gameState.ROUND1:
                        enemies.Add(new Enemy(this, enemyTextures["Enemy Type A"], new Vector2(RandomInt(10, GraphicsDevice.Viewport.Width - 100), GraphicsDevice.Viewport.Y - 100),
                        Color.White, 1, "A", enemyTextures, enemyProjectile, sfx));
                        counter = 3;
                        break;

                    case gameState.ROUND2:
                        enemies.Add(new Enemy(this, enemyTextures["Enemy Type A"], new Vector2(RandomInt(10, GraphicsDevice.Viewport.Width - 100), GraphicsDevice.Viewport.Y - 100),
                        Color.White, 1, enemyTypes[RandomInt(0, 2)], enemyTextures, enemyProjectile, sfx));
                        counter = 3;
                        break;

                    case gameState.ROUND3:
                        enemies.Add(new Enemy(this, enemyTextures["Enemy Type A"], new Vector2(RandomInt(10, GraphicsDevice.Viewport.Width - 100), GraphicsDevice.Viewport.Y - 100),
                        Color.White, 1, enemyTypes[RandomInt(0, 3)], enemyTextures, enemyProjectile, sfx));
                        counter = 2;
                        break;
                }



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

            #region Remove any dead or off-screen enemies from the list.
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

                    sfx["Hit"].Play();

                    p1.Health -= 20;

                    // Remove enemy.
                    enemies.RemoveAt(i);
                }
            }
            #endregion
        }

        // Quick method to get a random number within a specified range.
        public int RandomInt(int min, int max)
        {
            return randomG.Next(min, max);
        }
    }
}
