using FromTheBasement.Data.ContainerSystem;
using FromTheBasement.Domain.ContainerSystem;
using FromTheBasement.Domain.ContainerSystem.Interactors;
using System;

namespace FromTheBasement.Application.ContainerSystem.Interactors
{
    public class GetContainerInteractor : IGetContainerInteractor
    {
        private ContainerFeatureModel _model;

        public GetContainerInteractor(ContainerFeatureModel model)
        {
            _model = model;
        }

        public Container Execute(string id)
        {
            if (_model.AllContainersMap.ContainsKey(id))
            {
                return _model.AllContainersMap[id];
            }

            throw new NullReferenceException($"Container with ID {id} not exist in ContainerWarehouse");
        }
    }
}
