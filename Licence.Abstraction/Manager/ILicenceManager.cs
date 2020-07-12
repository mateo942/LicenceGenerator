using System;
using System.Threading.Tasks;

namespace Licence.Abstraction.Manager
{
    public interface ILicenceManager
    {
        string GetInfo(string name);
        bool IsActiveModule(string moduleName);
        Task<bool> Valid();
    }

    public interface ILicenceManager<TData> : ILicenceManager
    {
        ILicenceManager<TData> AddParser(string key, Action<string, TData> action);
    }
}
