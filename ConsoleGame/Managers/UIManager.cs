using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
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
            Console.WriteLine("6. 길드입장");
            Console.WriteLine("7. 저장하기");
            Console.WriteLine("8. 불러오기");
            Console.WriteLine("0. 게임 종료");
            Console.WriteLine("===================");
            Console.Write("원하시는 행동을 입력해주세요: ");
        }

        public void ShowStatus(Character player)
        {
            Console.Clear();
            Console.WriteLine("상태 보기");
            Console.WriteLine($"이름: {player.Name}");
            Console.WriteLine($"직업: {player.Job}");
            Console.WriteLine($"레벨: {player.Level}");
            Console.WriteLine($"체력: {player.Health}");
            Console.WriteLine($"마나: {player.MP}");
            Console.WriteLine($"Gold: {player.Gold}");
            Console.WriteLine($"공격력: {(double)player.CalculateTotalAttackPower()}");
            Console.WriteLine($"방어력: {(double)player.CalculateTotalDefensePower()}");

            // 장착한 아이템 정보 출력
            if (player.WeaponInventoryManager.GetItemsByType(ItemType.Weapon).Count > 0 ||
               player.ArmorInventoryManager.GetItemsByType(ItemType.Armor).Count > 0)
            {
                Console.WriteLine("[장착 아이템]");

                foreach (var weapon in player.WeaponInventoryManager.GetItemsByType(ItemType.Weapon))
                {
                    Console.WriteLine($"- {weapon.Name} (무기) : +{weapon.StatBonus}");
                }

                foreach (var armor in player.ArmorInventoryManager.GetItemsByType(ItemType.Armor))
                {
                    Console.WriteLine($"- {armor.Name} (방어구) : +{armor.StatBonus}");
                }
            }
            else
            {
                Console.WriteLine("장착한 아이템이 없습니다.");
            }
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

        public void ShowRestMenu()
        {
            Console.Clear();
            Console.WriteLine($"휴식하기");
            Console.WriteLine($"500 G 를 내면 체력을 회복할 수 있습니다. (보유 골드 : {Game.instance.player.Gold} G)");
            Console.WriteLine();
            Console.WriteLine("1. 휴식하기");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");
        }
    }


}