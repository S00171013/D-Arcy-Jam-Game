using System;
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


        // Declare const int for the player's speed.
        const int PLAYER_SPEED = 5;

        Vector2 previousPosition;

        #region Declare variables to handle animation for this class.
        // Set up enum to keep track of player orientation.    
        public enum Direction { UP, TOPRIGHT, RIGHT, BOTTOMRIGHT, DOWN, BOTTOMLEFT, LEFT, TOPLEFT }
        public Direction playerDirection;

        #region Idle Texture Properties
        public Texture2D IdleUp { get; }
        public Texture2D IdleTRight { get; }
        public Texture2D IdleRight { get; }
        public Texture2D IdleBRight { get; }
        public Texture2D IdleDown { get; }
        public Texture2D IdleBLeft { get; }
        public Texture2D IdleLeft { get; }
        public Texture2D IdleTLeft { get; }
        #endregion

        #region Movement Texture Properties
        public Texture2D MoveUp { get; }
        public Texture2D MoveTRight { get; }
        public Texture2D MoveRight { get; }
        public Texture2D MoveBRight { get; }
        public Texture2D MoveDown { get; }
        public Texture2D MoveBLeft { get; }
        public Texture2D MoveLeft { get; }
        public Texture2D MoveTLeft { get; }
        #endregion

        #region Shoot Texture Properties
        public Texture2D ShootUp { get; }
        public Texture2D ShootTRight { get; }
        public Texture2D ShootRight { get; }
        public Texture2D ShootBRight { get; }
        public Texture2D ShootDown { get; }
        public Texture2D ShootBLeft { get; }
        public Texture2D ShootLeft { get; }
        public Texture2D ShootTLeft { get; }
        #endregion

        #region Hurt Texture Properties
        public Texture2D HurtTRight { get; }
        public Texture2D HurtBRight { get; }
        public Texture2D HurtBLeft { get; }
        public Texture2D HurtTLeft { get; }
        #endregion
        #endregion

        // Constructor.
        public Player(Game gameIn, Texture2D image, Vector2 position, Color tint, int frameCount) : base(image, position, tint, frameCount)
        {
            myGame = gameIn;

            // Get screen size.
            gameScreen = myGame.GraphicsDevice.Viewport;       
        }

        public virtual void Update(GameTime gameTime)
        {
            // Set previous position, this is needed to handle collision.
            previousPosition = Position;

            // Call the method that allows the player to move.
            HandleMovement(gameTime);

            #region Make sure the player stays in the bounds of the screen.
            Position = Vector2.Clamp(Position, Vector2.Zero,
                new Vector2(gameScreen.Width - Image.Width,
                gameScreen.Height - Image.Height));
            #endregion          
        }

        // This method will take in the player animations that have already been loaded in the game1 class.
        public void GetAnimations(Texture2D faceDownIn, Texture2D faceUpIn, Texture2D faceLeftIn, Texture2D faceRightIn,
            Texture2D moveDownIn, Texture2D moveUpIn, Texture2D moveLeftIn, Texture2D moveRightIn)
        {
            #region Take in Idle textures.
            FaceDown = faceDownIn;
            FaceUp = faceUpIn;
            FaceLeft = faceLeftIn;
            FaceRight = faceRightIn;
            #endregion

            #region Take in Movement textures.
            MoveDown = moveDownIn;
            MoveUp = moveUpIn;
            MoveLeft = moveLeftIn;
            MoveRight = moveRightIn;
            #endregion
        }

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

        // This method is called whenever the player goes through a door.
        public void Enter(Vector2 newPositionIn)
        {
            // Change the player's position to where they entered the room.
            Position = newPositionIn;
        }

        // This method determines what will happen when the player collides with a solid object.
        public void Collision(AnimatedSprite other)
        {
            if (Bounds.Intersects(other.Bounds))
            {
                Position = previousPosition;
            }
        }

        public bool Collect(Item itemPickedUp)
        {
            if (Bounds.Intersects(itemPickedUp.Bounds))
            {
                Inventory.Add(itemPickedUp);
                return true;
            }

            return false;
        }

        // Methods to add: Examine(etc), Attack(etc), Shoot(etc), ViewInventory(etc), ViewWeapons(etc). 
    }
}
