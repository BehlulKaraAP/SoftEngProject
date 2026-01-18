using SoftEngProject.Enemies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftEngProject.Combat
{
    internal class CombatSystem
    {
        private readonly int heroAttackDamage;

        public CombatSystem(int heroAttackDamage = 1)
        {
            this.heroAttackDamage = heroAttackDamage;
        }

        public void ApplyHeroMeleeAttack(Hero hero, EnemyManager enemyManager)
        {
            if (!hero.IsAttacking || hero.AttackDamageApplied)
                return;

            var atk = hero.AttackHitbox;

            foreach (var e in enemyManager.Enemies)
            {
                if (!e.IsDead && atk.Intersects(e.Hitbox))
                {
                    e.TakeDamage(heroAttackDamage);
                    hero.MarkAttackDamageApplied();
                    break;
                }
            }
        }
    }
}
