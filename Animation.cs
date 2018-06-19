using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HalloweenStory
{
    class Animation
    {
        #region Fields 
        Texture2D spriteStrip;  //hình ảnh chứa loạt các chuyển động 
        int elapsedTime; // thời gian đã trôi qua
        int frameTime; //thời gian giữa các khung hình
        public int frameCount; //số khung hình
        public int currentFrame;// chỉ số của khung hình hiện tại  
        Rectangle sourceRectangle = new Rectangle();//khu vực của spriteStrip mà chúng ta muốn hiển thị
        public Rectangle DestinationRectangle = new Rectangle();//khu vực của spriteTrip mà chúng ta muốn hiển thị trong game
        public int FrameWidth;//chiều rộng của khung hình
        public int FrameHeight;//chiều cao của khung hình
        public SpriteEffects FlipEffect;  //Flip hình ảnh trong spriteStrip
        public bool Active;//trạng thái của Animation
        public bool Looping; //có lặp hay không
        public Vector2 Position;//vị trí của Animationz
        public Vector2 OffsetTopLeft;  //Độ chênh lệch trên trái giữa vị trí của Animation và vị trí của Box
        public Vector2 OffsetBottomRight;// Độ chênh lệch dưới phải giữa vị trí của Animation và vị trí của Box
      
        #endregion
        #region Constructors
        public Animation(Texture2D spriteStrip, Vector2 position, int frameWidth, int frameHeight, int frameCount, int frameTime, bool looping, Vector2 offsetTopLeft)
        {
            this.spriteStrip = spriteStrip;
            this.Position = position;
            this.FrameWidth = frameWidth;
            this.FrameHeight = frameHeight;
            this.frameCount = frameCount;
            this.FlipEffect = SpriteEffects.FlipHorizontally;
            this.frameTime = frameTime; 
            this.Looping = looping;
            this.OffsetTopLeft = offsetTopLeft;

            //Khởi tạo giá trị mặc định
            this.OffsetBottomRight = Vector2.Zero;
            elapsedTime = 0;
            currentFrame = 0;
            Active = true;
            EndOfFrame = false;
        }
        public Animation(Texture2D spriteStrip, Vector2 offsetTopLeft, int frameCount, int frameTime, bool looping)
        {
            this.spriteStrip = spriteStrip;
            this.Position = Vector2.Zero;
            this.OffsetTopLeft = offsetTopLeft;
            this.frameCount = frameCount;
            this.FrameWidth = spriteStrip.Width/frameCount;
            this.FrameHeight = spriteStrip.Height;
            this.FlipEffect = SpriteEffects.FlipHorizontally;
            this.frameTime = frameTime;
            this.Looping = looping;

            //Khởi tạo giá trị mặc định
            this.OffsetBottomRight = Vector2.Zero;
            elapsedTime = 0;
            currentFrame = 0;
            Active = true;
        }
        public Animation(Texture2D spriteStrip, Vector2 offsetTopLeft,Vector2 offsetBottomRight, int frameCount, int frameTime, bool looping)
        {
            this.spriteStrip = spriteStrip;
            this.Position = Vector2.Zero;
            this.OffsetTopLeft = offsetTopLeft;
            this.OffsetBottomRight = offsetBottomRight;
            this.frameCount = frameCount;
            this.FrameWidth = spriteStrip.Width / frameCount;
            this.FrameHeight = spriteStrip.Height;
            this.FlipEffect = SpriteEffects.FlipHorizontally;
            this.frameTime = frameTime;
            this.Looping = looping;

            //Khởi tạo giá trị mặc định
            
            elapsedTime = 0;
            currentFrame = 0;
            Active = true;
        }
        public Animation(Texture2D spriteStrip, Vector2 position,Vector2 offsetTopLeft, Vector2 offsetBottomRight, int frameCount, int frameTime, bool looping)
        {
            this.spriteStrip = spriteStrip;
            this.Position = position;
            this.OffsetTopLeft = offsetTopLeft;
            this.OffsetBottomRight = offsetBottomRight;
            this.frameCount = frameCount;
            this.FrameWidth = spriteStrip.Width / frameCount;
            this.FrameHeight = spriteStrip.Height;
            this.FlipEffect = SpriteEffects.FlipHorizontally;
            this.frameTime = frameTime;
            this.Looping = looping;

            //Khởi tạo giá trị mặc định

            elapsedTime = 0;
            currentFrame = 0;
            Active = true;
        }

        #endregion
        #region Properties
        public bool EndOfFrame { get; private set; }
        public int Width
        {
            get
            {
                return FrameWidth - (int)OffsetTopLeft.X- (int)OffsetBottomRight.X;
            }
        }
        public int Height
        {
            get
            {
                return FrameHeight - (int)OffsetTopLeft.Y - (int)OffsetBottomRight.Y;
            }
        }
        #endregion
        #region Public Methods
        public void Reload()
        {
            currentFrame = 0;
            EndOfFrame = false;
            elapsedTime = 0;
        }
        #endregion
        #region Update
        public void Update(GameTime gameTime)
        {        
            if (!Active) return;
            elapsedTime += (int)gameTime.ElapsedGameTime.Milliseconds; //cập nhập thời gian đã trôi qua
            if (elapsedTime > frameTime)  //nếu elapsedTime lớn hơn frameTime thì đến khung hình tiếp theo
            {
                currentFrame++;
                if (currentFrame == frameCount)
                {
                    currentFrame = 0;
                    if (!Looping) Active = false; // nếu không loop thì dừng    
                }
                if(currentFrame == frameCount - 1)
                {
                    EndOfFrame = true;
                }
                elapsedTime = 0;
            }
            sourceRectangle = new Rectangle(currentFrame * FrameWidth, 0, FrameWidth, FrameHeight);
            if(FlipEffect == SpriteEffects.None) DestinationRectangle = new Rectangle((int)(Position.X- OffsetTopLeft.X), (int)(Position.Y- OffsetTopLeft.Y), (int)FrameWidth, (int)FrameHeight);
            else DestinationRectangle = new Rectangle((int)(Position.X - OffsetBottomRight.X), (int)(Position.Y - OffsetTopLeft.Y), (int)FrameWidth, (int)FrameHeight);
        }
        #endregion
        #region Draw
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteStrip, DestinationRectangle, sourceRectangle, Color.White, 0f, new Vector2(0), FlipEffect, 0f);
        }
        public void Draw(SpriteBatch spriteBatch, Color color)
        {
            spriteBatch.Draw(spriteStrip, DestinationRectangle, sourceRectangle, color, 0f, new Vector2(0), FlipEffect, 0f);
        }
        #endregion
    }
}