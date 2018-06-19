using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory.Traps
{
    class CampFireTrap :Trap
    {
        #region Constructors
        public CampFireTrap(ContentManager Content, Vector2 position)
        {
            this.animation = new Animation(Content.Load<Texture2D>("Trap/campfire_6"), new Vector2(84, 88), new Vector2(83, 78), 6, 100, true);
            animation.Position = position;
            trapRect = new Rectangle((int)position.X, (int)position.Y, animation.Width, animation.Height);
            this.damage = 20;
        }
        #endregion
    }
}
