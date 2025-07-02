using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public interface ISpecifications<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Criteria { set; get; }

        public List<Expression<Func<T,object>>> Includes { set; get; }

        public Expression<Func<T,object>> OrderBy { set; get; }

        public Expression<Func<T, object>> OrderByDesc { set; get; }

        public int Skip { set; get; }
        public int Take { set; get; }

        public bool IsPagination { set; get; }
    }
}
