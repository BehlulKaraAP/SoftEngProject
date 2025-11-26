using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftEngProject.Animation;
using SoftEngProject.Input;
using SoftEngProject.States;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject
{
    internal class Hero
    {
        public Animator Animator { get; private set; }
        public Vector2 Position { get; set; }
        public SpriteEffects SpriteEffect { get; set; } = SpriteEffects.None;
        public IInputReader InputReader { get; set; }

        private PlayerState currentState;

        public Hero(IInputReader reader)
        {
            InputReader = reader;
            Animator = new Animator();
            Position = new Vector2(50, 50);
        }

        public void Start()
        {
            TransitionTo(new IdleState(this));
        }
        public void AddAnimation(string name, SpriteAnimation animation)
        {
            Animator.AddAnimation(name, animation);
        }

        public void TransitionTo(PlayerState state)
        {
            currentState = state;
            currentState.Enter();
        }
        //Texture2D idleTexture;
        //Texture2D runTexture;
        //Texture2D attackTexture;
        //Animatie animatieIdle;
        //Animatie animatieRun;
        //Animatie animatieAttack;
        //Animatie currentAnimatie;
        //private Vector2 positie;
        //private Vector2 snelheid;
        //private Vector2 versnelling;
        //private SpriteEffects spriteEffect = SpriteEffects.None;

        //IInputReader inputReader;

        //int frameWidth = 960 / 10;
        //int frameWidthRun = 1536 / 16;
        //int frameWidthAttack = 672 / 7;
        //int frameHeight = 96;
        //public Hero(Texture2D idleTexture, Texture2D runTexture, Texture2D attackTexture,IInputReader reader)
        //{
        //    this.idleTexture = idleTexture;
        //    this.runTexture = runTexture;
        //    this.attackTexture = attackTexture;
        //    animatieIdle = new Animatie();
        //    animatieRun = new Animatie();
        //    animatieAttack = new Animatie();

        //    for (int i = 0; i < 10; i++)
        //    {
        //        animatieIdle.AddFrames(new AnimationFrame(new Rectangle(i * frameWidth, 0, frameWidth, frameHeight)));
        //    }
        //    for (int i = 0; i < 16; i++)
        //    {
        //        animatieRun.AddFrames(new AnimationFrame(new Rectangle(i * frameWidthRun, 0, frameWidthRun, frameHeight)));
        //    }
        //    for (int i = 0; i < 7; i++)
        //    {
        //        animatieAttack.AddFrames(new AnimationFrame(new Rectangle(i * frameWidthAttack, 0, frameWidthAttack, frameHeight)));
        //    }



        //    positie = new Vector2(50, 50);
        //    //snelheid = new Vector2(1, 1);
        //    //versnelling = new Vector2(0.1f, 0.1f);

        //    this.inputReader = reader;
        //}

        public void Update(GameTime gameTime)
        {
            currentState.Update(gameTime);
            Animator.Update(gameTime);
            //var direction = inputReader.ReadInput();

            //bool isAttacking = Keyboard.GetState().IsKeyDown(Keys.Space);

            //if (direction.X > 0)
            //    spriteEffect = SpriteEffects.None;
            //if (direction.X < 0)
            //    spriteEffect = SpriteEffects.FlipHorizontally;

            //direction *= 4;
            //positie += direction;

            //if (isAttacking)
            //    currentAnimatie = animatieAttack;
            //else if (Math.Abs(direction.X) > 0)
            //    currentAnimatie = animatieRun;
            //else
            //    currentAnimatie = animatieIdle;

            //PlayerState state = direction.X == 0 ? PlayerState.Idle : PlayerState.Run;


            //currentAnimatie.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            Animator.Draw(spriteBatch, Position, SpriteEffect);
            //Texture2D textureToDraw;

            //if (currentAnimatie == animatieAttack)
            //{
            //    textureToDraw = attackTexture;
            //}
            //else if (currentAnimatie == animatieRun)
            //{
            //    textureToDraw = runTexture;
            //}
            //else
            //{
            //    textureToDraw = idleTexture;
            //}

            //spriteBatch.Draw(textureToDraw, positie, currentAnimatie.CurrentFrame.SourceRectangle, Color.White, 0, new Vector2(0, 0), 2f, spriteEffect, 0);
        }
    }
}
