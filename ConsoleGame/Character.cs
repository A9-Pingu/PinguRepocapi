using System;
using System.Collections.Generic;
using System.Numerics;
using ConsoleGame.Managers;

namespace ConsoleGame
{
    public enum JobType
    {
        전사,
        마법사,
        도적
    }

    public class Character
    {
        public string Name { get; set; }
        public JobType Job { get; set; }
        public int Level { get; set; }
        public LevelUp LevelUp { get; set; }
        public int Exp { get; set; }
        public int MaxExp { get; set; }
        public int AttackPower { get; set; }
        public int DefensePower { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; } = 100;
        public int Gold { get; set; }
        public int AdditionalDamage { get; set; } = 0;

        public int DungeonClearCount { get; private set; } = 0;
        public int MP { get; set; } = 50;

        public InventoryManager InventoryManager { get; set; }

        public delegate void SkillAction(Character player, Enemy enemy, int skillIndex);
        private SkillAction[] SkillSet;

        private readonly string[] WarriorSkills = { "펭귄 슬래시 - MP 10", "크로스 어택 - MP 15" };
        private readonly string[] MageSkills = { "펭귄 행진곡 - MP 15", "아르페지오 - MP 20" };
        private readonly string[] RogueSkills = { "더블 펭펭이 - MP 10", "스프릿 대거 - MP 15" };

        public Character(string name, JobType job)
        {
            Name = name;
            Job = job;
            Level = 1;
            Exp = 0;
            AttackPower = 10;
            DefensePower = 5;
            Gold = 1500;
            Health = MaxHealth;

            MaxExp = CalculateMaxExp(Level);
            InventoryManager = new InventoryManager(this);
            InitializeSkillSet();
        }

        private void InitializeSkillSet()
        {
            SkillSet = new SkillAction[3];
            SkillSet[(int)JobType.전사] = UseWarriorSkill;
            SkillSet[(int)JobType.마법사] = UseMageSkill;
            SkillSet[(int)JobType.도적] = UseRogueSkill;
            LevelUp = new LevelUp(this);
        }

        private int CalculateMaxExp(int level)
        {
            return level * 100;
        }

        public void Attack(Enemy enemy)
        {
            Random random = new Random();
            double percentage = random.NextDouble() * 0.10 - 0.05; // 공격력 10% 오차범위
            int totalAttackPower = (int)(AttackPower * (1 + percentage)) + AdditionalDamage;

            int enemyMaxHP = enemy.Health;
            bool isCritical = random.Next(1, 101) <= 15; // 15% 확률로 치명타 발생

            if (isCritical)
            {
                totalAttackPower = (int)(totalAttackPower * 1.6); // 치명타 시 160% 데미지
                Console.WriteLine($"당신이 {enemy.Name}에게 {totalAttackPower}의 피해를 입혔습니다. - 치명타 공격!!");
            }
            else
            {
                Console.WriteLine($"당신이 {enemy.Name}에게 {totalAttackPower}의 피해를 입혔습니다.");
            }

            if (enemy.Health <= totalAttackPower)
            {
                enemy.Health = 0;
            }
            else
            {
                enemy.Health -= totalAttackPower;
            }

            Console.WriteLine("===================");
            Console.WriteLine($"{Name} 의 공격!");
            Console.WriteLine($"Lv.{enemy.Level} {enemy.Name} 에게 {totalAttackPower} 데미지를 가했습니다.");
            Console.WriteLine($"");
            Console.WriteLine($"Lv.{enemy.Level} {enemy.Name}");
            Console.WriteLine($"HP {enemyMaxHP} -> {enemy.Health}");
            Console.WriteLine($"");
            Console.WriteLine($"0. 다음");
            Console.WriteLine($"");
            Console.Write(">>");
        }


        public void UseSkill(Enemy enemy)
        {
            Console.WriteLine("[내정보]");
            Console.WriteLine($"Lv.{Level} {Name} ({Job})");
            Console.WriteLine($"HP {Health}/{MaxHealth}");
            Console.WriteLine($"MP {MP}/50");

            Console.WriteLine("\n[스킬]");
            string[] skills = GetSkillList();
            for (int i = 0; i < skills.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {skills[i]}");
            }
            Console.WriteLine("0. 취소");

            Console.Write("\n원하시는 행동을 입력해주세요: ");
            int skillChoice;
            while (!int.TryParse(Console.ReadLine(), out skillChoice) || (skillChoice < 0 || skillChoice > skills.Length))
            {
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                Console.Write("원하시는 행동을 입력해주세요: ");
            }

            if (skillChoice == 0)
            {
                Console.WriteLine("취소되었습니다.");
            }
            else
            {
                SkillSet[(int)Job](this, enemy, skillChoice - 1);
            }
        }

        private string[] GetSkillList()
        {
            return Job switch
            {
                JobType.전사 => WarriorSkills,
                JobType.마법사 => MageSkills,
                JobType.도적 => RogueSkills,
                _ => new string[0],
            };
        }

        private void UseWarriorSkill(Character player, Enemy enemy, int skillIndex)
        {
            int requiredMP = skillIndex == 0 ? 10 : 15;
            int damage = skillIndex == 0 ? player.AttackPower * 3 : player.AttackPower * 2;
            if (player.MP < requiredMP)
            {
                Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                return;
            }

            string skillName = WarriorSkills[skillIndex];
            Console.WriteLine($"{skillName}를 사용합니다. - MP {requiredMP}");
            Console.WriteLine($"당신이 {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
            enemy.Health -= damage;
            player.MP -= requiredMP;
        }

        private void UseMageSkill(Character player, Enemy enemy, int skillIndex)
        {
            int requiredMP = skillIndex == 0 ? 15 : 20;
            int damage = skillIndex == 0 ? player.AttackPower * 3 : player.AttackPower * 5;
            if (player.MP < requiredMP)
            {
                Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                return;
            }

            string skillName = MageSkills[skillIndex];
            Console.WriteLine($"{skillName}를 사용합니다. - MP {requiredMP}");
            Console.WriteLine($"당신이 {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
            enemy.Health -= damage;
            player.MP -= requiredMP;
        }

        private void UseRogueSkill(Character player, Enemy enemy, int skillIndex)
        {
            int requiredMP = skillIndex == 0 ? 10 : 15;
            int damage = skillIndex == 0 ? player.AttackPower * 2 : player.AttackPower * 4;
            if (player.MP < requiredMP)
            {
                Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                return;
            }

            string skillName = RogueSkills[skillIndex];
            Console.WriteLine($"{skillName}를 사용합니다. - MP {requiredMP}");
            Console.WriteLine($"당신이 {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
            enemy.Health -= damage;
            player.MP -= requiredMP;
        }

        private int ChooseSkillIndex(Enemy enemy)
        {
            Console.Write("\n원하시는 행동을 입력해주세요: ");
            int skillChoice;
            while (!int.TryParse(Console.ReadLine(), out skillChoice) || (skillChoice < 0 || skillChoice > 2))
            {
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                Console.Write("원하시는 행동을 입력해주세요: ");
            }
            return skillChoice;
            Console.WriteLine($"당신이 {enemy.Name}에게 {AttackPower}의 피해를 입혔습니다.");
            enemy.Health -= AttackPower;

        }

        public void UseItem(Item item, int count = 1)
        {
            if (item.Type != ItemType.Consumable) return;
            InventoryManager.AddItemStatBonus(item);
            InventoryManager.RemoveItem(item, count);
            Console.WriteLine($"{item.Name}을 {count}개를 사용하였습니다.");
        }

    }
}
 
