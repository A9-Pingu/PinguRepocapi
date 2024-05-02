using ConsoleGame.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleGame.Managers
{
    public class UIManager
    {

        public void DisplayMainMenu()
        {
            Console.Clear();
            Console.WriteLine("===== 게임 메뉴 =====");
            Console.WriteLine("1. 상태 보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine("3. 상점");
            Console.WriteLine("4. 던전 입장");
            Console.WriteLine("5. 휴식하기");
            Console.WriteLine("6. 길드입장");
            Console.WriteLine("7. 저장하기");
            Console.WriteLine("0. 게임 종료");
            Console.WriteLine("===================");
        }

        public void ShowStatus(Character player)
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine($"이름: {player.Name}");
            Console.WriteLine($"직업: {player.Job}");
            Console.WriteLine($"레벨: {player.Level}");
            Console.WriteLine($"체력: {player.Health}");
            Console.WriteLine($"Gold: {player.Gold}");
            Console.WriteLine($"공격력: {player.AttackPower}");
            Console.WriteLine($"방어력: {player.DefensePower}");

            Console.WriteLine("[장착 아이템]");
            // 장착한 아이템 정보 출력
            if (player.InventoryManager.dicEquipItem[ItemType.Weapon] != null)
            {
                Console.WriteLine($"- {player.InventoryManager.dicEquipItem[ItemType.Weapon].Name} (무기) : +{player.InventoryManager.dicEquipItem[ItemType.Weapon].StatBonus}");
            }
            else
            {
                Console.WriteLine("장착한 무기아이템이 없습니다.");
            }

            if (player.InventoryManager.dicEquipItem[ItemType.Armor] != null)
            {
                Console.WriteLine($"- {player.InventoryManager.dicEquipItem[ItemType.Armor].Name} (방어구) : +{player.InventoryManager.dicEquipItem[ItemType.Armor].StatBonus}");           
            }
            else
            {
                Console.WriteLine("장착한 방어구아이템이 없습니다.");
            }

            Game.instance.inputManager.InputAnyKey();
        }

        public bool DisplayInventory(InventoryManager inventory)
        {
            Console.Clear();
            if (inventory.Inventory.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어 있습니다.");
                return false;
            }

            Console.WriteLine("인벤토리");
            Console.WriteLine($"아이템 개수: {inventory.Inventory.Count}\n");

            int index = 1;
            foreach (var item in inventory.Inventory)
            {
                Console.WriteLine($"- {index++}. {item.Name} ({item.Type}) : {(item.Equipped ? "장착됨" : "미장착")}");
            }

            Console.WriteLine();

            Console.WriteLine("1. 아이템 관리");
            Console.WriteLine("0. 나가기");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
            return true;
        }

        public void DisplayShopMenu()
        {
            Console.Clear();
            Console.WriteLine("===== 상점 메뉴 =====");
            Console.WriteLine("1. 무기 상점");
            Console.WriteLine("2. 방어구 상점");
            Console.WriteLine("3. 소모품 상점");
            Console.WriteLine("0. 상점 나가기");
            Console.WriteLine("===================");
            Console.Write("원하시는 행동을 입력해주세요: ");
        }

        public void ShowItemsForSale(ItemType itemType)
        {
            Console.Clear();
            string itemTypeString = itemType switch
            {
                ItemType.Weapon => "Weapon",
                ItemType.Armor => "Armor",
                ItemType.Consumable => "Consumable",
                ItemType.All => "All",
            };

            Console.WriteLine($"상점 - {itemTypeString} 상점");

            // 보유한 골드 출력
            Console.WriteLine("\n[보유 골드]");
            Console.WriteLine($"{Game.instance.player.Gold} G");

            // 상점 아이템 목록 출력
            Console.WriteLine("\n[아이템 목록]");

            int index = 1;
            Game.instance.itemManager.ItemInfos.FindAll(obj => obj.Type == itemType).ForEach(obj => Console.WriteLine($"- {index++}. {obj.Name} : {obj.Price} G"));

            Console.WriteLine("\n1. 아이템 구매");
            Console.WriteLine("2. 아이템 판매");
            Console.WriteLine("0. 나가기");
            Console.Write("원하시는 행동을 선택해주세요.\n>> ");
        }

        public void ShowRestMenu()
        {
            Console.Clear();
            Console.WriteLine($"휴식하기");
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {Game.instance.player.Gold} G)");
            Console.WriteLine("\n1. 휴식하기");
            Console.WriteLine("0. 나가기\n");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
        }
        private Random random;
        //전투장면
        public void BattleScene(Difficulty difficulty, Dungeon dungeon, List<Enemy> selectedMonsters, bool isReadyToFight, Character player)
        {
            Console.Clear();
            random = new Random();
            Console.WriteLine("");
            Console.WriteLine("     Battle!!     ");
            Console.WriteLine("");

            //배틀용 몬스터 리스트
            if (isReadyToFight)
            {
                int index = 1;
                foreach (var monster in selectedMonsters)
                {
                    if (monster.isDead)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        Console.WriteLine($"- {index++} Lv.{monster.Level} {monster.Name} Dead");
                        Console.ResetColor();
                    }
                    else
                        Console.WriteLine($"- {index++} Lv.{monster.Level} {monster.Name} HP {monster.Health} ATK {monster.Attack}");
                }
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("[플레이어 정보]");
                Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
                Console.WriteLine($"HP {player.Health}/{player.MaxHealth}");
                Console.WriteLine("");
                Console.WriteLine("0. 취소");
                Console.WriteLine("");
                Console.WriteLine("대상을 선택해주세요.");
                Console.Write(">> ");
            }
            else
            {
                foreach(var monster in selectedMonsters)
                {
                    Console.WriteLine($"Lv.{monster.Level} {monster.Name} HP {monster.Health} ATK {monster.Attack}");
                }
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("[플레이어 정보]");
                Console.WriteLine($"Lv.{player.Level} {player.Name} ({player.Job})");
                Console.WriteLine($"HP {player.Health}/{player.MaxHealth}");
                Console.WriteLine("");
                Console.WriteLine("0. 취소");
                Console.WriteLine("1. 공격");
                //Console.WriteLine("2. 아이템 사용");
                Console.WriteLine("대상을 선택해주세요.");
                Console.Write(">> ");
            }
        }
    }
}
