using FromTheBasement.Data.InventorySystem;
using UnityEngine;

namespace FromTheBasement.Data.CraftSystem
{
    [CreateAssetMenu(fileName = "New CraftFactory", menuName = "Application/Craft/CraftFactory")]
    public class CraftFactory : ScriptableObject
    {
        [field: SerializeField] public Sprite Icon { get; private set; }
        [field: SerializeField] public string Id { get; private set; }
        [SerializeField] private CraftPair[] _pairs;

        public ItemMeta GetResult(string id)
        {
            for(int i = 0; i < _pairs.Length; i++)
            {
                var pair = _pairs[i];
                if (pair.Ingridient.Id == id)
                    return pair.Result;
            }
            return null;    
        }
    }
}
