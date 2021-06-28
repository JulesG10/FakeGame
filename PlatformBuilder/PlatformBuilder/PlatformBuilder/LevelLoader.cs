using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Text;

namespace PlatformBuilder
{
    class LevelLoader
    {
        private string folderpath;

        public LevelLoader(string path)
        {
            this.folderpath = path;
        }

        public Level Load(string name)
        {
            Level level = new Level();
            string path = Path.Combine(this.folderpath, name);
            if (File.Exists(path))
            {
                byte[] levelData = File.ReadAllBytes(path);
                string[] levelStr = Encoding.UTF8.GetString(levelData).Split('\n');
            }

            return level;
        }
        
    }

    class Level
    {
        public Vector2 position;

        public Object[] items;
        public Object[] blocks;
    }
}
