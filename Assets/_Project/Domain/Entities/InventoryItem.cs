using UnityEngine;
using System;

namespace HospitalRescue.Domain.Entities
{
    [Serializable]
    public class InventoryItem
    {
        public string itemId;
        public string itemName;
        public string itemDescription;
        public Sprite icon;
        public int maxStack;
        public int currentStack;
        public GameObject prefab;
        public bool isMedicalEquipment;
        
        // Medical-specific properties
        public float healAmount;
        public float usageTime;
        public string targetCondition;
        
        public InventoryItem()
        {
            maxStack = 99;
            currentStack = 1;
        }
        
        public InventoryItem(string id, string name, int stack = 1)
        {
            itemId = id;
            itemName = name;
            maxStack = 99;
            currentStack = stack;
        }
    }
}
