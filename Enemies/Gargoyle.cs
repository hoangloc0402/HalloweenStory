using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HalloweenStory.Enemies
{
    class Gargoyle :Enemy
    {
        #region Fields
        public const int GARGOYLE_ATTACK = 20;
        public const int GARGOYLE_STARTING_HP = 20;
        public const int GARGOYLE_DAMAGE = 10;
        public const int GARGOYLE_POINT = 50;
        public const int VELOCITY_CONST = 200;

        public float distanceX, distanceY;
        #endregion
        #region Constructors
        public Gargoyle(ContentManager Content, Vector2 Left, Vector2 Right)
        {
            HP = GARGOYLE_STARTING_HP;
            Position = landMarkLeft = Left; 
            landMarkRight = Right;
            IsMovingForward = IsAlive = true;
            distanceX = (landMarkRight.X - landMarkLeft.X) / VELOCITY_CONST; //VX
            distanceY = (landMarkRight.Y - landMarkLeft.Y) / VELOCITY_CONST;//VY
            currentState = previousState = State.Move;
            hitSound = Content.Load<SoundEffect>("Player/hitSound");
            MovingAnimation = new Animation(Content.Load<Texture2D>("EnemyGargoyle/Move_103x118x4_10x20"),new Vector2(30,53),  new Vector2(40,25),   4, 150, true); 
            HurtAnimation =   new Animation(Content.Load<Texture2D>("EnemyGargoyle/Hurt_103x118x1_0x0"),  new Vector2(10,60),  new Vector2(40, 25),  1, 250, false);
            DiedAnimation =   new Animation(Content.Load<Texture2D>("EnemyGargoyle/Died_103x118x6_0x0"),  new Vector2(10, 60), new Vector2(40, 25),  6, 150, false);
            currentAnimation = MovingAnimation;
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

            if (currentAnimation.EndOfFrame)
                if (currentState == State.Hurt || currentState == State.Appear)
                    ChangeState(State.Move);
                else if (currentState == State.Died)
                {
                    IsAlive = false;
                    currentAnimation.Active = false;
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
                player.PlayerScore += GARGOYLE_POINT;
            }
            if (this.HitPlayer(player))
                player.HP -= GARGOYLE_DAMAGE;
            currentAnimation.Update(gameTime);
        }
        #endregion
    }
}
