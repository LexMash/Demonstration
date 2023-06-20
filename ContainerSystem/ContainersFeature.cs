using FromTheBasement.Application.ContainerSystem.Interactors;
using FromTheBasement.Data.ContainerSystem;
using FromTheBasement.Domain.ContainerSystem.Interactors;
using Lukomor.Application.Features;
using System.Threading.Tasks;
using UnityEngine;

namespace FromTheBasement.Domain.ContainerSystem
{
    public class ContainersFeature : Feature
    {
        private static class Keys
        {
            public const string ContainerDTOPath = "DTO/Containers/Saved"; //TODO
            public const string ContainerDefaultPath = "Meta/Containers/DefaultStates"; //TODO
        }
        
        public IContainerStateInteractor ContainerState { get; private set; }
        public IGetContainerContentInteractor GetContainerContent { get; private set; }
        public ITryRemoveItemFromContainerInteractor TryRemoveItem { get; private set; }
        public ITryAddItemToContainerInteractor TryAddItem { get; private set; }

        private IGetContainerInteractor _getContainer;
        private ContainerFeatureModel _model;

        protected override async Task InitializeInternal()
        {
            var containersDefaultStates = Resources.LoadAll<ContainerDefaultState>(Keys.ContainerDefaultPath);

            //для теста

            Container[] containers = new Container[containersDefaultStates.Length];

            for(int k = 0; k < containersDefaultStates.Length; k++)
            {
                var container = new Container(containersDefaultStates[k].Id);

                for (int i = 0; i < container.ItemCells.Length; i++)
                {
                    if (k == containersDefaultStates[k].Items.Count)
                        break;

                    if (containersDefaultStates[k].Items[i] != null)
                    {
                        container.ItemCells[i].ItemId = containersDefaultStates[k].Items[i].Id;
                    }
                }

                containers[k] = container;
            }

            var allContainers = new AllContainers(containers);

            //TODO загрузка данных из DTO

            _model = new ContainerFeatureModel(allContainers);

            _getContainer = new GetContainerInteractor(_model);
            ContainerState = new ContainerStateInteractor(_getContainer);
            GetContainerContent = new GetContainerContentInteractor(_getContainer);
            TryRemoveItem = new TryRemoveItemFromContainerInteractor(_getContainer);
            TryAddItem = new TryAddItemToContainerInteractor(_getContainer, ContainerState);
        }
    }
}
