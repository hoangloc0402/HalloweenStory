using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;
using System;

namespace HalloweenStory.Objects
{
    class HealthPotion
    {
        #region Fields
        public static List<HealthPotion> hpList = new List<HealthPotion>();
        const int HEALTH_AMOUNT = 25;
        bool Active;
        Vector2 Position;
        Texture2D HealthPotionGraphic;
        int Height, Width;
        int HealthAmount;
        #endregion
        #region Constructors
        public HealthPotion(ContentManager Content, Vector2 position)
        {
            Active = true;
            Position = position;
            Random Rand = new Random();
            int Temp = Rand.Next(1, 10);
            if (Temp <= 7) //Xác suất ra bình máu nhỏ là 70%, 30% ra bình máu lớn
            {
                HealthAmount = HEALTH_AMOUNT;
                HealthPotionGraphic = Content.Load<Texture2D>("Items/HealthPotion");
            }
            else
            {
                HealthAmount = 3*HEALTH_AMOUNT;
                HealthPotionGraphic = Content.Load<Texture2D>("Items/BigHealthPotion");
            }

            Width = HealthPotionGraphic.Width;
            Height = HealthPotionGraphic.Height;
        }
        public HealthPotion(ContentManager Content, Vector2 position, bool IsSmallBottle)
        {
            Active = true;
            Position = position;
            if (IsSmallBottle) 
            {
                HealthAmount = HEALTH_AMOUNT;
                HealthPotionGraphic = Content.Load<Texture2D>("Items/HealthPotion");
            }
            else
            {
                HealthAmount = 3*HEALTH_AMOUNT;
                HealthPotionGraphic = Content.Load<Texture2D>("Items/BigHealthPotion");
            }

            Width = HealthPotionGraphic.Width;
            Height = HealthPotionGraphic.Height;
        }
        #endregion
        #region Update
        public void Update(GameTime gameTime, Player player, List<Rectangle> boxCollider)
        {
            if (!Active) return;
            if (Position.X <= player.Position.X + player.Width && Position.X + Width >= player.Position.X)
            {
                if (Position.Y <= player.Position.Y + player.Height && Position.Y + Height >= player.Position.Y)
                {
                    if (player.HP < player.MaxHP)
                    {
                        player.HP += HealthAmount;
                        if (player.HP > player.MaxHP)
                            player.HP = player.MaxHP;
                        Active = false;
                    }
                }
            }
            Position.Y += 5;
            if (Position.Y > 630 - Height)
                Position.Y = 630 - Height;
            Rectangle HPBox = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            foreach (Rectangle box in boxCollider)
            {
                if (HPBox.Left <= box.Right && HPBox.Right >= box.Left)
                    if (HPBox.Intersects(box))
                    {
                        Position.Y = box.Top - Height;
                        return;
                    }

            }
        }
        #endregion
        #region Draw
        public void Draw(SpriteBatch spriteBach)
        {
            if (Active)
                spriteBach.Draw(HealthPotionGraphic, Position, Color.White);
        }
        #endregion
    }
}
