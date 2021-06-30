using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformBuilder.GameObjects
{
    enum Direction
    {
        TOP,
        BOTTOM,
        LEFT,
        RIGHT,
        NONE
    }

    class Utils
    {

        public static List<T> LoadList<T>(ContentManager Content, string id, int length)
        {
            List<T> textures2D = new List<T>();
            for (int i = 0; i < length; i++)
            {
                textures2D.Add(Content.Load<T>(id + i.ToString()));
            }

            return textures2D;
        }

        public static List<T> LoadList<T>(ContentManager Content, string[] texturesID)
        {
            List<T> textures2D = new List<T>();
            for (int i = 0; i < texturesID.Length; i++)
            {
                textures2D.Add(Content.Load<T>(texturesID[i]));
            }

            return textures2D;
        }

        public static Rectangle ToRectangle(Vector2 position, Vector2 size)
        {
            return new Rectangle(new Point((int)position.X, (int)position.Y), new Point((int)size.X, (int)size.Y));
        }

        public static void ListUpdate(List<Object> objs, float deltatime,GameData gameData)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].Update(deltatime, gameData);
            }
        }

        public static void ListDraw(List<Object> objs, SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera mainCamera, GameData gameData)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                objs[i].Draw(spriteBatch, graphicsDeviceManager, mainCamera, gameData);
            }
        }

        public static bool AABB(Vector2 p1,Vector2 s1,Vector2 p2,Vector2 s2)
        {
            if (p1.X < p2.X + s2.X && p1.X + s1.X > p2.X && p1.Y < p2.Y + s2.Y && s1.Y + p1.Y > p2.Y)
            {
                return true;
            }
            return false;
        }

        public static Direction[] AABBDirection(Vector2 p1, Vector2 s1, Vector2 p2, Vector2 s2)
        {
            Direction[] dir = { Direction.NONE,Direction.NONE };
            float p1b = p1.Y + s1.Y;
            float p2b = p2.Y + s2.Y;

            float p1r = p1.X + s1.X;
            float p2r = p2.X + s2.X;

            int b_collision = (int)Math.Floor(p2b - p1.Y) - 1;
            int t_collision = (int)Math.Floor(p1b - p2.Y) - 1;

            int l_collision = (int)Math.Floor(p1r - p2.X) - 1;
            int r_collision = (int)Math.Floor(p2r - p1.X) - 1;

            if (t_collision < b_collision && t_collision < l_collision && t_collision < r_collision)
            {
                dir[0] = Direction.TOP;
            }
            if (b_collision < t_collision && b_collision < l_collision && b_collision < r_collision)
            {
                dir[0] = Direction.BOTTOM;
            }
            
            if (l_collision < r_collision && l_collision < t_collision && l_collision < b_collision)
            {
                dir[1] = Direction.LEFT;
            }
            if (r_collision < l_collision && r_collision < t_collision && r_collision < b_collision)
            {
                dir[1] = Direction.RIGHT;
            }

            return dir;
        }

        private static Texture2D _pointTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }

        private static Texture2D _lineTexture;
        public static void DrawLine(SpriteBatch sb, Vector2 start, Vector2 end,Color c)
        {
            if (_lineTexture == null)
            {
                _lineTexture = new Texture2D(sb.GraphicsDevice, 1, 1);
                _lineTexture.SetData<Color>(new Color[] { Color.White });
            }
            Vector2 edge = end - start;
            float angle =
                (float)Math.Atan2(edge.Y, edge.X);


            sb.Draw(_lineTexture,
                new Rectangle(
                    (int)start.X,
                    (int)start.Y,
                    (int)edge.Length(),
                    1),
                null,
                c,
                angle,
                new Vector2(0, 0),
                SpriteEffects.None,
                0);
        }

        public static void DrawGrid(SpriteBatch spriteBatch,Vector2 size,Color color,int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            for (int i = 0; i < size.X; i += MainGame.tileSize)
            {
                spriteBatch.Draw(_pointTexture, new Rectangle(0, i, (int)size.X, lineWidth), color);
            }
            for (int i = 0; i < size.X; i += MainGame.tileSize)
            {
                spriteBatch.Draw(_pointTexture, new Rectangle(i,0, lineWidth, (int)size.Y), color);
            }

        }

        public static void RandomBlockGenerator(ref GameData gameData, int startX, int endX)
        {
            int index = 0;
            bool lastNone = false;

            for (int i = startX; i < endX; i++)
            {
                Random random = new Random();
                if (random.Next(0, 6) != 0)
                {
                    int add = 0;
                    if(!lastNone && random.Next(0, 8) == 0)
                    {
                        gameData.blocks.Add(new Block(gameData.windowSize, new Vector2(i * MainGame.tileSize, (gameData.windowSize.Y / 2 + (MainGame.tileSize * 2))), BlockType.FILL_GROUND));
                        index++;
                        add = MainGame.tileSize;
                    }
                    Vector2 position = new Vector2(i * MainGame.tileSize, (gameData.windowSize.Y / 2 + (MainGame.tileSize * 2)-add));
                    
                    if (random.Next(0, 12) == 0)
                    {
                        gameData.items.Add(new Item(gameData.windowSize, new Vector2(position.X, position.Y - 25), ItemType.ROCK_BLOCK));
                    }
                    else if(random.Next(0,30) == 0)
                    {
                        gameData.boxs.Add(new Box(gameData.windowSize, new Vector2(position.X, position.Y - 90)));
                    }

                    if (lastNone)
                    {
                        lastNone = false;
                        gameData.blocks.Add(new Block(gameData.windowSize, position, BlockType.GROUND_END_LEFT));
                    }
                    else
                    {
                        gameData.blocks.Add(new Block(gameData.windowSize, position, BlockType.GROUND));
                    }

                    index++;
                }
                else
                {
                    lastNone = true;
                    if (index - 1 >= 0 && gameData.blocks[index - 1] != null)
                    {
                        Block b = (Block)gameData.blocks[index - 1];

                        if (b.type == BlockType.GROUND_END_LEFT)
                        {
                            b.type = BlockType.GROUND_NO_GRASS;
                        }
                        else
                        {
                            b.type = BlockType.GROUND_END_RIGHT;
                        }

                        gameData.blocks[index - 1] = b;
                    }
                }
            }
        }
    }
}
