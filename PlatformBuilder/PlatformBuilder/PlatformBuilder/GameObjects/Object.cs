using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformBuilder.GameObjects
{
    public class Object
    {
        public virtual Camera camera { get; protected set; }
        public virtual Vector2 size { get; private set; }
        public virtual Vector2 position { get; set; }

        public delegate void OnCreate(object source, EventArgs args);
        public event OnCreate CreateEvent;

        public delegate void OnDestroy(object source, EventArgs args);
        public event OnDestroy DestroyEvent;

        public Object(Vector2 winSize,Vector2 size, Vector2 position)
        {
            this.size = size;
            this.position = position;

            this.camera = new Camera(winSize, this.position);
            this.camera.states = CameraStates.ObjectCenterFocus;
        }

        public  Object(Vector2 winSize, Vector2 size)
        {
            this.size = size;
            this.position = new Vector2(0,0);

            this.camera = new Camera(winSize, this.position);
            this.camera.states = CameraStates.ObjectCenterFocus;
        }

        public Object(Vector2 winSize)
        {
            this.size = new Vector2(0, 0);
            this.position = new Vector2(0, 0);

            this.camera = new Camera(winSize, this.position);
            this.camera.states = CameraStates.ObjectCenterFocus;
        }

        public virtual bool AABB(Object obj)
        {
            if (obj.position.X < this.position.X + this.size.X && obj.position.X + obj.size.X > this.position.X && obj.position.Y < this.position.Y + this.size.Y && obj.size.Y + obj.position.Y > this.position.Y)
            {
                return true;
            }
            return false;
        }

        public virtual void Update(float deltatime)
        {
            switch(this.camera.states)
            {
                case CameraStates.ObjectFocus:
                    this.camera.position = this.position;
                    break;
                case CameraStates.ObjectCenterFocus:
                    this.camera.position = new Vector2(this.position.X + (this.camera.size.X - this.size.X / 2), this.position.Y + (this.camera.size.Y - this.size.Y / 2));
                    break;
            }
        }

        public bool isInView(Camera camera)
        {
            return camera.isInView(this.camera);
        }

        public virtual void Draw(SpriteBatch spriteBatch, Camera mainCamera, GameData data)
        {
            
        }
    }
}
