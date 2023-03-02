using FromTheBasement.Data.InventorySystem;
using System;
using UnityEngine;

namespace FromTheBasement.Data.CraftSystem
{
    [Serializable]
    public class CraftPair
    {
        [field: SerializeField] public ItemMeta Ingridient { get; private set; }
        [field: SerializeField] public ItemMeta Result { get; private set; }
    }
}
