using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
namespace HalloweenStory.Enemies
{
    class SkeletonZombie : Enemy
    {
        #region Fields
        const int SKELETONZOMBIE_ATTACK = 30;
        const int SKELETONZOMBIE_DAMAGE = 20;
        const int SKELETONZOMBIE_STARTING_HP = 150;
        const int SKELETONZOMBIE_POINT = 150;
        const float SKELETONZOMBIE_V_X = 2f;
        #endregion
        #region Constructors
        public SkeletonZombie(ContentManager Content, Vector2 Left, Vector2 Right)
        {
            HP = SKELETONZOMBIE_STARTING_HP;
            Position = landMarkLeft = Left;
            landMarkRight = Right;
            IsMovingForward = IsAlive = true;
            hitSound = Content.Load<SoundEffect>("Player/hitSound");
            MovingAnimation = new Animation(Content.Load<Texture2D>("EnemySkeleton/Move_5"),   new Vector2(28, 0), new Vector2(30, 0), 5, 100, true);
            AttackAnimation = new Animation(Content.Load<Texture2D>("EnemySkeleton/Attack_8"), new Vector2(70, 0), new Vector2(70, 0), 8, 75, false);
            HurtAnimation =   new Animation(Content.Load<Texture2D>("EnemySkeleton/Hurt_1"),   new Vector2(33, 0), new Vector2(28, 0), 1, 200, false);
            DiedAnimation =   new Animation(Content.Load<Texture2D>("EnemySkeleton/Died_12"),  new Vector2(33, 0), new Vector2(28, 0), 12, 100, false);
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
                    Position.X += SKELETONZOMBIE_V_X;
                }
                else
                {
                    currentAnimation.FlipEffect = SpriteEffects.None;
                    Position.X -= SKELETONZOMBIE_V_X;
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
            if (base.HitPlayer(player))
                player.HP -= SKELETONZOMBIE_DAMAGE;
            if (HP <= 0 && addPoint)
            {
                addPoint = false;
                player.PlayerScore += SKELETONZOMBIE_POINT;
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
                if (player.Position.X > Position.X + Width && player.Position.X - Position.X - Width < 25) //Nếu Player đứng bên phải zombie
                {
                    ChangeState(State.Attack);
                    IsMovingForward = true;
                    currentAnimation.FlipEffect = SpriteEffects.FlipHorizontally;
                }
                else if (player.Position.X + player.Width < Position.X && Position.X - player.Position.X - player.Width < 25) //Nếu player đứng bên trái, bỏ qua trường hợp bằng
                {
                    ChangeState(State.Attack);
                    IsMovingForward = false;
                    currentAnimation.FlipEffect = SpriteEffects.None;
                }
            }
            if (currentState == State.Attack)
            {
                if (currentAnimation.currentFrame == 5 && Math.Abs(player.Position.Y - Position.Y) < 100 && ((player.Position.X + player.Width < Position.X && Position.X - player.Position.X - player.Width < 25) || player.Position.X > Position.X + Width && player.Position.X - Position.X - Width < 25))
                    player.HP -= SKELETONZOMBIE_ATTACK;
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
