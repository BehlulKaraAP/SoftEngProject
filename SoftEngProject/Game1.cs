using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftEngProject.Animation;
using SoftEngProject.Input;
using SoftEngProject.Interfaces;
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

        private StartScreen _startScreen;
        Texture2D startScreen;
        Hero hero;
        IHeroFactory heroFactory;
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
            startScreen = Content.Load<Texture2D>("StartScreen");
            _startScreen = new StartScreen(startScreen);

            InitializeGameObjects();
        }
        private void InitializeGameObjects()
        {
            heroFactory = new HeroFactory(Content);
            hero = heroFactory.CreateHero(new KeyBoardReader());
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (currentState == GameState.StartScreen)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    currentState = GameState.Playing;
                }
                return;
            }
            // TODO: Add your update logic here

            hero.Update(gameTime);
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
                hero.Draw(_spriteBatch);
            }
            // TODO: Add your drawing code here
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
