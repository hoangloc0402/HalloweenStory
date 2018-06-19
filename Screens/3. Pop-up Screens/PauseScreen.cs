using HalloweenStory.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory.Screens
{
    class PauseScreen : GameScreen
    {
        #region Fields
        public bool IsPopUp = false;//pop up màn hình pause  
        Texture2D background;//ảnh nển của pausescreen
        
        Button resumeButton;
        Button menuButton;
        Button exitButton;
        #endregion
        #region Load Content
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);

            background = Content.Load<Texture2D>("PauseScreen/background");
            
            resumeButton = new Button(Content.Load<Texture2D>("PauseScreen/resume"), Content.Load<Texture2D>("PauseScreen/resumehover"), Content.Load<Texture2D>("PauseScreen/resumepressed"), new Vector2(525, 355));
            resumeButton.ButtonClick += ResumeButton_ButtonClick;
            menuButton = new Button(Content.Load<Texture2D>("PauseScreen/menu"), Content.Load<Texture2D>("PauseScreen/menuhover"), Content.Load<Texture2D>("PauseScreen/menupressed"), new Vector2(525, 460));
            menuButton.ButtonClick += MenuButton_ButtonClick;                                                                  
            exitButton = new Button(Content.Load<Texture2D>("PauseScreen/exit"), Content.Load<Texture2D>("PauseScreen/exithover"), Content.Load<Texture2D>("PauseScreen/exitpressed"), new Vector2(525, 565));
            exitButton.ButtonClick += ExitButton_ButtonClick;
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            resumeButton.Update(gameTime);
            menuButton.Update(gameTime);
            exitButton.Update(gameTime);
        }
        #endregion
        #region Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(background, Vector2.Zero, Color.White);
            
            resumeButton.Draw(spriteBatch);
            menuButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
        }
        #endregion
        #region Events Handler
        private void ExitButton_ButtonClick()
        {
            ScreenManager.Instance.Game.Exit();
        }

        private void MenuButton_ButtonClick()
        {
            IsPopUp = false;
            ScreenManager.Instance.ChangeScreen(new MainMenuScreen());
        }

        private void ResumeButton_ButtonClick()
        {
            IsPopUp = !IsPopUp;
            ScreenManager.Instance.Game.IsMouseVisible = !ScreenManager.Instance.Game.IsMouseVisible;
        }
        #endregion
    }
}
