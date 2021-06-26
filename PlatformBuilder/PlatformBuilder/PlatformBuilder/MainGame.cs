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
        private bool showGrid = false;

        public MainGame()
        {

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            _graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            _graphics.IsFullScreen = true;  

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this.gameData = new GameData(new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height));

            this.gameData.items.Add(new Item(this.gameData.windowSize, new Vector2(100, 100), ItemType.ROCK_BLOCK));
            this.gameData.blocks.Add(new Block(this.gameData.windowSize, new Vector2(500, 400), BlockType.GROUND));
        }


        protected override void Initialize()
        {
            base.Initialize();  
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this.gameData.itemTextures = Utils.GetTextures(Content, "items/item_", 2).ToArray();
            this.gameData.iconTextures = Utils.GetTextures(Content, "icons/icon_", 3).ToArray();
            this.gameData.groundTextures = Utils.GetTextures(Content, "ground/ground_", 4).ToArray();
            this.gameData.playerTextures = Utils.GetTextures(Content, "player/player_", 1).ToArray();

            this.gameData.animationsTextures.Add(0, Utils.GetTextures(Content, "animations/box/box_", 6).ToArray());
            this.gameData.animationsTextures.Add(1, Utils.GetTextures(Content, "animations/tnt/tnt_", 5).ToArray());
        }

        private double rightPress = 0;

        protected override void Update(GameTime gameTime)
        {
            float deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if(Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                rightPress += deltatime * 1000;
            }
            if(Mouse.GetState().RightButton == ButtonState.Released && rightPress != 0)
            {
                if (rightPress > 10)
                {
                    this.showGrid = !showGrid;
                }
                rightPress = 0;
            }

            this.gameData.player.Update(deltatime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            Camera mainCamera = this.gameData.player.camera;
            this.gameData.player.Draw(this._spriteBatch, this._graphics, mainCamera, this.gameData);
            
            Utils.ListDraw(this.gameData.items, this._spriteBatch, this._graphics, mainCamera, this.gameData);
            Utils.ListDraw(this.gameData.blocks, this._spriteBatch, this._graphics, mainCamera, this.gameData);


            if(showGrid)
            {
                Utils.DrawGrid(this._spriteBatch, this.gameData.windowSize, Color.Gray, 1);
            }
            Utils.DrawRectangle(this._spriteBatch, Utils.ToRectangle(new Vector2(Mouse.GetState().Position.X / 100 * 100, Mouse.GetState().Position.Y / 100 * 100), new Vector2(100, 100)), Color.White, 1);

            base.Draw(gameTime);

        }
    }
}
