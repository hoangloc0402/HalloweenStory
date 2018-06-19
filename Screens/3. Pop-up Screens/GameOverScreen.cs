using HalloweenStory.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory.Screens
{
    class GameOverScreen : GameScreen
    {
        #region Fields
        public bool IsPopUp = false;//pop up màn hình gameover
        Texture2D background;//ảnh nển của GameOverScreen
        Texture2D score;

        float width ;

        Button restartButton;
        Button exitButton;
        #endregion
        #region LoadContent
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            background = Content.Load<Texture2D>("GameOverScreen/Background");
            score = Content.Load<Texture2D>("GameOverScreen/score");

            restartButton = new Button(Content.Load<Texture2D>("GameOverScreen/restart"), Content.Load<Texture2D>("GameOverScreen/restarthover"), Content.Load<Texture2D>("GameOverScreen/restartpressed"), new Vector2(525, 465));
            restartButton.ButtonClick += restartButton_ButtonClick;
            exitButton = new Button(Content.Load<Texture2D>("GameOverScreen/exit"), Content.Load<Texture2D>("GameOverScreen/exithover"), Content.Load<Texture2D>("GameOverScreen/exitpressed"), new Vector2(525, 565));
            exitButton.ButtonClick += ExitButton_ButtonClick;
            width = 0f;

        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            restartButton.Update(gameTime);
            exitButton.Update(gameTime);
        }
        #endregion
        #region Draw
        public void Draw(SpriteBatch spriteBatch, Player player)
        {
            Rectangle temp;          
            width = MathHelper.Lerp(width,(float)player.PlayerScore / Player.PLAYER_MAX_SCORE * 357,0.05f); 
       
            temp = new Rectangle(504, 405, (int)width, 28);
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            spriteBatch.Draw(score, temp, Color.White);

            restartButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
        }
        #endregion
        #region Events Handler
        private void ExitButton_ButtonClick()
        {
            ScreenManager.Instance.Game.Exit();
        }

        private void restartButton_ButtonClick()
        {
            IsPopUp = false;
            if (ScreenManager.Instance.currentScreen.Type == ScreenType.Boss)
            {
                ScreenManager.Instance.ChangeScreen(new BossScreen());
            }
            else if (ScreenManager.Instance.currentScreen.Type == ScreenType.Graveyard)
            {
                ScreenManager.Instance.ChangeScreen(new GraveyardScreen());
            }
        }
        #endregion
    }
}

