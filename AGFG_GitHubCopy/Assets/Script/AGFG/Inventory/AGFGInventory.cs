using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Inventory
{
    // FOR UI LATER ?
    //public class InventoryItem
    //{
    //    public SOInventoryItemBase ItemData;
    //    public int Stack;
    //}

    public class AGFGInventory : MonoBehaviour
    {
        #region Singleton
        private static AGFGInventory _instance;
        public static AGFGInventory Instance
        {
            get { return _instance; }
            private set
            {
                if (_instance != null)
                    Debug.LogError("[Area] Trying to set singleton instance when instance already exists !");
                else
                    _instance = value;
            }
        }
        #endregion

        public InventoryStack<ResourceData> Resources { get; set; } = new InventoryStack<ResourceData>();
        public InventoryStack<ToolData> Tools { get; set; } = new InventoryStack<ToolData>();
        public InventoryStack<GearData> Gears { get; set; } = new InventoryStack<GearData>();
        public InventoryStack<FishData> Fish { get; set; } = new InventoryStack<FishData>();

        private void Awake()
        {
            _instance = this;
        }
    }

    public class InventoryStack<T> where T : SOInventoryItemBase
    {
        public Dictionary<T, int> dic { get; set; } = new Dictionary<T, int>();

        public int Add(T t, int quantity)
        {
            if (dic.TryGetValue(t, out int stack))
            {
                stack += quantity;
            }
            else
            {
                dic.Add(t, quantity);
            }

            return stack;
        }

        //public int Remove(T t, int quantity)
    }


}