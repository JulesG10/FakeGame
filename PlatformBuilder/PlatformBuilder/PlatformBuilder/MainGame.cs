using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformBuilder.GameObjects;
using System.Collections.Generic;
using System.Diagnostics;

namespace PlatformBuilder
{
    class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameData gameData;

        public MainGame()
        {

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;  

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
        }


        protected override void Initialize()
        {
            base.Initialize();
            this.gameData = new GameData(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this.gameData.itemTextures = Utils.GetTextures(Content, "items/item_", 2);
            this.gameData.iconTextures = Utils.GetTextures(Content, "icons/icon_", 3);
            this.gameData.groundTextures = Utils.GetTextures(Content, "ground/ground_", 4);
            
            this.gameData.playerTextures = Utils.GetTextures(Content, "player/player_", 0);

            this.gameData.animationsTextures.Add(0, Utils.GetTextures(Content, "animations/box/box_", 6).ToArray());
            this.gameData.animationsTextures.Add(1, Utils.GetTextures(Content, "animations/tnt/tnt_", 5).ToArray());
        }



        protected override void Update(GameTime gameTime)
        {
            float deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            this.gameData.player.Update(deltatime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Camera mainCamera = this.gameData.player.camera;
            this.gameData.player.Draw(this._spriteBatch, this._graphics, mainCamera, this.gameData);
            
            base.Draw(gameTime);

        }
    }
}
