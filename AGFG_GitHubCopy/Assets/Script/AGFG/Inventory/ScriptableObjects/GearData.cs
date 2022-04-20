using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/GearSO")]
    public class GearData : SOInventoryItemBase
    {
        public EGears GearID;
        public int Defense;
        public int Attack;
    }
}