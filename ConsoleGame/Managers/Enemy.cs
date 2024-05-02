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

        public Enemy(string name, int level)
        {
            Level = level;
            Name = name;
            Health = CalculateHealth(level); //레벨반영
            Attack = CalculateAttack(level);
        }

        //플레이어 레벨 반영하여 몬스터 체력 조정
        private int CalculateHealth(int level)
        {
            switch (Name)
            {
                case "도둑갈매기":
                    return 30 + level * 4;
                case "야생들개":
                    return 40 + level * 4;
                case "여우":
                    return 45 + level * 4;
                case "바다표범":
                    return 80 + level * 5;
                case "늑대":
                    return 60 + level * 5;
                case "북극곰":
                    return 100 + level * 6;
                case "범고래":
                    return 150 + level * 6;
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
                    return 4 + level;
                case "야생들개":
                    return 6 + level;
                case "여우":
                    return 6 + level;
                case "바다표범":
                    return 10 + level * 2;
                case "늑대":
                    return 14 + level * 2;
                case "북극곰":
                    return 20 + level * 3;
                case "범고래":
                    return 17 + level * 3;

                default:
                    return 0;
            }
        }
        public void EnemyAttack(Character player)
        {
            if (player.Health < Attack)
            {
                player.Health = 0;
            }
            else
                player.Health -= Attack;
            Console.WriteLine($"{Name} 의 공격!");
            Console.WriteLine($"Lv.{player.Level} {player.Name} 에게 {Attack} 데미지를 가했습니다.");
            Console.WriteLine($"");
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {player.MaxHealth} -> {player.Health}");
            Console.WriteLine($"");
            Console.WriteLine($"0. 다음");
            Console.WriteLine($"1. 아이템사용");
            Console.WriteLine($"");
            Console.Write(">>");
        }
    }
}
