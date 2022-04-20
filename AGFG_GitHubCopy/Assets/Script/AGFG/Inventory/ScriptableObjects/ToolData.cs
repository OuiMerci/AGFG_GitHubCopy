using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/ToolSO")]
    public class ToolData : SOInventoryItemBase
    {
        public ETools ToolId;
        public int Efficiency;
    }
}