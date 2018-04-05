using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Hong_Kong_97_Gaiden
{
    class Projectile : AnimatedSprite
    {
        public Game myGame;
        Viewport gameScreen;
        private Player.Direction directionFired;

        const int P_SPEED = 7;


        public Projectile(Game gameIn, Texture2D image, Vector2 position, Color tint, int frameCountIn, Player.Direction pDirection) : base(image, position, tint, frameCountIn)
        {
            myGame = gameIn;

            // Get direction fired.
            directionFired = pDirection;

            gameScreen = myGame.GraphicsDevice.Viewport;

        }

        public virtual void Update(GameTime gtIn)
        {
            #region Ensure the projectile is fired in the correct direction.
            switch (directionFired)
            {
                case Player.Direction.DOWN:
                    this.Position += new Vector2(0, P_SPEED);
                    break;

                case Player.Direction.UP:
                    this.Position += new Vector2(0, -P_SPEED);
                    break;

                case Player.Direction.LEFT:
                    this.Position += new Vector2(-P_SPEED, 0);
                    break;

                case Player.Direction.RIGHT:
                    this.Position += new Vector2(P_SPEED, 0);
                    break;
            }
            #endregion

            #region Ensure off-screen projectiles disappear.
            if (this.Position.X > gameScreen.Width + Image.Width || this.Position.X < 0 - Image.Width
                || this.Position.Y > gameScreen.Height + Image.Width || this.Position.Y < 0 - Image.Height)
            {
                this.Visible = false;
            }
            #endregion

        }
    }
}
