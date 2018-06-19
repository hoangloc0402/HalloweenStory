using HalloweenStory.Objects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HalloweenStory.Enemies
{
    class Enemy
    {
        #region Fields
        public int HP;
        public Animation currentAnimation; //Chứa animation hiện tại của Enemy
        protected Animation MovingAnimation;
        protected Animation IdleAnimation;
        protected Animation DiedAnimation;
        protected Animation AttackAnimation;
        protected Animation DefenseAnimation;
        protected Animation HurtAnimation;
        protected Animation UltimateAttackAnimation;
        protected Vector2 landMarkLeft, landMarkRight, Position;
        public const int RECOIL_DISTANCE = 7; //Khoảng giật lùi khi bị Player tấn công
        public bool IsMovingForward, IsAlive;
        protected bool addPoint = true;
        public enum State { Appear, Idle, Move , Attack, Defense , UltimateAttack, Hurt, Died} //Các state có thể có của Enemy
        protected State currentState, previousState;
        protected SoundEffect hitSound;
        #endregion
        #region Properties
        public int Width
        {
            get { return currentAnimation.Width; }
        }
        public int Height
        {
            get { return currentAnimation.Height; }
        }
        #endregion
        #region Protected Methods
        protected virtual void ChangeState(State state) //Hàm đổi trạng thái Enemy
        {
            previousState = currentState;
            currentState = state;
            if (currentState != previousState) //Nếu 2 trang thái liên tiếp khác nhau mới cần load lại animation
            {
                Position = currentAnimation.Position; //Animation mới vẽ ra ở cùng vị trí animation cũ             
                switch (currentState)
                {
                    case State.Idle:
                        IdleAnimation.FlipEffect = currentAnimation.FlipEffect;
                        currentAnimation = IdleAnimation;
                        break;
                    case State.Move:
                        MovingAnimation.FlipEffect = currentAnimation.FlipEffect;
                        currentAnimation = MovingAnimation;
                        break;
                    case State.Attack:
                        AttackAnimation.FlipEffect = currentAnimation.FlipEffect;
                        currentAnimation = AttackAnimation;
                        break;
                    case State.Defense:
                        DefenseAnimation.FlipEffect = currentAnimation.FlipEffect;
                        currentAnimation = DefenseAnimation;
                        break;
                    case State.UltimateAttack:
                        UltimateAttackAnimation.FlipEffect = currentAnimation.FlipEffect;
                        currentAnimation = UltimateAttackAnimation;
                        break;
                    case State.Hurt:
                        HurtAnimation.FlipEffect = currentAnimation.FlipEffect; //Nếu đang xoay mặt về bên nào thì khi bị thương animation cũng xoay mặt về bên đó
                        currentAnimation = HurtAnimation;
                        break;
                    case State.Died:
                        DiedAnimation.FlipEffect = currentAnimation.FlipEffect; //Nếu đang xoay mặt về bên nào thì khi chết animation cũng xoay mặt về bên đó
                        currentAnimation = DiedAnimation;
                        break;
                    default:
                        break;
                }
                currentAnimation.Reload(); //reload lại animation
                currentAnimation.Active = true; //Cho active trở lại (với những animation không lặp)
                currentAnimation.Position = Position; 
            }
        }
        protected virtual bool HitPlayer(Player player) //Hàm player va chạm với enemy
        {
            if (currentState == State.Died)
                return false; //Nếu trạng thái ở animation đang chết thì không xét va chạm
            Rectangle EnemyRect = new Rectangle((int)currentAnimation.Position.X, (int)currentAnimation.Position.Y, Width, Height);
            Rectangle PlayerRect = new Rectangle((int)player.Position.X, (int)player.Position.Y, player.Width, player.Height);
            if (PlayerRect.Intersects(EnemyRect))
                return true; //Nếu va chạm với nhau, trả về true
            else
                return false;
        }
        protected virtual void PlayerAttack(Player player, ContentManager content) //Hàm enemy nhận attack từ player
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
                if (currentState == State.Hurt || currentState == State.Appear) ChangeState(State.Move); 
                else if (currentState == State.Died)
                {
                    Random Rand = new Random();
                    int X = Rand.Next(1, 100); //Xác xuất rơi ra bình máu khi enemy chết
                    if (X <= 15) HealthPotion.hpList.Add(new HealthPotion(content, currentAnimation.Position));
                    currentAnimation.Active = false;
                    IsAlive = false;
                }
            }
        }
        #endregion
        #region Draw
        public virtual void Draw(SpriteBatch spriteBatch) //Hàm vẽ  lên màn hình đồ hoạ
        {
            if ( !IsAlive || (currentState == State.Died && currentAnimation.EndOfFrame)) return;
            currentAnimation.Draw(spriteBatch);
        }
        #endregion
    }
}
