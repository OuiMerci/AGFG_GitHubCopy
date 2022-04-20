using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Inventory
{
    public class SOInventoryItemBase : ScriptableObject
    {
        public string ItemName;
        public Sprite Icon;
        public int StackLimit;
    }
}