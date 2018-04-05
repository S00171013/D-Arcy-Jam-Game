using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Monogame classes are in these namespaces.
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace Hong_Kong_97_Gaiden
{
    class AnimatedSprite
    {
        // Variables
        public Texture2D Image { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Bounds;
        public Color Tint { get; set; }

        public int FrameCount { get; set; }

        // Used to determine where inside the spritesheet we are drawing
        public Rectangle SourceRectangle;

        // Variables for animation
        public int currentFrame = 0;
        int numberOfFrames = 0;
        int millisecondsBetweenFrames = 200;
        float elapsedTime = 0;

        public bool Visible = false;
       
        // Constructor
        public AnimatedSprite(Texture2D image, Vector2 position, Color tint, int frameCountIn)
        {          
            // Set up properties.           
            Image = image;
            Position = position;
            Tint = tint;
            FrameCount = frameCountIn;

            // Width is now width/number of frames
            Bounds = new Rectangle((int)position.X, (int)position.Y, image.Width/FrameCount, image.Height);

            Visible = true;

            // Set up source rectangle initially. The player sprite won't appear otherwise.
            SourceRectangle = new Rectangle(
                currentFrame * Image.Width / FrameCount, // Sprite width
                0,
                Image.Width / FrameCount,
                Image.Height);
        }

        public void UpdateAnimation(GameTime gameTime)
        {                     
            // Track how much time has passed
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            // If it's greater than the frame time then move to the next frame
            if (elapsedTime >= millisecondsBetweenFrames)
            {
                currentFrame++;

                if (currentFrame > FrameCount - 1)
                {
                    currentFrame = 0;
                }

                elapsedTime = 0;
            }
          
            // Update our source rectangle
            SourceRectangle = new Rectangle(
                currentFrame * Image.Width / FrameCount, // Sprite width
                0,
                Image.Width / FrameCount,
                Image.Height);
        }

        // Caller has a spritebatch ready and has already called Begin
        public void Draw(SpriteBatch sp)
        {
            sp.Draw(Image, Position, SourceRectangle, Tint);
        }

        public void DrawNoRect(SpriteBatch sp)
        {
            sp.Draw(Image, Position, Tint);                 
        }

        public void Draw(SpriteBatch sp, SpriteFont sfont)
        {
            sp.Draw(Image, Position, Tint);
            sp.DrawString(sfont, Position.ToString(), Position, Color.White);
        }

        public void Move(Vector2 delta)
        {
            Position += delta;
            Bounds.X = (int)Position.X;
            Bounds.Y = (int)Position.Y;
        }

        // Check for collision
        public bool CheckCollision(AnimatedSprite other)
        {
            // Rectangle intersects

            // If there's a collision change the tint to red
            // Otherwise set tint to white
            if ((Bounds.Intersects(other.Bounds)))
            {
                Tint = Color.Red;
                return true;
            }

            else
            {
                Tint = Color.White;
                return false;
            }
        }
    }
}
