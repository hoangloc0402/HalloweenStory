using HalloweenStory.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory.Screens
{
    class WinningScreen : GameScreen
    {
        #region Fields
        public bool IsPopUp = false;//pop up màn hình win
        Texture2D background;//ảnh nển của WinningScreen
        Texture2D score;

        float width;
        
        Button restartButton;
        Button exitButton;
        #endregion
        #region Load Content
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            background = Content.Load<Texture2D>("WinningScreen/Background");
            
            score = Content.Load<Texture2D>("WinningScreen/score");
            
            restartButton = new Button(Content.Load<Texture2D>("WinningScreen/restart"), Content.Load<Texture2D>("WinningScreen/restarthover"), Content.Load<Texture2D>("WinningScreen/restartpressed"), new Vector2(525, 465));
            restartButton.ButtonClick += restartButton_ButtonClick;
            exitButton = new Button(Content.Load<Texture2D>("WinningScreen/exit"), Content.Load<Texture2D>("WinningScreen/exithover"), Content.Load<Texture2D>("WinningScreen/exitpressed"), new Vector2(525, 565));
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
        public void Draw(SpriteBatch spriteBatch,Player player)
        {
            Rectangle temp;            
            width = MathHelper.Lerp(width, (float)player.PlayerScore / Player.PLAYER_MAX_SCORE * 357, 0.05f);

            temp = new Rectangle(504, 404, (int)width, 28);

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
                ScreenManager.Instance.ChangeScreen(new GraveyardScreen());
            }
            else if (ScreenManager.Instance.currentScreen.Type == ScreenType.Graveyard)
            {
                ScreenManager.Instance.ChangeScreen(new GraveyardScreen());
            }
        }
        #endregion

    }
}

