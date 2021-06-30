using System;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformBuilder.GameObjects
{
    class Box : Object
    {
        private bool active = false;
        private bool wasActive = false;
        private int state = 0;
        private float animationDuration = 200;
        private float animationTime;
        public ItemType item { get; private set; }

        public Box(Vector2 winSize,Vector2 position,ItemType item = ItemType.NONE) : base(winSize)
        {
            this.size = new Vector2(100,100);
            this.position = position;
            this.camera.size = this.size;
            this.camera.position = position;
            this.item = item;
        }

        public bool Active()
        {
            if(this.wasActive)
            {
                return false;
            }
            this.active = true;
            this.wasActive = true;

            return true;
        }

        public override void Update(float deltatime, GameData gameData)
        {
            if(this.active)
            {
                this.animationTime += deltatime * 800;
                if(this.animationTime >= this.animationDuration)
                {
                    this.animationTime = 0;
                    this.state++;
                    if (this.state >= gameData.boxTextures.Length)
                    {
                        this.state--;
                        this.active = false;
                        this.wasActive = true;
                    }
                }
            }
            base.Update(deltatime, gameData);
        }

        public override bool Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera mainCamera, GameData gameData)
        {
            spriteBatch.Draw(gameData.boxTextures[this.state], Utils.ToRectangle(this.GetPosition(mainCamera), this.size), Color.White);
            return true;
        }
    }

}
