using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Core.Repositories
{
    public interface IGenericRepository<T> where T : BaseModel
    {
        #region  WithOutSpecification
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);

        Task AddAsync(T item);
        void Update(T item);
        void Delete(T item);
        #endregion

        #region  WithSpecification
        Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec);
        Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec);
        Task<int> CountWithSpecAsync(ISpecifications<T> Spec);

        #endregion
    }
}
