using FromTheBasement.Data.CraftSystem;
using FromTheBasement.Domain.CraftSystem;
using FromTheBasement.Domain.CraftSystem.Interactors;
using System;

namespace FromTheBasement.Application.CraftSystem
{
    public class GetCraftFactoryInteractor : IGetCraftFactoryInteractor
    {
        private readonly CraftFactoryModel _model;

        public GetCraftFactoryInteractor(CraftFactoryModel model)
        {
            _model = model;
        }

        public CraftFactory Execute(string factoryId)
        {
            if (_model.FactoriesMap.ContainsKey(factoryId))
            {
                return _model.FactoriesMap[factoryId];
            }

            throw new NullReferenceException($"Craft factory with id {factoryId} not exist in CraftFactoryMap");
        }
    }
}
