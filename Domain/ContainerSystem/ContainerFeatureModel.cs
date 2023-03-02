using FromTheBasement.Data.ContainerSystem;
using System.Collections.Generic;

namespace FromTheBasement.Domain.ContainerSystem
{
    public class ContainerFeatureModel : IContainerModel
    {
        public IReadOnlyDictionary<string, Container> AllContainersMap => _allContainers.ContainersMap;
        
        private AllContainers _allContainers;
        
        public ContainerFeatureModel(AllContainers allContainers)
        {
            _allContainers = allContainers;
        }
    }
}
