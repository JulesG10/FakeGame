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
        MAX,
        BOTTOM
    }

    class Player : Object
    {


        public Player(Vector2 winSize) : base(winSize)
        {
            this.size = new Vector2(79, 198);
            this.position = new Vector2(winSize.X / 2 - this.size.X / 2, winSize.Y / 2 - this.size.Y / 2);
        }

        private PlayerJumpStates jumpStates = PlayerJumpStates.BOTTOM;
        private float JUMP_MAX_HEIGHT = 0;
        private float JUMP_MAX_TIME = 150;
        private float jumpTime = 0;
        private float moveSpeed = 300;
        private float jumpSpeed = 500;


        public override void Update(float deltatime, GameData gameData)
        {
            Vector2 velocity = this.camera.position;

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X -= deltatime * moveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {

                velocity.X += deltatime * moveSpeed;

            }
            velocity.Y += deltatime * this.jumpSpeed;
            this.JumpAction(deltatime, ref velocity);
            this.CheckBlockCollision(deltatime, gameData, ref velocity);

            this.camera.position = velocity;
            base.Update(deltatime, gameData);
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
                velocity.Y -= deltatime * this.jumpSpeed*2;
            }


        }

        private void CheckBlockCollision(float deltatime, GameData gameData, ref Vector2 velocity)
        {
            for (int i = 0; i < gameData.blocks.Count; i++)
            {
                if (Utils.AABB(this.GetStaticPosition(velocity), this.size, gameData.blocks[i].position, gameData.blocks[i].size))
                {
                    Direction[] dir = Utils.AABBDirection(this.GetStaticPosition(velocity), this.size, gameData.blocks[i].position, gameData.blocks[i].size);

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
            spriteBatch.Draw(gameData.playerTextures[0], Utils.ToRectangle(this.position, this.size), Color.White);//this.MarginEffect(5), Color.White);
            return true;
        }
    }
}
