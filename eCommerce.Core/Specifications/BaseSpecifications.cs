using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using eCommerce.Core.Entities;

namespace eCommerce.Core.Specifications
{
    public class BaseSpecifications<T> : ISpecifications<T> where T : BaseEntity
    {
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool isPagniationEnabled { get; set; }

        public BaseSpecifications()
        {
        }

        public BaseSpecifications(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }

        public void SetOrderBy(Expression<Func<T, object>> expression)
        {
            OrderBy = expression;
        }

        public void SetOrderByDesc(Expression<Func<T, object>> expression)
        {
            OrderByDesc = expression;
        }

        public void ApplyPagniation(int skip, int take)
        {
            isPagniationEnabled = true;
            Skip = skip;
            Take = take;
        }
    }
}