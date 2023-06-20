namespace FromTheBasement.Domain.ContainerSystem.Interactors
{
    public interface ITryRemoveItemFromContainerInteractor
    {
        bool Execute(object sender, string containerId, string itemId);
        bool Execute(object sender, string containerId, string itemId, int index);
    }
}
