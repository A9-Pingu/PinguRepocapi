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
            // 회피할 확률을 확인합니다.
            Random random = new Random();
            bool isDodged = random.Next(1, 101) <= 10; // 10% 확률로 회피 발생

            // 회피가 발생한 경우
            if (isDodged && Name != "스킬") // 스킬은 회피할 수 없음
            {
                Console.WriteLine($"{Name}이(가) 당신의 공격을 회피했습니다.");
            }
            else
            {
                // 회피가 발생하지 않은 경우에만 공격을 수행합니다.
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
}