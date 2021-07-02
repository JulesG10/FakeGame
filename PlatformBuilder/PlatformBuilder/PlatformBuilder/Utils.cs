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

        public static Vector2[] LineVector2(Vector2 p1,Vector2 p2,int lineWidth)
        {
            float dx = p2.X - p1.X;
            float dy = p2.Y - p1.Y;

            float x = p1.X;
            float y = p1.Y;

            float p = 2 * dy - dx;

            List<Vector2> result = new List<Vector2>();

            while (x < p2.X)
            {
                if (p >= 0)
                {
                    result.Add(new Vector2(x, y));
                    y = y + 1;
                    p = p + 2 * dy - 2 * dx;
                }
                else
                {
                    result.Add(new Vector2(x, y));
                    p = p + 2 * dy;
                }
                x += lineWidth;
            }

            return result.ToArray();
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


        public static void ProceduralBlockGenerator(ref GameData gameData,Vector2 mapSize,int startX = 0)
        {
            PerlinNoise perlinNoise = new PerlinNoise();
            perlinNoise.SetDetail(10, 0.5);

            Vector2[] blocks = perlinNoise.Generate2D(mapSize, 1, startX, 0.01, 0.05, 20);
            double maxY = (gameData.windowSize.Y / 2 + (MainGame.tileSize * 20));
            Vector2 last = new Vector2(-1, -1);

            Random rnd = new Random();

            for (int i = 0; i < blocks.Length; i++)
            {
                Vector2 pos = new Vector2(((int)(blocks[i].X / MainGame.tileSize) * MainGame.tileSize), ((int)(blocks[i].Y / MainGame.tileSize) * MainGame.tileSize));
                if (last == new Vector2(-1, -1))
                {
                    last = pos;
                }
                if (gameData.blocks.Count == 0 || gameData.blocks[gameData.blocks.Count-1].position.X != pos.X)
                {
                    gameData.blocks.Add(new Block(gameData.windowSize, pos, BlockType.GROUND));

                    if(rnd.Next(0,50) == 0)
                    {
                        gameData.boxs.Add(new Box(gameData.windowSize, new Vector2(pos.X, pos.Y - 90)));
                    }

                    if (rnd.Next(0, 20) == 0)
                    {
                        gameData.items.Add(new Item(gameData.windowSize, new Vector2(pos.X, pos.Y - 25), ItemType.ROCK_BLOCK));
                    }



                    for (int k = (int)(pos.Y + MainGame.tileSize); k < maxY; k += MainGame.tileSize)
                    {
                        gameData.blocks.Add(new Block(gameData.windowSize, new Vector2(pos.X, k), BlockType.FILL_GROUND));
                    }
                }
               
                
                
                last = pos;
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
