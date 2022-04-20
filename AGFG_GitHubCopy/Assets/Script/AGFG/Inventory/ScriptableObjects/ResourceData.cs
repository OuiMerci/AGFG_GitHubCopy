using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AGFG.Inventory
{
    [CreateAssetMenu(menuName = "Inventory/ResourceSO")]
    public class ResourceData : SOInventoryItemBase
    {
        public ERessources RessourceID;
    } 
}