using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace PlatformBuilder.GameObjects
{
    class Player : Object
    {
        public Player(Vector2 winSize) : base(winSize)
        {
            this.size = new Vector2(120, 200);
            this.position = new Vector2(winSize.X/2 - this.size.X/2, winSize.Y/2 - this.size.Y / 2);
        }


        public override void Update(float deltatime)
        {
            float moveSpeed = deltatime * 1200;
            Vector2 velocity = this.camera.position;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                velocity.Y -= moveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                velocity.Y += moveSpeed;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity.X -= moveSpeed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity.X += moveSpeed;
            }

            this.camera.position = velocity;
            base.Update(deltatime);
        }

        public override bool Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera mainCamera, GameData gameData)
        {
            spriteBatch.Draw(gameData.playerTextures[0],Utils.ToRectangle(this.position,this.size), Color.White);
            return true;
        }
    }
}
