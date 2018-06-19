using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Microsoft.Xna.Framework.Input;

namespace HalloweenStory.Screens
{
    class ScreenManager
    {
        #region Fields & Properties & Constructors
        private ScreenManager() { }
        private static ScreenManager instance;  //tạo instance duy nhất của screen manager
        public static ScreenManager Instance
        {
            get
            {
                if (instance == null) instance = new ScreenManager();
                return instance;
            }
        }  
        public ContentManager Content;                        
        public Game Game;//dùng để truy xuất đến game hiện tại   
        PauseScreen pauseScreen = new PauseScreen();
        WinningScreen winningScreen = new WinningScreen();
        GameOverScreen gameoverScreen = new GameOverScreen();
        public GameScreen currentScreen = new MainMenuScreen(); //khởi tạo màn hình hiện tại  
        KeyboardState lastKeyState;//trạng thái cũ của phím
        KeyboardState keyState;//trạng thái hiện tại của phím
        public Player Player;
        #endregion
        #region Public Methods
        public void ChangeScreen(GameScreen gameScreen)//thay màn hình hiện tại thành màn hình mới
        {
            currentScreen.UnloadContent();
            currentScreen = gameScreen;
            currentScreen.LoadContent(Content);
            if (currentScreen.Type != GameScreen.ScreenType.MainMenu)//nếu không phải main menu thì 
            {
                pauseScreen.LoadContent(Content);
                winningScreen.LoadContent(Content);
                gameoverScreen.LoadContent(Content);
            }
        }
        #endregion
        #region Load Content
        public void LoadContent(ContentManager Content)
        {                 
            this.Content = Content;
            currentScreen.LoadContent(Content);
            if (currentScreen.Type != GameScreen.ScreenType.MainMenu)//nếu không phải main menu thì 
            {
                pauseScreen.LoadContent(Content);
                winningScreen.LoadContent(Content);
                gameoverScreen.LoadContent(Content);
            }
        }
        #endregion
        #region Update
        public void Update(GameTime gameTime)
        {
            
            if (!pauseScreen.IsPopUp && currentScreen.ScreenState != GameScreen.State.Lose && currentScreen.ScreenState != GameScreen.State.Win) currentScreen.Update(gameTime);
            if (currentScreen.Type != GameScreen.ScreenType.MainMenu) //nếu không phải main menu thì cập nhập pause screen
            {
                if (currentScreen.ScreenState == GameScreen.State.Lose)
                {
                    ScreenManager.Instance.Game.IsMouseVisible = true;
                    gameoverScreen.Update(gameTime);
                }
                else if (currentScreen.ScreenState == GameScreen.State.Win)
                {
                    ScreenManager.Instance.Game.IsMouseVisible = true;
                    winningScreen.Update(gameTime);
                }
                else
                {
                    keyState = Keyboard.GetState();
                    pauseScreen.Update(gameTime);
                    if (keyState.IsKeyDown(Keys.Escape) && !lastKeyState.IsKeyDown(Keys.Escape))
                    {
                        pauseScreen.IsPopUp = !pauseScreen.IsPopUp;
                        ScreenManager.Instance.Game.IsMouseVisible = !ScreenManager.Instance.Game.IsMouseVisible;
                    }
                    lastKeyState = keyState;
                }
            }
        }
        #endregion
        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {     
            currentScreen.Draw(spriteBatch); 

            spriteBatch.Begin();
            if (pauseScreen.IsPopUp)   ///vẽ pause screen
            {
                pauseScreen.Draw(spriteBatch);
            }
            if (currentScreen.ScreenState == GameScreen.State.Win) winningScreen.Draw(spriteBatch,Player);
            if (currentScreen.ScreenState == GameScreen.State.Lose) gameoverScreen.Draw(spriteBatch,Player);
            spriteBatch.End();      
        }
        #endregion
    }
}
