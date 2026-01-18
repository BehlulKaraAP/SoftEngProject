using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftEngProject.Animation;
using SoftEngProject.Enemies;
using SoftEngProject.Input;
using SoftEngProject.Interfaces;
using SoftEngProject.Levels;
using SoftEngProject.Screens;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.ExceptionServices;

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

        private Level currentLevel;
        private readonly int tileSize = 32;

        private EnemyManager enemyManager;
        private Texture2D meleeTexture;

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

            meleeTexture = Content.Load<Texture2D>("MeleeIdle");
            enemyManager = new EnemyManager();

            InitializeGameObjects();
        }
        private void InitializeGameObjects()
        {
            heroFactory = new HeroFactory(Content);
            hero = heroFactory.CreateHero(new KeyBoardReader());
        }

        private void LoadLevel(Level level)
        {
            currentLevel = level;

            hero.Position = currentLevel.HeroSpawn;
            hero.Physics.velocity = Vector2.Zero;

            enemyManager = new EnemyManager();
            enemyManager.Add(new MeleeEnemy(Content, new Vector2(200, 50)));
            enemyManager.Add(new ArcherEnemy(Content, new Vector2(250, 50)));
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
                    LoadLevel(LevelFactory.CreateLevel1(Content, tileSize));
                    currentState = GameState.Playing;
                }
                else if (ks.IsKeyDown(Keys.D2))
                {
                    LoadLevel(LevelFactory.CreateLevel2(Content, tileSize));
                    currentState = GameState.Playing;

                }
                return;
            }
            // TODO: Add your update logic here

            hero.Update(gameTime, currentLevel);
            enemyManager.Update(gameTime, currentLevel, hero);
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
                currentLevel.Draw(_spriteBatch);
                enemyManager.Draw(_spriteBatch);
                hero.Draw(_spriteBatch);
                if (showhitboxes)
                {
                    DrawRectOutline(hero.Hitbox, Color.LimeGreen);
                    foreach(var enemy in enemyManager.Enemies)
                    {
                        DrawRectOutline(enemy.Hitbox, Color.LimeGreen);
                    }

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
