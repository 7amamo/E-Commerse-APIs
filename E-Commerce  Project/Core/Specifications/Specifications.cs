using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Specifications
{
    public abstract class Specifications<T> where T : BaseEntity
    {
        protected Specifications(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public Expression<Func<T, bool>> Criteria { get; set; }
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();

        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDescending { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; }
        public bool IsPaginationEnable { get; set; }


        public void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        public void AddOrderByDescinding(Expression<Func<T, object>> orderByExpression)
        {
            OrderByDescending = orderByExpression;
        }


        public void ApplyPagination(int pageindex, int pagesize)
        {
            IsPaginationEnable = true;
            Take = pagesize;
            Skip = (pageindex - 1) * pagesize;
        }


    }
}
