using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HalloweenStory.Screens;

namespace HalloweenStory.Objects
{
    class Portal
    {
        #region Fields
        Animation animation;
        Rectangle portalRect;
        #endregion
        #region Constructors
        public Portal(ContentManager Content, Vector2 position)
        {
            animation = new Animation(Content.Load<Texture2D>("Graveyard/portal"),Vector2.Zero, 12, 100, true);
            animation.FlipEffect = SpriteEffects.None;
            animation.Position = position;
            portalRect = new Rectangle((int)position.X, (int)position.Y, animation.Width, animation.Height);
        }
        #endregion
        #region Update
        public void Update(GameTime gameTime, Player player)
        {
            Rectangle playerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);
            if (playerRect.Intersects(portalRect) && player.IsAlive)
            {                               
                player.Position = Vector2.Lerp(player.Position, animation.Position + new Vector2(130,150), 0.04f);
                if (player.Position.X >= animation.Position.X + 125 &&player.Position.X <= animation.Position.X + 135) ScreenManager.Instance.ChangeScreen(new BossScreen(player));
            }
            animation.Update(gameTime);
        }
        #endregion
        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
        #endregion
    }
}
