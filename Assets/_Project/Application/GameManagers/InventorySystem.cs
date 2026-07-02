using System.Collections.Generic;
using UnityEngine;
using HospitalRescue.Domain.Entities;

namespace HospitalRescue.Application.GameManagers
{
    /// <summary>
    /// Manages player inventory with items, equipment, and usage
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        public static InventorySystem Instance { get; private set; }
        
        [Header("Inventory Settings")]
        [SerializeField] private int maxSlots = 12;
        [SerializeField] private List<InventoryItem> startingItems = new List<InventoryItem>();
        
        private List<InventoryItem> inventory = new List<InventoryItem>();
        private List<InventoryItem> equipment = new List<InventoryItem>();
        
        // Events
        public System.Action<InventoryItem> OnItemAdded;
        public System.Action<InventoryItem> OnItemRemoved;
        public System.Action<InventoryItem> OnItemUsed;
        public System.Action OnInventoryChanged;
        
        public int MaxSlots => maxSlots;
        public List<InventoryItem> Items => inventory;
        public List<InventoryItem> Equipment => equipment;
        
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            InitializeStartingItems();
        }
        
        private void InitializeStartingItems()
        {
            foreach (var item in startingItems)
            {
                AddItem(item);
            }
        }
        
        public bool AddItem(InventoryItem item)
        {
            if (item == null) return false;
            
            // Try to stack with existing item
            InventoryItem existing = FindItem(item.itemId);
            if (existing != null && existing.currentStack < existing.maxStack)
            {
                int spaceAvailable = existing.maxStack - existing.currentStack;
                int toAdd = Mathf.Min(spaceAvailable, item.currentStack);
                existing.currentStack += toAdd;
                item.currentStack -= toAdd;
                
                if (item.currentStack > 0)
                {
                    return TryAddToNewSlot(item);
                }
                OnInventoryChanged?.Invoke();
                return true;
            }
            
            return TryAddToNewSlot(item);
        }
        
        private bool TryAddToNewSlot(InventoryItem item)
        {
            if (inventory.Count >= maxSlots)
            {
                Debug.Log("Inventory full!");
                return false;
            }
            
            inventory.Add(item);
            OnItemAdded?.Invoke(item);
            OnInventoryChanged?.Invoke();
            return true;
        }
        
        public bool RemoveItem(string itemId, int count = 1)
        {
            InventoryItem item = FindItem(itemId);
            if (item == null) return false;
            
            item.currentStack -= count;
            
            if (item.currentStack <= 0)
            {
                inventory.Remove(item);
                OnItemRemoved?.Invoke(item);
            }
            
            OnInventoryChanged?.Invoke();
            return true;
        }
        
        public bool UseItem(string itemId)
        {
            InventoryItem item = FindItem(itemId);
            if (item == null)
            {
                Debug.Log($"Item {itemId} not found in inventory");
                return false;
            }
            
            OnItemUsed?.Invoke(item);
            
            // Medical items heal patients, etc.
            if (item.isMedicalEquipment)
            {
                Debug.Log($"Using medical equipment: {item.itemName}");
            }
            
            RemoveItem(itemId, 1);
            return true;
        }
        
        public InventoryItem FindItem(string itemId)
        {
            return inventory.Find(item => item.itemId == itemId);
        }
        
        public bool HasItem(string itemId, int count = 1)
        {
            InventoryItem item = FindItem(itemId);
            return item != null && item.currentStack >= count;
        }
        
        public void EquipItem(InventoryItem item)
        {
            if (!inventory.Contains(item)) return;
            
            inventory.Remove(item);
            equipment.Add(item);
            OnInventoryChanged?.Invoke();
        }
        
        public void UnequipItem(InventoryItem item)
        {
            if (!equipment.Contains(item)) return;
            
            if (AddItem(item))
            {
                equipment.Remove(item);
                OnInventoryChanged?.Invoke();
            }
        }
        
        public InventoryItem GetEquippedItem(string slotId)
        {
            return equipment.Find(item => item.itemId == slotId);
        }
        
        public void ClearInventory()
        {
            inventory.Clear();
            equipment.Clear();
            OnInventoryChanged?.Invoke();
        }
    }
}
