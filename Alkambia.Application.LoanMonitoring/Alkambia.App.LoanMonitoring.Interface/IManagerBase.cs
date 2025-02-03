using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.Interface
{
    public interface IManagerBase
    {
        void Add<T>(T entity);
        void SaveorUpdate<T>(T entity);
        void Delete(Guid Id);
        T Get<T>(Guid Id);
        List<T> Get<T>();
        List<T> Get<T>(int skip, int page);

        List<T> Get<T>(string search, int skip, int page);
    }
}
