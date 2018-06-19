using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using HalloweenStory.Enemies;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using HalloweenStory.Traps;
using HalloweenStory.Objects;
using HalloweenStory.UI;

namespace HalloweenStory.Screens
{
    class GraveyardScreen : GameScreen
    {
        #region Fields

        Player player = new Player(new Vector2(200, 240), new Rectangle(0, -500, 13500, 1130));
        Camera camera = new Camera(0, 13500);//camera
        HealthBarPlayer healthBar = new HealthBarPlayer();

        List<Background> backgrounds = new List<Background>();//mảng background
        List<Rectangle> boxCollider = new List<Rectangle>(); // mảng chứa box collider để xử lí va chạm

        List<FireTrap> fireTraps = new List<FireTrap>();
        List<CampFireTrap> campfireTraps = new List<CampFireTrap>();
        List<Spear> spearTraps = new List<Spear>();
        List<SpearV> spearVTraps = new List<SpearV>();
        List<SpearH> spearHTraps = new List<SpearH>();

        Portal portal;
        Animation pumpkin;

        List<Zombie> zombiesList = new List<Zombie>();
        List<Gargoyle> gargoyleList = new List<Gargoyle>();
        List<Skeleton> skeletonList = new List<Skeleton>();
        List<SkeletonZombie> skeletonzombieList = new List<SkeletonZombie>();

        #endregion
        #region Load Content
        public override void LoadContent(ContentManager Content)
        {
            base.LoadContent(Content);
            Type = ScreenType.Graveyard;
            ScreenManager.Instance.Player = player;
            ScreenManager.Instance.Game.IsMouseVisible = false;

            Song song = Content.Load<Song>("Graveyard/HalloweenSong");
            MediaPlayer.Play(song);
            MediaPlayer.IsRepeating = true;
            //MediaPlayer.Volume = 0.5f;
            player.LoadContent(Content);
            healthBar.LoadContent(Content);

            portal = new Portal(Content, new Vector2(13200, 410));
            pumpkin = new Animation(Content.Load<Texture2D>("Graveyard/pumpkin_12"), new Vector2(12183, 300), Vector2.Zero, Vector2.Zero, 12, 1000, true);
            pumpkin.FlipEffect = SpriteEffects.None;

            #region Load Traps                                                                                                                       
            fireTraps.Add(new FireTrap(Content, new Vector2(10520, 440)));
            fireTraps.Add(new FireTrap(Content, new Vector2(10700, 200)));

            campfireTraps.Add(new CampFireTrap(Content, new Vector2(9970, 594)));

            //spearTraps.Add(new Spear(Content, new Vector2( 7301, 588)));  
            for (int i = 0; i < 19; i++)
                spearTraps.Add(new Spear(Content, new Vector2(7603 + 55 * i, 567)));

            spearVTraps.Add(new SpearV(Content, new Vector2(6745, 538), 500));
            spearVTraps.Add(new SpearV(Content, new Vector2(7301, 538), 0));
            spearVTraps.Add(new SpearV(Content, new Vector2(7805, 290), 900));
            spearVTraps.Add(new SpearV(Content, new Vector2(8440, 307), 0));
            spearVTraps.Add(new SpearV(Content, new Vector2(8515, 307), 1000));
            spearVTraps.Add(new SpearV(Content, new Vector2(11160, 540), 0));

            spearHTraps.Add(new SpearH(Content, new Vector2(11130, 380), true, 800));
            spearHTraps.Add(new SpearH(Content, new Vector2(11010, 380), false, 800));
            spearHTraps.Add(new SpearH(Content, new Vector2(11010, 560), false, 0));
            spearHTraps.Add(new SpearH(Content, new Vector2(11010, 160), false, 0));

            //spearHTraps.Add(new SpearH(Content, new Vector2(), false));
            #endregion
            #region Load Map and boxCollider
            //tạo mảng để chèn texture vào map
            backgrounds.Add(new Background(camera, true, new Sprite(Content.Load<Texture2D>("Graveyard/Bg"))));
            backgrounds.Add(new Background(camera, false, new Sprite(Content.Load<Texture2D>("Graveyard/Moon"), new Vector2(1100, 50), 0.9999f, -0.1f)));
            backgrounds.Add(new Background(camera, true, new Sprite(Content.Load<Texture2D>("Graveyard/Cloud"), Vector2.Zero, 0.9f, -1f)));
            backgrounds.Add(new Background(camera, true, new Sprite(Content.Load<Texture2D>("Graveyard/BgCloud"), new Vector2(0, 350), 0.7f, -0.5f)));
            backgrounds.Add(new Background(camera, true, new Sprite(Content.Load<Texture2D>("Graveyard/BgTree"), new Vector2(0, 420), 0.3f)));
            backgrounds.Add(new Background(camera, false, new Sprite(Content.Load<Texture2D>("Graveyard/Object"))));
            backgrounds.Add(new Background(camera, false, new Sprite(Content.Load<Texture2D>("Graveyard/Block"))));


            boxCollider.Add(new Rectangle(517, 450, 585, 60));
            boxCollider.Add(new Rectangle(877, 270, 315, 60));
            boxCollider.Add(new Rectangle(1507, 450, 225, 60));
            boxCollider.Add(new Rectangle(1687, 270, 225, 60));
            boxCollider.Add(new Rectangle(1957, 90, 315, 60));
            boxCollider.Add(new Rectangle(2317, 450, 855, 60));
            boxCollider.Add(new Rectangle(3262, 330, 45, 60));
            boxCollider.Add(new Rectangle(3442, 210, 45, 60));
            boxCollider.Add(new Rectangle(3577, 90, 315, 60));
            boxCollider.Add(new Rectangle(3982, 210, 45, 60));
            boxCollider.Add(new Rectangle(4162, 330, 45, 60));
            boxCollider.Add(new Rectangle(4657, 270, 315, 60));
            boxCollider.Add(new Rectangle(5107, 390, 225, 60));
            boxCollider.Add(new Rectangle(5467, 510, 585, 120));
            boxCollider.Add(new Rectangle(5467, 210, 405, 60));
            boxCollider.Add(new Rectangle(6862, 450, 45, 60));
            boxCollider.Add(new Rectangle(7042, 330, 45, 60));
            boxCollider.Add(new Rectangle(7222, 210, 45, 60));
            boxCollider.Add(new Rectangle(7537, 210, 45, 420));
            boxCollider.Add(new Rectangle(7807, 330, 45, 300));
            boxCollider.Add(new Rectangle(8077, 210, 45, 420));
            boxCollider.Add(new Rectangle(8437, 390, 180, 60));
            boxCollider.Add(new Rectangle(8595, 345, 45, 45));
            boxCollider.Add(new Rectangle(8527, 450, 90, 60));
            boxCollider.Add(new Rectangle(8662, 510, 180, 60));
            boxCollider.Add(new Rectangle(8617, 390, 45, 240));
            boxCollider.Add(new Rectangle(9067, 330, 495, 60));
            boxCollider.Add(new Rectangle(9652, 150, 45, 60));
            boxCollider.Add(new Rectangle(10102, 90, 45, 60));
            boxCollider.Add(new Rectangle(11227, -500, 270, 590));
            boxCollider.Add(new Rectangle(11497, -500, 45, 830));
            boxCollider.Add(new Rectangle(11272, 270, 225, 60));
            boxCollider.Add(new Rectangle(11227, 270, 45, 240));
            boxCollider.Add(new Rectangle(11272, 450, 540, 60));
            boxCollider.Add(new Rectangle(10867, 90, 135, 540));
            boxCollider.Add(new Rectangle(10777, 210, 90, 420));
            boxCollider.Add(new Rectangle(10687, 330, 90, 300));
            boxCollider.Add(new Rectangle(10597, 450, 90, 180));
            boxCollider.Add(new Rectangle(10507, 570, 90, 60));
            #endregion
            #region Add Enemy
            zombiesList.Add(new Zombie(Content, new Vector2(517, 530), new Vector2(1100, 0)));
            zombiesList.Add(new Zombie(Content, new Vector2(2317, 350), new Vector2(3100, 0)));
            zombiesList.Add(new Zombie(Content, new Vector2(4700, 530), new Vector2(5350, 0)));
            zombiesList.Add(new Zombie(Content, new Vector2(5467, 110), new Vector2(5800, 0)));
            zombiesList.Add(new Zombie(Content, new Vector2(8888, 530), new Vector2(9540, 0)));
            zombiesList.Add(new Zombie(Content, new Vector2(11230, 170), new Vector2(11400, 0)));

            gargoyleList.Add(new Gargoyle(Content, new Vector2(1327, 420), new Vector2(1822, 0)));
            gargoyleList.Add(new Gargoyle(Content, new Vector2(3240, 495), new Vector2(3510, 180)));
            gargoyleList.Add(new Gargoyle(Content, new Vector2(3000, 0), new Vector2(4500, 0)));
            gargoyleList.Add(new Gargoyle(Content, new Vector2(3847, 180), new Vector2(4207, 495)));
            gargoyleList.Add(new Gargoyle(Content, new Vector2(6590, 435), new Vector2(7200, 90)));
            gargoyleList.Add(new Gargoyle(Content, new Vector2(8212, 435), new Vector2(8572, 0)));
            gargoyleList.Add(new Gargoyle(Content, new Vector2(4680, 180), new Vector2(5625, 12)));
            gargoyleList.Add(new Gargoyle(Content, new Vector2(11675, 323), new Vector2(13350, 550)));

            skeletonList.Add(new Skeleton(Content, new Vector2(883, 144), new Vector2(1110, 144)));
            skeletonList.Add(new Skeleton(Content, new Vector2(5480, 382), new Vector2(5940, 382)));
            skeletonList.Add(new Skeleton(Content, new Vector2(6095, 500), new Vector2(7422, 500)));
            skeletonList.Add(new Skeleton(Content, new Vector2(11340, 500), new Vector2(12000, 500)));

            skeletonzombieList.Add(new SkeletonZombie(Content, new Vector2(3212, 503), new Vector2(5330, 503)));
            skeletonzombieList.Add(new SkeletonZombie(Content, new Vector2(9494, 503), new Vector2(10320, 503)));
            skeletonzombieList.Add(new SkeletonZombie(Content, new Vector2(11878, 503), new Vector2(13170, 503)));

            #endregion

            HealthPotion.hpList.Add(new HealthPotion(Content, new Vector2(3725, 55), false));
            HealthPotion.hpList.Add(new HealthPotion(Content, new Vector2(10100, 55), false));
            HealthPotion.hpList.Add(new HealthPotion(Content, new Vector2(11300, 390), false));
        }
        #endregion
        #region Update
        public override void Update(GameTime gameTime)
        {
            foreach (HealthPotion HP in HealthPotion.hpList) HP.Update(gameTime, player, boxCollider);

            foreach (Zombie zombie in zombiesList) if (zombie.IsAlive) zombie.Update(gameTime, player, Content);
            foreach (Gargoyle gargoyle in gargoyleList) if (gargoyle.IsAlive)  gargoyle.Update(gameTime, player, Content);
            foreach (Skeleton skeleton in skeletonList) if (skeleton.IsAlive) skeleton.Update(gameTime, player, Content);
            foreach (SkeletonZombie skezom in skeletonzombieList) if (skezom.IsAlive) skezom.Update(gameTime, player, Content);

            foreach (FireTrap f in fireTraps) f.Update(gameTime, player);    //update fire trap    
            foreach (CampFireTrap c in campfireTraps) c.Update(gameTime, player);//update campfire trap
            foreach (Spear s in spearTraps) s.Update(gameTime, player);
            foreach (SpearV sV in spearVTraps) sV.Update(gameTime, player);
            foreach (SpearH sH in spearHTraps) sH.Update(gameTime, player);

            player.Update(gameTime, boxCollider);
            camera.LookAt(player.Position);
            healthBar.Update(gameTime, camera.Position, player);
            foreach (Background b in backgrounds) b.Update(gameTime, camera);  //update map 
            portal.Update(gameTime, player);
            pumpkin.Update(gameTime);
            if (!player.IsActive)
            {
                ScreenState = State.Lose;
            }

        }
        #endregion
        #region Draw
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(transformMatrix: camera.GetViewMatrix());

            for (int i = 0; i <= backgrounds.Count - 2; i++) backgrounds[i].Draw(spriteBatch);

            foreach (FireTrap f in fireTraps) f.Draw(spriteBatch);
            foreach (CampFireTrap c in campfireTraps) c.Draw(spriteBatch);
            foreach (Spear s in spearTraps) s.Draw(spriteBatch);
            foreach (SpearV sV in spearVTraps) sV.Draw(spriteBatch);
            foreach (SpearH sH in spearHTraps) sH.Draw(spriteBatch);
            pumpkin.Draw(spriteBatch);
            backgrounds[backgrounds.Count - 1].Draw(spriteBatch);
            portal.Draw(spriteBatch);

            foreach (HealthPotion HP in HealthPotion.hpList) HP.Draw(spriteBatch);
            foreach (Zombie zombie in zombiesList) zombie.Draw(spriteBatch);
            foreach (Gargoyle gargoyle in gargoyleList) gargoyle.Draw(spriteBatch);
            foreach (Skeleton skeleton in skeletonList) skeleton.Draw(spriteBatch);
            foreach (SkeletonZombie skezom in skeletonzombieList)  skezom.Draw(spriteBatch);

            player.Draw(spriteBatch);
            healthBar.Draw(spriteBatch);
            spriteBatch.End();
        }
        #endregion
    }
}

