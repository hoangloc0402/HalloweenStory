using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace HalloweenStory.Screens
{
    class GameScreen
    {
        #region Fields & Properties
        public ContentManager Content;
        public enum ScreenType
        {
            MainMenu,Graveyard, Boss
        }
        public ScreenType Type { get; protected set; }
        public enum State {Win , Lose, Default}
        public State ScreenState { get; protected set; }
        #endregion
        #region Public Methods
        public virtual void LoadContent(ContentManager Content)
        {    
            this.Content = Content;
            ScreenState = State.Default;  
        }
        public virtual void UnloadContent()
        {
            Content.Unload();
        }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(SpriteBatch spriteBatch) { }
        #endregion
    }
}
