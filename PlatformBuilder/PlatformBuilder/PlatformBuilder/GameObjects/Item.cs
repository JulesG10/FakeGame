using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformBuilder.GameObjects
{
    enum ItemType
    {
        ROCK_BLOCK,
        WOOD_BRIDGE
    }

    class Item : Object
    {
        public ItemType type;

        public Item(Vector2 winSize,Vector2 position, ItemType type) : base(winSize)
        {
            this.size = new Vector2(50, 50);
            this.type = type;
            this.position = position;
            this.camera.size = this.size;
            this.camera.position = position;
        }


        public override void Update(float deltatime)
        {
            base.Update(deltatime);
        }

        public override bool Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera mainCamera, GameData gameData)
        {
            if(base.Draw(spriteBatch, graphicsDeviceManager, mainCamera, gameData))
            {
                spriteBatch.Begin();
                    spriteBatch.Draw(gameData.itemTextures[(int)this.type], Utils.ToRectangle(this.GetPosition(mainCamera), this.size) , Color.White);
                spriteBatch.End();
                
                return true;
            }
            return false;
        }
    }
}
