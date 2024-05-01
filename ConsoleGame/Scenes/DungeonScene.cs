using ConsoleGame.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Numerics;
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
        private Enemy monster;

        public DungeonScene(Character character)
        {
            player = character;
            Random random = new Random(Guid.NewGuid().GetHashCode());
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

            Console.WriteLine("");
            Console.WriteLine($"{difficulty} 던전 입장 성공!");

            // 몬스터 선택
            List<Enemy> selectedMonsters = SelectMonsters(difficulty);
            List<Enemy> deadMonsters = new List<Enemy>(); //죽은 몬스터 수

            // UIManager의 ShowBattle 메서드 호출
            Game.instance.uiManager.BattleScene(difficulty, dungeon, selectedMonsters, false, player); //--
            int inputKey = Game.instance.inputManager.GetValidSelectedIndex(3);

            switch (inputKey)
            {
                case 0: return;
                case 1: //공격
                    Game.instance.uiManager.BattleScene(difficulty, dungeon, selectedMonsters, true, player); //1 2
                    while(player.Health > 0)
                    {
                        inputKey = Game.instance.inputManager.GetValidSelectedIndex(3);
                        // monster.health > 0 -> player attack
                        if (selectedMonsters[inputKey - 1].Health > 0)
                        {
                            player.Attack(selectedMonsters[inputKey - 1]);
                            Game.instance.inputManager.GetValidSelectedIndex(1);
                            //monster attack
                            if (!deadMonsters.Contains(selectedMonsters[inputKey - 1]))
                            {
                                selectedMonsters[inputKey - 1].EnemyAttack(player);
                            }
                        }
                        // monster.health == 0
                        else if (deadMonsters.Contains(selectedMonsters[inputKey - 1]))
                        {
                            deadMonsters.Add(selectedMonsters[inputKey - 1]);
                            Console.WriteLine("잘못된 입력입니다");
                            // all monsters die
                            if (deadMonsters.Contains(selectedMonsters[inputKey - 1]) && deadMonsters.Count >= 3)
                            {
                                break;
                            }
                        }
                    }
                    if (player.Health <= 0)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Battle!! - Result");
                        Console.WriteLine("");
                        Console.WriteLine("You Lose.");
                        Console.WriteLine("");
                        Console.WriteLine($"Lv.{player.Level} {player.Name}");
                        Console.WriteLine($"HP {player.MaxHealth} -> {player.Health}");
                        Console.WriteLine("0. 다음");
                        Console.WriteLine("");
                        Game.instance.inputManager.GetValidSelectedIndex(1);
                    }
                    else if (deadMonsters.Count <= 0)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("Battle!! - Result");
                        Console.WriteLine("");
                        Console.WriteLine("Victory");
                        Console.WriteLine("");
                        Console.WriteLine("던전에서 몬스터 3마리를 잡았습니다.");
                        Console.WriteLine("");
                        Console.WriteLine($"Lv.{player.Level} {player.Name}");
                        Console.WriteLine($"HP {player.MaxHealth} -> {player.Health}");
                        int additionalReward = CalculateAdditionalReward(player.AttackPower);

                        Console.WriteLine($"기본 보상: {dungeon.baseReward} G");
                        Console.WriteLine($"공격력으로 인한 추가 보상: {additionalReward} G");

                        int totalReward = dungeon.baseReward + additionalReward;
                        Console.WriteLine($"총 보상: {totalReward} G를 획득하였습니다.");
                        Console.WriteLine("0. 다음");
                        Console.WriteLine("");
                        Game.instance.inputManager.GetValidSelectedIndex(1);

                        player.Gold += totalReward;

                        if (random.Next(1, 101) <= 20) // 15~20% 확률로 드롭
                        {
                            DropHighTierItem();
                        }
                    }
                    break;
                case 2: //아이템
                    UseItem();
                    break;

                default:
                    Console.WriteLine("잘못된 입력입니다");
                    break;

            }
            ClearDungeon();

        }
        private List<Enemy> SelectMonsters(Difficulty difficulty)
        {
            // 모든 몬스터
            List<Enemy> allMonsters = new List<Enemy>
            {
                new Enemy("도둑갈매기", 2),
                new Enemy("야생들개", 2),
                new Enemy("여우", 2),
                new Enemy("바다표범", 3),
                new Enemy("늑대", 3),
                new Enemy("북극곰", 4),
                new Enemy("범고래", 5)
            };

            // 선택한 난이도에 따라 랜덤으로 몬스터 3마리 선택
            int difficultyIndex = difficulty switch
            {
                Difficulty.Easy => 0,
                Difficulty.Normal => 2,
                Difficulty.Hard => 4,
                _ => 0
            };

            List<Enemy> selectedMonsters1 = new List<Enemy>();
            List<Enemy> selectedMonsters2 = new List<Enemy>();

            for (int i = difficultyIndex; i < difficultyIndex + 3; i++)
            {
                selectedMonsters1.Add(allMonsters[i]);
                selectedMonsters2.Add(allMonsters[i]);
                selectedMonsters2.Add(selectedMonsters1[i]);
                Random random = new Random();
                if (selectedMonsters2.Count > 5)
                {
                    selectedMonsters2.Remove(selectedMonsters2[random.Next(0, 4)]);
                    selectedMonsters2.Remove(selectedMonsters2[random.Next(0, 4)]);
                    selectedMonsters2.Remove(selectedMonsters2[random.Next(0, 4)]);
                }
            }
            return selectedMonsters2;
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
            Console.WriteLine("0. 나가기"); //$$나가기가 안 됨
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            int InputKey = Game.instance.inputManager.GetValidSelectedIndex((int)Difficulty.Max-1, (int)Difficulty.Easy);
            dungeon = new Dungeon((Difficulty)InputKey);

            if (!player.HasRequiredDefense(dungeon.requiredDefense))
            {
                Console.WriteLine($"방어력이 {dungeon.requiredDefense} 이상이어야 {dungeon.difficulty} 던전에 입장할 수 있습니다.");
                return;
            }

            Start(dungeon.difficulty);

            DropSpecialItem(dungeon.difficulty);

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

        private void UseItem()
        {
            Console.WriteLine("사용할 아이템을 선택하세요.");
            // 아이템 사용 로직은 구현하지 못했습니다
        }
    }
}
