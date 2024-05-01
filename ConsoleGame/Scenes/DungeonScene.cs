using ConsoleGame.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleGame.Scenes
{
    public class DungeonScene
    {
        private Character player;
        private Random random;
        private Dungeon dungeon;
        public DungeonScene(Character character)
        {
            player = character;
            random = new Random();
        }



        private int CalculateAdditionalReward(int attackPower)
        {
            double percentage = random.Next(10, 21) / 100.0; // 10% ~ 20% 랜덤 값

            int additionalReward = (int)(attackPower * percentage);  // double 값을 int로 캐스팅

            return additionalReward;
        }

        private int CalculateReward(int attackPower)
        {
            int additionalReward = CalculateAdditionalReward(attackPower);

            int totalReward = dungeon.baseReward + additionalReward;

            return totalReward;
        }

        public void Start(Difficulty difficulty)
        {
            if (!player.HasRequiredDefense(dungeon.requiredDefense))
            {
                Console.WriteLine($"방어력이 {dungeon.requiredDefense} 이상이어야 {difficulty} 던전에 입장할 수 있습니다.");
                return;
            }

            bool success = random.Next(1, 101) <= 60;

            if (!success)
            {
                Console.WriteLine($"{difficulty} 던전 입장 실패! 보스를 처치하면 보상이 없으며 체력이 절반으로 감소합니다.");
                player.Health /= 2;
                return;
            }

            Console.WriteLine($"{difficulty} 던전 입장 성공!");

            Enemy enemy = GenerateEnemy(difficulty);

            Console.WriteLine($"[적 정보: {enemy.Name}, 레벨 {enemy.Level}, 체력 {enemy.Health}, 공격력 {enemy.AttackPower}]");

            while (player.Health > 0 && enemy.Health > 0)
            {
                Console.WriteLine("\n턴을 선택하세요:");
                Console.WriteLine("1. 공격");
                Console.WriteLine("2. 아이템 사용");
                Console.Write(">> ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        player.Attack(enemy);
                        break;
                    case "2":
                        UseItem();
                        break;
                    default:
                        Console.WriteLine("잘못된 선택입니다.");
                        break;
                }

                if (enemy.Health > 0)
                {
                    enemy.EnemyAttack(player);
                }
            }

            if (player.Health <= 0)
            {
                Console.WriteLine("당신은 패배했습니다.");
            }
            else
            {
                Console.WriteLine("적을 물리쳤습니다!");
                int additionalReward = CalculateAdditionalReward(player.AttackPower);

                Console.WriteLine($"기본 보상: {dungeon.baseReward} G");
                Console.WriteLine($"공격력으로 인한 추가 보상: {additionalReward} G");

                int totalReward = dungeon.baseReward + additionalReward;
                Console.WriteLine($"총 보상: {totalReward} G를 획득하였습니다.");

                player.Gold += totalReward;

                if (random.Next(1, 101) <= 20) // 15~20% 확률로 드롭
                {
                    DropHighTierItem();
                }
            }

            ClearDungeon();
        }

        private void DropHighTierItem()
        {
            //List<Item> highTierItems = new List<Item>();

            //highTierItems.AddRange(player.WeaponInventoryManager.GetItemsByType(ItemType.Weapon));
            //highTierItems.AddRange(player.ArmorInventoryManager.GetItemsByType(ItemType.Armor));

            //if (highTierItems.Count == 0)
            //{
            //    Console.WriteLine("상위 무기나 방어구가 없습니다.");
            //    return;
            //}

            //Item droppedItem = highTierItems[random.Next(highTierItems.Count)];

            //Console.WriteLine($"상위 아이템을 획득하였습니다: {droppedItem.Name}");
            //player.InventoryManager.AddItem(droppedItem);
        }

        private void ClearDungeon()
        {
            int damage = CalculateDamage();

            player.Health -= damage;

            Console.WriteLine($"던전 클리어! 체력 {damage} 소모됨.");
            Console.WriteLine($"남은 체력: {player.Health}");

            if (random.Next(1, 101) <= 20) // 20% 확률로 특별한 아이템 드롭
            {
                DropSpecialItem(dungeon.difficulty); // difficulty를 전달
            }
        }

        public void EnterDungeon()
        {
            Console.WriteLine("1. 쉬운 던전     | 방어력 5 이상 권장");
            Console.WriteLine("2. 일반 던전     | 방어력 11 이상 권장");
            Console.WriteLine("3. 어려운 던전    | 방어력 17 이상 권장");
            Console.WriteLine("0. 나가기");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            int InputKey = Game.instance.inputManager.GetValidSelectedIndex((int)Difficulty.Max - 1, (int)Difficulty.Easy);
            dungeon = new Dungeon((Difficulty)InputKey);

            if (!player.HasRequiredDefense(dungeon.requiredDefense))
            {
                Console.WriteLine($"방어력이 {dungeon.requiredDefense} 이상이어야 {dungeon.difficulty} 던전에 입장할 수 있습니다.");
                return;
            }

            Start(dungeon.difficulty);

            DropSpecialItem(dungeon.difficulty);

        }

        private void DropSpecialItem(Difficulty difficulty)
        {
            // 노말 던전부터 하드 던전까지 특별 아이템 드롭
            if (difficulty == Difficulty.Normal ||
                difficulty == Difficulty.Hard)
            {
                // 무작위로 하나의 아이템 선택
                Item droppedItem = Game.instance.itemManager.specialItems[random.Next(Game.instance.itemManager.specialItems.Count)];

                Console.WriteLine($"특별한 아이템을 획득하였습니다: {droppedItem.Name}");

                // 귀속 아이템이므로 Purchased 값을 true로 설정
                Game.instance.itemManager.UpdateItemPurchasedStatus(droppedItem);

                // 아이템을 인벤토리의 장비 카테고리에 추가
                player.InventoryManager.AddItem(droppedItem);
            }
        }



        private int CalculateDamage()
        {
            int baseDamage = random.Next(20, 36); // 20 ~ 35 랜덤 값
            int difference = player.DefensePower - dungeon.requiredDefense;
            int extraDamage = difference > 0 ? random.Next(difference + 1) : 0;
            int totalDamage = baseDamage + extraDamage;

            return totalDamage;
        }

        //몬스터 작업하는 분이 가져가야 할듯
        private Enemy GenerateEnemy(Difficulty difficulty)
        {
            int level;
            int health;
            int attackPower;

            switch (difficulty)
            {
                case Difficulty.Easy:
                    level = player.Level + 1;
                    health = 50 + (level * 10);
                    attackPower = 5 + (level * 2);
                    break;
                case Difficulty.Normal:
                    level = player.Level + 2;
                    health = 200 + (level * 20);
                    attackPower = 20 + (level * 10);
                    break;
                case Difficulty.Hard:
                    level = player.Level + 5;
                    health = 350 + (level * 40);
                    attackPower = 35 + (level * 40);
                    break;
                default:
                    throw new ArgumentException("Invalid difficulty");
            }

            return new Enemy(level, health, attackPower, $"적 레벨 {level}");
        }

        private void UseItem()
        {
            Console.WriteLine("사용할 아이템을 선택하세요.");
            // 아이템 사용 로직은 구현하지 못했습니다
        }
    }
}
