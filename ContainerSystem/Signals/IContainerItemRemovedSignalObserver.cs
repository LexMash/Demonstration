using Lukomor.Application.Signals;

namespace FromTheBasement.Application.ContainerSystem.Signals
{
    public interface IContainerItemRemovedSignalObserver : ISignalObserver<ContainerItemRemovedSignal>
    {
    }
}
