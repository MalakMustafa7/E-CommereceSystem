using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    public static class SpecificationsEvalutor<T> where T : BaseModel
    {
        //Fun To Build Query
        //_dbcontext.set<T>().where(p=>p.Id == Id).Include(p=>p.ProductBrand).Include(p=>p.ProductType).ToListAsync();

        public static IQueryable<T> GetQuery(IQueryable<T> inputquery,ISpecifications<T> spec)
        {
            var Query = inputquery; //_dbcontext.set<T>()
            if (spec.Criteria is not null) //p=>p.Id == Id
            {  
                Query = Query.Where(spec.Criteria);// _dbcontext.set<T>().where(p => p.Id == Id)
            }
            if(spec.OrderBy is not null)
            {
                Query = Query.OrderBy(spec.OrderBy);
            }
            if(spec.OrderByDesc is not null)
            {
                Query = Query.OrderByDescending(spec.OrderByDesc);
            }
            if (spec.IsPagination)
            {
                Query=  Query.Skip(spec.Skip).Take(spec.Take);
            }
            Query = spec.Includes.Aggregate(Query, (CuurentQuery, IncludeExpression) => CuurentQuery.Include(IncludeExpression));
            //_dbcontext.set<T>().where(p=>p.Id == Id).Include(p=>p.ProductBrand)
            //_dbcontext.set<T>().where(p=>p.Id == Id).Include(p=>p.ProductBrand).Include(p=>p.ProductType).ToListAsync();
            return Query;
        }
    }
}
