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

        private GameState currentState = GameState.StartScreen;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private StartScreen _startScreen;
        Texture2D startScreen;

        private GameOverScreen _gameOverScreen;
        Texture2D gameOverScreen;

        Hero hero;
        IHeroFactory heroFactory;

        private readonly int tileSize = 32;

        private EnemyManager enemyManager;

        private Texture2D uiHeart;

        private GameSession session;

        private Texture2D background1;
        private Texture2D background2;


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
            gameOverScreen = Content.Load<Texture2D>("GameOver");
            _gameOverScreen = new GameOverScreen(gameOverScreen);

            enemyManager = new EnemyManager();
            session = new GameSession(new LevelFactory(), tileSize);

            uiHeart = new Texture2D(GraphicsDevice, 1, 1);
            uiHeart.SetData(new[] { Color.White });

            background1 = Content.Load<Texture2D>("Background1");
            background2 = Content.Load<Texture2D>("Background2");

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
                if (session.IsRestartPending)
                {
                    currentState = GameState.GameOver;
                }
            }
            else if (currentState == GameState.GameOver)
            {
                session.Update(gameTime, Content, hero, enemyManager);

                if (!session.IsRestartPending)
                {
                    currentState = GameState.Playing;
                }
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
                Texture2D bg = session.CurrentLevelIndex == 2 ? background2 : background1;
                _spriteBatch.Draw(bg, GraphicsDevice.Viewport.Bounds, Color.White);

                var level = session.CurrentLevel;

                level.Draw(_spriteBatch);
                enemyManager.Draw(_spriteBatch);
                hero.Draw(_spriteBatch);

                DrawLivesAsSquares();
            }
            else if (currentState == GameState.GameOver)
            {
                var level = session.CurrentLevel;

                level.Draw(_spriteBatch);
                enemyManager.Draw(_spriteBatch);
                hero.Draw(_spriteBatch);

                _gameOverScreen.Draw(_spriteBatch, GraphicsDevice);
            }
                // TODO: Add your drawing code here
                _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
