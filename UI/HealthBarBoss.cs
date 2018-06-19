using HalloweenStory.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory.UI
{
    class HealthBarBoss
    {
        #region Fields
        protected Texture2D healthTexture;  //hình ảnh của thanh máu
        protected Rectangle healthRectangle; //hình chữ nhật để vẽ thanh máu
        protected Vector2 healthPosition;    //vị trí của thanh máu

        protected Texture2D healthborderTexture;//hình ảnh của viền thanh máu
        protected Rectangle healthborderRectangle;  //hình chữ nhật để vẽ viền thanh máu
        protected Vector2 healthborderPosition;   //vị trí của viền thanh máu

        protected float widthHP; //chiều dài của thanh HP

        #endregion
        #region Load Content
        public virtual void LoadContent(ContentManager Content)//load dữ liệu
        {
            healthTexture = Content.Load<Texture2D>("HealthBar/healthbar2");
            healthborderTexture = Content.Load<Texture2D>("HealthBar/HealthBarBorderBoss");
            widthHP = 0;
        }
        #endregion
        #region Update
        public virtual void Update(GameTime gameTime, Vector2 cameraPosition, Dragon boss)
        {

            healthPosition.X = cameraPosition.X + 684 ;
            healthPosition.Y = cameraPosition.Y + 70;
            widthHP = MathHelper.Lerp(widthHP, (float)boss.HP / boss.MaxHP * 417f, 0.05f);
            healthRectangle = new Rectangle((int)(healthPosition.X + 595f - widthHP), (int)healthPosition.Y, (int)widthHP, 36);

            healthborderPosition.X = cameraPosition.X + 858;
            healthborderPosition.Y = cameraPosition.Y + 29;                                                       
            healthborderRectangle = new Rectangle((int)healthborderPosition.X, (int)healthborderPosition.Y, 480, 81);
        }
        #endregion
        #region Draw
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(healthTexture, healthRectangle, Color.White);
            spriteBatch.Draw(healthborderTexture, healthborderRectangle, Color.White);
        }
        #endregion
    }
}
