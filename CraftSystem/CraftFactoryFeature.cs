using FromTheBasement.Application.CraftSystem;
using FromTheBasement.Data.CraftSystem;
using FromTheBasement.Domain.CraftSystem.Interactors;
using Lukomor.Application.Features;
using System.Threading.Tasks;
using UnityEngine;

namespace FromTheBasement.Domain.CraftSystem
{
    public class CraftFactoryFeature : Feature
    {
        private static class Keys
        {
            public const string CraftFactoryMetasPath = "Meta/CraftFactory";
        }
        
        public IGetCraftResultInteractor GetResult { get; private set; }
        public IGetCraftFactoryInteractor GetFactory { get; private set; }
        private CraftFactoryModel _model;

        protected override Task InitializeInternal()
        {
            var allFactories = Resources.LoadAll<CraftFactory>(Keys.CraftFactoryMetasPath);

            _model = new CraftFactoryModel(allFactories);
            GetFactory = new GetCraftFactoryInteractor(_model);
            GetResult = new GetCraftResultInteractor(GetFactory);

            return Task.CompletedTask;
        }
    }
}
