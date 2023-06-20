namespace FromTheBasement.Domain.ContainerSystem.Interactors
{
    public interface IHasItemInteractor
    {
        bool Execute(string containerID, string itemID);
    }
}
