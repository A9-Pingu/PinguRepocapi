using System.Collections.Generic;

namespace ConsoleGame.Managers
{
    public class ItemManager
    {
        public List<Item> ItemInfos = new List<Item>
        {
            new Item("천 갑옷", ItemType.Armor, 1000, 5, "수련에 도움을 주는 갑옷입니다."),
            new Item("무쇠갑옷", ItemType.Armor, 2000, 9, "무쇠로 만들어져 튼튼한 갑옷입니다."),
            new Item("스파르타의 갑옷", ItemType.Armor, 3500, 15, "스파르타의 전사들이 사용했다는 전설의 갑옷입니다."),
            new Item("헤라클레스의 망토", ItemType.Armor, 5000, 30, "헤라클레스가 헤르메스에게 받았다는 전설의 망토입니다."),
            new Item("도란의 검", ItemType.Weapon, 600, 2, "쉽게 볼 수 있는 낡은 검입니다."),
            new Item("청동 도끼", ItemType.Weapon, 1400, 5, "어디선가 사용됐던거 같은 도끼입니다."),
            new Item("스파르타의 창", ItemType.Weapon, 2500, 7, "스파르타의 전사들이 사용했다는 전설의 창입니다."),
            new Item("헤르메스의 시미터", ItemType.Weapon, 3500, 20, "달의 여신의 축복을 받아 만들어진 검입니다."),
            new Item("체력 회복 물약", ItemType.Consumable, 100, 0, "체력을 50 회복시키는 물약입니다."),
            new Item("마나 회복 물약", ItemType.Consumable, 100, 0, "마나를 30 회복시키는 물약입니다."),
            new Item("공격력 증가 물약", ItemType.Consumable, 200, 0, "공격력을 5 증가시키는 물약입니다."),
            new Item("펭귄 성장용 깃털", ItemType.Armor, 900, 5, "특별 제작된 펭모로 늠름함을 높입니다."),
            new Item("북극곰의 투구", ItemType.Armor, 1200, 10, "털갈이를 하는 북극곰의 옆에서 채취해 만든 투구입니다."),
            new Item("순록의 털옷", ItemType.Armor, 2100, 15, "북극에서 천적에 의해 사냥당한 순록의 털로 만든 옷입니다."),
            new Item("방문객의 연구복", ItemType.Armor, 3000, 20, "이곳을 연구하는 연구원이 마지막으로 남기고 간 옷입니다."),
            new Item("조개 껍데기", ItemType.Weapon, 1000, 5, "물개 아저씨의 비상식량이었습니다. 쓸만합니다."),
            new Item("말린 연어", ItemType.Weapon, 1500, 15, "연어를 햇볕에 말려 딱딱해졌습니다. 제법 단단합니다."),
            new Item("참치 대가리", ItemType.Weapon, 2500, 25, "어린 참치의 동족으로 만들어진 검입니다. 묵직합니다!"),
            new Item("냉동 상어", ItemType.Weapon, 3500, 35, "영하 30도의 온도로 꽝꽝 얼려진 냉동 대검입니다!"),
            new Item("알", ItemType.Consumable, 1000,5, "폭탄은 예술입니다."),                               //임시
            new Item("눈덩이", ItemType.Consumable, 1500, 10, "무언가를 눈으로 감싸만든 눈덩이입니다."),     //임시
            new Item("고드름", ItemType.Consumable, 2400, 20, "가시와 동급이거나 그 이상입니다."),           //임시
            new Item("뼈다귀 부메랑", ItemType.Consumable, 3000, 30, "사랑은 돌아오는 거야!"),               //임시
            new Item("새우", ItemType.Consumable, 200, 20, "맛있습니다! 체력을 20 회복하는 음식입니다."),
            new Item("조개살 죽", ItemType.Consumable, 200, 20, "물개 아저씨의 야심작입니다! 마나를 20 회복하는 음식입니다."),
            new Item("해파리", ItemType.Consumable, 1000, 5, "먹으면 따끔합니다! 공격력이 5 증가하는 음식입니다."),
        };

        public List<Item> specialItems = new List<Item>
        {
            new Item("용의 검", ItemType.Weapon, 10000, 40, "드래곤의 힘을 담은 무기입니다.", true),
            new Item("신의 갑옷", ItemType.Armor, 12000, 50, "신들의 보호를 받는 갑옷입니다.", true),
            new Item("마법의 지팡이", ItemType.Weapon, 9000, 35, "마법사의 필수 아이템입니다.", true),
            new Item("마력의 로브", ItemType.Armor, 11000, 45, "마법의 힘을 강화시켜주는 로브입니다.", true),
            new Item("지혜의 서", ItemType.Consumable, 8000, 30, "지식과 경험을 향상시켜주는 서입니다.", true),
            new Item("천둥의 방패", ItemType.Armor, 13000, 55, "천둥의 힘으로 공격을 막아주는 방패입니다.", true),
            new Item("빛의 검", ItemType.Weapon, 9500, 38, "빛의 힘을 가진 검입니다.", true),
            new Item("천사의 날개", ItemType.Armor, 11500, 48, "천사의 보호를 받는 날개입니다.", true)
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