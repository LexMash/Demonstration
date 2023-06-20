using FromTheBasement.Data.CraftSystem;
using System.Collections.Generic;

namespace FromTheBasement.Domain.CraftSystem
{
    public class CraftFactoryModel
    {
        public IReadOnlyDictionary<string, CraftFactory> FactoriesMap => _factoriesMap;

        private readonly Dictionary<string, CraftFactory> _factoriesMap;

        public CraftFactoryModel(CraftFactory[] factories)
        {
            _factoriesMap = new Dictionary<string, CraftFactory>();

            for(int i = 0; i < factories.Length; i++)
            {
                _factoriesMap.Add(factories[i].Id, factories[i]);
            }
        }
    }
}
