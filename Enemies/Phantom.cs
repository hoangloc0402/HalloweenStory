using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HalloweenStory.Enemies
{
    class Phantom : Enemy
    {
        #region Fields
        const int PHANTOM_STARTING_HP = 10;
        const int PHANTOM_DAMAGE = 10;
        const float VELOCITY_X = 1f;
        const float VELOCITY_Y = 0.5f;          
        public static int CountPhantomExist = 0;
        #endregion
        #region Constructors
        public Phantom(ContentManager Content)
        {
            HP = PHANTOM_STARTING_HP; 
            CountPhantomExist++;
            IsMovingForward = IsAlive = true;
            Random Rand = new Random();
            Position.X = (float)Rand.Next(100, 1700); //Phantom xuất hiện trong khu vực X từ 100 đến 1700, Y từ 50 đến 300
            Position.Y = (float)Rand.Next(50, 300);
            previousState = currentState = State.Appear;
            hitSound = Content.Load<SoundEffect>("Player/hitSound");
            currentAnimation = new Animation(Content.Load<Texture2D>("EnemyPhantom/Appear_114x91x6_0x0"), Position, 114, 91, 6, 100, false, new Vector2(0));
            MovingAnimation  = new Animation(Content.Load<Texture2D>("EnemyPhantom/Move_114x91x6_37x0"),   Position, 114, 91, 6, 150, true, new Vector2(37, 0));
            HurtAnimation    = new Animation(Content.Load<Texture2D>("EnemyPhantom/Hurt_114x91x1_30x0"),  Position, 114, 91, 1, 250, false, new Vector2(30,0));
            DiedAnimation    = new Animation(Content.Load<Texture2D>("EnemyPhantom/Died_114x91x6_0x0"),   Position, 114, 91, 6, 100, false, new Vector2(0));
        }
        #endregion
        #region Update
        private void UpdateMovement(Vector2 PlayerPosition)
        {
            if (currentState == State.Move) //Cập nhật vị trí Phantom theo Player
            {
                ChangeState(State.Move);
                if (PlayerPosition.X > this.Position.X) 
                {
                    currentAnimation.FlipEffect = SpriteEffects.FlipHorizontally;
                    Position.X += VELOCITY_X;
                }
                else if (PlayerPosition.X < this.Position.X) //Bỏ qua trường hợp X bằng nhau
                {
                    currentAnimation.FlipEffect = SpriteEffects.None;
                    Position.X -= VELOCITY_X;
                }
                if (PlayerPosition.Y > Position.Y) Position.Y += VELOCITY_Y;
                else Position.Y -= VELOCITY_Y;
                currentAnimation.Position = Position;
            }

            if (currentAnimation.EndOfFrame)
            {
                if (currentState == State.Died) CountPhantomExist--;
            }

        }  
        public void Update(GameTime gameTime, Player player, ContentManager content)
        {
            if (!IsAlive) return;
            UpdateMovement(player.Position);
            PlayerAttack(player, content);
            if (this.HitPlayer(player)) player.HP -= PHANTOM_DAMAGE;
            currentAnimation.Update(gameTime);
        }
        #endregion
    }
}
