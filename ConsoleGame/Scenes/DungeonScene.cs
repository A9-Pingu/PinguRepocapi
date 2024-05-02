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
        bool useItem = false;

        public DungeonScene(Character character)
        {
            player = character;
            Random random = new Random(Guid.NewGuid().GetHashCode());
        }

        //던전 입장 조건
        public void EnterDungeon()
        {
            Console.WriteLine("===================");
            Console.WriteLine("1. 쉬운 던전     | 누구나 가능");
            Console.WriteLine("2. 일반 던전     | 방어력 12 이상 권장");
            Console.WriteLine("3. 어려운 던전    | 방어력 24 이상 권장");
            Console.WriteLine("0. 나가기");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            int InputKey = Game.instance.inputManager.GetValidSelectedIndex((int)Difficulty.Max - 1);//, (int)Difficulty.Easy);
            dungeon = new Dungeon((Difficulty)InputKey);
            while (true)
            {
                switch (InputKey)
                {
                    case 0:
                        Game.instance.Run();
                        break;
                    case 1:
                        if (player.DefensePower > 0)
                        {
                            Start(dungeon.difficulty);
                            DropSpecialItem(dungeon.difficulty);
                        }
                        break;
                    case 2:
                        if (player.DefensePower >= 12)
                        {
                            Start(dungeon.difficulty);
                            DropSpecialItem(dungeon.difficulty);
                        }
                        else
                        {
                            Console.WriteLine($"방어력이 12 이상이어야 {dungeon.difficulty} 던전에 입장할 수 있습니다.");
                            Game.instance.inputManager.InputAnyKey();
                            Console.Clear();
                            Game.instance.DungeonRun();
                        }
                        break;
                    case 3:
                        if (player.DefensePower >= 24)
                        {
                            Start(dungeon.difficulty);
                            DropSpecialItem(dungeon.difficulty);
                        }
                        else
                        {
                            Console.WriteLine($"방어력이 24 이상이어야 {dungeon.difficulty} 던전에 입장할 수 있습니다.");
                            Game.instance.inputManager.InputAnyKey();
                            Console.Clear();
                            Game.instance.DungeonRun();
                        }
                        break;
                }
            }
        }

        List<Enemy> deadMonsters = new List<Enemy>(); //죽은 몬스터 수
        Dictionary<int,string> deadMonsterIndex = new Dictionary<int,string>(); //죽은 몬스터 번호
        public void Start(Difficulty difficulty)
        {
            Console.WriteLine("");
            Console.WriteLine("===================");
            Console.WriteLine($"{difficulty} 던전 입장 성공!");
            Console.WriteLine("===================");
            List<Enemy> selectedMonsters = SelectMonsters(difficulty);
            // 몬스터 선택
            // UIManager의 ShowBattle 메서드 호출
            Game.instance.uiManager.BattleScene(difficulty, dungeon, selectedMonsters, false, player); //--
            int inputKey = Game.instance.inputManager.GetValidSelectedIndex(3);
            while (true)
            {
                switch (inputKey)
                {
                    case 0: Game.instance.Run(); //취소
                        break;
                    case 1: //공격
                        while (deadMonsters.Count < 3 && player.Health > 0) //내가 또는 몬스터 한 마리가 살아있을 때
                        {
                            Game.instance.uiManager.BattleScene(difficulty, dungeon, selectedMonsters, true, player); //1 2
                            inputKey = int.Parse(Console.ReadLine());
                            switch (inputKey)
                            {
                                case 0://취소
                                    Console.WriteLine("===================");
                                    Console.WriteLine("이대로 던전을 나가시겠습니까?");
                                    Console.WriteLine("0. 네");
                                    Console.WriteLine("1. 아니오");
                                    int nextKey = int.Parse(Console.ReadLine());
                                    if (nextKey == 0)
                                        Game.instance.Run(); //취소
                                    else if (nextKey == 1)
                                        break;
                                    else
                                    {
                                        Console.WriteLine("===================");
                                        Console.WriteLine("잘못된 입력입니다");
                                        inputKey = int.Parse(Console.ReadLine());
                                    }
                                    break; 
                                case 1: //첫번째 몬스터
                                    if (!deadMonsters.Contains(selectedMonsters[inputKey - 1]))
                                    {
                                        Battle(selectedMonsters[inputKey - 1]);
                                        if (player.Health <= 0)
                                        {
                                            LoseScene(); 
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("===================");
                                        Console.WriteLine("잘못된 입력입니다");
                                        inputKey = int.Parse(Console.ReadLine());
                                    }
                                    break;
                                case 2: //두번째 몬스터
                                    if (!deadMonsters.Contains(selectedMonsters[inputKey - 1]))
                                    {
                                        Battle(selectedMonsters[inputKey - 1]);
                                        if (player.Health <= 0)
                                        {
                                            LoseScene();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("===================");
                                        Console.WriteLine("잘못된 입력입니다");
                                        inputKey = int.Parse(Console.ReadLine());
                                    }
                                    break;
                                case 3: //세번째 몬스터
                                    if (!deadMonsters.Contains(selectedMonsters[inputKey - 1]))
                                    {
                                        Battle(selectedMonsters[inputKey - 1]);
                                        if (player.Health <= 0)
                                        {
                                            LoseScene();
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("===================");
                                        Console.WriteLine("잘못된 입력입니다");
                                        inputKey = int.Parse(Console.ReadLine());
                                    }
                                    break;
                                default:
                                    Console.WriteLine("===================");
                                    Console.WriteLine("잘못된 입력입니다");
                                    inputKey = int.Parse(Console.ReadLine());
                                    break;
                            }
                        }
                        if (player.Health <= 0)
                        {
                            LoseScene();
                        }
                        else if (deadMonsters.Count >= 3)
                        {
                            Console.WriteLine("===================");
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
                            Console.WriteLine("");
                            ClearDungeon();
                            int nextKey = Game.instance.inputManager.GetValidSelectedIndex(0);
                            if (nextKey == 0)
                                Game.instance.Run();

                            player.Gold += totalReward;

                            if (random.Next(1, 101) <= 20) // 15~20% 확률로 드롭
                            {
                                DropHighTierItem();
                            }
                        }
                        break;
                    default:
                        Console.WriteLine("===================");
                        Console.WriteLine("잘못된 입력입니다");
                        inputKey = int.Parse(Console.ReadLine());
                        break;
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

            //난이도별 랜덤 몬스터 3마리 선택
            int difficultyIndex = difficulty switch
            {
                Difficulty.Easy => 0,
                Difficulty.Normal => 4,
                Difficulty.Hard => 8,
                _ => 0
            };

            List<Enemy> selectedMonsters1 = new List<Enemy>();
            
            for (int i = difficultyIndex; i < difficultyIndex + 6; i++)
            {
                selectedMonsters1.Add(allMonsters[i]);
                Random random = new Random();
                if (selectedMonsters1.Count > 5)
                {
                    selectedMonsters1.Remove(selectedMonsters1[random.Next(0, 5)]);
                    selectedMonsters1.Remove(selectedMonsters1[random.Next(0, 4)]);
                    selectedMonsters1.Remove(selectedMonsters1[random.Next(0, 3)]);
                }
            }
            return selectedMonsters1;
        }

        //던전에서 공격 시작 후 전투장면
        private void Battle(Enemy enemy)
        {
            while (!deadMonsters.Contains(enemy) && player.Health > 0) //몬스터와 나 둘 다 생존시
            {
                if (!useItem) //아이템 미사용
                {
                    player.Attack(enemy); //플레이어 공격
                    Game.instance.inputManager.GetValidSelectedIndex(0);
                    if (enemy.Health <= 0)
                    {
                        deadMonsters.Add(enemy); //몬스터 사망시 죽은 몬스터 리스트에 넣기
                        enemy.isDead = true;
                        break;
                    }
                    else if (enemy.Health > 0)
                    {
                        enemy.EnemyAttack(player); //몬스터 공격
                        int nextKey = Game.instance.inputManager.GetValidSelectedIndex(1);
                        if (nextKey == 1) //아이템 사용여부
                        {
                            useItem = true;
                            UseItem();
                        }
                    }
                }
                else //아이템 사용
                {
                    enemy.EnemyAttack(player); //몬스터 공격
                    int nextKey = Game.instance.inputManager.GetValidSelectedIndex(1);
                    if (nextKey == 1) //아이템 사용여부
                    {
                        useItem = true;
                        UseItem();
                    }
                }
            }
        }
        //전투 패배 결과 장면
        private void LoseScene()
        {
            Console.WriteLine("===================");
            Console.WriteLine("");
            Console.WriteLine("Battle!! - Result");
            Console.WriteLine("");
            Console.WriteLine("You Lose.");
            Console.WriteLine("");
            Console.WriteLine("전투에서 패배하였습니다.");
            Console.WriteLine("");
            Console.WriteLine($"Lv.{player.Level} {player.Name}");
            Console.WriteLine($"HP {player.MaxHealth} -> {player.Health}");
            Console.WriteLine("");
            Console.WriteLine("0. 다음");
            Console.WriteLine("");
            int nextKey = Game.instance.inputManager.GetValidSelectedIndex(0);
            if (nextKey == 0)
                Game.instance.Run();
        }

        //던전 클리어시 공격력에 따른 추가 보상
        private int CalculateAdditionalReward(int attackPower)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());
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

        //던전 클리어시 추가 정보 및 특별 아이템 드롭
        private void ClearDungeon()
        {

            int damage = player.MaxHealth - player.Health;
            Console.WriteLine("===================");
            Console.WriteLine($"던전 클리어! 체력 {damage} 소모됨.");
            Console.WriteLine($"남은 체력: {player.Health}");
            Console.WriteLine("");
            Console.WriteLine("0. 다음");
            Console.WriteLine("");
            Console.Write(">>");
            Random random = new Random();
            if (random.Next(1, 101) <= 20) // 20% 확률로 특별한 아이템 드롭
            {
                DropSpecialItem(dungeon.difficulty); // difficulty를 전달
            }
        }

        private void DropHighTierItem()
        {
        }

        private void DropSpecialItem(Difficulty difficulty)
        {
            // 노말 던전부터 하드 던전까지 특별 아이템 드롭
            if (difficulty == Difficulty.Normal ||
                difficulty == Difficulty.Hard)
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

        private void UseItem()
        {
            Console.WriteLine("===================");
            Console.WriteLine("사용할 아이템을 선택하세요.");
            // 아이템 사용 로직은 구현하지 못했습니다
        }
    }
}
