using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PlatformBuilder.GameObjects
{
    enum PlayerJumpStates
    {
        TOP,
        BOTTOM
    }

    enum PlayerWalk
    {
        STATIC_1,
        STATIC_2,
        W_1,
        W_2,
        W_3,
        W_4

    }

    class Player : Object
    {
        public Direction playerDirection { get; private set; } = Direction.RIGHT;
        private PlayerJumpStates jumpStates = PlayerJumpStates.BOTTOM;
        private PlayerWalk playerState = PlayerWalk.STATIC_1;
        private float jumpTime = 0;
        private float moveSpeed = 300;
        private float jumpSpeed = 500;
        private float MaxJumpTime = 50;
        private float playerSwitchTime = 0;
        private float playerMaxSwitch = 20;
        private float jumpWait = 0;
        private float jumpEndWait = 50;

        public Player(Vector2 winSize) : base(winSize)
        {
            this.size = new Vector2(200,200);
            this.position = new Vector2(winSize.X / 2 - this.size.X / 2, winSize.Y / 2 - this.size.Y / 2);
        }

        public override void Update(float deltatime, GameData gameData)
        {
            Vector2 velocity = this.camera.position;
            this.playerSwitchTime += deltatime * 100;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                this.playerDirection = Direction.LEFT;
                velocity.X -= deltatime * moveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                this.playerDirection = Direction.RIGHT;
                velocity.X += deltatime * moveSpeed;
            }

            
            if(this.jumpStates == PlayerJumpStates.BOTTOM)
            {
                velocity.Y += deltatime * this.jumpSpeed;
            }
            else if(this.jumpStates == PlayerJumpStates.TOP)
            {
                this.jumpTime += deltatime * 100;
                if(this.jumpTime > this.MaxJumpTime)
                {
                    this.jumpTime = 0;
                    this.jumpStates = PlayerJumpStates.BOTTOM;
                }
                else
                {
                    velocity.Y -= deltatime * this.jumpSpeed;
                }
            }

            this.JumpAction(deltatime, ref velocity);
            this.CheckBlockCollision(deltatime, gameData, ref velocity);
            this.UpdateState(velocity, gameData.L_playerTextures.Length);

            this.camera.position = velocity;
           
            base.Update(deltatime, gameData);
        }

        private void UpdateState(Vector2 velocity,int texturesLength)
        {
            if (this.playerSwitchTime >= this.playerMaxSwitch)
            {
                this.playerSwitchTime = 0;
                if (this.camera.position.X == velocity.X)
                {
                    if (this.playerState != PlayerWalk.STATIC_1)
                    {
                        this.playerState = PlayerWalk.STATIC_1;
                    }
                    else
                    {
                        this.playerState = PlayerWalk.STATIC_2;
                    }
                }
                else
                {
                    int state = (int)this.playerState;
                    if (state > 1)
                    {
                        state++;
                    }
                    else
                    {
                        state = 2;
                    }

                    if (state >= texturesLength)
                    {
                        state = 2;
                    }

                    this.playerState = (PlayerWalk)state;
                }
            }
        }

        private Vector2 getPositionHitBox(Vector2 position)
        {
            return new Vector2(position.X + (this.size.X - this.getSizeHitBox().X)/2, position.Y);
        }

        private Vector2 getSizeHitBox()
        {
            return new Vector2(80, 200);
        }

        
        private void JumpAction(float deltatime,ref Vector2 velocity)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if(this.jumpStates  == PlayerJumpStates.BOTTOM && this.jumpWait >= this.jumpEndWait)
                {
                    this.jumpWait = 0;
                    this.jumpStates = PlayerJumpStates.TOP;
                }
            }

            if(this.jumpStates == PlayerJumpStates.BOTTOM)
            {
                this.jumpWait += deltatime * 100;
            }
        }

        private void CheckBlockCollision(float deltatime, GameData gameData, ref Vector2 velocity)
        {
            for (int i = 0; i < gameData.blocks.Count; i++)
            {
                if (Utils.AABB(this.GetStaticPosition(this.getPositionHitBox(velocity)), this.getSizeHitBox(), gameData.blocks[i].position, gameData.blocks[i].size))
                {
                    Direction[] dir = Utils.AABBDirection(this.GetStaticPosition(this.getPositionHitBox(velocity)), this.getSizeHitBox(), gameData.blocks[i].position, gameData.blocks[i].size);

                    if (dir[0] == Direction.TOP)
                    {
                        velocity.Y -= deltatime * this.jumpSpeed;
                    }
                    else if (dir[0] == Direction.BOTTOM)
                    {
                        velocity.Y += deltatime * this.jumpSpeed;
                    }

                    if (dir[1] == Direction.RIGHT)
                    {
                        velocity.X += deltatime * this.moveSpeed;
                    }
                    else if (dir[1] == Direction.LEFT)
                    {
                        velocity.X -= deltatime * this.moveSpeed;
                    }

                }
            }

        }

        public Rectangle MarginEffect(int margin)
        {
           return Utils.ToRectangle(new Vector2(this.position.X, this.position.Y), new Vector2(this.size.X,this.size.Y + margin));
        }

        public override bool Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera mainCamera, GameData gameData)
        {
            if(this.playerDirection == Direction.RIGHT)
            {
                spriteBatch.Draw(gameData.R_playerTextures[(int)playerState],this.MarginEffect(5), Color.White);// Utils.ToRectangle(this.position, this.size), Color.White);
            }
            else
            {
                spriteBatch.Draw(gameData.L_playerTextures[(int)playerState],this.MarginEffect(5), Color.White);// Utils.ToRectangle(this.position, this.size), Color.White);
            }
           
            return true;
        }
    }
}
