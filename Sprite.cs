using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory
{
    class Sprite
    {
        #region Fields
        public Texture2D Texture; 
        public Vector2 Position = Vector2.Zero;
        public float ParallaxScale = 0; //càng gần 0  thì vật càng gần, càng gần 1 thì vật càng xa
        public float Speed = 0;
        #endregion
        #region Constructors
        public Sprite(Texture2D texture, Vector2 position, float parallaxScale, float speed)
        {
            this.Texture = texture;
            this.Position = position;
            this.ParallaxScale = parallaxScale;
            this.Speed = speed;
        }
        public Sprite(Texture2D texture, Vector2 position, float parallaxScale)
        {
            this.Texture = texture;
            this.Position = position;
            this.ParallaxScale = parallaxScale; 
        }
        public Sprite(Texture2D texture, Vector2 position)
        {
            this.Texture = texture;
            this.Position = position;  
        }
        public Sprite(Texture2D texture)
        {
            this.Texture = texture;    
        }
        public Sprite(Texture2D texture, float parallaxScale)
        {
            this.Texture = texture;       
            this.ParallaxScale = parallaxScale; 
        }
        #endregion
    }
}
