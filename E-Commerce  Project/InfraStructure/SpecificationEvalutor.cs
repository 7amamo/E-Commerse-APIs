using Core.Entities;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure
{
    public static class SpecificationEvalutor<T> where T : BaseEntity
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, Specifications<T> spec)
        {
            var Query = inputQuery;
            if (spec.Criteria is not null)
            {
                Query = Query.Where(spec.Criteria);
            }

            if (spec.OrderBy is not null)
            {
                Query.OrderBy(spec.OrderBy);
            }

            if (spec.OrderByDescending is not null)
            {
                Query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.IsPaginationEnable)
            {
                Query = Query.Skip(spec.Skip).Take(spec.Take);
            }

            Query = spec.Includes.Aggregate(Query, (currentQuery, IncludeExpression)
                => currentQuery.Include(IncludeExpression));

            return Query;
        }
    }
}
