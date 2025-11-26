using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftEngProject.Animation;
using SoftEngProject.Input;
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
        private GameState currentState = GameState.StartScreen;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        //private Texture2D idleTexture;
        //private Texture2D runTexture;
        //private Texture2D attackTexture;
        //private Texture2D startScreen;
        private IScreen screen;

        Hero hero;
        StartScreen scherm;
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
            Texture2D idleTexture = Content.Load<Texture2D>("Samurai_IDLE");
            Texture2D runTexture = Content.Load<Texture2D>("Samurai_RUN");
            Texture2D attackTexture = Content.Load<Texture2D>("Samurai_ATTACK 1");

            // TODO: use this.Content to load your game content here
            //idleTexture = Content.Load<Texture2D>("Samurai_IDLE");
            //runTexture = Content.Load<Texture2D>("Samurai_RUN");
            //attackTexture = Content.Load<Texture2D>("Samurai_ATTACK 1");
            //startScreen = Content.Load<Texture2D>("New Piskel");
            hero = new Hero(new KeyBoardReader());

            int frameWidthIdle = 960 / 10;
            int frameWidthRun = 1536 / 16;
            int frameWidthAttack = 672 / 7;
            int frameHeight = 96;

            var idleFrames = new List<Rectangle>();
            for (int i = 0; i < 10; i++) idleFrames.Add(new Rectangle(i * frameWidthIdle, 0, frameWidthIdle, frameHeight));
            hero.AddAnimation("Idle", new SpriteAnimation(idleTexture, idleFrames, 0.1f));

            var runFrames = new List<Rectangle>();
            for (int i = 0; i < 16; i++) runFrames.Add(new Rectangle(i * frameWidthRun, 0, frameWidthRun, frameHeight));
            hero.AddAnimation("Run", new SpriteAnimation(runTexture, runFrames, 0.08f));

            var attackFrames = new List<Rectangle>();
            for (int i = 0; i < 7; i++) attackFrames.Add(new Rectangle(i * frameWidthAttack, 0, frameWidthAttack, frameHeight));
            hero.AddAnimation("Attack", new SpriteAnimation(attackTexture, attackFrames, 0.08f, isLooping: false));

            hero.Start();

            InitializeGameObjects();
        }
        private void InitializeGameObjects()
        {
            
            //hero = new Hero(idleTexture, runTexture, attackTexture, new KeyBoardReader());
            //scherm = new StartScreen(startScreen);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            hero.Update(gameTime);
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            _spriteBatch.Begin();
            hero.Draw(_spriteBatch);
            //scherm.Draw(_spriteBatch);
            // TODO: Add your drawing code here
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
