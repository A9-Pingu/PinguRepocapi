using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Managers
{
    public class Enemy
    {
        public string Name { get; set; }
        public int Level { get; set; }
        public int Health { get; set; }
        public int AttackPower { get; set; }

        public Enemy(int level, int health, int attackPower, string name)
        {
            Level = level;
            Health = health;
            AttackPower = attackPower;
            Name = name;
        }
        public void EnemyAttack(Character player)
        {
            int damage = AttackPower;
            if (damage < 0)
            {
                damage = 0;
            }
            Console.WriteLine($"{Name}이(가) 당신에게 {damage}의 피해를 입혔습니다.");
            player.Health -= damage;
        }
    }
}
