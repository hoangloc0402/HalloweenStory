using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;               

namespace HalloweenStory.Traps
{
    class Spear : Trap
    {
        #region Constructors
        public Spear(ContentManager Content, Vector2 position)
        {
            this.animation = new Animation(Content.Load<Texture2D>("Trap/spear_4"),new Vector2(15,12), new Vector2(15,0), 4, 100, true);
            animation.Position = position;
            trapRect = new Rectangle((int)position.X, (int)position.Y, animation.Width, animation.Height);
            this.damage = 1000;
        }
        #endregion
    }
}
