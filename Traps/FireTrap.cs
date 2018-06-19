using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory.Traps
{
    class FireTrap : Trap 
    {
        #region Constructors
        public FireTrap(ContentManager Content, Vector2 position)
        {
            this.animation = new Animation(Content.Load<Texture2D>("Trap/fire_10"), new Vector2(31, 34), new Vector2(26, 16), 10, 150, true);
            animation.Position = position;
            trapRect = new Rectangle((int)position.X, (int)position.Y, animation.Width, animation.Height);
            this.damage = 50;
        }
        #endregion
        #region Methods
        protected override bool Hurt(Player player)
        {
            Rectangle playerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);
            if (animation.currentFrame >= 2 && animation.currentFrame <= 4 && trapRect.Intersects(playerRect)) return true;
            else return false;
        }
        #endregion
    }
}
