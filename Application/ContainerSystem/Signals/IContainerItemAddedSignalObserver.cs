using Lukomor.Application.Signals;

namespace FromTheBasement.Application.ContainerSystem.Signals
{
    public interface IContainerItemAddedSignalObserver : ISignalObserver<ContainerItemAddedSignal>
    {
    }
}
