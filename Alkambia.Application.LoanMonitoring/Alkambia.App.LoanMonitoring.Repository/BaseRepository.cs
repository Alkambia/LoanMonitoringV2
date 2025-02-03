using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Alkambia.App.LoanMonitoring.IRepository;
using Alkambia.App.LoanMonitoring.DataSource;

namespace Alkambia.App.LoanMonitoring.Repository
{
    public class BaseRepository : IBaseRepository
    {
        public void Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public bool Exists<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string search, string property)
        {
            throw new NotImplementedException();
        }

        public List<T> GetMultiple<T>(Guid id)
        {
            throw new NotImplementedException();
        }

        public List<T> GetMultiple<T>(string search, string property)
        {
            throw new NotImplementedException();
        }

        public void Save<T>(T entity)
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
