using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PlatformBuilder.GameObjects;

namespace PlatformBuilder
{
    class GameData
    {
        public List<Texture2D> groundTextures = new List<Texture2D>();
        public List<Texture2D> itemTextures = new List<Texture2D>();
        public List<Texture2D> iconTextures = new List<Texture2D>();
        public Dictionary<int, Texture2D[]> animationsTextures = new Dictionary<int, Texture2D[]>();

        public List<Texture2D> playerTextures = new List<Texture2D>();
        public Player player;

        public GameData(Vector2 winSize)
        {
            this.player = new Player(winSize);
        }

        

    }
}
