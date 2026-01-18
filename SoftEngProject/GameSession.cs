using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoftEngProject.Combat;
using SoftEngProject.Levels;

namespace SoftEngProject
{
    internal class GameSession
    {
        private readonly int tileSize;
        private readonly LevelFactory levelFactory;

        private readonly CombatSystem combatSystem = new CombatSystem(1);
        public Level CurrentLevel { get; private set; }
        public bool IsRestartPending => pendingRestart;

        private bool pendingRestart = false;
        private float restartTimer = 0f;
        private const float RestartDelaySeconds = 1.0f;

        public float RestartSecondsLeft => restartTimer;

        private Texture2D arrowTex;

        public GameSession(LevelFactory levelFactory, int tileSize)
        {
            this.levelFactory = levelFactory;
            this.tileSize = tileSize;
        }

        public void LoadLevel1(ContentManager content, Hero hero, Enemies.EnemyManager enemyManager)
        {
            CurrentLevel = LevelFactory.CreateLevel1(content, tileSize);
            ResetWorld(hero, enemyManager, content);
        }

        public void LoadLevel2(ContentManager content, Hero hero, Enemies.EnemyManager enemyManager)
        {
            CurrentLevel = LevelFactory.CreateLevel2(content, tileSize);
            ResetWorld(hero, enemyManager, content);
        }

        private void ResetWorld(Hero hero, Enemies.EnemyManager enemyManager, ContentManager content)
        {
            hero.ResetHealth();
            hero.Position = CurrentLevel.HeroSpawn;
            hero.Physics.velocity = Vector2.Zero;
            arrowTex = content.Load<Texture2D>("Arrow");

            enemyManager.Clear();
            enemyManager.Add(new Enemies.MeleeEnemy(content, new Vector2(200, 50)));
            enemyManager.Add(new Enemies.ArcherEnemy(content, enemyManager, arrowTex, new Vector2(250, 50)));

            pendingRestart = false;
            restartTimer = 0f;
        }

        public void Update(GameTime gameTime, ContentManager content, Hero hero, Enemies.EnemyManager enemyManager)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!pendingRestart)
            {
                hero.Update(gameTime, CurrentLevel);
                enemyManager.Update(gameTime, CurrentLevel, hero);
                combatSystem.ApplyHeroMeleeAttack(hero, enemyManager);

                if (hero.Health <= 0)
                {
                    pendingRestart = true;
                    restartTimer = RestartDelaySeconds;
                }
            }
            else
            {
                restartTimer -= dt;
                if (restartTimer <= 0f)
                {
                    pendingRestart = false;
                    restartTimer = 0f;

                    LoadLevel1(content, hero, enemyManager);
                }
            }
        }
    }
}
