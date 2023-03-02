using Lukomor.Application.Signals;

namespace FromTheBasement.Application.NotepadSystem.Signals
{
    public interface INoteOpenedSignalObserver : ISignalObserver<NoteOpenedSignal>
    {
    }
}
