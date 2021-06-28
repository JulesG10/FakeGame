﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformBuilder.GameObjects;
using System;
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
        public static  int tileSize = 100;

        public MainGame()
        {
            Vector2 windowSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width/ tileSize * tileSize + tileSize, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / tileSize * tileSize + tileSize);
            
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)windowSize.X;
            _graphics.PreferredBackBufferHeight = (int)windowSize.Y;
            _graphics.SynchronizeWithVerticalRetrace = false;

            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            
            
            IsFixedTimeStep = false;
           // TargetElapsedTime = System.TimeSpan.FromSeconds(1d / 1000);
            

            this.gameData = new GameData(windowSize);


            for (int i = -200;i < 200;i++)
            {

                Random random = new Random();
                if(random.Next(0,6) != 0)
                {
                    Vector2 position = new Vector2(i * tileSize, (windowSize.Y / 2 + (tileSize*2)));
                    if (random.Next(0,12) == 0)
                    {
                        this.gameData.items.Add(new Item(this.gameData.windowSize, new Vector2(position.X,position.Y - 25), ItemType.ROCK_BLOCK));
                    }

                    this.gameData.blocks.Add(new Block(this.gameData.windowSize, position, BlockType.GROUND));
                }
                
            }
        }


        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            this.gameData.itemTextures = Utils.LoadList<Texture2D>(Content, "items/item_", 2).ToArray();
            this.gameData.iconTextures = Utils.LoadList<Texture2D>(Content, "icons/icon_", 3).ToArray();
            this.gameData.groundTextures = Utils.LoadList<Texture2D>(Content, "ground/ground_", 4).ToArray();
            this.gameData.L_playerTextures = Utils.LoadList<Texture2D>(Content, "player/left/l_player_", 6).ToArray();
            this.gameData.R_playerTextures = Utils.LoadList<Texture2D>(Content, "player/right/r_player_", 6).ToArray();

            this.gameData.animationsTextures.Add(0, Utils.LoadList<Texture2D>(Content, "animations/box/box_", 6).ToArray());
            this.gameData.animationsTextures.Add(1, Utils.LoadList<Texture2D>(Content, "animations/tnt/tnt_", 5).ToArray());

            string[] effects = { "effects/test" };
            this.gameData.effects = Utils.LoadList<Effect>(Content, effects).ToArray();

            this.gameData.font = Content.Load<SpriteFont>("font");
        }

        private int previousScrollValue;
        private int GetScroll()
        {
            int res = 0;
            if (Mouse.GetState().ScrollWheelValue < previousScrollValue)
            {
                res = -1;
            }
            else if (Mouse.GetState().ScrollWheelValue > previousScrollValue)
            {
                res = 1;
            }
            previousScrollValue = Mouse.GetState().ScrollWheelValue;
            return res;
        }

        private double rightPress = 0;

        protected override void Update(GameTime gameTime)
        {
            float deltatime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                rightPress += deltatime * 1000;
            }
            if (Mouse.GetState().RightButton == ButtonState.Released && rightPress != 0)
            {
                if (rightPress > 10)
                {
                    this.showGrid = !showGrid;
                }
                rightPress = 0;
            }

            this.gameData.player.Update(deltatime,this.gameData);
            base.Update(gameTime);
           
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);
            this._spriteBatch.GraphicsDevice.Clear(Color.Black);

            //this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            //this._spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            this._spriteBatch.Begin();

            Camera mainCamera = this.gameData.player.camera;
            this.gameData.player.Draw(this._spriteBatch, this._graphics, mainCamera, this.gameData);

            Utils.ListDraw(this.gameData.items, this._spriteBatch, this._graphics, mainCamera, this.gameData);
            Utils.ListDraw(this.gameData.blocks, this._spriteBatch, this._graphics, mainCamera, this.gameData);

#if DEBUG
            string info = "FPS: " + (1.0 / gameTime.ElapsedGameTime.TotalSeconds).ToString() + "\n(" + mainCamera.position.X + ";" + mainCamera.position.Y + ")\nTime: " + gameTime.TotalGameTime.TotalSeconds + "\n";

            _spriteBatch.DrawString(this.gameData.font, info, new Vector2(10, 10), Color.White);
            if(showGrid)
            {
                Utils.DrawGrid(this._spriteBatch, this.gameData.windowSize, Color.Gray, 1);
            }

            Utils.DrawRectangle(this._spriteBatch, Utils.ToRectangle(new Vector2(Mouse.GetState().Position.X / tileSize * tileSize, Mouse.GetState().Position.Y / tileSize * tileSize), new Vector2(tileSize, tileSize)), Color.White, 1);
#endif
            this._spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
