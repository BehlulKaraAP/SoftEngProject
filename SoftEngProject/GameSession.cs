using Microsoft.Xna.Framework;
using SoftEngProject.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject
{
    internal class GameSession
    {
        private readonly int tileSize;
        private readonly LevelFactory levelFactory;

        public Level CurrentLevel { get; private set; }
        public bool IsRestartPending => pendingRestart;

        private bool pendingRestart = false;
        private float restartTimer = 0f;
        private const float RestartDelaySeconds = 1.0f;

        public GameSession(LevelFactory levelFactory, int tileSize)
        {
            this.levelFactory = levelFactory;
            this.tileSize = tileSize;
        }

        public void LoadLevel1(Microsoft.Xna.Framework.Content.ContentManager content, Hero hero, Enemies.EnemyManager enemyManager)
        {
            CurrentLevel = LevelFactory.CreateLevel1(content, tileSize);
            ResetWorld(hero, enemyManager, content);
        }

        public void LoadLevel2(Microsoft.Xna.Framework.Content.ContentManager content, Hero hero, Enemies.EnemyManager enemyManager)
        {
            CurrentLevel = LevelFactory.CreateLevel2(content, tileSize);
            ResetWorld(hero, enemyManager, content);
        }

        private void ResetWorld(Hero hero, Enemies.EnemyManager enemyManager, Microsoft.Xna.Framework.Content.ContentManager content)
        {
            hero.ResetHealth();
            hero.Position = CurrentLevel.HeroSpawn;
            hero.Physics.velocity = Microsoft.Xna.Framework.Vector2.Zero;

            enemyManager.Clear();
            enemyManager.Add(new Enemies.MeleeEnemy(content, new Microsoft.Xna.Framework.Vector2(200, 50)));
            enemyManager.Add(new Enemies.ArcherEnemy(content, new Microsoft.Xna.Framework.Vector2(250, 50)));

            pendingRestart = false;
            restartTimer = 0f;
        }

        public void Update(GameTime gameTime, Microsoft.Xna.Framework.Content.ContentManager content, Hero hero, Enemies.EnemyManager enemyManager)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (!pendingRestart)
            {
                hero.Update(gameTime, CurrentLevel);
                enemyManager.Update(gameTime, CurrentLevel, hero);

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
                    // always restart to level 1
                    LoadLevel1(content, hero, enemyManager);
                }
            }
        }
    }
}
