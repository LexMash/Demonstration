namespace FromTheBasement.Domain.ContainerSystem.Interactors
{
    public interface IContainerStateInteractor
    {
        bool IsFull(string containerID);
        bool IsEmpty(string containerID);
        bool HasEmptyCells(string containerID);
    }
}
