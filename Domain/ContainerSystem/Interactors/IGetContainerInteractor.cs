using FromTheBasement.Data.ContainerSystem;

namespace FromTheBasement.Domain.ContainerSystem.Interactors
{
    public interface IGetContainerInteractor
    {
        Container Execute(string id);
    }
}
