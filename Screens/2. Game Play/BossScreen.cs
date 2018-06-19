using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using HalloweenStory.Traps;
using HalloweenStory.Enemies;
using HalloweenStory.Objects;
using Microsoft.Xna.Framework.Media;
using HalloweenStory.UI;

namespace HalloweenStory.Screens
{
    class BossScreen : GameScreen
    {
        #region Fields
        Camera camera = new Camera(0, 1800);//camera
        Player player;
        HealthBarPlayer healthBar = new HealthBarPlayer();
        HealthBarBoss healthBarBoss = new HealthBarBoss();

        Torch torch1;
        Torch torch2;

        List<Background> backgrounds = new List<Background>();//mảng background
        List<Rectangle> boxCollider = new List<Rectangle>(); // mảng chứa box collider để xử lí va chạm

        List<Phantom> phantomList = new List<Phantom>();
        Dragon boss;
        #endregion
        #region Constructors
        public BossScreen(Player player)
        {
            this.player = player;             
            this.player.Position = new Vector2(0, 500);
            this.player.limitRectangle = new Rectangle(0, -500, 1800, 1130);
        }
        public BossScreen()
        {
            this.player = new Player(new Vector2(0, 500), new Rectangle(0, -500, 1800, 1130));
        }
        #endregion
        #region Load Content
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            Type = ScreenType.Boss;
            ScreenManager.Instance.Player = player;
            ScreenManager.Instance.Game.IsMouseVisible = false;

            Song song = Content.Load<Song>("BossMap/Bg music");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            boss = new Dragon(Content, new Vector2(200, 50), new Vector2(1500, 150));
            Phantom.CountPhantomExist = 0;
            player.LoadContent(Content);
            healthBar.LoadContent(Content);
            healthBarBoss.LoadContent(Content);
 
            #region Load Map and boxCollider
            //tạo mảng để chèn texture vào map
            Animation torch1Anima = new Animation(Content.Load<Texture2D>("BossMap/torch_5"),new  Vector2(68,27), new Vector2(70,188), 5, 100, true);
            Animation torch2Anima = new Animation(Content.Load<Texture2D>("BossMap/torch2_5"), new Vector2(68, 27), new Vector2(70, 188), 5, 100, true);
            torch1 = new Torch(torch1Anima, new Vector2(550, 380));
            torch2 = new Torch(torch2Anima, new Vector2(1085, 380));

            backgrounds.Add(new Background(camera, true, new Sprite(Content.Load<Texture2D>("BossMap/Bg"))));
            backgrounds.Add(new Background(camera, false, new Sprite(Content.Load<Texture2D>("BossMap/moon"), new Vector2(300, -250), 0.9999f, -0.1f)));
            backgrounds.Add(new Background(camera, true, new Sprite(Content.Load<Texture2D>("BossMap/cloud"), new Vector2(0,-200), 0.9f, -1f)));
            backgrounds.Add(new Background(camera, true, new Sprite(Content.Load<Texture2D>("BossMap/bgcloud"), new Vector2(0, 300), 0.7f, -0.5f)));  
            backgrounds.Add(new Background(camera, true, new Sprite(Content.Load<Texture2D>("BossMap/BgTree"), new Vector2(0, 0), 0.3f)));
            backgrounds.Add(new Background(camera, false, new Sprite(Content.Load<Texture2D>("BossMap/object"))));
            backgrounds.Add(new Background(camera, false, new Sprite(Content.Load<Texture2D>("BossMap/block"))));

            boxCollider.Add(new Rectangle(247, 450, 225, 60));
            boxCollider.Add(new Rectangle(562, 270, 45, 60));
            boxCollider.Add(new Rectangle(787, 150, 135, 60));
            boxCollider.Add(new Rectangle(697, 450, 315, 60));
            boxCollider.Add(new Rectangle(1102, 270, 45, 60));
            boxCollider.Add(new Rectangle(1237, 450, 225, 60));
            #endregion
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            foreach (HealthPotion HP in HealthPotion.hpList) HP.Update(gameTime, player, boxCollider);
            player.Update(gameTime, boxCollider);
            camera.LookAt(player.Position);//update camera
            healthBar.Update(gameTime, camera.Position, player);
            healthBarBoss.Update(gameTime, camera.Position, boss);
            foreach (Background b in backgrounds) b.Update(gameTime, camera);  //update map
            torch1.Update(gameTime, player);
            torch2.Update(gameTime,player);

            foreach (Phantom phantom in phantomList) if (phantom.IsAlive) phantom.Update(gameTime, player, Content);          
            if (Phantom.CountPhantomExist < 2 ) phantomList.Add(new Phantom(Content));
            if (boss.IsAlive) boss.Update(gameTime, player, Content);
            if (!player.IsActive) //màn hình thua xuất hiện
            {
                ScreenState = State.Lose;
            }
            else if (!boss.IsAlive) ///màn hình win xuất hiện
            {
                ScreenState = State.Win;
            }
            
        }
        #endregion
        #region Draw
        public override void Draw(SpriteBatch spriteBatch)  
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

            for (int i = 0; i <= backgrounds.Count - 2; i++) backgrounds[i].Draw(spriteBatch);
            torch1.Draw(spriteBatch);
            torch2.Draw(spriteBatch);
            backgrounds[backgrounds.Count - 1].Draw(spriteBatch);
            foreach (HealthPotion HP in HealthPotion.hpList) HP.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);
            healthBarBoss.Draw(spriteBatch);
            foreach (Phantom phantom in phantomList) phantom.Draw(spriteBatch);
            boss.Draw(spriteBatch);
            player.Draw(spriteBatch);

            spriteBatch.End();
        }
        #endregion
    }
}
