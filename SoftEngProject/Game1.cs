using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftEngProject.Animation;
using SoftEngProject.Input;
using SoftEngProject.Screens;
using System.Diagnostics;

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
        private Texture2D idleTexture;
        private Texture2D runTexture;
        private Texture2D attackTexture;
        private Texture2D startScreen;
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
            
            // TODO: use this.Content to load your game content here
            idleTexture = Content.Load<Texture2D>("Samurai_IDLE");
            runTexture = Content.Load<Texture2D>("Samurai_RUN");
            attackTexture = Content.Load<Texture2D>("Samurai_ATTACK 1");
            startScreen = Content.Load<Texture2D>("New Piskel");


            InitializeGameObjects();
        }
        private void InitializeGameObjects()
        {
            hero = new Hero(idleTexture, runTexture, attackTexture,new KeyBoardReader());
            scherm = new StartScreen(startScreen);

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
