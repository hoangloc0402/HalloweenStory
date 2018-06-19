using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory.Traps
{
    abstract class Trap
    {
        #region Fields
        protected Animation animation;
        protected Vector2 position;
        protected Rectangle trapRect;  //hình chữ nhật của trap
        protected int damage;
        #endregion
        #region Update
        public virtual void Update(GameTime gameTime, Player player)
        {                                                                                     
            if(Hurt(player))
            {
                player.HP -= damage;  //trừ máu player
            }
                                              
            animation.Update(gameTime);
        }
        #endregion
        #region Draw
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            animation.Draw(spriteBatch);
        }
        #endregion
        #region Methods
        protected virtual bool Hurt(Player player)  //trả về true nếu trúng player
        {
            Rectangle playerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);
            if (trapRect.Intersects(playerRect)) return true;
            else return false;
        }
        #endregion
    }
}
