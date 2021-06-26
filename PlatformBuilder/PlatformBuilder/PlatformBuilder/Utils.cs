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
        public static List<Texture2D> GetTextures(ContentManager Content, string id, int length)
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

        public static Rectangle ToRectangle(Vector2 position, Vector2 size)
        {
            return new Rectangle(new Point((int)position.X, (int)position.Y), new Point((int)size.X, (int)size.Y));
        }

        public static void ListUpdate(List<Object> objs, float deltatime)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].Update(deltatime);
            }
        }

        public static void ListDraw(List<Object> objs, SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera mainCamera, GameData gameData)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].Draw(spriteBatch, graphicsDeviceManager, mainCamera, gameData);
            }
        }

        private static Texture2D _pointTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }
            spriteBatch.Begin();
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.End();
        }

        public static void DrawGrid(SpriteBatch spriteBatch,Vector2 size,Color color,int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }
            spriteBatch.Begin();

            for (int i = 0; i < size.X; i += 100)
            {
                spriteBatch.Draw(_pointTexture, new Rectangle(0, i, (int)size.X, lineWidth), color);
            }
            for (int i = 0; i < size.X; i += 100)
            {
                spriteBatch.Draw(_pointTexture, new Rectangle(i, 0, lineWidth, (int)size.Y), color);
            }

            spriteBatch.End();
        }
    }
}
