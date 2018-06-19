using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HalloweenStory.UI
{
    class HealthBarPlayer : HealthBarBoss
    {
        #region Fields    
        protected Texture2D scoreTexture;  //hình ảnh của thanh score
        protected Rectangle scoreRectangle; //hình chữ nhật để vẽ thanh score
        protected Vector2 scorePosition;    //vị trí của thanh score
        
        protected float widthSC; //chiều dài của thanh Score
        #endregion
        #region Load Content
        public override void LoadContent(ContentManager Content) //load dữ liệu
        {
            healthTexture = Content.Load<Texture2D>("HealthBar/healthbar");
            scoreTexture = Content.Load<Texture2D>("HealthBar/healthbar3");
            healthborderTexture = Content.Load<Texture2D>("HealthBar/HealthBarBorder");
            widthHP = 0;
            widthSC = 0;
        }
        #endregion
        #region Update
        public void Update(GameTime gameTime, Vector2 cameraPosition, Player player) //cập nhập thanh máu
        {               
            healthPosition.X = cameraPosition.X + 113;
            healthPosition.Y = cameraPosition.Y + 57;
            widthHP = MathHelper.Lerp(widthHP ,(float)player.HP/ player.MaxHP * 339,0.05f);
            healthRectangle = new Rectangle((int)healthPosition.X, (int)healthPosition.Y,(int)widthHP , 23);

            scorePosition.X = cameraPosition.X + 93;
            scorePosition.Y = cameraPosition.Y + 89;
            widthSC = MathHelper.Lerp(widthSC, (float)player.PlayerScore / Player.PLAYER_MAX_SCORE * 360, 0.05f);
            scoreRectangle = new Rectangle((int)scorePosition.X, (int)scorePosition.Y, (int)widthSC, 23);
            
            healthborderPosition.X = cameraPosition.X + 25;
            healthborderPosition.Y = cameraPosition.Y + 25;               
            healthborderRectangle = new Rectangle((int)healthborderPosition.X, (int)healthborderPosition.Y, 432, 92);
        }
        #endregion
        #region Draw   
        public override  void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(scoreTexture, scoreRectangle, Color.White);
            base.Draw(spriteBatch);
        }
        #endregion
    }

}
