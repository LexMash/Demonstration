using Lukomor.Application.Signals;

namespace FromTheBasement.Application.ContainerSystem.Signals
{
    public interface IContainerOpenedSignalObserver : ISignalObserver<ContainerOpenedSignal>
    {
    }
}
