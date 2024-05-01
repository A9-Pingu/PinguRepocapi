using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleGame.Managers;

namespace ConsoleGame.Scenes
{
    public class Shop
    {
        Character player;
        ItemManager item;
        public Shop(Character character, ItemManager itemManager)
        {
            player = character;
            item = itemManager;
        }
        public void ShowShop()
        {
            while (true)
            {
                Game.instance.uiManager.DisplayShopMenu();

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowItemActions(ItemType.Weapon);
                        break;
                    case "2":                       
                        ShowItemActions(ItemType.Armor);
                        break;
                    case "3":
                        ShowConsumables();            // 소모품 목록 출력
                        BuyConsumable();      // 소모품 구매
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }

                Game.instance.inputManager.InputAnyKey();
            }
        }


        private void ShowItemActions(ItemType itemType)
        {
            while (true)
            {
                Game.instance.uiManager.ShowItemsForSale(itemType);

                int inputKey = Game.instance.inputManager.GetValidSelectedIndex(2);

                switch (inputKey)
                {
                    case 1:
                        BuyItem(itemType);
                        break;
                    case 2:
                        SellItem();
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("잘못된 입력입니다.");
                        break;
                }
            }

        }

        private void BuyItem(ItemType itemType)
        {
            Game.instance.uiManager.ShowItemsForSale(itemType);
            Console.WriteLine("구매할 아이템 번호를 선택해주세요. (취소: 0)");
            //
            var itemsToBuy = item.ItemInfos.Where(i => i.Type == itemType).ToList();

            int choice = Game.instance.inputManager.GetValidSelectedIndex(itemsToBuy.Count);

            if (choice == 0)
            {
                Console.WriteLine("구매가 취소되었습니다.");
                Thread.Sleep(1000);
                return;
            }

            var selectedToBuy = itemsToBuy[choice - 1];

            if (player.Gold < selectedToBuy.Price)
            {
                Console.WriteLine("골드가 부족합니다.");
                Thread.Sleep(1000);
                return;
            }

            player.Gold -= selectedToBuy.Price;
            item.UpdateItemPurchasedStatus(selectedToBuy);

            // 인벤토리에 아이템 추가
            player.InventoryManager.AddItem(new Item(selectedToBuy.Name, selectedToBuy.Type, selectedToBuy.Price, selectedToBuy.StatBonus, selectedToBuy.Description, false));

            Console.WriteLine($"{selectedToBuy.Name}을(를) 구매했습니다.");
            Thread.Sleep(1000);
        }


        private void SellItem()
        {
            var inventoryManager = player.InventoryManager;

            if (inventoryManager.Inventory.Count == 0)
            {
                Console.WriteLine("인벤토리가 비어 있습니다.");
                Thread.Sleep(1000);
                return;
            }

            int index = 1;
            foreach (var item in inventoryManager.Inventory)
            {
                Console.WriteLine($"{index++}. {item.Name} ({item.Type}) : {(int)(item.Price * 0.85)} G");
            }

            Console.WriteLine("판매할 아이템을 선택하세요. (취소: 0)");



            int choice = Game.instance.inputManager.GetValidSelectedIndex(index);

            if (choice == 0)
            {
                Console.WriteLine("판매가 취소되었습니다.");
                Thread.Sleep(1000);
                return;
            }

            var selectedItem = inventoryManager.Inventory[choice - 1];

            if (selectedItem.Equipped)
            {
                Console.WriteLine($"{selectedItem.Name}은(는) 현재 장착 중입니다. 장착을 해제하고 판매하시겠습니까? (Y/N)");
                string confirm = Console.ReadLine().ToUpper();

                if (confirm != "Y")
                {
                    Console.WriteLine("판매가 취소되었습니다.");
                    Thread.Sleep(1000);
                    return;
                }
                player.InventoryManager.RemoveItemStatBonus(selectedItem);
                player.InventoryManager.RemoveItem(selectedItem);
            }

            int sellPrice = (int)(selectedItem.Price * 0.85);
            player.Gold += sellPrice;
            inventoryManager.RemoveItem(selectedItem);

            Console.WriteLine($"{selectedItem.Name}을(를) {sellPrice} G에 판매했습니다.");
            Thread.Sleep(1000);
        }

        private void ShowConsumables()
        {
            Console.WriteLine("[소모품 목록]");

            int index = 1;
            foreach (var item in item.ItemInfos.Where(item => item.Type == ItemType.Consumable))
            {
                Console.WriteLine($"- {index++}. {item.Name} : {item.Price} G");
            }

            if (index == 1)
            {
                Console.WriteLine("소모품이 없습니다.");
            }

            Console.WriteLine();

        }

        private void BuyConsumable()
        {
            Console.Write("구매할 아이템 번호를 입력하세요: ");
            if (int.TryParse(Console.ReadLine(), out int itemIndex))
            {
                var consumables = item.ItemInfos.Where(item => item.Type == ItemType.Consumable).ToList();

                if (itemIndex >= 1 && itemIndex <= consumables.Count)
                {
                    var selectedConsumable = consumables[itemIndex - 1];

                    Console.Write($"구매할 {selectedConsumable.Name}의 수량을 입력하세요: ");
                    if (int.TryParse(Console.ReadLine(), out int quantity) && quantity > 0)
                    {
                        int totalPrice = selectedConsumable.Price * quantity;

                        if (player.Gold >= totalPrice)
                        {
                            player.Gold -= totalPrice;
                            item.UpdateItemPurchasedStatus(selectedConsumable);

                            for (int i = 0; i < quantity; i++)
                            {
                                player.InventoryManager.AddItem(new Item(selectedConsumable.Name, selectedConsumable.Type, selectedConsumable.Price, selectedConsumable.StatBonus, selectedConsumable.Description, false));
                            }

                            Console.WriteLine($"{selectedConsumable.Name} {quantity}개를 구매했습니다.");
                        }
                        else
                        {
                            Console.WriteLine("골드가 부족합니다.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("잘못된 수량입니다.");
                    }
                }
                else
                {
                    Console.WriteLine("잘못된 아이템 번호입니다.");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
            Thread.Sleep(1000);
        }

    }
}