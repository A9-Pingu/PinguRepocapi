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
        public int Exp { get; set; }         // 경험치 추가
        public int MaxExp { get; set; }     // 최대 경험치
        public int AttackPower { get; set; }
        public int DefensePower { get; set; }
        public int Health { get; set; }
        public int MP { get; set; }
        public int Gold { get; set; }
        public int AdditaionalDamage { get; set; } = 0;
        public int DungeonClearCount { get; private set; } = 0;  // 던전 클리어 횟수 카운트
        public int MP { get; private set; } = 50; // 기본 MP는 50


        public int MaxHealth { get; set; } = 100;  // 최대 체력 속성 추가

        public InventoryManager InventoryManager { get; set; }


        public LevelUp LevelUp { get; set; }

        public Character(string name, string job)

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
            MP = 50;


            // 최대 경험치를 초기화합니다. 예를 들어 레벨이 1일 때 최대 경험치를 설정할 수 있습니다.
            MaxExp = CalculateMaxExp(Level);
            // InventoryManager 및 EquipmentManager 초기화
            InventoryManager = new InventoryManager();
            WeaponInventoryManager = new InventoryManager();
            ArmorInventoryManager = new InventoryManager();
            ConsumableInventoryManager = new InventoryManager();
            WeaponEquipmentManager = new EquipmentManager(this);
            ArmorEquipmentManager = new EquipmentManager(this);

            InitializeSkillSet();
        }
        private void InitializeSkillSet()
        {
            // 각 직업에 따라 스킬을 선택하고 사용합니다.
            SkillSet = new SkillAction[3];
            SkillSet[(int)JobType.전사] = UseWarriorSkill;
            SkillSet[(int)JobType.마법사] = UseMageSkill;
            SkillSet[(int)JobType.도적] = UseRogueSkill;
            InventoryManager = new InventoryManager(this);

            LevelUp = new LevelUp(this);
        }

        private int CalculateMaxExp(int level)
        {
            return level * 100;
        }

        public bool HasRequiredDefense(int requiredDefense)
        {
            return DefensePower >= requiredDefense;
        }

        public void Attack(Enemy enemy)
        {
            int damage = CalculateTotalAttackPower();
            Console.WriteLine($"당신이 {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
            enemy.Health -= damage;

            // 치명타가 발생할 확률을 확인합니다.
            Random random = new Random();
            // 15% 확률로 치명타 발생
            bool isCritical = random.Next(1, 101) <= 15; 

            // 치명타가 발생한 경우
            if (isCritical)
            {
                // 160% 데미지
                int criticalDamage = (int)(AttackPower * 1.6); 
                Console.WriteLine($"당신이 {enemy.Name}에게 {criticalDamage}의 피해를 입혔습니다. - 치명타 공격!!");
                enemy.Health -= criticalDamage;
            }
            else
            {
                Console.WriteLine($"당신이 {enemy.Name}에게 {AttackPower}의 피해를 입혔습니다.");
                enemy.Health -= AttackPower;
            }
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
            int skillchoice;
            while (!int.TryParse(Console.ReadLine(), out skillchoice) || (skillchoice < 0 || skillchoice > skills.Length))
            {
                Console.WriteLine("잘못된 입력입니다. 다시 입력해주세요.");
                Console.Write("원하시는 행동을 입력해주세요: ");
            }

            if (skillchoice == 0)
            {
                Console.WriteLine("취소되었습니다.");
            }
            else
            {
                // 사용자가 선택한 인덱스에서 1을 빼서 실제 스킬의 인덱스를 계산합니다.
                SkillSet[(int)Job](this, enemy, skillchoice - 1);
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

        // 전사 스킬 사용 부분
        private void UseWarriorSkill(Character player, Enemy enemy, int skillIndex)
        {
            // int skillIndex = ChooseSkillIndex(player); // 메서드 내부에서 선언된 skillIndex 변수 제거
            if (skillIndex == -1) return;

            int requiredMP = skillIndex == 0 ? 10 : 15;
            // 펭귄 슬래시와 크로스 어택의 데미지를 각각 3배와 2배로 설정
            int damage = skillIndex == 0 ? player.AttackPower * 3 : player.AttackPower * 2;
            if (player.MP < requiredMP)
            {
                Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                return;
            }

            // 스킬을 선택하고 필요한 MP 및 데미지 계산 후 스킬을 사용합니다.
            string skillName = WarriorSkills[skillIndex];
            Console.WriteLine($"{skillName}를 사용합니다. - MP {requiredMP}");
            Console.WriteLine($"당신이 {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
            enemy.Health -= damage;
            player.MP -= requiredMP;
        }

        // 마법사 스킬 사용 부분
        private void UseMageSkill(Character player, Enemy enemy, int skillIndex)
        {
            // int skillIndex = ChooseSkillIndex(player); // 메서드 내부에서 선언된 skillIndex 변수 제거
            if (skillIndex == -1) return;

            int requiredMP = skillIndex == 0 ? 15 : 20;
            // 파이어볼과 아르페지오의 데미지를 각각 3배와 5배로 설정
            int damage = skillIndex == 0 ? player.AttackPower * 3 : player.AttackPower * 5;
            if (player.MP < requiredMP)
            {
                Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                return;
            }

            // 스킬을 선택하고 필요한 MP 및 데미지 계산 후 스킬을 사용합니다.
            string skillName = MageSkills[skillIndex];
            Console.WriteLine($"{skillName}를 사용합니다. - MP {requiredMP}");
            Console.WriteLine($"당신이 {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
            enemy.Health -= damage;
            player.MP -= requiredMP;
        }

        // 도적 스킬 사용 부분
        private void UseRogueSkill(Character player, Enemy enemy, int skillIndex)
        {
            // int skillIndex = ChooseSkillIndex(player); // 메서드 내부에서 선언된 skillIndex 변수 제거
            if (skillIndex == -1) return;

            int requiredMP = skillIndex == 0 ? 10 : 15;
            // 더블 펭펭이와 스프릿 대거의 데미지를 각각 2배와 4배로 설정
            int damage = skillIndex == 0 ? player.AttackPower * 2 : player.AttackPower * 4;
            if (player.MP < requiredMP)
            {
                Console.WriteLine("MP가 부족하여 스킬을 사용할 수 없습니다.");
                return;
            }

            // 스킬을 선택하고 필요한 MP 및 데미지 계산 후 스킬을 사용합니다.
            string skillName = RogueSkills[skillIndex];
            Console.WriteLine($"{skillName}를 사용합니다. - MP {requiredMP}");
            Console.WriteLine($"당신이 {enemy.Name}에게 {damage}의 피해를 입혔습니다.");
            enemy.Health -= damage;
            player.MP -= requiredMP;
        }



        private int ChooseSkillIndex(Character player)
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
            enemy.Health -= AttackPower
        }

        public void UseItem(Item item, int count = 1)
        {
            if(item.Type != ItemType.Consumable) return;
            InventoryManager.AddItemStatBonus(item);
            InventoryManager.RemoveItem(item, count);
            Console.WriteLine($"{item.Name}을 {count}개를 사용하였습니다.");
        }

    }
}
