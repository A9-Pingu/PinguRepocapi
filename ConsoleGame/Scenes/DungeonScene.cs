using ConsoleGame.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

        List<Enemy> allMonsters = new List<Enemy>
        {
            new Enemy("도둑갈매기", 2), // 플레이어 레벨을 1로 설정(난이도 선택시 구분됨)
            new Enemy("야생들개", 2),
            new Enemy("여우", 2),
            new Enemy("바다표범", 3),
            new Enemy("늑대", 3),
            new Enemy("북극곰", 4),
            new Enemy("범고래", 5)

        };
        //-------------------------------------------------------
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
            //----------------------------------------

            Console.WriteLine($"{difficulty} 던전 입장 성공!");
            Console.WriteLine("     Battle!!     ");
            Console.WriteLine("");

            int difficultyIndex = 0;
            if (difficulty == Difficulty.Easy)
            {
                difficultyIndex = 0;
            }
            else if (difficulty == Difficulty.Normal)
            {
                difficultyIndex = 2;
            }
            else
            {
                difficultyIndex = 4;
            }

            List<Enemy> selectedMonsters = new List<Enemy>();
            // 선택된 난이도에 따라 전투참가몬스터 선택
            for (int i = difficultyIndex; i < difficultyIndex + 3; i++) //0,1,2//2,3,4//4,5,6
            {
                selectedMonsters.Add(allMonsters[i]); //0,1,2번째 몬스터 추가
            }
            List<Enemy> battleMonsters = new List<Enemy>();
            for (int i = 0; i < 3; i++)
            {
                int index = random.Next(selectedMonsters.Count);
                battleMonsters.Add(selectedMonsters[index]);
                selectedMonsters.Remove(selectedMonsters[index]);
            }

            foreach (var monster in battleMonsters)
            {
                Console.WriteLine($"Lv.{monster.Level} {monster.Name} HP {monster.Health} ATK {monster.Attack}");
            }

            //int keyInput = int.Parse(Console.ReadLine());

            while (player.Health > 0 && monster.Health > 0)
            //&& enemy.Health > 0)
            {
                Console.WriteLine(""); //여기 반복이 안되고 있음.
                Console.WriteLine("");
                //플레이어 정보(Lv. 이름 (직업) /n HP)
                Console.WriteLine("");
                Console.WriteLine("1. 공격");
                Console.WriteLine("2. 아이템 사용");
                Console.WriteLine("원하시는 행동을 입력해주세요.");
                Console.Write(">> ");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1: //공격 선택

                        break;
                    case 2:
                        UseItem();
                        break;
                    default:
                        Console.WriteLine("잘못된 선택입니다.");
                        break;
                }

                if (monster.Health > 0)
                {
                    monster.EnemyAttack(player);
                }
            }


            if (player.Health <= 0)
            {
                Console.WriteLine("당신은 패배했습니다.");
            }
            else
            {
                Console.WriteLine("적을 물리쳤습니다!");
                int additionalReward = CalculateAdditionalReward(player.CalculateTotalAttackPower());

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
        //-------------------------------------------------------

        private void DropHighTierItem()
        {
            List<Item> highTierItems = new List<Item>();

            highTierItems.AddRange(player.WeaponInventoryManager.GetItemsByType(ItemType.Weapon));
            highTierItems.AddRange(player.ArmorInventoryManager.GetItemsByType(ItemType.Armor));

            if (highTierItems.Count == 0)
            {
                Console.WriteLine("상위 무기나 방어구가 없습니다.");
                return;
            }

            Item droppedItem = highTierItems[random.Next(highTierItems.Count)];

            Console.WriteLine($"상위 아이템을 획득하였습니다: {droppedItem.Name}");
            player.AddItem(droppedItem);
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

            int InputKey = Game.instance.inputManager.GetValidSelectedIndex((int)Difficulty.Max, (int)Difficulty.Easy);
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
                player.AddItem(droppedItem);
            }
        }



        private int CalculateDamage()
        {
            int baseDamage = random.Next(20, 36); // 20 ~ 35 랜덤 값
            int difference = player.CalculateTotalDefensePower() - dungeon.requiredDefense;
            int extraDamage = difference > 0 ? random.Next(difference + 1) : 0;
            int totalDamage = baseDamage + extraDamage;

            return totalDamage;
        }

        private void UseItem()
        {
            Console.WriteLine("사용할 아이템을 선택하세요.");
            // 아이템 사용 로직은 구현하지 못했습니다
        }
    }
}
