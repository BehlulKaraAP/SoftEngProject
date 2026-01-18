using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D11;
using SoftEngProject.Levels;
using SoftEngProject.Weapons;
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
        public readonly List<ArrowProjectile> arrows = new List<ArrowProjectile>();
        public void AddArrow(ArrowProjectile arrow) => arrows.Add(arrow);
        public IReadOnlyList<Enemy> Enemies => enemies;

        public void Add(Enemy enemy) => enemies.Add(enemy);
        
        public void Clear()
        {
            enemies.Clear();
            arrows.Clear();
        }

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
            for (int i = arrows.Count - 1; i >= 0; i--)
            {
                var a = arrows[i];
                a.Update(gameTime, level);

                if (a.Hitbox.Intersects(hero.Hitbox))
                {
                    hero.TakeDamage(a.Damage);
                    arrows.RemoveAt(i);
                    continue;
                }

                if (a.IsExpired)
                    arrows.RemoveAt(i);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(var e in enemies)
            {
                e.Draw(spriteBatch);
            }
            foreach (var a in arrows)
            {
                a.Draw(spriteBatch);
            }
        }
    }
}
