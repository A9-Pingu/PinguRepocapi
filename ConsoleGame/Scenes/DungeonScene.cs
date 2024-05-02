using ConsoleGame.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGame.Scenes
{
    public class DungeonScene
    {
        private Character player;
        private Random random;
        private Dungeon dungeon;
        bool useItem = false;

        public DungeonScene(Character character)
        {
            player = character;
            random = new Random(Guid.NewGuid().GetHashCode());
        }

        // 던전 입장 조건
        public void EnterDungeon()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("===================");
                Console.WriteLine("1. 쉬운 던전     | 누구나 가능");
                Console.WriteLine("2. 일반 던전     | 방어력 12 이상 권장");
                Console.WriteLine("3. 어려운 던전    | 방어력 24 이상 권장");
                Console.WriteLine("0. 나가기");
                Console.Write("원하시는 행동을 입력해주세요.\n>> ");
                int InputKey = Game.instance.inputManager.GetValidSelectedIndex((int)Difficulty.Max - 1);//, (int)Difficulty.Easy);
                dungeon = new Dungeon((Difficulty)InputKey);

                if (InputKey == 0)
                    return;

                if (dungeon.requiredDefense < player.DefensePower)
                {
                    Start(dungeon.difficulty);
                    DropSpecialItem(dungeon.difficulty);
                }
                else
                {
                    Console.WriteLine($"방어력이 {dungeon.requiredDefense} 이상이어야 {dungeon.difficulty} 던전에 입장할 수 있습니다.");
                    Game.instance.inputManager.InputAnyKey();
                    Console.Clear();
                }
            }
        }

        List<Enemy> selectedMonsters;
        public void Start(Difficulty difficulty)
        {
            Console.WriteLine("");
            Console.WriteLine("===================");
            Console.WriteLine($"{difficulty} 던전 입장 성공!");
            Console.WriteLine("===================");
            selectedMonsters = SelectMonsters(difficulty);
            // 몬스터 선택
            // UIManager의 ShowBattle 메서드 호출

            while (true)
            {
                Game.instance.uiManager.BattleScene(selectedMonsters, player); // --
                int inputKey = Game.instance.inputManager.GetValidSelectedIndex(selectedMonsters.Count + 1);

                if (inputKey == 0)
                {
                    Console.WriteLine("===================");
                    Console.WriteLine("이대로 던전을 나가시겠습니까?");
                    Console.WriteLine("0. 네");
                    Console.WriteLine("1. 아니오");
                    int nextKey = int.Parse(Console.ReadLine());
                    if (nextKey == 0)
                        return;
                }                
                Battle(inputKey);
                UseItem();                        
                if (player.Health <= 0)
                {
                    LoseScene();
                    return;
                }
                else if (selectedMonsters.Count == 0)
                {
                    ClearDungeon();
                    return;
                }
            }
        }

        private List<Enemy> SelectMonsters(Difficulty difficulty)
        {
            // 모든 몬스터
            List<Enemy> allMonsters = new List<Enemy>
            {
                new Enemy("도둑갈매기", 2),
                new Enemy("도둑갈매기", 2),
                new Enemy("야생들개", 2),
                new Enemy("야생들개", 2),
                new Enemy("여우", 2),
                new Enemy("여우", 2),
                new Enemy("바다표범", 3),
                new Enemy("바다표범", 3),
                new Enemy("늑대", 3),
                new Enemy("늑대", 3),
                new Enemy("북극곰", 4),
                new Enemy("북극곰", 4),
                new Enemy("범고래", 5),
                new Enemy("범고래", 5),
            };

            // 난이도별 랜덤 몬스터 3마리 선택
            int difficultyIndex = difficulty switch
            {
                Difficulty.Easy => 0,
                Difficulty.Normal => 4,
                Difficulty.Hard => 8,
                _ => 0
            };

            List<Enemy> selectedMonsters = new List<Enemy>();

            for (int i = difficultyIndex; i < difficultyIndex + 6; i++)
            {
                selectedMonsters.Add(allMonsters[i]);
                if (selectedMonsters.Count > 5)
                {
                    selectedMonsters.Remove(selectedMonsters[random.Next(0, 5)]);
                    selectedMonsters.Remove(selectedMonsters[random.Next(0, 4)]);
                    selectedMonsters.Remove(selectedMonsters[random.Next(0, 3)]);
                }
            }
            return selectedMonsters;
        }

        private void Battle(int EnemyNum)
        {
            player.Attack(selectedMonsters[EnemyNum]); // 플레이어 공격
            if (selectedMonsters[EnemyNum].Health <= 0)
            {
                selectedMonsters.RemoveAt(EnemyNum);
            }                

            for (int i = 0; i < selectedMonsters.Count; i++)
                selectedMonsters[i].EnemyAttack(player); // 몬스터 공격
            
        }

        private void LoseScene()
        {
            Console.WriteLine("===================");
            Console.WriteLine("\nBattle!! - Result");
            Console.WriteLine("\nYou Lose.");
            Console.WriteLine("\n전투에서 패배하였습니다.");
            Console.WriteLine($"\nLv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {player.MaxHealth} -> {player.Health}");
            //대기
        }

        private void UseCharacterSkill(Character player, Enemy enemy)
        {
            // 스킬 사용 메서드 호출
            player.UseSkill(enemy);
        }

        private void UseItem()
        {
            Console.WriteLine("===================");
            Console.WriteLine("사용할 아이템을 선택하세요.");
            // 아이템 사용 로직은 구현하지 못했습니다
            //할지말지 + 뭐 사용할지
        }

        private void ClearDungeon()
        {
            //최대체력바꾸기
            int damage = player.MaxHealth - player.Health;
            Console.WriteLine("===================");
            Console.WriteLine($"던전 클리어! 체력 {damage} 소모됨.");
            Console.WriteLine($"남은 체력: {player.Health}");
            Console.WriteLine("\n0. 다음");
            Console.Write("\n>>");

            player.Exp += 1;       // 적을 물리칠 때마다 경험치 1 증가
            Console.WriteLine($"경험치획득: {player.Exp}");

            player.LevelUp.CheckLevelUp();

            if (random.Next(1, 101) <= 20) // 20% 확률로 특별한 아이템 드롭
                DropSpecialItem(dungeon.difficulty); // difficulty를 전달

            // 사용자 입력 기다리기
            Console.ReadLine();
        }

        private void DropSpecialItem(Difficulty difficulty)
        {
            // 노말 던전부터 하드 던전까지 특별 아이템 드롭
            if (difficulty == Difficulty.Normal || difficulty == Difficulty.Hard)
            {
                // 무작위로 하나의 아이템 선택
                Item droppedItem = Game.instance.itemManager.specialItems[random.Next(Game.instance.itemManager.specialItems.Count)];

                Console.WriteLine("===================");
                Console.WriteLine($"특별한 아이템을 획득하였습니다: {droppedItem.Name}");

                // 귀속 아이템이므로 Purchased 값을 true로 설정
                Game.instance.itemManager.UpdateItemPurchasedStatus(droppedItem);

                // 아이템을 인벤토리의 장비 카테고리에 추가
                player.InventoryManager.AddItem(droppedItem);
            }
        }
    }
}
