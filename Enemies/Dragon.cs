using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace HalloweenStory.Enemies
{
    class Dragon : Enemy
    {
        #region Fields
        const int DRAGON_DAMAGE = 10;
        const int DRAGON_ATTACK = 15;
        const int DRAGON_ULTIMATE_ATTACK = 20;
        const int DRAGON_POINT = 500;
        const int DRAGON_STARTING_HEALTH = 300;
        const float V_X_SPEED = 2f;
        const float V_Y_SPEED = 2f;
        Animation PowerAnimation, PowerHitAnimation; //Animation của viên đạn dragon bắn ra
        float PowerVX, PowerVY; //Vận tốc đạn của Dragon
        Vector2 AttackTarget;
        public int MaxHP { get; private set; }
        #endregion
        #region Constructor
        public Dragon(ContentManager Content, Vector2 Left, Vector2 Right)
        {
            HP = MaxHP = DRAGON_STARTING_HEALTH;
            IsAlive = IsMovingForward = true;
            Position = landMarkLeft = Left;
            landMarkRight = Right;
            PowerVX = PowerVY = 0;
            AttackTarget = Vector2.Zero;
            hitSound = Content.Load<SoundEffect>("Player/hitSound");
            MovingAnimation =         new Animation(Content.Load<Texture2D>("EnemyDragon/Move_6"),      new Vector2(0),  6,  100, true);
            IdleAnimation =           new Animation(Content.Load<Texture2D>("EnemyDragon/Idle_6"),      new Vector2(0), 6,  150, true);
            HurtAnimation =           new Animation(Content.Load<Texture2D>("EnemyDragon/Hurt_1"),      new Vector2(0), 1,  250, false);
            AttackAnimation =         new Animation(Content.Load<Texture2D>("EnemyDragon/Attack_6"),    new Vector2(0), 6,  100, true);
            DefenseAnimation =        new Animation(Content.Load<Texture2D>("EnemyDragon/Attack_6"),    new Vector2(0), 6, 100, true);
            UltimateAttackAnimation = new Animation(Content.Load<Texture2D>("EnemyDragon/Ultimate_12"), new Vector2(0), 12, 100, true);
            DiedAnimation =           new Animation(Content.Load<Texture2D>("EnemyDragon/Died_5"),      new Vector2(0), 5,  150, false);
            PowerAnimation =          new Animation(Content.Load<Texture2D>("EnemyDragon/Power_8"),     new Vector2(0), 8,  100, false);
            PowerHitAnimation =       new Animation(Content.Load<Texture2D>("EnemyDragon/PowerHit_1"),  new Vector2(0), 1,  250, false);
            currentAnimation = MovingAnimation ;
            currentAnimation.Position = Position;
            previousState = currentState = State.Idle;
            PowerAnimation.Active = PowerHitAnimation.Active = false; //trạng thái ban đầu, chưa cho avtive đạn

        }
        #endregion
        #region Methods
        private void Chasing(Animation Animation, Vector2 Target, int Vscale) //Vscale là hệ số vận tốc, mỗi hoạt động truy đuổi có thể có vận tốc khác nhau
        {
            if ((Animation.Position.X + Width / 2) < Target.X)  //Hàm truy đuổi, nếu X bé hơn mục tiêu thì tăng X, ngược lại giảm X
            {
                Animation.FlipEffect = SpriteEffects.FlipHorizontally;
                Animation.Position.X += V_X_SPEED*Vscale;
            }
            else if ((Animation.Position.X + Width/2) > Target.X) //Bỏ qua trường hợp X bằng nhau
            {
                Animation.FlipEffect = SpriteEffects.None;
                Animation.Position.X -= V_X_SPEED * Vscale;
            }
            if ((Animation.Position.Y + Height/ 2) < Target.Y) //Nếu Y bé hơn Y của mục tiêu thì tăng Y, ngược lại giảm Y
                Animation.Position.Y += V_Y_SPEED * Vscale;
            else if ((Animation.Position.Y + Height / 2) > Target.Y) 
                Animation.Position.Y -= V_Y_SPEED * Vscale;
            if (Math.Abs((Animation.Position.X + Width / 2) - Target.X) < 5) //Nếu sai khác X Y quá nhỏ thì cho bằng nhau để tránh hình ảnh bị giật liên tục
                Animation.Position.X = Target.X - Width / 2;
            if (Math.Abs((Animation.Position.Y + Height / 2) - Target.Y) < 5)
                Animation.Position.Y = Target.Y - Height / 2;
        }   
        private bool IsCloseToTarget(Animation Animation, Vector2 Target)
        {
            if (Math.Abs(Animation.Position.X + Width / 2 - Target.X) < 10 && Math.Abs(Animation.Position.Y + Height / 2 - Target.Y) < 10)
                return true;
            else return false;
        }
        protected override bool HitPlayer(Player player)
        {
            if (currentState == State.Died)
                return false;
            Rectangle EnemyRect = new Rectangle((int)currentAnimation.Position.X + 50, (int)currentAnimation.Position.Y + 50, Width / 4, Height / 4);
            Rectangle PlayerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);
            if (PlayerRect.Intersects(EnemyRect))
                return true;
            else
                return false;
        }
        protected override void PlayerAttack(Player player, ContentManager content)
        {
            if (player.Hit(this) && currentState != State.Hurt && currentState != State.Died && currentState != State.Defense)
            {
                ChangeState(State.Hurt);
                this.HP -= player.Attack;
                if (this.HP <= 0) //Nếu đánh hết máu thì chuyển qua trạng thái chết
                    ChangeState(State.Died);
                if (hitSound != null)
                    hitSound.CreateInstance().Play();
                if (Position.X > player.Position.X && currentAnimation.FlipEffect == SpriteEffects.None)
                {
                    Position.X += RECOIL_DISTANCE;
                    if (Position.X > landMarkRight.X) Position.X = landMarkRight.X;
                }
                else if (Position.X < player.Position.X && currentAnimation.FlipEffect == SpriteEffects.FlipHorizontally)
                {
                    Position.X -= RECOIL_DISTANCE;
                    if (Position.X < landMarkLeft.X) Position.X = landMarkLeft.X;
                }
            }
            if (currentAnimation.EndOfFrame)
            {
                if (currentState == State.Hurt)
                    ChangeState(State.Move);
                if (currentState == State.Died)
                {
                    IsAlive = false;
                    currentAnimation.Active = false;
                }
            }
        }
        private bool ShootPlayer(Player player)
        {
            Rectangle Power = new Rectangle((int)PowerAnimation.Position.X, (int)PowerAnimation.Position.Y, PowerAnimation.Width, PowerAnimation.Height);
            Rectangle Player = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);
            if (Power.Intersects(Player)) return true;
            else return false;
        }
        #endregion
        #region Update
        private void UpdateStage1() //Stage1, giai đoạn ban đầu, khi máu còn >70%, Dragon chỉ bay qua bay lại
        {
            if (!currentAnimation.Active)
                if (currentState == State.Hurt || currentState == State.Appear)
                    ChangeState(State.Move);
                else if (currentState == State.Died)
                    IsAlive = false;
            if (currentState == State.Idle)
            {
                ChangeState(State.Idle);
                if (currentAnimation.EndOfFrame)
                    ChangeState(State.Move);
            }
            else if (currentState == State.Move)
            {
                if (IsMovingForward)
                {
                    Chasing(currentAnimation, landMarkRight, 1);
                    if (IsCloseToTarget(currentAnimation, landMarkRight))
                    {
                        ChangeState(State.Idle);
                        IsMovingForward = false;
                    }              
                }
                else
                {
                    Chasing(currentAnimation, landMarkLeft, 1);
                    if (IsCloseToTarget(currentAnimation, landMarkLeft))
                    {
                        ChangeState(State.Idle);
                        IsMovingForward = true;
                    }
                }
            }
        }
        private void UpdateStage2(Player player) //Stage2, khi máu còn từ 40->70%, Dragon lượn qua lại, đâm vào player
        {
            if (currentState != State.Defense && AttackTarget == Vector2.Zero)
                if (IsCloseToTarget(currentAnimation, landMarkLeft) || IsCloseToTarget(currentAnimation, landMarkRight))
                {
                    ChangeState(State.Move);
                    AttackTarget = player.Position;
                    if (IsCloseToTarget(currentAnimation, landMarkLeft))
                        IsMovingForward = true;
                    else IsMovingForward = false;
                }
                else
                {
                    ChangeState(State.Move);
                    if (IsMovingForward) Chasing(currentAnimation, landMarkRight, 2);
                    else Chasing(currentAnimation, landMarkLeft, 2);
                }

            if (AttackTarget != Vector2.Zero && currentState == State.Move)
            {
                Chasing(currentAnimation, AttackTarget, 4);
                if (IsCloseToTarget(currentAnimation, AttackTarget))

                    ChangeState(State.Defense);
            }
            if (currentState == State.Defense)
            {
                ChangeState(State.Defense);
                if (currentAnimation.EndOfFrame)
                {
                    AttackTarget = Vector2.Zero;
                    ChangeState(State.Move);
                }
            }

        }
        private void UpdateStage3(Player player)
        {
            if (HP <= 0) ChangeState(State.Died);
            if (currentState != State.Died)
            {
                ChangeState(State.UltimateAttack);

                if (!IsMovingForward) //Di chuyển qua lại
                {
                    Chasing(currentAnimation, landMarkLeft, 3);
                    if (IsCloseToTarget(currentAnimation, landMarkLeft))
                        IsMovingForward = true;
                }
                else //Di chuyển qua lại
                {
                    Chasing(currentAnimation, landMarkRight, 3);
                    if (IsCloseToTarget(currentAnimation, landMarkRight))
                        IsMovingForward = false;
                }

                if (currentAnimation.currentFrame == 6 && !PowerAnimation.Active && !PowerHitAnimation.Active) //Frame thứ 6 là frame bắt đầu bắn ra Power
                {
                    AttackTarget = player.Position; //Update vị trí bắn
                    PowerAnimation.Active = true; //Cho đạn avtive
                    if (!IsMovingForward) //Update vị trí xuất hiện của đạn
                    {
                        PowerAnimation.Position.X = currentAnimation.Position.X + 56;
                        PowerAnimation.Position.Y = currentAnimation.Position.Y + 117;
                    }
                    else
                    {
                        PowerAnimation.Position.X = currentAnimation.Position.X + 183;
                        PowerAnimation.Position.Y = currentAnimation.Position.Y + 119;
                    }
                    PowerVX = Math.Abs(PowerAnimation.Position.X - AttackTarget.X) / 50; //Vận tốc đạn, đảm bảo cho đạn bay thẳng
                    PowerVY = Math.Abs(PowerAnimation.Position.Y - AttackTarget.Y) / 50;
                }

                if (PowerAnimation.Active && !PowerHitAnimation.Active)
                {
                    if (PowerAnimation.Position.X > AttackTarget.X)
                        PowerAnimation.Position.X -= PowerVX;
                    else if (PowerAnimation.Position.X < AttackTarget.X)
                        PowerAnimation.Position.X += PowerVX;
                    if (PowerAnimation.Position.Y > AttackTarget.Y)
                        PowerAnimation.Position.Y -= PowerVY;
                    else if (PowerAnimation.Position.Y < AttackTarget.Y)
                        PowerAnimation.Position.Y += PowerVY;

                    if (ShootPlayer(player))  //Nếu Power đã di chuyển tới vị trí bắn thì chuyển animation qua nổ
                    {
                        PowerAnimation.Active = false;
                        PowerHitAnimation.Active = true;
                        PowerHitAnimation.Position.X = player.Position.X - 10;
                        PowerHitAnimation.Position.Y = player.Position.Y;
                        player.HP -= DRAGON_ATTACK;
                    }
                    else if (IsCloseToTarget(PowerHitAnimation, AttackTarget))
                    {
                        PowerAnimation.Active = false;
                        PowerHitAnimation.Active = true;
                        PowerHitAnimation.Position = AttackTarget;
                    }
                }
            }
            else
            {
                ChangeState(State.Died);
                if (currentAnimation.EndOfFrame)
                    IsAlive = false;
            }
        }
        public void Update(GameTime gameTime, Player player, ContentManager content)
        {
            if (!IsAlive)
                return;
            if ((100 * HP) / DRAGON_STARTING_HEALTH > 70)
                UpdateStage1();
            else if ((100 * HP) / DRAGON_STARTING_HEALTH > 40)
                UpdateStage2(player);
            else
                UpdateStage3(player);
            if (HitPlayer(player))
                player.HP -= DRAGON_DAMAGE;
            PlayerAttack(player, content);
            if (HP <= 0 && addPoint)
            {
                addPoint = false;
                player.PlayerScore += DRAGON_POINT;
            }
            currentAnimation.Update(gameTime);
            if (PowerAnimation.Active)
                PowerAnimation.Update(gameTime);
            if (PowerHitAnimation.Active)
                PowerHitAnimation.Update(gameTime);
        }
        #endregion
        #region Draw            
        public override void Draw(SpriteBatch spriteBatch) 
        {
            currentAnimation.Draw(spriteBatch);
            if (PowerAnimation.Active)
                PowerAnimation.Draw(spriteBatch);
            if (PowerHitAnimation.Active)
                PowerHitAnimation.Draw(spriteBatch);
        }
        #endregion
    }
}
