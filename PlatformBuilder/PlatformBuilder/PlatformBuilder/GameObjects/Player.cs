using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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
        W_4,
    }

    enum PlayerJump
    {
        J1,
        J2,
        J3,
        J4
    }

    class Player : Object
    {
        public Direction playerDirection { get; private set; } = Direction.RIGHT;
        public PlayerJumpStates jumpStates { get; private set; } = PlayerJumpStates.BOTTOM;
        public PlayerWalk playerState { get; private set; } = PlayerWalk.STATIC_1;
        public PlayerJump playerJumpState { get; private set; } = PlayerJump.J1;
        public List<ItemType> inventory { get; private set; } = new List<ItemType>();
        public Dictionary<int, Direction> collisionBlockList { get; private set; } = new Dictionary<int, Direction>();
        public Direction blockActionDir { get; private set; }  = Direction.TOP;

        public float jumpTime { get; private set; } = 0;
        public float moveSpeed { get; private set; } = 300;
        public float jumpSpeed { get; private set; } = 500;
        public float MaxJumpTime { get; private set; } = 40;
        public float playerSwitchTime { get; private set; } = 0;
        public float playerMaxSwitch { get; private set; } = 20;
        public float jumpWait { get; private set; } = 0;
        public float jumpEndWait { get; private set; } = 50;
        public float life { get; private set; } = 100.0f;
        public bool canJump { get; private set; } = false;
        public bool isAlive { get; private set; } = true;
        public double Wpress { get; private set; } = 0;
        public double Xpress { get; private set; } = 0;
        public int marginValue { get; private set; } = 0;

        public Player(Vector2 winSize) : base(winSize)
        {
            this.size = new Vector2(200, 200);
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

            if(this.blockActionDir == Direction.LEFT ||this.blockActionDir == Direction.RIGHT)
            {
                this.blockActionDir = this.playerDirection == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
            }

            /* test
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                velocity.Y -= deltatime * jumpSpeed*2;
            }
            */

            collisionBlockList.Clear();

            this.GetCollisionBlock(gameData, velocity, false);
            this.CheckBlockCollision(deltatime, gameData, ref velocity, false);
            
            this.JumpAction(deltatime, ref velocity);

            this.GetCollisionBlock(gameData, velocity , true);
            this.CheckBlockCollision(deltatime, gameData, ref velocity, true);

            this.UpdateState(velocity, gameData);
            this.ItemAction(velocity, gameData);

            this.BlockAction(gameData,deltatime);

            this.camera.position = velocity;
            base.Update(deltatime, gameData);
        }



        private void BlockAction(GameData gameData,float deltatime)
        {
            /* test
            foreach (KeyValuePair<int, Direction> item in collisionBlockList)
            {
                if(item.Value == this.blockActionDir)
                {
                    Block b = (Block)gameData.blocks[item.Key];
                    b.active = true;
                }
               
            }
            */

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                Wpress += deltatime * 1000;
                if(Wpress > 200)
                {
                    Wpress = 0;
                    if(blockActionDir == Direction.TOP)
                    {
                        blockActionDir = Direction.BOTTOM;
                    }else if(blockActionDir == Direction.BOTTOM)
                    {
                        blockActionDir = this.playerDirection == Direction.LEFT ? Direction.RIGHT : Direction.LEFT;
                    }else if(blockActionDir == Direction.LEFT || blockActionDir == Direction.RIGHT)
                    {
                        blockActionDir = Direction.TOP;
                    }
                }
            }

            if(Keyboard.GetState().IsKeyDown(Keys.X))
            {
                Xpress += deltatime * 1000;
                if (Xpress > 200)
                {
                    Xpress = 0;
                    if (collisionBlockList.ContainsValue(this.blockActionDir))
                    {
                        foreach (KeyValuePair<int, Direction> item in collisionBlockList)
                        {
                            if (item.Value == this.blockActionDir)
                            {
                                Block b = (Block)gameData.blocks[item.Key];
                                b.type = BlockType.NONE;
                            }
                        }
                    }
                }
            }
        }
        
        private void  GetCollisionBlock(GameData gameData,Vector2 velocity,bool y)
        {
            int count = 0;
            int max = y ? 2 : (int)this.size.Y / MainGame.tileSize;
            for(int i = 0;i< gameData.blocks.Count;i++)
            {
                if (count > max)
                {
                    break;
                }

                Direction[] dir = Utils.AABBDirection(this.GetStaticPosition(this.getPositionHitBox(velocity)), this.getSizeHitBox(), gameData.blocks[i].position, gameData.blocks[i].size);
                if ((dir[0] != Direction.NONE || dir[1] != Direction.NONE) && !collisionBlockList.ContainsKey(i))
                {
                    count++;
                    collisionBlockList.Add(i, dir[y ? 0 : 1]);
                    if (!y && !collisionBlockList.ContainsKey(i + 1) && !collisionBlockList.ContainsKey(i - 1))
                    {
                        if(gameData.blocks[i-1].position.Y == gameData.blocks[i].position.Y - MainGame.tileSize)
                        {
                            Block b = (Block)gameData.blocks[i - 1];
                            if(b.type != BlockType.NONE)
                            {
                                collisionBlockList.Add(i + 1, dir[y ? 0 : 1]);
                            }
                            
                        }
                        
                    }
                }
            }
        }

        private void ItemAction(Vector2 velocity, GameData gameData)
        {
            for (int i = 0; i < gameData.boxs.Count; i++)
            {
                if (Utils.AABB(this.GetStaticPosition(this.getPositionHitBox(velocity)), this.getSizeHitBox(), gameData.boxs[i].position, gameData.boxs[i].size))
                {
                    Box b = (Box)gameData.boxs[i];
                    if(b.Active())
                    {
                        inventory.Add(b.item);
                        gameData.boxs[i] = b;
                    }
                }
            }
        }

        private void UpdateState(Vector2 velocity, GameData gameData)
        {
            if (this.playerSwitchTime >= this.playerMaxSwitch)
            {
                this.playerSwitchTime = 0;
                if (this.jumpStates == PlayerJumpStates.BOTTOM)
                {
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

                        if (state >= gameData.L_playerTextures.Length)
                        {
                            state = 2;
                        }

                        this.playerState = (PlayerWalk)state;
                    }
                }
                else
                {
                    int state = (int)this.playerJumpState;
                    state++;
                    if (state >= gameData.L_playerJumpTextures.Length)
                    {
                        state = 0;
                    }

                    this.playerJumpState = (PlayerJump)state;
                }
            }
        }

        public Vector2 getPositionHitBox(Vector2 position)
        {
            return new Vector2(position.X + (this.size.X - this.getSizeHitBox().X) / 2, position.Y);
        }

        public Vector2 getSizeHitBox()
        {
            return new Vector2(40, 198);
        }


        private void JumpAction(float deltatime, ref Vector2 velocity)
        {
            if (this.jumpStates == PlayerJumpStates.BOTTOM)
            {
                velocity.Y += deltatime * this.jumpSpeed;
            }
            else if (this.jumpStates == PlayerJumpStates.TOP)
            {
                this.jumpTime += deltatime * 100;
                if (this.jumpTime > this.MaxJumpTime)
                {
                    this.jumpTime = 0;
                    this.jumpStates = PlayerJumpStates.BOTTOM;
                }
                else
                {
                    velocity.Y -= deltatime * this.jumpSpeed;
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if (canJump && this.jumpStates == PlayerJumpStates.BOTTOM && this.jumpWait >= this.jumpEndWait)
                {
                    canJump = false;
                    this.jumpWait = 0;
                    this.jumpStates = PlayerJumpStates.TOP;
                }
            }

            if (this.jumpStates == PlayerJumpStates.BOTTOM)
            {
                this.jumpWait += deltatime * 100;
            }
        }

        private void CheckBlockCollision(float deltatime, GameData gameData, ref Vector2 velocity, bool y)
        {
            int collisionCount = 0;
            for (int i = 0; i < gameData.blocks.Count; i++)
            {
                if(collisionCount >= 6)
                {
                    break;
                }

                if (Utils.AABB(this.GetStaticPosition(this.getPositionHitBox(velocity)), this.getSizeHitBox(), gameData.blocks[i].position, gameData.blocks[i].size))
                {
                    collisionCount++;
                    Block b = (Block)gameData.blocks[i];

                    if(b.type == BlockType.NONE)
                    {

                    }
                    else if (b.type == BlockType.BORDER)
                    {
                        this.life = 0;
                        this.isAlive = false;
                        this.OnDestroy(this, new EventArgs());
                    }
                    else
                    {

                        Direction[] dir = Utils.AABBDirection(this.GetStaticPosition(this.getPositionHitBox(velocity)), this.getSizeHitBox(), gameData.blocks[i].position, gameData.blocks[i].size);
                        if (y)
                        {
                            if (dir[0] == Direction.TOP)
                            {
                                if(b.type == BlockType.GROUND)
                                {
                                    this.marginValue = 5;
                                }
                                else
                                {
                                    this.marginValue = 0;
                                }
                                velocity.Y -= deltatime * this.jumpSpeed; //Math.Abs(gameData.blocks[i].position.Y - (this.GetStaticPosition(this.getPositionHitBox(velocity)).Y + this.getSizeHitBox().Y)) + 1;
                                if (this.jumpStates != PlayerJumpStates.TOP)
                                {
                                    canJump = true;
                                }
                            }
                            else if (dir[0] == Direction.BOTTOM)
                            {
                                velocity.Y += deltatime * this.jumpSpeed;// Math.Abs((gameData.blocks[i].position.Y + gameData.blocks[i].size.Y) - this.GetStaticPosition(this.getPositionHitBox(velocity)).Y);
                            }
                        }
                        else
                        {
                            if (dir[1] == Direction.RIGHT)
                            {
                                velocity.X += Math.Abs((gameData.blocks[i].position.X + gameData.blocks[i].size.X) - this.GetStaticPosition(this.getPositionHitBox(velocity)).X);
                                // deltatime * this.moveSpeed;
                            }
                            else if (dir[1] == Direction.LEFT)
                            {
                                velocity.X -= Math.Abs(gameData.blocks[i].position.X - (this.GetStaticPosition(this.getPositionHitBox(velocity)).X + this.getSizeHitBox().X));
                                //deltatime * this.moveSpeed;
                            }
                        }
                    }
                }
            }

        }

        public Rectangle MarginEffect(int margin)
        {
            return Utils.ToRectangle(new Vector2(this.position.X, this.position.Y), new Vector2(this.size.X, this.size.Y + margin));
        }

        public Texture2D getDrawTexture(GameData gameData)
        {
            if (this.playerDirection == Direction.RIGHT)
            {
                if (this.jumpStates == PlayerJumpStates.TOP)
                {
                    return gameData.R_playerJumpTextures[(int)this.playerJumpState];
                }
                else
                {
                    return gameData.R_playerTextures[(int)playerState];
                }
            }
            else
            {
                if (this.jumpStates == PlayerJumpStates.TOP)
                {
                    return gameData.L_playerJumpTextures[(int)this.playerJumpState];
                }
                else
                {
                    return gameData.L_playerTextures[(int)playerState];
                }
            }
        }

        public override bool Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera mainCamera, GameData gameData)
        {
            spriteBatch.Draw(this.getDrawTexture(gameData), this.MarginEffect(this.marginValue), Color.White);
            return true;
        }
    }
}
