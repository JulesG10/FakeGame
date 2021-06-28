using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformBuilder.GameObjects;
using Object = PlatformBuilder.GameObjects.Object;

namespace PlatformBuilder
{
    class GameData
    {
        public Vector2 windowSize { get; private set; }
        public Texture2D[] groundTextures;
        public Texture2D[] itemTextures;
        public Texture2D[] iconTextures;
        public Effect[] effects;
        public Dictionary<int, Texture2D[]> animationsTextures = new Dictionary<int, Texture2D[]>();
        public SpriteFont font;

        public Texture2D[] R_playerTextures;
        public Texture2D[] L_playerTextures;
        public Texture2D[] R_playerJumpTextures;
        public Texture2D[] L_playerJumpTextures;
        public Player player;

        public List<Object> items = new List<Object>();
        public List<Object> blocks = new List<Object>();

        public GameData(Vector2 winSize)
        {
            this.windowSize = winSize;
            this.player = new Player(winSize);
        }

        

    }
}
