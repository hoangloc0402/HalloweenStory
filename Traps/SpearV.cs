using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HalloweenStory.Traps
{
    class SpearV : Trap
    {
        #region Fields
        protected double elapsedTime;
        protected bool delay = true;
        protected double delayTime;
        #endregion
        #region Constructors
        public SpearV(ContentManager Content, Vector2 position, double delayTime)
        {
            this.animation = new Animation(Content.Load<Texture2D>("Trap/spearV_16"), new Vector2(10, 10), new Vector2(10, 10), 16, 100, true);
            this.delayTime = delayTime;

            animation.Position = position;
            trapRect = new Rectangle((int)position.X, (int)position.Y, animation.Width, animation.Height);
            this.damage = 30;
        }
        public SpearV() { }
        #endregion
        #region Update
        public override void Update(GameTime gameTime, Player player)
        {
            if (delay) elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (Hurt(player))
            {
                player.HP -= damage;
            }
            if (elapsedTime >= delayTime)
            {
                animation.Update(gameTime);
                delay = false;
            }
        }
        #endregion
        #region Methods
        protected override bool Hurt(Player player)
        {
            Rectangle playerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);
            if (animation.currentFrame >= 4 && animation.currentFrame <= 7 && trapRect.Intersects(playerRect)) return true;
            else return false;
        }
        #endregion
    }
}
