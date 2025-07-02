using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Repositories;
using Talabat.Core.Specifications;
using Talabat.Repository.Data;

namespace Talabat.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseModel
    {
        private readonly StoreContext _dbcontext;

        public GenericRepository(StoreContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
                return await _dbcontext.Set<T>().ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbcontext.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> GetAllWithSpecAsync(ISpecifications<T> Spec)
        {
              return await ApplySpecifications(Spec).ToListAsync();
        }

        public async Task<T> GetEntityWithSpecAsync(ISpecifications<T> Spec)
        {
            return await ApplySpecifications(Spec).FirstOrDefaultAsync();
        }

        private IQueryable<T> ApplySpecifications(ISpecifications<T> Spec)
        {
            return SpecificationsEvalutor<T>.GetQuery(_dbcontext.Set<T>(), Spec);
        }

        public async Task<int> CountWithSpecAsync(ISpecifications<T> Spec)
        {
             return await ApplySpecifications(Spec).CountAsync();
        }

        public async Task AddAsync(T item)
        {
           await  _dbcontext.Set<T>().AddAsync(item);
        }

        public void Update(T item)
         => _dbcontext.Set<T>().Update(item);

        public void Delete(T item)
         =>_dbcontext.Set<T>().Remove(item);
    }
}
