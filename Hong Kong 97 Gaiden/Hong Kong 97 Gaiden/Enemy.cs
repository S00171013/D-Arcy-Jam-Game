using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Hong_Kong_97_Gaiden
{
    class Enemy : AnimatedSprite
    {
        public Game myGame;
        Viewport gameScreen;

        public string Type { get; }
        public int EnemySpeed { get; }
        public int EnemyHealth { get; set; }
        public int ScoreWorth { get; }
        public int ShootCounter { get; }

        // Projectile texture.
        Texture2D projectileImage;

        // Projectile timer. Initial count time is 5.
        int counter = 1;
        int limit = 0;
        float countDuration = 1f; //every  1s.
        float currentTime = 0f;

        // List of projectiles fired.
        public List<Projectile> projectilesFired = new List<Projectile>();

        // SFX Dictionary.
        Dictionary<string, SoundEffect> sfx;

        public Enemy(Game gameIn, Texture2D image, Vector2 position, Color tint, int frameCountIn, string typeIn, Dictionary<string, Texture2D> enemyImagesIn, Texture2D projectileImgIn, Dictionary<string, SoundEffect> sfxIn) : base(image, position, tint, frameCountIn)
        {
            myGame = gameIn;

            Type = typeIn;

            sfx = sfxIn;

            #region Determine enemy type.
            switch(Type)
            {
                case "A":
                    Image = enemyImagesIn["Enemy Type A"];
                    EnemySpeed = 2;
                    EnemyHealth = 5;
                    ScoreWorth = 50;
                    ShootCounter = 3;
                    break;

                case "B":
                    Image = enemyImagesIn["Enemy Type B"];
                    EnemySpeed = 3;
                    EnemyHealth = 1;
                    ScoreWorth = 100;
                    ShootCounter = 3;
                    break;

                case "C":
                    Image = enemyImagesIn["Enemy Type C"];
                    EnemySpeed = 1;
                    EnemyHealth = 8;
                    ScoreWorth = 150;
                    ShootCounter = 2;
                    break;           
            }
            #endregion

            projectileImage = projectileImgIn;

            gameScreen = myGame.GraphicsDevice.Viewport;
        }

        public virtual void Update(GameTime gtIn)
        {
            // Enemies will attempt to move down to the bottom of the screen.
            this.Position += new Vector2(0, EnemySpeed);

            #region Ensure enemies that go past the bottom of the screen disappear.
            if (this.Position.Y > gameScreen.Height + Image.Width)
            {
                this.Visible = false;
            }
            #endregion

            // Reset bounds to account for enemy movement.
            Bounds = new Rectangle((int)Position.X, (int)Position.Y, Image.Width / FrameCount, Image.Height);

            if(EnemyHealth <= 0)
            {
                Visible = false;
            }

            // Allow the enemy to shoot
            Shoot(gtIn);
        }

        public void Shoot(GameTime gameTime)
        {
            #region Timer ensures that projectiles can only be fired at a set pace.
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

            if (currentTime >= countDuration)
            {
                counter--;
                currentTime -= countDuration; // "use up" the time
                                              //any actions to perform
            }
            #endregion

            // Shoot when the count reaches 0.
            if (counter <= limit)
            {
                projectilesFired.Add(new Projectile(myGame, projectileImage, this.Position, Color.White, 1));
                sfx["Shoot"].Play();
                counter = ShootCounter;
            }

            // Update projectiles fired.
            foreach (Projectile p in projectilesFired)
            {
                p.Update(gameTime);
            }

            // Remove any offscreen projectiles from the list.
            for (int i = 0; i < projectilesFired.Count; i++)
            {
                if (!projectilesFired[i].Visible)
                {
                    projectilesFired.RemoveAt(i);
                }
            }
        }

        // Check for collision
        public void CheckPlayerCollision(Player player)
        {
            // Rectangle intersects            
            if ((Bounds.Intersects(player.Bounds)))
            {
                player.Health -= 20;

                //sfx["Hit"].Play();
                //this.EnemyHealth = 0;
                this.Visible = false;                           
            }                    
        }
    }
}
