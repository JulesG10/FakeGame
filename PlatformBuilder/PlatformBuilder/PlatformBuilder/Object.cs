using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PlatformBuilder.GameObjects
{
    class Object
    {
        public virtual Camera camera { get; protected set; }
        public virtual Vector2 size { get; protected set; }
        public virtual Vector2 position { get; set; }

        public event CreateEventHandler CreateEvent;
        public delegate void CreateEventHandler(object source, EventArgs args);
        protected virtual void OnCreate(object source, EventArgs args)
        {
            if(this.CreateEvent != null)
            {
                CreateEvent(this, args);
            }
        }

        public delegate void DestroyEventHandler(object source, EventArgs args);
        public event DestroyEventHandler DestroyEvent;
        protected virtual void OnDestroy(object source, EventArgs args)
        {
            if (this.DestroyEvent != null)
            {
                DestroyEvent(this, args);
            }
        }

        public  Object(Vector2 winSize,Vector2 size, Vector2 position)
        {
            this.ConfigureObject(winSize, size, position);
        }

        public  Object(Vector2 winSize, Vector2 size)
        {
            this.ConfigureObject(winSize, size, new Vector2(0, 0));

        }

        public Object(Vector2 winSize)
        {
            this.ConfigureObject(winSize, new Vector2(0, 0), new Vector2(0, 0));
        }

        private void ConfigureObject(Vector2 winSize, Vector2 size, Vector2 position)
        {
            this.size = size;
            this.position = position;

            this.camera = new Camera(winSize, this.position);
            this.camera.position = new Vector2(0, 0);
        }

        public Vector2 GetPosition(Camera mainCamera)
        {
            return new Vector2(this.position.X - mainCamera.position.X, this.position.Y - mainCamera.position.Y);
        }

        public Vector2 GetPosition(Vector2 mainCamera)
        {
            return new Vector2(this.position.X - mainCamera.X, this.position.Y - mainCamera.Y);
        }

        public Vector2 GetStaticPosition(Camera mainCamera)
        {
            return new Vector2(this.position.X + mainCamera.position.X, this.position.Y + mainCamera.position.Y);
        }

        public Vector2 GetStaticPosition(Vector2 mainCamera)
        {
            return new Vector2(this.position.X + mainCamera.X, this.position.Y + mainCamera.Y);
        }

        public virtual bool AABB(Object obj)
        {
            return Utils.AABB(this.position, this.size, obj.position, obj.size);
        }

        public virtual void Update(float deltatime, GameData gameData)
        {
        }

        public virtual bool Draw(SpriteBatch spriteBatch, GraphicsDeviceManager graphicsDeviceManager, Camera mainCamera, GameData data)
        {
            if(this.camera.isInView(mainCamera))
            {
                return true;
            }

            return false;
        }
    }
}
