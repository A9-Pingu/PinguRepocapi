using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGame.Managers
{
    public class InventoryManager
    {
        //public Dictionary<ItemType, List<Item>> dicInventory = new Dictionary<ItemType, List<Item>>();
        public Dictionary<ItemType, Item> dicEquipItem = new Dictionary<ItemType, Item>();
        public List<Item> Inventory { get; set; }
        public Character player;

        // 인벤토리 초기화
        public InventoryManager(Character character)
        {
            Inventory = new List<Item>();
            player = character;
            dicEquipItem[ItemType.Weapon] = null;
            dicEquipItem[ItemType.Armor] = null;
            dicEquipItem[ItemType.Consumable] = null;
            dicEquipItem[ItemType.All] = null;


        }
        // 아이템 타입에 따른 아이템 목록 반환
        public Item GetItemsByType(ItemType itemType)
        {
            return dicEquipItem[itemType];
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
            var list = Inventory;
            return list[index];
        }

        // 인벤토리 출력 및 아이템 장착/해제 기능
        public void ShowInventory()
        {
            while (true)
            {
                bool isEmpty = Game.instance.uiManager.DisplayInventory(this);
                if (!isEmpty)
                {
                    Game.instance.inputManager.InputAnyKey();
                    return;
                }

                int actionIndex = Game.instance.inputManager.GetValidSelectedIndex(1);

                switch (actionIndex)
                {
                    case 0:
                        Console.WriteLine("인벤토리에서 나갑니다.");
                        Game.instance.inputManager.InputAnyKey();
                        return;
                    case 1:
                        ManagedEquip();
                        break;
                    default:
                        break;
                }
                Game.instance.inputManager.InputAnyKey();
            }
        }

        private void ManagedEquip()
        {
            Console.Write("아이템 번호를 입력하세요: ");
            int itemIndex = Game.instance.inputManager.GetValidSelectedIndex(Inventory.Count);
            if (itemIndex == 0)
            {
                return;
            }
            EquipItem(Inventory[itemIndex - 1]);
        }

        public string GetCategoryName(ItemType itemType)
        {
            switch (itemType)
            {
                case ItemType.Weapon:
                case ItemType.Armor:
                    return "장비";
                case ItemType.Consumable:
                    return "소비";
                case ItemType.All:
                    return "기타";
                default:
                    return "알 수 없음";
            }
        }

        public bool CheckedEquipItem(Item item)
        {
            if (dicEquipItem[item.Type] == item)
            {
                return true;
            }
            return false;
        }

        public void EquipItem(Item item)
        {
            if(item.Type == ItemType.Consumable || item.Type == ItemType.All) 
            {
                Console.WriteLine("장착 아이템이 아닙니다.");
                Thread.Sleep(1000);
            }

            if(CheckedEquipItem(item))
            {
                RemoveItemStatBonus(item);
                item.Equipped = false;
                dicEquipItem[item.Type] = null;
            }
            else
            {
                if (dicEquipItem[item.Type] != null)
                {
                    RemoveItemStatBonus(dicEquipItem[item.Type]);
                    dicEquipItem[item.Type].Equipped = false;
                }
                dicEquipItem[item.Type] = item;
                item.Equipped = true;
                AddItemStatBonus(item);
            }
            Console.WriteLine("\t[아이템]{0}을/를 장착{1}하였습니다.", item.Name, CheckedEquipItem(item) ? " " : " 해제");
            Thread.Sleep(1000);
        }

        public void AddItemStatBonus(Item item)
        {
            switch (item.Type)
            {
                case ItemType.Weapon:
                    player.AttackPower += item.StatBonus;
                    break;

                case ItemType.Armor:
                    player.DefensePower += item.StatBonus;
                    break;
                default:
                    return;
            }
        }

        public void RemoveItemStatBonus(Item item)
        {
            switch (item.Type)
            {
                case ItemType.Weapon:
                    player.AttackPower -= item.StatBonus;
                    break;

                case ItemType.Armor:
                    player.DefensePower -= item.StatBonus;
                    break;
                default:
                    return;
            }
        }
    }
}
