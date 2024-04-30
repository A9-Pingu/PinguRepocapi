﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGame.Managers
{
    public class InventoryManager
    {
        public List<Item> Inventory { get; set; }

        // 아이템 타입에 따른 아이템 목록 반환
        public List<Item> GetItemsByType(ItemType itemType)
        {
            return Inventory.Where(item => item.Type == itemType).ToList();
        }

        // 인벤토리 초기화
        public InventoryManager()
        {
            Inventory = new List<Item>();
        }

        // 아이템 추가
        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }

        // 아이템 삭제
        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }

        // 인덱스로 아이템 조회
        public Item GetItem(int index)
        {
            return Inventory.ElementAtOrDefault(index);
        }

        // 인벤토리 출력 및 아이템 장착/해제 기능
        public void ShowInventory()
        {
            if (Inventory.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어 있습니다.");
                Game.instance.inputManager.InputAnyKey();
                return;
            }

            Console.WriteLine("인벤토리");
            Console.WriteLine($"아이템 개수: {Inventory.Count}");
            Console.WriteLine();

            int index = 1;
            foreach (var item in Inventory)
            {
                string category = GetCategoryName(item.Type);
                Console.WriteLine($"- {index}. {item.Name} ({category}) : {(item.Equipped ? "장착됨" : "미장착")}");
                index++;
            }

            Console.WriteLine();

            Console.WriteLine("1. 아이템 장착");
            Console.WriteLine("2. 아이템 해제");
            Console.WriteLine("0. 나가기");
            Console.Write("원하시는 행동을 입력해주세요.\n>> ");

            string input = Console.ReadLine();

            if (int.TryParse(input, out int actionIndex))
            {
                if (actionIndex >= 1 && actionIndex <= 2)
                {
                    Console.Write("아이템 번호를 입력하세요: ");
                    if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex >= 1 && itemIndex <= Inventory.Count)
                    {
                        if (actionIndex == 1)
                        {
                            EquipItem(itemIndex);
                        }
                        else
                        {
                            UnequipItem(itemIndex);
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 아이템 번호입니다.");
                    }
                }
                else if (actionIndex == 0)
                {
                    return;
                }
                else
                {
                    Console.WriteLine("잘못된 입력입니다.");
                }
            }

            Game.instance.inputManager.InputAnyKey();
        }

        private string GetCategoryName(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                    return "장비";
                case ItemType.Potion:
                case ItemType.Scroll:
                    return "소비";
                case ItemType.Loot:
                    return "기타";
                default:
                    return "알 수 없음";
            }
        }


        // 아이템 장착
        public void EquipItem(int index)
        {
            var item = GetItem(index - 1); // 인덱스 1부터 시작하므로 1을 빼줌
            if (item != null && (item.Type == ItemType.Weapon || item.Type == ItemType.Armor))
            {
                item.Equipped = true;
                Console.WriteLine($"{item.Name}을(를) 장착했습니다.");
            }
            else
            {
                Console.WriteLine("잘못된 선택입니다.");
            }
        }

        // 아이템 해제
        public void UnequipItem(int index)
        {
            var item = GetItem(index - 1); // 인덱스 1부터 시작하므로 1을 빼줌
            if (item != null && (item.Type == ItemType.Weapon || item.Type == ItemType.Armor))
            {
                item.Equipped = false;
                Console.WriteLine($"{item.Name}을(를) 해제했습니다.");
            }
            else
            {
                Console.WriteLine("잘못된 선택입니다.");
            }
        }
    }
}