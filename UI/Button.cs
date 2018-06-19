using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HalloweenStory.UI
{
    class Button
    {
        #region Fields
        MouseState mouseState; //trạng thái hiện tại của mouse 
        Texture2D currentImage;       //nút help
        Texture2D[] buttonImages = new Texture2D[3];  //hình ảnh của nút help
        Vector2 position;
        bool buttonPressed;  //nút help có được nhấn hay chưa
        #endregion
        #region Events & Delegates
        public delegate void EventClickHandler();
        public event EventClickHandler ButtonClick;
        #endregion
        #region Constructors
        public Button(Texture2D defaultTexture, Texture2D holdOverTexture, Texture2D pressedTexture, Vector2 position)
        {
            buttonImages[0] = defaultTexture;
            buttonImages[1] = holdOverTexture;
            buttonImages[2] = pressedTexture;
            this.position = position;
            currentImage = defaultTexture;
        }
        #endregion
        #region Update
        public void Update(GameTime gameTime)
        {
            mouseState = Mouse.GetState();
            Rectangle helpRect = new Rectangle((int)position.X, (int)position.Y, currentImage.Width, currentImage.Height);
            if (mouseState.LeftButton == ButtonState.Released && helpRect.Contains(mouseState.Position) && buttonPressed)  //nếu nhả chuột ra thì thực hiện event click
            {
                ButtonClick();
                buttonPressed = false;
            }
            else buttonPressed = false;
            if (mouseState.LeftButton == ButtonState.Pressed && helpRect.Contains(mouseState.Position))       //nếu nhấn nút
            {
                currentImage = buttonImages[2];        //hiển thị hình ảnh được nhấn
                buttonPressed = true;

            }
            else if (helpRect.Contains(mouseState.Position)) //nếu chuột chạm vào nút
            {
                currentImage = buttonImages[1];    //hiển thị hình ảnh được chạm
            }
            else currentImage = buttonImages[0];   //hình ảnh mặc định
        }
        #endregion
        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentImage, position, Color.White);
        }
        #endregion
    }
}
