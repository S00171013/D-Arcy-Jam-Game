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

        public Projectile(Game gameIn, Texture2D image, Vector2 position, Color tint, int frameCountIn) : base(image, position, tint, frameCountIn)
        {
            myGame = gameIn;

  
            gameScreen = myGame.GraphicsDevice.Viewport;

        }

        public virtual void Update(GameTime gtIn)
        {
            this.Position += new Vector2(0, -5);

            if (this.Position.X > gameScreen.Width + Image.Width || this.Position.X < 0 - Image.Width
                || this.Position.Y > gameScreen.Height + Image.Width || this.Position.Y < 0 - Image.Height)
            {
                this.Visible = false;
            }

        }
    }
}
