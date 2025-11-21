using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct3D9;
using SoftEngProject.Animation;
using SoftEngProject.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject
{
    internal class Hero
    {
        //Texture2D idleTexture;
        //Texture2D runTexture;
        //Texture2D attackTexture;
        //Animatie animatieIdle;
        //Animatie animatieRun;
        //Animatie animatieAttack;
        //Animatie currentAnimatie;
        private Vector2 positie;
        private Vector2 snelheid;
        private Vector2 versnelling;
        //private SpriteEffects spriteEffect = SpriteEffects.None;

        IInputReader inputReader;
        private readonly PlayerAnimationController animationController;
        private readonly float speed;

        int frameWidth = 960 / 10;
        int frameWidthRun = 1536 / 16;
        int frameWidthAttack = 672 / 7;
        int frameHeight = 96;
        public Hero(/*Texture2D idleTexture, Texture2D runTexture, Texture2D attackTexture*/IInputReader reader,PlayerAnimationController controller, Vector2 startPosition, float moveSpeed = 4f)
        {
            //this.idleTexture = idleTexture;
            //this.runTexture = runTexture;
            //this.attackTexture = attackTexture;
            //animatieIdle = new Animatie();
            //animatieRun = new Animatie();
            //animatieAttack = new Animatie();

            //for (int i = 0; i < 10; i++)
            //{
            //    animatieIdle.AddFrames(new AnimationFrame(new Rectangle(i * frameWidth,0,frameWidth,frameHeight)));
            //}
            //for (int i = 0; i < 16; i++)
            //{
            //    animatieRun.AddFrames(new AnimationFrame(new Rectangle(i * frameWidthRun, 0, frameWidthRun, frameHeight)));
            //}
            //for (int i = 0; i < 7; i++)
            //{
            //    animatieAttack.AddFrames(new AnimationFrame(new Rectangle(i * frameWidthAttack, 0, frameWidthAttack, frameHeight)));
            //}



            //positie = new Vector2(50, 50);
            snelheid = new Vector2(1, 1);
            versnelling = new Vector2(0.1f, 0.1f);

            this.inputReader = reader;
            this.animationController = controller;
            this.positie = startPosition;
            this.speed = moveSpeed;
        }

        public void Update(GameTime gameTime)
        {
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

            Vector2 direction = inputReader.ReadInput();
        }
        public void Draw(SpriteBatch spriteBatch)
        {
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
