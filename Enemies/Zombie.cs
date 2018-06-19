using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace HalloweenStory.Enemies
{
    class Zombie : Enemy
    {
        #region Fields
        const int ZOMBIE_ATTACK= 30;       
        const int ZOMBIE_DAMAGE = 20;
        const int ZOMBIE_STARTING_HP = 50;
        const int ZOMBIE_POINT = 100;
        const float ZOMBIE_V_X = 0.8f;
        #endregion
        #region Constructors
        public Zombie(ContentManager Content, Vector2 Left, Vector2 Right) 
        {
            HP = ZOMBIE_STARTING_HP;
            Position = landMarkLeft = Left;
            landMarkRight = Right;
            IsMovingForward = IsAlive = true;
            hitSound = Content.Load<SoundEffect>("Player/hitSound");
            MovingAnimation = new Animation(Content.Load<Texture2D>("EnemyZombie/Move_83x97x4"),   Left, 83, 97, 4, 150, true,  new Vector2(22, 0));
            AttackAnimation = new Animation(Content.Load<Texture2D>("EnemyZombie/Attack_83x97x4"), Left, 83, 97, 4, 100, false, new Vector2(22, 0));
            HurtAnimation = new Animation(Content.Load<Texture2D>("EnemyZombie/Hurt_83x97x1"),     Left, 83, 97, 1, 300, false, new Vector2(22, 0));
            DiedAnimation = new Animation(Content.Load<Texture2D>("EnemyZombie/Die_83x97x11"),     Left, 83, 97, 11, 100, true, new Vector2(22, 0));
            currentAnimation = MovingAnimation;
            currentState = previousState = State.Move;         
        }
        #endregion
        #region Update
        private void UpdateMovement()
        {
            if (currentState == State.Move)
            {
                ChangeState(State.Move);
                if (IsMovingForward)
                {
                    currentAnimation.FlipEffect = SpriteEffects.FlipHorizontally;
                    Position.X += ZOMBIE_V_X;
                }
                else
                {
                    currentAnimation.FlipEffect = SpriteEffects.None;
                    Position.X -= ZOMBIE_V_X;
                }
                if (Position.X <= landMarkLeft.X)
                {
                    IsMovingForward = true;
                    currentAnimation.FlipEffect = SpriteEffects.FlipHorizontally;
                }
                else if (Position.X >= landMarkRight.X)
                {
                    IsMovingForward = false;
                    currentAnimation.FlipEffect = SpriteEffects.None;
                }
                currentAnimation.Position = Position;
            }


            if (currentAnimation.EndOfFrame) //Update bị thương và chết
                if (currentState == State.Hurt)
                    ChangeState(State.Move);
                else if (currentState == State.Died)
                {
                    
                    IsAlive = false;
                    currentAnimation.Active = false;
                }
        }    
        public void Update(GameTime gameTime, Player player, ContentManager content)
        {
            if (!IsAlive)
                return;
            UpdateMovement();
            PlayerAttack(player, content);
            AttackPlayer(player);
            if (base.HitPlayer(player)) player.HP -= ZOMBIE_DAMAGE;
            if (HP <= 0 && addPoint)
            {
                addPoint = false;
                player.PlayerScore += ZOMBIE_POINT;
            }               
            currentAnimation.Update(gameTime);
        }
        #endregion
        #region Private Methods 
        private void AttackPlayer(Player player)
        {
            //Nếu player ở trong phạm vi tấn công của zombie
            if (Math.Abs(player.Position.Y - Position.Y) < 150 && currentState != State.Attack && currentState != State.Died)
            {
                if (player.Position.X > Position.X + Width && player.Position.X - Position.X - Width < 15) //Nếu Player đứng bên phải zombie
                {
                    ChangeState(State.Attack);
                    IsMovingForward = true;
                    currentAnimation.FlipEffect = SpriteEffects.FlipHorizontally;
                }
                else if (player.Position.X + player.Width < Position.X && Position.X - player.Position.X - player.Width < 15) //Nếu player đứng bên trái, bỏ qua trường hợp bằng
                {
                    ChangeState(State.Attack);
                    IsMovingForward = false;
                    currentAnimation.FlipEffect = SpriteEffects.None;
                }
            }
            if (currentState == State.Attack)
            {
                if (currentAnimation.currentFrame == 2 && Math.Abs(player.Position.Y - Position.Y) < 50 && ((player.Position.X + player.Width < Position.X && Position.X - player.Position.X - player.Width < 15) || player.Position.X > Position.X + Width && player.Position.X - Position.X - Width < 15))
                    player.HP -= ZOMBIE_ATTACK;
                else if (!currentAnimation.Active) //Nếu zombie tấn công xong thì chuyển về trạng thái di chuyển bình thường
                {
                    ChangeState(State.Move);
                    if (IsMovingForward)
                        currentAnimation.FlipEffect = SpriteEffects.FlipHorizontally;
                    else currentAnimation.FlipEffect = SpriteEffects.None;
                }
            }
        }
        #endregion
    }
}
