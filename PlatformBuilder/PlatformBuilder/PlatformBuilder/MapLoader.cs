using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using System.Text;
using System.Collections.Generic;
using PlatformBuilder.GameObjects;
using System.Linq;
using System.Threading.Tasks;

namespace PlatformBuilder
{
    class MapLoader
    {
        private string folderpath;

        public MapLoader(string path)
        {
            this.folderpath = path;
        }

        public void Load(string name)
        {
            string path = Path.Combine(this.folderpath, name);
            if (File.Exists(path))
            {
                byte[] levelData = File.ReadAllBytes(path);
                string[] levelStr = Encoding.UTF8.GetString(levelData).Split('\n');
            }

           
        }

    }

  
}
