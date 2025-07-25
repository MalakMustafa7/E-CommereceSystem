﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Models;

namespace Talabat.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseModel
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPagination { get; set; }
   
        //Get All
        public BaseSpecifications()
        {

        }
        //Get By Id
        public BaseSpecifications(Expression<Func<T, bool>> criteriaExpression)
        {
            Criteria = criteriaExpression;
        }

        public void AddOrderBy(Expression<Func<T,  object>> OrderByExp)
        {
            OrderBy = OrderByExp;
        }

        public void AddOrderByDesc(Expression<Func<T, object>> OrderByDescExp)
        {
             OrderByDesc = OrderByDescExp;
        }
        public void ApplyPagination(int skip,int take)
        {
            IsPagination= true;
            Skip= skip;
            Take= take;
        }
    }
}
