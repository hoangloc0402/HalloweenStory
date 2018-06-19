using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HalloweenStory.Enemies
{
    class Skeleton : Enemy
    {
        #region Fields
        public const int SKELETON_ATTACK = 30;
        public const int SKELETON_DAMAGE = 20;
        const int SKELETON_STARTING_HP = 60;
        const int SKELETON_POINT = 75;
        const int VELOCITY_CONST = 300;

        float distanceX, distanceY;
        #endregion
        #region Constructors
        public Skeleton(ContentManager Content, Vector2 Left, Vector2 Right)
        {
            HP = SKELETON_STARTING_HP;
            Position = landMarkLeft = Left;
            landMarkRight = Right;
            IsMovingForward = IsAlive = true;
            hitSound = Content.Load<SoundEffect>("Player/hitSound");
            MovingAnimation = new Animation(Content.Load<Texture2D>("EnemySkeleton/Move_5"), new Vector2(28,0) , new Vector2(30,0),  5, 150, true);
            HurtAnimation = new Animation(Content.Load<Texture2D>("EnemySkeleton/Hurt_1"),   new Vector2(33,0) , new Vector2(28,0),  1, 250, false);
            DiedAnimation = new Animation(Content.Load<Texture2D>("EnemySkeleton/Died_12"),  new Vector2(33, 0), new Vector2(28, 0), 12, 100, false);
            currentAnimation = MovingAnimation;
            currentState = previousState = State.Move;
            distanceX = (landMarkRight.X - landMarkLeft.X) / VELOCITY_CONST;
            distanceY = (landMarkRight.Y - landMarkLeft.Y) / VELOCITY_CONST;
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
                    Position.X += distanceX;
                    Position.Y += distanceY;
                }
                else
                {
                    Position.X -= distanceX;
                    Position.Y -= distanceY;
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

        }
        public void Update(GameTime gameTime, Player player, ContentManager content)
        {
            if (!IsAlive) return;
            UpdateMovement();
            PlayerAttack(player, content);
            if (HP <= 0 && addPoint)
            {
                addPoint = false;
                player.PlayerScore += SKELETON_POINT;
            }
            if (this.HitPlayer(player))
                player.HP -= SKELETON_DAMAGE;
            currentAnimation.Update(gameTime);
        }
        #endregion
    }
}
