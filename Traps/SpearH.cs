using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace HalloweenStory.Traps
{
    class SpearH : SpearV
    {   
        public SpearH(ContentManager Content, Vector2 position,bool Flip,double delayTime) : base()
        {
            this.animation = new Animation(Content.Load<Texture2D>("Trap/spearH_16"), new Vector2(10,10),  new Vector2(10,10), 16, 100, true);
            if(!Flip)animation.FlipEffect = SpriteEffects.None;
            animation.Position = position;
            this.delayTime = delayTime;
            trapRect = new Rectangle((int)position.X, (int)position.Y, animation.Width, animation.Height);
            this.damage = 30;
        } 
    }
}
