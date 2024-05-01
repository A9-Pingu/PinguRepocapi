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
        public int Attack { get; set; }

        public bool IsFighting { get; private set; } //몬스터 전투 참가
        public bool IsDead { get; private set; }     //몬스터 사망

        public Enemy(string name, int level, bool isFighting = false, bool isDead = false)
        {
            Level = level;
            Name = name;
            Health = CalculateHealth(level); //레벨반영
            Attack = CalculateAttack(level);
            IsFighting = isFighting;
            IsDead = isDead;
        }

        //플레이어 레벨 반영하여 몬스터 체력 조정
        private int CalculateHealth(int level)
        {
            switch (Name)
            {
                case "도둑갈매기":
                    return 50 + level * 10;
                case "야생들개":
                    return 60 + level * 10;
                case "여우":
                    return 70 + level * 10;
                case "바다표범":
                    return 100 + level * 20;
                case "늑대":
                    return 80 + level * 20;
                case "북극곰":
                    return 150 + level * 30;
                case "범고래":
                    return 200 + level * 30;
                default:
                    return 0;
            }
        }

        //플레이어 레벨 반영하여 몬스터 공격력 조정
        private int CalculateAttack(int level)
        {
            switch (Name)
            {
                case "도둑갈매기":
                    return 5 + level * 2;
                case "야생들개":
                    return 7 + level * 2;
                case "여우":
                    return 6 + level * 2;
                case "바다표범":
                    return 15 + level * 5;
                case "늑대":
                    return 20 + level * 5;
                case "북극곰":
                    return 35 + level * 10;
                case "범고래":
                    return 30 + level * 10;
                default:
                    return 0;
            }
        }
        public void EnemyAttack(Character player)
        {
            int damage = Attack;
            if (damage < 0)
            {
                damage = 0;
            }
            player.Health -= damage;
            Console.WriteLine($"{Name} 의 공격!");
            Console.WriteLine($"Lv.{player.Level} {player.Name} 에게 {Attack} 데미지를 가했습니다.");
            Console.WriteLine($"");
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {player.MaxHealth} -> {player.Health}");
            Console.WriteLine($"");
            Console.WriteLine($"0. 다음");
            Console.WriteLine($"");
            Console.Write(">>");
        }
    }
}
