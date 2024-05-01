using System;
using System.Collections.Generic;
using System.Numerics;
using ConsoleGame.Managers;

namespace ConsoleGame
{
    public class Character
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public int Level { get; set; }
        public int AttackPower { get; set; }
        public int DefensePower { get; set; }
        public int Health { get; set; }
        public int Gold { get; set; }

        public int DungeonClearCount { get; private set; } = 0;  // 던전 클리어 횟수 카운트

        public int MaxHealth { get; private set; } = 100;  // 최대 체력 속성 추가

        public InventoryManager InventoryManager { get; set; }

        public Character(string name, string job)
        {
            Name = name;
            Job = job;
            Level = 1;
            AttackPower = 10;
            DefensePower = 5;
            Health = 100;
            Gold = 1500;

            // InventoryManager 및 EquipmentManager 초기화
            InventoryManager = new InventoryManager(this);
        }

        public bool HasRequiredDefense(int requiredDefense)
        {
            return DefensePower >= requiredDefense;
        }

        public void ResetDungeonClearCount()
        {
            DungeonClearCount = 0;
        }

<<<<<<< HEAD
        public void AddItem(Item item)
        {
            InventoryManager.AddItem(item);  // 인벤토리에 아이템 추가

            switch (item.Type)
            {
                case ItemType.Weapon:
                    WeaponInventoryManager.AddItem(item);
                    break;
                case ItemType.Armor:
                    ArmorInventoryManager.AddItem(item);
                    break;
                case ItemType.Potion:
                case ItemType.Scroll:
                    ConsumableInventoryManager.AddItem(item);
                    break;
                default:
                    throw new ArgumentException($"Invalid item type: {item.Type}");
            }
        }

        public void RemoveItem(Item item)
        {
            InventoryManager.RemoveItem(item);  // 인벤토리에서 아이템 제거

            switch (item.Type)
            {
                case ItemType.Weapon:
                    WeaponInventoryManager.RemoveItem(item);
                    break;
                case ItemType.Armor:
                    ArmorInventoryManager.RemoveItem(item);
                    break;
                case ItemType.Potion:
                case ItemType.Scroll:
                    ConsumableInventoryManager.RemoveItem(item);
                    break;
                default:
                    throw new ArgumentException($"Invalid item type: {item.Type}");
            }
        }

        public int CalculateTotalAttackPower()
        {
            int totalAttackPower = AttackPower;
            foreach (var weapon in WeaponEquipmentManager.EquippedItems)
            {
                totalAttackPower += weapon.StatBonus;
            }
            return totalAttackPower;
        }

        public int CalculateTotalDefensePower()
        {
            int totalDefensePower = DefensePower;
            foreach (var armor in ArmorEquipmentManager.EquippedItems)
            {
                totalDefensePower += armor.StatBonus;
            }
            return totalDefensePower;
        }

=======
>>>>>>> main
        public void Attack(Enemy enemy)
        {
            Console.WriteLine($"당신이 {enemy.Name}에게 {AttackPower}의 피해를 입혔습니다.");
            enemy.Health -= AttackPower;
        }
    }
}