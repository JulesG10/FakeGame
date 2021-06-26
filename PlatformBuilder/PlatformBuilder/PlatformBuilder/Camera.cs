using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformBuilder.GameObjects
{

    class Camera
    {
        public Vector2 size { get;  set; }
        public Vector2 position { get; set; }

        public Camera(Vector2 size, Vector2 position)
        {
            this.size = size;
            this.position = position;
        }

        public bool isInView(Camera camera)
        {
            if (camera.position.X < this.position.X + this.size.X && camera.position.X + camera.size.X > this.position.X && camera.position.Y < this.position.Y + this.size.Y && camera.size.Y + camera.position.Y > this.position.Y)
            {
                return true;
            }
            return false;
        }

    }
}
