using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformBuilder.GameObjects
{
    class HUD : Object
    {

        public HUD(Vector2 winSize) : base(winSize)
        {
            this.size = winSize;
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
            
            return true;
        }
    }
}
