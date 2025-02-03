using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Alkambia.App.LoanMonitoring.IRepository
{
    public interface IBaseRepository
    {
        T Get<T>(Guid id);
        T Get<T>(string search, string property);
        List<T> GetMultiple<T>(Guid id);
        List<T> GetMultiple<T>(string search, string property);
        bool Exists<T>(Guid id);
        void Save<T>(T entity);
        void Update<T>(T entity);
        void Delete(Guid id);
    }
}
