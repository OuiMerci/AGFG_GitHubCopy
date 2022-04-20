using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/FishSO")]
    public class FishData : SOInventoryItemBase
    {
        public int FishID;
        public int Strength;
        public int BaseReelTime;
        public int BasePrice;
    }
}