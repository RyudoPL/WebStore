using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.Infrastructure
{
    /// <summary>
    /// Interface from which all session mechanisms will be implementing methods
    /// </summary>
    public interface ISessionManager
    {
        T Get<T>(string key);
        void Set<T>(string name, T value);
        void Abandon();
        T TryGet<T>(string key);
    }
}
