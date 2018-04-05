﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Hong_Kong_97_Gaiden
{
    class Player : AnimatedSprite
    {
        protected Game myGame;
        Viewport gameScreen;

        // Properties.
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Score { get; set; }
     
        // Declare const int for the player's speed.
        const int PLAYER_SPEED = 5;

        SpriteFont scoreFont;

        #region Declare variables to handle animation for this class.
        // Set up enum to keep track of player orientation.    
        public enum Direction { DOWN, UP, LEFT, RIGHT }
        public Direction playerDirection;

        // Idle Textures
        public Texture2D FaceDown { get; set; }
        public Texture2D FaceUp { get; set; }
        public Texture2D FaceLeft { get; set; }
        public Texture2D FaceRight { get; set; }

        // Movement Textures.
        public Texture2D MoveDown { get; set; }
        public Texture2D MoveUp { get; set; }
        public Texture2D MoveLeft { get; set; }
        public Texture2D MoveRight { get; set; }
        #endregion

        // Constructor.
        public Player(Game gameIn, Texture2D image, Vector2 position, Color tint, int frameCount, Dictionary<string, Texture2D> texturesIn, SpriteFont fontIn) : base(image, position, tint, frameCount)
        {
            myGame = gameIn;

            // The original example had this running in the player's Update method. - In case problems arise later with different rooms.
            gameScreen = myGame.GraphicsDevice.Viewport;

            #region Take in Idle textures.
            FaceDown = texturesIn["Stand Down"];
            FaceUp = texturesIn["Stand Up"];
            FaceLeft = texturesIn["Stand Left"];
            FaceRight = texturesIn["Stand Right"];
            #endregion

            #region Take in Movement textures.
            MoveDown = texturesIn["Move Down"];
            MoveUp = texturesIn["Move Up"];
            MoveLeft = texturesIn["Move Left"];
            MoveRight = texturesIn["Move Right"];
            #endregion

            // Set initial score.
            Score = 0;
        }

        public virtual void Update(GameTime gameTime)
        {           
            // Call the method that allows the player to move.
            HandleMovement(gameTime);

            #region Make sure the player stays in the bounds of the screen.
            Position = Vector2.Clamp(Position, Vector2.Zero,
                new Vector2(gameScreen.Width - Image.Width,
                gameScreen.Height - Image.Height));
            #endregion          
        }

        // This method will take in the player animations that have already been loaded in the game1 class.
        //public void GetAnimations(Texture2D faceDownIn, Texture2D faceUpIn, Texture2D faceLeftIn, Texture2D faceRightIn,
        //    Texture2D moveDownIn, Texture2D moveUpIn, Texture2D moveLeftIn, Texture2D moveRightIn)
        //{
        //    #region Take in Idle textures.
        //    FaceDown = faceDownIn;
        //    FaceUp = faceUpIn;
        //    FaceLeft = faceLeftIn;
        //    FaceRight = faceRightIn;
        //    #endregion

        //    #region Take in Movement textures.
        //    MoveDown = moveDownIn;
        //    MoveUp = moveUpIn;
        //    MoveLeft = moveLeftIn;
        //    MoveRight = moveRightIn;
        //    #endregion
        //}

        public void HandleMovement(GameTime gameTime)
        {
            #region Handle movement
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                // Left             
                Move(new Vector2(-PLAYER_SPEED, 0));
                Image = MoveLeft;

                playerDirection = Direction.LEFT;

                UpdateAnimation(gameTime);
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                // Right                
                Move(new Vector2(PLAYER_SPEED, 0));
                Image = MoveRight;

                playerDirection = Direction.RIGHT;

                UpdateAnimation(gameTime);
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                // Up                
                Move(new Vector2(0, -PLAYER_SPEED));
                Image = MoveUp;

                playerDirection = Direction.UP;

                UpdateAnimation(gameTime);
            }

            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                // Down              
                Move(new Vector2(0, PLAYER_SPEED));

                Image = MoveDown;

                playerDirection = Direction.DOWN;

                UpdateAnimation(gameTime);
            }
            #endregion

            #region Otherwise, if the player is idle...
            // Display the correct idle sprite depending on the player's current orientation.
            else
            {
                switch (playerDirection)
                {
                    case Direction.DOWN:
                        Image = FaceDown;
                        break;

                    case Direction.UP:
                        Image = FaceUp;
                        break;

                    case Direction.LEFT:
                        Image = FaceLeft;
                        break;

                    case Direction.RIGHT:
                        Image = FaceRight;
                        break;
                }
            }
            #endregion                   
        }                           
    }
}