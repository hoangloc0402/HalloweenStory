using HalloweenStory.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HalloweenStory.Screens
{
    class MainMenuScreen : GameScreen
    {
        #region Fields
        Sprite background;//hình nền
        double elapsedTime;//thời gian trôi qua dùng để đổi màu text
        Color textColor;  //màu của text                         
        Song song;   //nhạc nền cho main menu                                    
        Sprite helpPanel;        //help panel
        Sprite text;//chuỗi văn bản Press Enter to play    
        bool helpPanelPopUp = false;    //có hiện help pop up hay không
        Button helpButton;
        Button exitButton;  
        #endregion
        #region Load Content
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            Type = ScreenType.MainMenu;
            ScreenManager.Instance.Game.IsMouseVisible = true;     //cho phép sử dụng chuột
            //Load background
            background = new Sprite(Content.Load<Texture2D>("MainMenu/Background"), Vector2.Zero);         

            //Load song
            song = Content.Load<Song>("MainMenu/BgSong");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.25f;
            //Load exit button                                  
            exitButton = new Button(Content.Load<Texture2D>("MainMenu/exitbut"), Content.Load<Texture2D>("MainMenu/exitbuthover"), Content.Load<Texture2D>("MainMenu/exitbutpressed"), new Vector2(10, 683));
            exitButton.ButtonClick += ExitButtonClick;        
            //Load help button  
            helpButton = new Button(Content.Load<Texture2D>("MainMenu/helpbut"), Content.Load<Texture2D>("MainMenu/helpbuthover"), Content.Load<Texture2D>("MainMenu/helpbutpressed"), new Vector2(90, 683));
            helpButton.ButtonClick += HelpButtonClick;//thêm sự kiện khi click vào button

            //Load help panel
            helpPanel = new Sprite(Content.Load<Texture2D>("MainMenu/HelpPanel"), new Vector2(10, 362));

            //Load text
            text = new Sprite(Content.Load<Texture2D>("MainMenu/Text"), new Vector2(530, 630)); 
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            UpdateTextColor(gameTime);         
            if (Keyboard.GetState().IsKeyDown(Keys.Enter))
            {
                MediaPlayer.Stop();
                ScreenManager.Instance.ChangeScreen(new GraveyardScreen()); //chuyển qua màn 1
            }
            
            exitButton.Update(gameTime);
            helpButton.Update(gameTime);                      
        }

        #endregion
        #region Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin();

            spriteBatch.Draw(background.Texture, new Rectangle(0, 0, 1366, 768), Color.White);
            spriteBatch.Draw(text.Texture,text.Position, null, textColor , 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);
            exitButton.Draw(spriteBatch);   
            helpButton.Draw(spriteBatch);                                                               
            if (helpPanelPopUp) spriteBatch.Draw(helpPanel.Texture, helpPanel.Position,null, Color.White, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0f);

            spriteBatch.End();
        }
        #endregion
        #region Methods & Events Handler
        void UpdateTextColor(GameTime gameTime)            //Hàm thay đổi màu sắc của chữ
        {
            elapsedTime += gameTime.ElapsedGameTime.TotalSeconds;
            if (elapsedTime <= 1) textColor = Color.White * (1 - (float)(elapsedTime / 2 + 0.3));
            else if (elapsedTime <= 2) textColor = Color.White * (float)(elapsedTime / 2 - 0.3);
            else elapsedTime = 0;
        }
        private void HelpButtonClick()
        {
            helpPanelPopUp = !helpPanelPopUp;
        }
        private void ExitButtonClick()
        {
            ScreenManager.Instance.Game.Exit();
        }
        #endregion
    }
}
