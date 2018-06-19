using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory.Traps
{
    class Torch : Trap
    {
        #region Constructors
        public Torch(Animation animation, Vector2 position)
        {
            this.animation = animation;
            this.animation.Position = position;
            trapRect = new Rectangle((int)position.X, (int)position.Y, animation.Width, animation.Height);
            this.damage = 10;
        }
        #endregion
    }
}
