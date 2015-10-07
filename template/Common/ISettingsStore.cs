using System.Runtime.CompilerServices;

namespace <%= props.organization %>.DNN.Modules.<%= props.projectName %>.Common
{
    public interface ISettingsStore
    {
        T Get<T>(T @default = default(T), [CallerMemberName] string name = null);
        void Set<T>(T value, [CallerMemberName] string name = null);
        void Save();
    }
}