using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HospitalRescue.Application.GameManagers;
using HospitalRescue.Domain.Entities;

namespace HospitalRescue.Application.Presentation.UI.Components
{
    /// <summary>
    /// Inventory UI display with slots and item selection
    /// </summary>
    public class InventoryUI : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private CanvasGroup inventoryCanvas;
        [SerializeField] private Transform inventoryGrid;
        [SerializeField] private GameObject inventorySlotPrefab;
        [SerializeField] private TextMeshProUGUI slotCountText;
        
        [Header("Selected Item")]
        [SerializeField] private Image selectedItemIcon;
        [SerializeField] private TextMeshProUGUI selectedItemName;
        [SerializeField] private TextMeshProUGUI selectedItemDesc;
        [SerializeField] private Button useButton;
        
        private InventorySystem inventorySystem;
        private InventoryItem selectedItem;
        private bool isOpen;
        
        private void Awake()
        {
            inventorySystem = InventorySystem.Instance;
            
            if (inventorySystem != null)
            {
                inventorySystem.OnInventoryChanged += RefreshInventory;
            }
        }
        
        private void Start()
        {
            if (inventoryCanvas != null)
                inventoryCanvas.alpha = 0f;
            
            RefreshInventory();
        }
        
        private void Update()
        {
            // Toggle inventory with Tab key
            if (UnityEngine.Input.GetKeyDown(KeyCode.Tab))
            {
                ToggleInventory();
            }
        }
        
        public void ToggleInventory()
        {
            isOpen = !isOpen;
            
            if (inventoryCanvas != null)
            {
                inventoryCanvas.alpha = isOpen ? 1f : 0f;
                inventoryCanvas.blocksRaycasts = isOpen;
            }
            
            if (isOpen)
            {
                RefreshInventory();
            }
        }
        
        public void RefreshInventory()
        {
            if (inventorySystem == null || inventoryGrid == null) return;
            
            // Clear existing slots
            foreach (Transform child in inventoryGrid)
            {
                Destroy(child.gameObject);
            }
            
            // Create slots for each item
            foreach (var item in inventorySystem.Items)
            {
                CreateInventorySlot(item);
            }
            
            // Update slot count
            if (slotCountText != null)
            {
                slotCountText.text = $"{inventorySystem.Items.Count}/{inventorySystem.MaxSlots}";
            }
        }
        
        private void CreateInventorySlot(InventoryItem item)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryGrid);
            
            // Get slot components
            Image icon = slot.transform.Find("Icon")?.GetComponent<Image>();
            TextMeshProUGUI countText = slot.transform.Find("Count")?.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI nameText = slot.transform.Find("Name")?.GetComponent<TextMeshProUGUI>();
            Button slotButton = slot.GetComponent<Button>();
            
            // Populate slot
            if (icon != null && item.icon != null)
            {
                icon.sprite = item.icon;
                icon.enabled = true;
            }
            else if (icon != null)
            {
                icon.enabled = false;
            }
            
            if (countText != null)
            {
                countText.text = item.currentStack > 1 ? item.currentStack.ToString() : "";
            }
            
            if (nameText != null)
            {
                nameText.text = item.itemName;
            }
            
            if (slotButton != null)
            {
                slotButton.onClick.AddListener(() => SelectItem(item));
            }
        }
        
        private void SelectItem(InventoryItem item)
        {
            selectedItem = item;
            
            if (selectedItemIcon != null && item.icon != null)
            {
                selectedItemIcon.sprite = item.icon;
            }
            
            if (selectedItemName != null)
            {
                selectedItemName.text = item.itemName;
            }
            
            if (selectedItemDesc != null)
            {
                selectedItemDesc.text = item.itemDescription;
            }
        }
        
        public void UseSelectedItem()
        {
            if (selectedItem != null)
            {
                inventorySystem.UseItem(selectedItem.itemId);
                RefreshInventory();
            }
        }
        
        public void CloseInventory()
        {
            isOpen = false;
            if (inventoryCanvas != null)
            {
                inventoryCanvas.alpha = 0f;
                inventoryCanvas.blocksRaycasts = false;
            }
        }
        
        private void OnDestroy()
        {
            if (inventorySystem != null)
            {
                inventorySystem.OnInventoryChanged -= RefreshInventory;
            }
        }
    }
}
