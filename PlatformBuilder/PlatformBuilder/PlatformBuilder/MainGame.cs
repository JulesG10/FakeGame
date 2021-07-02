using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformBuilder.GameObjects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace PlatformBuilder
{
    class MainGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private GameData gameData;
        private HUD hud;
        private bool showGrid = false;
        public static int tileSize = 100;


        public MainGame()
        {
            Vector2 windowSize = new Vector2(GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / tileSize * tileSize + tileSize, GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / tileSize * tileSize + tileSize);

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = (int)windowSize.X;
            _graphics.PreferredBackBufferHeight = (int)windowSize.Y;
            _graphics.SynchronizeWithVerticalRetrace = false;

            _graphics.IsFullScreen = true;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            IsFixedTimeStep = false;
            // if fps < 30 block collision bug
            //TargetElapsedTime = System.TimeSpan.FromSeconds(1d / 30);

            this.gameData = new GameData(windowSize);
            this.hud = new HUD(windowSize);

            //Utils.RandomBlockGenerator(ref gameData, -200, 200);
            Utils.ProceduralBlockGenerator(ref gameData, windowSize);
            gameData.player.camera.position = new Vector2(-tileSize,-tileSize*20);// Vector2(gameData.blocks[0].position.X, gameData.blocks[0].position.Y - gameData.player.size.Y*2);
          
            /*for (int i = -200; i < 200; i++)
            {
                gameData.blocks.Add(new Block(gameData.windowSize, new Vector2(i * MainGame.tileSize, (gameData.windowSize.Y / 2 + (MainGame.tileSize * 20))), BlockType.BORDER));
            }*/

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
            this.gameData.groundTextures = Utils.LoadList<Texture2D>(Content, "ground/ground_", 6).ToArray();
            this.gameData.L_playerTextures = Utils.LoadList<Texture2D>(Content, "player/left/l_player_", 6).ToArray();
            this.gameData.R_playerTextures = Utils.LoadList<Texture2D>(Content, "player/right/r_player_", 6).ToArray();
            this.gameData.L_playerJumpTextures = Utils.LoadList<Texture2D>(Content, "player/jump/left/l_jump_", 4).ToArray();
            this.gameData.R_playerJumpTextures = Utils.LoadList<Texture2D>(Content, "player/jump/right/r_jump_", 4).ToArray();
            this.gameData.boxTextures = Utils.LoadList<Texture2D>(Content, "animations/box/box_", 6).ToArray();

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
        private int generated = 1;

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

           

            if (gameData.player.isAlive)
            {
                this.gameData.player.Update(deltatime, this.gameData);
                Utils.ListUpdate(this.gameData.boxs, deltatime, this.gameData);
                
                double playerX = gameData.player.GetStaticPosition(gameData.player.getPositionHitBox(this.gameData.player.camera.position)).X;
                int reloadDistance = ((int)(gameData.windowSize.X / 2) / MainGame.tileSize);

                if (playerX + tileSize * reloadDistance > this.gameData.blocks[gameData.blocks.Count - 1].position.X)
                {
                    Utils.ProceduralBlockGenerator(ref gameData, this.gameData.windowSize, (int)(this.gameData.windowSize.X * this.generated) + MainGame.tileSize);
                    this.generated++;
                }
            }

            this.MaxDelta = Math.Max(this.MaxDelta, deltatime);
            base.Update(gameTime);
        }

        private bool activeHUD = true;
        private float MaxDelta = 0;

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);
            this._spriteBatch.GraphicsDevice.Clear(Color.Black);

            this._spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone);
            //this._spriteBatch.Begin(SpriteSortMode.Texture, BlendState.AlphaBlend, SamplerState.LinearWrap, DepthStencilState.None, RasterizerState.CullCounterClockwise);
            //this._spriteBatch.Begin();
            Camera mainCamera = this.gameData.player.camera;


            if (gameData.player.isAlive)
            {
                Utils.ListDraw(this.gameData.items, this._spriteBatch, this._graphics, mainCamera, this.gameData);
                Utils.ListDraw(this.gameData.boxs, this._spriteBatch, this._graphics, mainCamera, this.gameData);

                this.gameData.player.Draw(this._spriteBatch, this._graphics, mainCamera, this.gameData);

                Utils.ListDraw(this.gameData.blocks, this._spriteBatch, this._graphics, mainCamera, this.gameData);
            }
#if DEBUG

            if (Keyboard.GetState().IsKeyDown(Keys.I))
            {
                activeHUD = !activeHUD;
            }

            if (activeHUD)
            {
                string info = "FPS: " + (1.0 / gameTime.ElapsedGameTime.TotalSeconds).ToString() +
                    "\n(" + (int)mainCamera.position.X + ";" + (int)mainCamera.position.Y +
                    ")\nTime: " + gameTime.TotalGameTime.TotalSeconds +
                    "\nDeltatime: " + gameTime.ElapsedGameTime.TotalSeconds +
                "\nMax Delta: " + this.MaxDelta;

                _spriteBatch.DrawString(this.gameData.font, info, new Vector2(10, 10), Color.White);
                if (showGrid)
                {
                    Utils.DrawGrid(this._spriteBatch, this.gameData.windowSize, Color.Gray, 1);
                }

                Utils.DrawRectangle(this._spriteBatch, Utils.ToRectangle(new Vector2(Mouse.GetState().Position.X / tileSize * tileSize, Mouse.GetState().Position.Y / tileSize * tileSize), new Vector2(tileSize, tileSize)), Color.White, 1);
            }
            else
            {
                this.hud.Draw(this._spriteBatch, this._graphics, mainCamera, this.gameData);
            }

#endif
            this._spriteBatch.End();
            base.Draw(gameTime);

        }
    }
}
