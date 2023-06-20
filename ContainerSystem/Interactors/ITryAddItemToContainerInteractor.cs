namespace FromTheBasement.Domain.ContainerSystem.Interactors
{
    public interface ITryAddItemToContainerInteractor
    {
        bool Execute(object sender, string containerId, string itemId);
        bool Execute(object sender, string containerId, string itemId, int index);
    }
}
