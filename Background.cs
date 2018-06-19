using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace HalloweenStory
{
    class Background
    {
        #region Fields
        Sprite background; //mảng các background
        Vector2 previousCameraPosition;//vị trí cũ của camera
        Vector2[] position; //mảng các vị trí của background
        bool tiling;   //có tiling hat không
        #endregion
        #region Constructors
        public Background(Camera camera, bool tiling, Sprite background)//constructor
        {
            previousCameraPosition = camera.Position;
            this.background = background;
            this.tiling = tiling;
            if (tiling)    //nếu có tiling thì xếp background thành 1 mảng cho vừa màn hình
            {
                position = new Vector2[1366 / background.Texture.Width + 2];
                for (int i = 0; i < position.Length; i++)
                {
                    position[i] = new Vector2(background.Texture.Width * i, background.Position.Y);
                }
            }
        }
        #endregion
        #region Update
        public void Update(GameTime gameTime, Camera camera)
        {
            float offset = camera.Position.X - previousCameraPosition.X; // độ lệch của vị trí cũ và hiện tại của camera
            if (tiling)  //nếu hình ảnh có tiling
            {
                for (int i = 0; i < position.Length; i++)      //tính vị trí mới
                {
                    position[i].X += offset * background.ParallaxScale + background.Speed;
                }
                if (camera.Position.X <= position[0].X)   //nếu đi qua bên trái cùng của background đầu tiên thì dời các background qua bên trái
                {
                    for (int i = position.Length - 1; i >= 0; i--)
                    {
                        if (i == 0) position[i].X = position[i].X - background.Texture.Width ;
                        else
                        {
                            position[i].X = position[i - 1].X;
                        }
                    }
                }
                else if (camera.Position.X + 1366 >= position[position.Length-1].X + background.Texture.Width) //nếu đi qua bên phải cùng của background đầu tiên thì dời các background qua bên phải
                {
                    for (int i = 0; i < position.Length; i++)
                    {
                        if (i == position.Length - 1) position[i].X = position[i].X + background.Texture.Width;
                        else
                        {
                            position[i].X = position[i + 1].X ;
                        }
                    }
                }
            }
            else
            {
                background.Position.X += offset * background.ParallaxScale + background.Speed;  //tính vị trí mới
            }
            previousCameraPosition = camera.Position;
        }
        #endregion
        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            if (tiling)
            {
                for (int i = 0; i < position.Length; i++) //vẽ từng background
                {
                    spriteBatch.Draw(background.Texture, position[i], Color.White);
                }
            }
            else
            {
                spriteBatch.Draw(background.Texture, background.Position, Color.White);
            }
        }
        #endregion
    }
}
