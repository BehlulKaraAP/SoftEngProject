using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftEngProject.Enemies;
using SoftEngProject.Input;
using SoftEngProject.Interfaces;
using SoftEngProject.Levels;
using SoftEngProject.Screens;

namespace SoftEngProject
{
    public enum GameState
    {
        StartScreen,
        Playing,
        GameOver
    }
    public class Game1 : Game
    {
        private Texture2D debugPixel;
        private bool showhitboxes = true;

        private GameState currentState = GameState.StartScreen;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private StartScreen _startScreen;
        Texture2D startScreen;

        Hero hero;
        IHeroFactory heroFactory;

        private readonly int tileSize = 32;

        private EnemyManager enemyManager;

        private Texture2D uiHeart;

        private GameSession session;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            debugPixel = new Texture2D(GraphicsDevice, 1, 1);
            debugPixel.SetData(new[] { Color.White });

            startScreen = Content.Load<Texture2D>("StartScreen");
            _startScreen = new StartScreen(startScreen);

            enemyManager = new EnemyManager();
            session = new GameSession(new LevelFactory(), tileSize);

            uiHeart = new Texture2D(GraphicsDevice, 1, 1);
            uiHeart.SetData(new[] { Color.White });

            InitializeGameObjects();
        }
        private void InitializeGameObjects()
        {
            heroFactory = new HeroFactory(Content);
            hero = heroFactory.CreateHero(new KeyBoardReader());
        }
        private void DrawLivesAsSquares()
        {
            int size = 16;
            int spacing = 6;
            int startX = 12;
            int startY = 12;

            for (int i = 0; i < hero.Health; i++)
            {
                var rect = new Rectangle(startX + i * (size + spacing), startY, size, size);
                _spriteBatch.Draw(uiHeart, rect, Color.Red);
            }
        }
        private void DrawRectOutline(Rectangle rect, Color color, int thickness = 2)
        {
            // top
            _spriteBatch.Draw(debugPixel, new Rectangle(rect.Left, rect.Top, rect.Width, thickness), color);
            // bottom
            _spriteBatch.Draw(debugPixel, new Rectangle(rect.Left, rect.Bottom - thickness, rect.Width, thickness), color);
            // left
            _spriteBatch.Draw(debugPixel, new Rectangle(rect.Left, rect.Top, thickness, rect.Height), color);
            // right
            _spriteBatch.Draw(debugPixel, new Rectangle(rect.Right - thickness, rect.Top, thickness, rect.Height), color);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (currentState == GameState.StartScreen)
            {
                var ks = Keyboard.GetState();
                if (ks.IsKeyDown(Keys.D1))
                {
                    session.LoadLevel1(Content, hero, enemyManager);
                    currentState = GameState.Playing;
                }
                else if (ks.IsKeyDown(Keys.D2))
                {
                    session.LoadLevel2(Content, hero, enemyManager);
                    currentState = GameState.Playing;
                }
                return;
            }
            if (currentState == GameState.Playing)
            {
                session.Update(gameTime, Content, hero, enemyManager);
            }
            // TODO: Add your update logic here

            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();

            if (currentState == GameState.StartScreen)
            {
                _startScreen.Draw(_spriteBatch, GraphicsDevice);
            }
            else if (currentState == GameState.Playing)
            {
                var level = session.CurrentLevel;

                level.Draw(_spriteBatch);
                enemyManager.Draw(_spriteBatch);
                hero.Draw(_spriteBatch);

                DrawLivesAsSquares();
                if (showhitboxes)
                {
                    DrawRectOutline(hero.Hitbox, Color.LimeGreen);
                    foreach (var e in enemyManager.Enemies)
                    {
                        DrawRectOutline(e.Hitbox, Color.Red);

                        if (e is MeleeEnemy ms && ms.DebugIsAttackActive)
                        {
                            DrawRectOutline(ms.DebugAttackHitbox, Color.Orange);
                        }
                    }
                }
            }
            // TODO: Add your drawing code here
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
