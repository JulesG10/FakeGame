using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformBuilder.GameObjects
{
    class Utils
    {
        public static List<Texture2D> GetTextures(ContentManager Content,string id, int length)
        {
            List<Texture2D> textures2D = new List<Texture2D>();
            for (int i = 0; i < length; i++)
            {
                textures2D.Add(Content.Load<Texture2D>(id + i.ToString()));
            }

            return textures2D;
        }

        public static List<Texture2D> GetTextures(ContentManager Content, string[] texturesID)
        {
            List<Texture2D> textures2D = new List<Texture2D>();
            for (int i = 0; i < texturesID.Length; i++)
            {
                textures2D.Add(Content.Load<Texture2D>(texturesID[i]));
            }

            return textures2D;
        }
    }
}
