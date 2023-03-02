using FromTheBasement.Data.InteractableObjects;
using FromTheBasement.Data.InventorySystem;
using System.Collections.Generic;
using UnityEngine;

namespace FromTheBasement.Data.ContainerSystem
{
    [CreateAssetMenu(fileName = "ContainerDefaultState", menuName = "Application/Containers/ContainerDefaultState")]
    public class ContainerDefaultState : InteractableObjectMeta
    {
        [SerializeField] private ItemMeta[] _items = new ItemMeta[12];
        public IReadOnlyList<ItemMeta> Items => _items;
    }
}
