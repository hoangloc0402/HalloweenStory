
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using HalloweenStory.Enemies;

namespace HalloweenStory
{
    class Player
    {
        #region Fields             
        const float PLAYER_V_X = 5f;
        const float PLAYER_V_Y = 21f;
        const double INVINCIBLE_TIME = 1500; //tính theo milisecond
        
        public int PlayerScore;
        public const int PLAYER_MAX_SCORE = 2250;  

        Animation walkAnimation;
        Animation jumpAnimation;
        Animation idleAnimation;
        Animation[] attackAnimation = new Animation[4];
        Animation dieAnimation;
        Animation currentAnimation;  //animation hiện tại của player
        Animation previousAnimation; //animation trước đó

        SoundEffect attacksound;

        Vector2 Velocity; //vận tốc của player
        public int HP, Attack;
        public Vector2 Position;
        public Rectangle limitRectangle;//giới hạn vị trí của player

        public bool IsJumping, IsAttacking , IsAlive, IsActive, IsInvincible;  //các trạng thái của player
        bool allowLeft, allowRight;//cho phép qua trái, phải hay không
        bool hit; //đã đánh ở khung hình này hay chưa

        KeyboardState previousKeyState;
        KeyboardState currentKeyState;
        double elapsedTime;    //thời gian đã trôi qua
        
        Color color; //màu của player
        int previousHP; //máu ở khung hình trước        
        int attackIndex = -1; //hệ số của animation tấn công
        #endregion
        #region Properties & Constructors
        public int MaxHP { get; private set; }
        public int Width
        {
            get { return currentAnimation.Width; }
        }
        public int Height
        {
            get { return currentAnimation.Height; }
        }                                                                                                     
        public Player(Vector2 position, Rectangle limitRectangle)
        {
            MaxHP = 200;
            HP = MaxHP;
            Attack = 10;
            PlayerScore = 0;
            IsJumping = IsActive = IsAlive = true;
            IsAttacking =  IsInvincible =  false;
            hit = false;
            this.Position = position;
            this.limitRectangle = limitRectangle;
            previousKeyState = Keyboard.GetState();
            elapsedTime = 0;
            previousHP = HP;
        }
        #endregion
        #region Load Content
        public void LoadContent(ContentManager Content) //Hàm khởi tạo 
        {
            walkAnimation = new Animation(Content.Load<Texture2D>("Player/Walk_4"), new Vector2(60, 26), 4, 100, true);
            jumpAnimation = new Animation(Content.Load<Texture2D>("Player/Jump_1"), new Vector2(60, 22), 1, 300, true);
            idleAnimation = new Animation(Content.Load<Texture2D>("Player/Idle_8"), new Vector2(60, 26), 8, 100, true);
            dieAnimation = new Animation(Content.Load<Texture2D>("Player/Die_12"), new Vector2(47, 26), 12, 300, true);
            attackAnimation[0] = new Animation(Content.Load<Texture2D>("Player/Attack_6"), new Vector2(60, 26), 6, 75, true);
            attackAnimation[1] = new Animation(Content.Load<Texture2D>("Player/Attack2_4"), new Vector2(60, 0), new Vector2(17,2), 4, 75, true);
            attackAnimation[3] = new Animation(Content.Load<Texture2D>("Player/Attack3_7"), new Vector2(84,14),new Vector2(56,13), 7, 75, true);
            attackAnimation[2] = new Animation(Content.Load<Texture2D>("Player/Attack4_4"), new Vector2(54,13),new Vector2(35,0), 4, 75, true);
            attacksound = Content.Load<SoundEffect>("Player/attackSound");
            currentAnimation = idleAnimation;
            previousAnimation = currentAnimation;
            
        }
        #endregion
        #region Update
        public void Update(GameTime gameTime, List<Rectangle> Boxes) 
        {
            UpdateMovement(gameTime, Boxes);
            #region Update Invincible
            if (HP < previousHP && IsInvincible)
            {
                HP = previousHP;
            }
            if(HP < previousHP && HP >0 )
            {
                IsInvincible = true;
            }
            if (IsInvincible)
            {
                //thay đổi màu
                elapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;  
                if (elapsedTime < INVINCIBLE_TIME && ((int)(elapsedTime / 50) % 3 == 0)) color = Color.Transparent;
                else if (elapsedTime < INVINCIBLE_TIME && ((int)(elapsedTime / 50) % 3) == 1 || ((int)(elapsedTime / 50) % 3)==2) color = Color.LightSalmon;
                else
                {
                    IsInvincible = false;
                    elapsedTime = 0;
                }
            }
            else color = Color.White;
            previousHP = HP;
            #endregion
            #region Update Animation
            if (HP <= 0)    //nếu chết thì
            {
                HP = 0;
                IsAlive = false;
                currentAnimation = dieAnimation;
                if (dieAnimation.EndOfFrame) IsActive = false;  
            }
            else if (IsAttacking)
            {
                currentAnimation = attackAnimation[attackIndex];
            }
            else if (Velocity.X != 0 && !IsJumping) currentAnimation = walkAnimation; //nếu vận tốc khác 0 và không nhảy
            else if (IsJumping) currentAnimation = jumpAnimation;   //nếu đang nhảy
            else
            {
                currentAnimation = idleAnimation;
            }
            if (Velocity.X > 0) currentAnimation.FlipEffect = SpriteEffects.FlipHorizontally;  //nếu v>0 thì xoay sang phải
            else if (Velocity.X < 0) currentAnimation.FlipEffect = SpriteEffects.None;       //nếu x<0 thì xoay sang trái
            else                                                        //nếu v=0 thì xoay theo khung hình trước đó
            {      
                if (previousAnimation.FlipEffect == SpriteEffects.FlipHorizontally) currentAnimation.FlipEffect = SpriteEffects.FlipHorizontally;
                else if (previousAnimation.FlipEffect == SpriteEffects.None) currentAnimation.FlipEffect = SpriteEffects.None;
            }
            //cập nhập vị trí của animation     
            currentAnimation.Position = Position;             
            currentAnimation.Update(gameTime);
            previousAnimation = currentAnimation;  
            #endregion
            
        }
        private void UpdateMovement(GameTime gameTime, List<Rectangle> Boxes)
        {
            if (IsAlive)     //nếu còn sống
            {
                currentKeyState = Keyboard.GetState();
                //đánh             
                if (IsAttacking && currentAnimation.EndOfFrame)
                {
                    IsAttacking = false;
                    currentAnimation.Reload();
                }

                if (!previousKeyState.IsKeyDown(Keys.X) && currentKeyState.IsKeyDown(Keys.X) && !IsAttacking)
                {                      
                    attackIndex = (++attackIndex) % 4;
                    IsAttacking = true;
                    attacksound.CreateInstance().Play();  
                }  

                //Đi chuyển qua lại
                if (currentKeyState.IsKeyDown(Keys.Right)) allowRight = true;
                else if (currentKeyState.IsKeyDown(Keys.Left)) allowLeft = true;
                //Nhảy
                if (currentKeyState.IsKeyDown(Keys.Z) && !IsJumping && !previousKeyState.IsKeyDown(Keys.Z)) //nếu đang đứng dưới đất và bấm Z để nhảy 
                {
                    Velocity.Y = -PLAYER_V_Y;
                    IsJumping = true;
                }
                if (IsJumping)
                {
                    Position.Y += Velocity.Y;
                    Velocity.Y += 0.5f;
                }    

                Position.Y += 7;//trọng lực kéo xuống
                IsJumping = true;
                
                foreach (Rectangle Box in Boxes)    //kiểm tra va chạm
                {
                    if (Position.X + Width >= Box.Left && Position.X + Width/2 <= Box.Left && Position.Y + Height  -10 >= Box.Top && Position.Y  - 5 <= Box.Bottom) allowRight = false;
                    else if (Position.X <= Box.Right && Position.X + Width -15 >= Box.Right && Position.Y + Height -10 >= Box.Top && Position.Y  - 5 <= Box.Bottom) allowLeft = false;
                    if (Position.Y + Height >= Box.Top && Position.X + Width >= Box.Left + 4 && Position.X <= Box.Right - 3 && Position.Y + Height -20<= Box.Top)
                    {
                        Position.Y = Box.Top - Height;
                        Velocity.Y = 0;
                        IsJumping = false;
                    }
                    else if (Position.Y <= Box.Bottom && Position.X + Width >= Box.Left + 4 && Position.X <= Box.Right - 3 && Position.Y +20 >= Box.Bottom)
                    {
                        Position.Y = Box.Bottom;
                        Velocity.Y = 0;
                    }
                }
                if (allowRight)
                {
                    Velocity.X = PLAYER_V_X;
                    allowRight = false;
                }
                else if (allowLeft)
                {
                    Velocity.X = -PLAYER_V_X;
                    allowLeft = false;
                }
                else Velocity.X = 0;
                Position.X += Velocity.X;
                
                previousKeyState = currentKeyState;// cập nhập lại previous key
            }
            else //nếu chết thì rơi xuống
            {
                
                if (IsJumping)
                {
                    Position.Y += Velocity.Y;
                    Velocity.Y += 0.5f;
                }
                Position.Y += 7;//trọng lực kéo xuống
                IsJumping = true;
                foreach (Rectangle Box in Boxes)    //kiểm tra va chạm
                {    
                    if (Position.Y + Height >= Box.Top && Position.X + Width >= Box.Left + 4 && Position.X <= Box.Right - 3 && Position.Y <= Box.Top)
                    {
                        Position.Y = Box.Top - Height;
                        Velocity.Y = 0;
                        IsJumping = false;
                    }
                }
            }
            if (Position.Y > limitRectangle.Bottom - Height) {  //giới hạn phía dưới cho player
                Position.Y = limitRectangle.Bottom - Height;
                IsJumping = false;  
            }
            if(Position.Y < limitRectangle.Top)      //giới hạn phía trên cho player
            {
                Position.Y = limitRectangle.Top;
                Velocity.Y = 0;
            }
            Position.X = MathHelper.Clamp(Position.X , limitRectangle.Left,limitRectangle.Right - Width);  //giới hạn hai bên cho player
        }
        #endregion
        #region Draw
        public void Draw(SpriteBatch spriteBatch) //Hàm vẽ  lên màn hình đồ hoạ
        {       
            if(IsActive) currentAnimation.Draw(spriteBatch, color);
        }
        #endregion
        #region Public Methods
        public bool Hit(Enemy enemy) //trả về true nếu đánh trúng enemy
        {
            if (IsAttacking) 
            {
                if (attackAnimation[0].currentFrame != 3 && attackAnimation[1].currentFrame != 1 && attackAnimation[2].currentFrame != 1 && attackAnimation[3].currentFrame != 2) hit = false; //nếu ra khỏi khung hình đánh thì hit về false
                if (!hit &&( currentAnimation == attackAnimation[0] && currentAnimation.currentFrame == 3 || currentAnimation == attackAnimation[2] && currentAnimation.currentFrame == 1 || currentAnimation == attackAnimation[3] && currentAnimation.currentFrame == 2) ) //nếu ở đúng khung hình đánh và chưa đánh ở khung hình đó
                {
                    Rectangle attackRectangle; //tầm đánh
                    if (currentAnimation.FlipEffect == SpriteEffects.None)
                    {
                        attackRectangle = new Rectangle(currentAnimation.DestinationRectangle.Location.X, currentAnimation.DestinationRectangle.Location.Y, (int)currentAnimation.OffsetTopLeft.X, currentAnimation.FrameHeight);
                    }
                    else
                    {
                        attackRectangle = new Rectangle(currentAnimation.DestinationRectangle.Location.X + currentAnimation.Width + (int)currentAnimation.OffsetBottomRight.X, currentAnimation.DestinationRectangle.Location.Y, (int)currentAnimation.OffsetTopLeft.X, currentAnimation.FrameHeight);
                    }
                    Rectangle enemyRectangle = new Rectangle((int)enemy.currentAnimation.Position.X, (int)enemy.currentAnimation.Position.Y, enemy.currentAnimation.Width, enemy.currentAnimation.Height);
                    if (attackRectangle.Intersects(enemyRectangle))
                    {
                        hit = true; //nếu đánh trúng ở khung hình này thì hit về true
                        return true;
                    }
                    else return false;
                }
                else if (!hit && (currentAnimation == attackAnimation[1] && currentAnimation.currentFrame == 1))   //nếu ở đúng khung hình đánh và chưa đánh ở khung hình đó
                {
                    Rectangle attackRectangle; //tầm đánh
                    if (currentAnimation.FlipEffect == SpriteEffects.None)
                    {
                        attackRectangle = new Rectangle(currentAnimation.DestinationRectangle.Location.X, currentAnimation.DestinationRectangle.Location.Y + 45, (int)currentAnimation.OffsetTopLeft.X, 10);
                    }
                    else
                    {
                        attackRectangle = new Rectangle(currentAnimation.DestinationRectangle.Location.X + currentAnimation.Width + (int)currentAnimation.OffsetBottomRight.X, currentAnimation.DestinationRectangle.Location.Y + 45, (int)currentAnimation.OffsetTopLeft.X, 10);
                    }
                    Rectangle enemyRectangle = new Rectangle((int)enemy.currentAnimation.Position.X, (int)enemy.currentAnimation.Position.Y, enemy.currentAnimation.Width, enemy.currentAnimation.Height);
                    if (attackRectangle.Intersects(enemyRectangle))
                    {
                        hit = true;  //nếu đánh trúng ở khung hình này thì hit về true
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
 
                return false;
            }
        }
        #endregion
    }
}
