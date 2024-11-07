using System.Linq.Expressions;
using eCommerce.Core.Entities;
using eCommerce.Core.Specifications;
using eCommerce.Repository.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Repository
{
    public static class SpecificationEvaluator<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecifications<T> specifications)
        {
            var query = inputQuery;

            if (specifications.Criteria is not null)
            {
                query = query.Where(specifications.Criteria);
            }

            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);
            if (specifications.OrderByDesc is not null)
                query = query.OrderByDescending(specifications.OrderByDesc);

            /* Apply Pagniation after orderby */
            if (specifications.isPagniationEnabled)
            {
                query = query.Skip(specifications.Skip).Take(specifications.Take);
            }
            query = specifications.Includes.Aggregate(query, (currentQuery, expression) => currentQuery.Include(expression));

            return query;
        }
    }
}