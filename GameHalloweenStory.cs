using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using HalloweenStory.Enemies;
using HalloweenStory.Screens;


namespace HalloweenStory
{

    public class GameHalloweenStory : Microsoft.Xna.Framework.Game
    {
        
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
                                   
        public GameHalloweenStory()
        {
            graphics = new GraphicsDeviceManager(this);
            this.Window.Position = new Point(0, 0);
            this.Window.IsBorderless = true;
            Content.RootDirectory = "Content";
            this.graphics.PreferredBackBufferWidth = 1366;
            this.graphics.PreferredBackBufferHeight = 768;
            this.graphics.ApplyChanges();
            //this.graphics.IsFullScreen = true;
            ScreenManager.Instance.Game = this; //khởi tạo Game cho ScreenManager 
        }
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScreenManager.Instance.LoadContent(Content); //Load dữ liệu cho ScreenManager
            base.LoadContent();
        }
        protected override void UnloadContent()
        {
           
        }
        protected override void Update(GameTime gameTime)
        {    
            ScreenManager.Instance.Update(gameTime); //Chạy hàm Update của ScreenManager
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            ScreenManager.Instance.Draw(spriteBatch);//Chạy hàm Draw của ScreenManager
        }
    }
}