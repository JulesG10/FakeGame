using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformBuilder.GameObjects
{
    enum BlockType
    {
        GROUND
    }

    class Block : Object
    {
        public BlockType type;

        public Block(Vector2 winSize, Vector2 position, BlockType type) : base(winSize)
        {
            this.size = new Vector2(MainGame.tileSize, MainGame.tileSize);
            this.type = type;
            this.position = position;
            this.camera.size = this.size;
            this.camera.position = position;
        }


        public override void Update(float deltatime, GameData gameData)
        {
            base.Update(deltatime, gameData);
        }

        public override bool Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera mainCamera, GameData gameData)
        {
            if (base.Draw(spriteBatch, graphicsDeviceManager, mainCamera, gameData))
            {
                spriteBatch.Draw(gameData.groundTextures[(int)this.type], Utils.ToRectangle(this.GetPosition(mainCamera), this.size), Color.White);
                
                return true;
            }
           

            return true;
        }
    }
}
