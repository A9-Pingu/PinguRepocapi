using System.Collections.Generic;

namespace ConsoleGame.Managers
{
    public class ItemManager
    {
        public List<Item> ItemInfos = new List<Item>
        {
            new Item("수련자 갑옷", ItemType.Armor, 1000, 5, "수련에 도움을 주는 갑옷입니다.", false, false),
            new Item("무쇠갑옷", ItemType.Armor, 2000, 9, "무쇠로 만들어져 튼튼한 갑옷입니다.", false, false),
            new Item("스파르타의 갑옷", ItemType.Armor, 3500, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다.", false, false),
            new Item("헤라클레스의 망토", ItemType.Armor, 5000, 30, "헤라클레스가 헤르메스에게 받았다는 전설의 망토입니다.", false, false),
            new Item("낡은 검", ItemType.Weapon, 600, 2, "쉽게 볼 수 있는 낡은 검입니다.", false, false),
            new Item("청동 도끼", ItemType.Weapon, 1400, 5, "어디선가 사용됐던거 같은 도끼입니다.", false, false),
            new Item("스파르타의 창", ItemType.Weapon, 2500, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다.", false, false),
            new Item("여명의 시미터", ItemType.Weapon, 3500, 20, "달의 여신의 축복을 받아 만들어진 검입니다.", false, false),
            new Item("체력 회복 물약", ItemType.Consumable, 100, 0, "체력을 50 회복시키는 물약입니다.", false, false),
            new Item("마나 회복 물약", ItemType.Consumable, 100, 0, "마나를 30 회복시키는 물약입니다.", false, false),
            new Item("공격력 증가 물약", ItemType.Consumable, 200, 0, "공격력을 5 증가시키는 물약입니다.", false, false)
        };

        public List<Item> specialItems = new List<Item>
        {
            new Item("용의 검", ItemType.SpecialWeapon, 10000, 40, "드래곤의 힘을 담은 무기입니다.", true),
            new Item("신의 갑옷", ItemType.SpecialArmor, 12000, 50, "신들의 보호를 받는 갑옷입니다.", true),
            new Item("마법의 지팡이", ItemType.SpecialWeapon, 9000, 35, "마법사의 필수 아이템입니다.", true),
            new Item("마력의 로브", ItemType.SpecialArmor, 11000, 45, "마법의 힘을 강화시켜주는 로브입니다.", true),
            new Item("지혜의 서", ItemType.SpecialScroll, 8000, 30, "지식과 경험을 향상시켜주는 서입니다.", true),
            new Item("천둥의 방패", ItemType.SpecialArmor, 13000, 55, "천둥의 힘으로 공격을 막아주는 방패입니다.", true),
            new Item("빛의 검", ItemType.SpecialWeapon, 9500, 38, "빛의 힘을 가진 검입니다.", true),
            new Item("천사의 날개", ItemType.SpecialArmor, 11500, 48, "천사의 보호를 받는 날개입니다.", true)
        };

        public void UpdateItemPurchasedStatus(Item item)
        {
            foreach (var itemInfo in ItemInfos)
            {
                if (itemInfo.Name == item.Name && itemInfo.Type == item.Type)
                {
                    itemInfo.Purchased = true;
                    break;
                }
            }
        }

        public  int GetIndexOfItem(Item item)
        {
            for (int i = 0; i < ItemInfos.Count; i++)
            {
                if (ItemInfos[i].Name == item.Name && ItemInfos[i].Type == item.Type)
                {
                    return i;
                }
            }
            return -1; // not found
        }
    }
}