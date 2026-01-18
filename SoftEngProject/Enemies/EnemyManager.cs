using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D11;
using SoftEngProject.Levels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Enemies
{
    internal class EnemyManager
    {
        private readonly List<Enemy> enemies = new List<Enemy>();

        public IReadOnlyList<Enemy> Enemies => enemies;

        public void Add(Enemy enemy) => enemies.Add(enemy);

        public void Update(GameTime gameTime, Level level, Hero hero)
        {
            for (int i = enemies.Count - 1; i >= 0; i--)
            {
                enemies[i].Update(gameTime, level, hero);
                if (enemies[i].IsDead)
                {
                    enemies.RemoveAt(i);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var e in enemies)
            {
                e.Draw(spriteBatch);
            }
        }
    }
}
